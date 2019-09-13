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
    public float p_axis;

    private Polygon s_Polygon;
    private List<Color32> s_Color32;
    public GameObject s_Display;
    public int s_MapResolution;

    public LibNoise.Generator.Perlin Perlin;
    #endregion

    #region Constructors
    public void Awake()
    {
        m_PlanetMesh = this.gameObject;
        p_Rotation = 10f;
        s_MapResolution = 256;
    }

    public void SetAxis(float axis)
    {
        Debug.Log("Axis: " + axis);
        p_axis = axis;
        if (m_PlanetMesh.transform == null) Debug.Log("[*] No transform.");
        m_PlanetMesh.transform.rotation = Quaternion.Euler(0, 0, p_axis);
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
                int j = 0;
                foreach (int ind in s_Polygon.indices)
                {
                    colors[ind + 0] = s_Color32[j++];
                    colors[ind + 1] = s_Color32[j++];
                    colors[ind + 2] = s_Color32[j++];
                }
            }

            s_Color32 = new List<Color32>();
            foreach (int ind in poly.indices)
            {
                s_Color32.Add(colors[ind + 0]);
                s_Color32.Add(colors[ind + 1]);
                s_Color32.Add(colors[ind + 2]);
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
                Texture2D texture = new Texture2D(s_MapResolution, s_MapResolution);

                Vector3 f = mesh.vertices[poly.t_Vertices[3]] - mesh.vertices[poly.t_Vertices[2]];
                Vector3 g = mesh.vertices[poly.t_Vertices[3]] - mesh.vertices[poly.t_Vertices[0]];
                Vector3 P = mesh.vertices[poly.t_Vertices[3]];
                Vector3 Q = mesh.vertices[poly.t_Vertices[0]];
                Vector3 R = mesh.vertices[poly.t_Vertices[2]];
                Vector3 p = new Vector3(0, 0, 0);


                float stepX = FindStep(mesh.vertices[poly.t_Vertices[3]].x, mesh.vertices[poly.t_Vertices[2]].x);
                float stepY = FindStep(mesh.vertices[poly.t_Vertices[3]].y, mesh.vertices[poly.t_Vertices[0]].y);

                float denom = (f.x * g.y) - (f.y * g.x);
                float alpha = ((p.x * g.y) - (p.y * g.x)) / denom;
                float beta = ((p.y * g.x) - (p.x * g.y)) / denom;

                Vector3 stepVector = new Vector3(stepX, stepY, 0);
                float stepAlpha = ((stepVector.x * g.y) - (stepVector.y * g.x)) / denom;
                float stepBeta = ((stepVector.y * g.x) - (stepVector.x * g.y)) / denom;

                float goalAlpha = ((R.x * g.y) - (R.y * g.x)) / denom;
                float goalBeta = ((Q.x * g.y) - (Q.y * g.x)) / denom;

                float currAlpha = alpha;
                for (int y = 0; y < texture.width; y++)
                {
                    for (int x = 0; x < texture.height; x++)
                    {
                        Vector3 P_prime = (currAlpha * f) + (beta * g) + P;
                        
                        double value = Mathf.Abs((float)Perlin.GetValue(P_prime.x, P_prime.y, P_prime.z));
                        Color32 polyColor = p_TerrainType[p_TerrainType.Count - 1];
                        for (int range = 0; range < p_TerrainHeight.Count; range++)
                        {
                            if (value <= p_TerrainHeight[range])
                            {
                                polyColor = p_TerrainType[range];
                                break;
                            }
                        }

                        texture.SetPixel(x, y, polyColor);
                        currAlpha += stepAlpha;
                    }
                    beta += stepBeta;
                    currAlpha = alpha;
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

    private float FindStep(float x1, float x2)
    {
        float smallerX = Mathf.Min(x1, x2);
        float greaterX = Mathf.Max(x1, x2);

        return (greaterX - smallerX) / s_MapResolution;
    }
    #endregion
}
