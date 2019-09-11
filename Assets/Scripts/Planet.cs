using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public GameObject m_PlanetMesh;
    public List<Polygon> m_Polygons;
    public float p_Rotation;
    public float axis;

    public void Start()
    {
        m_PlanetMesh = this.gameObject;
        //m_Polygons = i_Polygons;
        p_Rotation = 10f;
        axis = -10f;
        m_PlanetMesh.transform.rotation = Quaternion.Euler(0, 0, axis);
    }

    public void Update()
    {
        m_PlanetMesh.transform.Rotate(Vector3.up * p_Rotation * Time.deltaTime, Space.Self);

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {

            MeshCollider meshCollider = hit.collider as MeshCollider;
            if (meshCollider != null && meshCollider.sharedMesh != null)
            {
                Mesh mesh = meshCollider.sharedMesh;
                int[] triangles = mesh.triangles;
                setColor(triangles[hit.triangleIndex * 3], new Color32(255,255,255,255));

                //Mesh mesh = meshCollider.sharedMesh;
                //Vector3[] normals = mesh.normals;
                //int[] triangles = mesh.triangles;

                //Color32[] colors = mesh.colors32;

                //colors[triangles[hit.triangleIndex * 3 + 0]] = new Color32(0, 0, 0, 255);
                //colors[triangles[hit.triangleIndex * 3 + 1]] = new Color32(0, 0, 0, 255);
                //colors[triangles[hit.triangleIndex * 3 + 2]] = new Color32(0, 0, 0, 255);

                //mesh.colors32 = colors;
            }
        }
    }

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

    public void setColor(int index, Color32 color)
    {
        Polygon poly = GetPolygon(index);
        if (poly != null)
        {
            Mesh m_Mesh = m_PlanetMesh.GetComponent<MeshFilter>().mesh;
            Color32[] colors = m_Mesh.colors32;

            foreach (int ind in poly.indices)
            {
                colors[ind + 0] = color;
                colors[ind + 1] = color;
                colors[ind + 2] = color;
            }

            m_PlanetMesh.GetComponent<MeshFilter>().mesh.colors32 = colors;
        }
    }

    public void draw()
    {
        int vertexCount = m_Polygons.Count * 6;

        int[] indices = new int[vertexCount];

        Mesh m_Mesh = m_PlanetMesh.GetComponent<MeshFilter>().mesh;
        Color32[] colors = m_Mesh.colors32;

        Color32 polyColor = new Color32(0, 255, 255, 255);

        for (int i = 0; i < m_Polygons.Count; i++)
        {

            colors[i * 6 + 0] = polyColor;
            colors[i * 6 + 1] = polyColor;
            colors[i * 6 + 2] = polyColor;

            colors[i * 6 + 3] = polyColor;
            colors[i * 6 + 4] = polyColor;
            colors[i * 6 + 5] = polyColor;
        }

        m_PlanetMesh.GetComponent<MeshFilter>().mesh.colors32 = colors;
    }
}
