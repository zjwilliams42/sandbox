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
    }

    public void setColor(int index, Color32 color)
    {
        Mesh m_Mesh = m_PlanetMesh.GetComponent<MeshFilter>().mesh;
        Color32[] colors = m_Mesh.colors32;

        colors[index * 6 + 0] = color;
        colors[index * 6 + 1] = color;
        colors[index * 6 + 2] = color;

        colors[index * 6 + 3] = color;
        colors[index * 6 + 4] = color;
        colors[index * 6 + 5] = color;

        m_PlanetMesh.GetComponent<MeshFilter>().mesh.colors32 = colors;
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
