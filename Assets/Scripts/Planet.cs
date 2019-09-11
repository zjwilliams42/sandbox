using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    #region Fields
    public GameObject m_PlanetMesh;
    public List<Polygon> m_Polygons;

    public List<float> p_TerrainHeight;
    public List<Color32> p_TerrainType;

    public float p_Rotation;
    public float axis;

    private Polygon s_Polygon;
    private Color32 s_Color32;
    public GameObject s_Display;
    public int s_MapResolution;

    public LibNoise.Generator.Perlin Perlin;
    #endregion

    #region Constructors
    public void Start()
    {
        m_PlanetMesh = this.gameObject;
        p_Rotation = 10f;
        axis = -10f;
        m_PlanetMesh.transform.rotation = Quaternion.Euler(0, 0, axis);
        s_MapResolution = 10;
    }
    #endregion

    public void Update()
    {
        #region Planet Rotation
        m_PlanetMesh.transform.Rotate(Vector3.up * p_Rotation * Time.deltaTime, Space.Self);
        #endregion

        #region Area Selection
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                MeshCollider meshCollider = hit.collider as MeshCollider;
                if (meshCollider != null && meshCollider.sharedMesh != null)
                {
                    Mesh mesh = meshCollider.sharedMesh;
                    int[] triangles = mesh.triangles;
                    setColor(triangles[hit.triangleIndex * 3], new Color32(255, 255, 255, 255));
                }
            }
        }
        #endregion
    }

    public void setColor(int index, Color32 color)
    {
        Polygon poly = GetPolygon(index);
        if (poly != null)
        {
            #region Set Selection Color
            Mesh m_Mesh = m_PlanetMesh.GetComponent<MeshFilter>().mesh;
            Color32[] colors = m_Mesh.colors32;

            if (s_Polygon != null)
            {
                foreach (int ind in s_Polygon.indices)
                {
                    colors[ind + 0] = s_Color32;
                    colors[ind + 1] = s_Color32;
                    colors[ind + 2] = s_Color32;
                }
            }

            foreach (int ind in poly.indices)
            {
                s_Color32 = colors[ind];
                colors[ind + 0] = color;
                colors[ind + 1] = color;
                colors[ind + 2] = color;
            }

            s_Polygon = poly;

            m_PlanetMesh.GetComponent<MeshFilter>().mesh.colors32 = colors;
            #endregion

            #region Set Display Area (If Exists)
            if (s_Display != null)
            {
                Mesh mesh = m_PlanetMesh.GetComponent<MeshFilter>().mesh;
                Texture2D texture = new Texture2D(128, 128);
                for (int x = 0; x < texture.width; x++)
                {
                    double stepX = FindStep(mesh.vertices[poly.m_Vertices[2] * 3].x, mesh.vertices[poly.m_Vertices[3] * 3].x);
                    double stepY = FindStep(mesh.vertices[poly.m_Vertices[2] * 3].y, mesh.vertices[poly.m_Vertices[0] * 3].y);

                    for (int y = 0; y < texture.height; y++)
                    {
                        double value = Mathf.Abs((float)Perlin.GetValue(mesh.vertices[poly.m_Vertices[0] * 3].x + (stepX * x), mesh.vertices[poly.m_Vertices[0] * 3].y + (stepY * y), mesh.vertices[poly.m_Vertices[0] * 3].z));
                        Color32 polyColor = p_TerrainType[p_TerrainType.Count - 1];
                        for (int range = 0; range < p_TerrainHeight.Count; range++)
                        {
                            if (value <= p_TerrainHeight[range])
                            {
                                polyColor = p_TerrainType[range];
                                break;
                            }
                        }

                        //texture.SetPixel(x, y, s_Color32);
                        texture.SetPixel(x, y, polyColor);
                    }
                }
                s_Display.GetComponent<Renderer>().material.mainTexture = texture;
                texture.Apply();
            }
            #endregion
        }
    }

    #region Utility Methods
    private Polygon GetPolygon(int index)
    {
        foreach (Polygon poly in m_Polygons)
        {
            if (poly.indices.Contains(index))
            {
                return poly;
            }
        }
        return null;
    }

    private double FindStep(float x1, float x2)
    {
        float smallerX = Mathf.Min(x1, x2);
        float greaterX = Mathf.Max(x1, x2);

        return (greaterX - smallerX) / s_MapResolution;
    }
    #endregion
}
