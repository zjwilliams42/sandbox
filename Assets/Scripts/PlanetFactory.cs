/***
* This file is sort of a combo from my code and the code
* from the below url.
* https://medium.com/@peter_winslow/creating-procedural-planets-in-unity-part-1-df83ecb12e91
*
* The goal here is to procedurally create planets.
* Zachary Williams
* Created: 9/8/2019
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/***
* This is the class that will be each individual polygon
* of our planet. The tutorial has this as a triangle, ours
* is a square.
*/
public class Polygon
{
    public List<int> m_Vertices;

    public Polygon(int a, int b, int c, int d)
    {
        m_Vertices = new List<int>() { a, b, c, d};
    }

    public Polygon(int a, int b, int c)
    {
        m_Vertices = new List<int>() { a, b, c };
    }
}

public class PlanetFactory : MonoBehaviour
{

    public List<Polygon> m_Polygons;
    public List<Vector3> m_Vertices;
    public GameObject m_PlanetMesh;
    public Material m_Material;
    public int m_Subdivide;

    // Start is called before the first frame update
    void Start()
    {
        InitAsIcosohedron();
        //InitAsCube();
        SubdivideTriangle(m_Subdivide);
        //SubdivideSquare(m_Subdivide);
        GenerateMeshTriangle();
        //GenerateMeshSquare();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SubdivideTriangle(int recursions)
    {
        var midPointCache = new Dictionary<int, int>();

        for (int i = 0; i < recursions; i++)
        {
            var newPolys = new List<Polygon>();
            foreach (var poly in m_Polygons)
            {
                int a = poly.m_Vertices[0];
                int b = poly.m_Vertices[1];
                int c = poly.m_Vertices[2];

                // Use GetMidPointIndex to either create a
                // new vertex between two old vertices, or
                // find the one that was already created.
                int ab = GetMidPointIndex (midPointCache, a, b);
                int bc = GetMidPointIndex(midPointCache, b, c);
                int ca = GetMidPointIndex(midPointCache, c, a);

                // Create the four new polygons using our original
                // three vertices, and the three new midpoints.
                newPolys.Add (new Polygon (a, ab, ca));
                newPolys.Add(new Polygon(b, bc, ab));
                newPolys.Add(new Polygon(c, ca, bc));
                newPolys.Add(new Polygon(ab, bc, ca));
            }

            // Replace all our old polygons with the new set of
            // subdivided ones.
            m_Polygons = newPolys;
        }
    }

    public void SubdivideSquare(int recursions)
    {
        var midPointCache = new Dictionary<int, int>();

        for (int i = 0; i < recursions; i++)
        {
            var newPolys = new List<Polygon>();
            foreach (var poly in m_Polygons)
            {
                int a = poly.m_Vertices[0];
                int b = poly.m_Vertices[1];
                int c = poly.m_Vertices[2];
                int d = poly.m_Vertices[3];

                // Use GetMidPointIndex to either create a
                // new vertex between two old vertices, or
                // find the one that was already created.
                int ab = GetMidPointIndex(midPointCache, a, b);
                int bc = GetMidPointIndex(midPointCache, b, c);
                int cd = GetMidPointIndex(midPointCache, c, d);
                int da = GetMidPointIndex(midPointCache, d, a);
                int abcd = GetMidPointIndex(midPointCache, ab, cd);

                // Create the four new polygons using our original
                // three vertices, and the three new midpoints.
                newPolys.Add(new Polygon(a, ab, abcd, da));
                newPolys.Add(new Polygon(ab, b, bc, abcd));
                newPolys.Add(new Polygon(abcd, bc, c, cd));
                newPolys.Add(new Polygon(da, abcd, cd, d));
            }
            // Replace all our old polygons with the new set of
            // subdivided ones.
            m_Polygons = newPolys;
        }
    }

    public int GetMidPointIndex(Dictionary<int, int> cache, int indexA, int indexB)
    {
        // We create a key out of the two original indices
        // by storing the smaller index in the upper two bytes
        // of an integer, and the larger index in the lower two
        // bytes. By sorting them according to whichever is smaller
        // we ensure that this function returns the same result
        // whether you call
        // GetMidPointIndex(cache, 5, 9)
        // or...
        // GetMidPointIndex(cache, 9, 5)
        int smallerIndex = Mathf.Min(indexA, indexB);
        int greaterIndex = Mathf.Max(indexA, indexB);
        int key = (smallerIndex << 16) + greaterIndex;

        // If a midpoint is already defined, just return it.
        int ret;
        if (cache.TryGetValue(key, out ret))
            return ret;

        // If we're here, it's because a midpoint for these two
        // vertices hasn't been created yet. Let's do that now!
        Vector3 p1 = m_Vertices[indexA];
        Vector3 p2 = m_Vertices[indexB];
        Vector3 middle = Vector3.Lerp(p1, p2, 0.5f).normalized;

        ret = m_Vertices.Count;
        m_Vertices.Add(middle);

        cache.Add(key, ret);
        return ret;
    }

    public void InitAsIcosohedron()
    {
        m_Polygons = new List<Polygon>();
        m_Vertices = new List<Vector3>();

        // An icosahedron has 12 vertices, and
        // since it's completely symmetrical the
        // formula for calculating them is kind of
        // symmetrical too:

        float t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;

        m_Vertices.Add(new Vector3(-1, t, 0).normalized);
        m_Vertices.Add(new Vector3(1, t, 0).normalized);
        m_Vertices.Add(new Vector3(-1, -t, 0).normalized);
        m_Vertices.Add(new Vector3(1, -t, 0).normalized);
        m_Vertices.Add(new Vector3(0, -1, t).normalized);
        m_Vertices.Add(new Vector3(0, 1, t).normalized);
        m_Vertices.Add(new Vector3(0, -1, -t).normalized);
        m_Vertices.Add(new Vector3(0, 1, -t).normalized);
        m_Vertices.Add(new Vector3(t, 0, -1).normalized);
        m_Vertices.Add(new Vector3(t, 0, 1).normalized);
        m_Vertices.Add(new Vector3(-t, 0, -1).normalized);
        m_Vertices.Add(new Vector3(-t, 0, 1).normalized);

        // And here's the formula for the 20 sides,
        // referencing the 12 vertices we just created.
        m_Polygons.Add (new Polygon(0, 11, 5));
        m_Polygons.Add(new Polygon(0, 5, 1));
        m_Polygons.Add(new Polygon(0, 1, 7));
        m_Polygons.Add(new Polygon(0, 7, 10));
        m_Polygons.Add(new Polygon(0, 10, 11));
        m_Polygons.Add(new Polygon(1, 5, 9));
        m_Polygons.Add(new Polygon(5, 11, 4));
        m_Polygons.Add(new Polygon(11, 10, 2));
        m_Polygons.Add(new Polygon(10, 7, 6));
        m_Polygons.Add(new Polygon(7, 1, 8));
        m_Polygons.Add(new Polygon(3, 9, 4));
        m_Polygons.Add(new Polygon(3, 4, 2));
        m_Polygons.Add(new Polygon(3, 2, 6));
        m_Polygons.Add(new Polygon(3, 6, 8));
        m_Polygons.Add(new Polygon(3, 8, 9));
        m_Polygons.Add(new Polygon(4, 9, 5));
        m_Polygons.Add(new Polygon(2, 4, 11));
        m_Polygons.Add(new Polygon(6, 2, 10));
        m_Polygons.Add(new Polygon(8, 6, 7));
        m_Polygons.Add(new Polygon(9, 8, 1));
    }

    public void InitAsCube()
    {
        m_Polygons = new List<Polygon>();
        m_Vertices = new List<Vector3>();

        m_Vertices.Add(new Vector3(-0.5f, -0.5f, -0.5f));
        m_Vertices.Add(new Vector3(0.5f, -0.5f, -0.5f));
        m_Vertices.Add(new Vector3(0.5f, 0.5f, -0.5f));
        m_Vertices.Add(new Vector3(-0.5f, 0.5f, -0.5f));
        m_Vertices.Add(new Vector3(-0.5f, -0.5f, 0.5f));
        m_Vertices.Add(new Vector3(0.5f, -0.5f, 0.5f));
        m_Vertices.Add(new Vector3(0.5f, 0.5f, 0.5f));
        m_Vertices.Add(new Vector3(-0.5f, 0.5f, 0.5f));


        m_Polygons.Add(new Polygon(0, 1, 2, 3)); // front
        m_Polygons.Add(new Polygon(3, 2, 6, 7)); // top
        m_Polygons.Add(new Polygon(1, 5, 6, 2)); // right
        m_Polygons.Add(new Polygon(4, 0, 3, 7)); // left
        m_Polygons.Add(new Polygon(5, 4, 7, 6)); // back
        m_Polygons.Add(new Polygon(4, 5, 1, 0)); // bottom
    }
    public void GenerateMeshTriangle()
    {
        // We'll store our planet's mesh in the m_PlanetMesh
        // variable so that we can delete the old copy when
        // we want to generate a new one.
        if (m_PlanetMesh)
            Destroy(m_PlanetMesh);

        m_PlanetMesh = new GameObject("Planet Mesh");
        MeshRenderer surfaceRenderer =m_PlanetMesh.AddComponent<MeshRenderer>();
        surfaceRenderer.material = m_Material; Mesh terrainMesh = new Mesh();

        int vertexCount = m_Polygons.Count * 3;

        int[] indices = new int[vertexCount];

        Vector3[] vertices = new Vector3[vertexCount];
        Vector3[] normals = new Vector3[vertexCount];
        Color32[] colors = new Color32[vertexCount];

        Color32 green = new Color32(20, 255, 30, 255);
        Color32 brown = new Color32(220, 150, 70, 255);
        for (int i = 0; i < m_Polygons.Count; i++)
        {
            var poly = m_Polygons[i]; indices[i * 3 + 0] = i * 3 + 0;
            indices[i * 3 + 1] = i * 3 + 1;
            indices[i * 3 + 2] = i * 3 + 2; vertices[i * 3 + 0] = m_Vertices[poly.m_Vertices[0]];
            vertices[i * 3 + 1] = m_Vertices[poly.m_Vertices[1]];
            vertices[i * 3 + 2] = m_Vertices[poly.m_Vertices[2]];
            
            // Here's where we assign each polygon a random color.
            Color32 polyColor = Color32.Lerp(green, brown, Random.Range(0.0f, 1.0f)); colors[i * 3 + 0] = polyColor;
            colors[i * 3 + 1] = polyColor;
            colors[i * 3 + 2] = polyColor;

            // For now our planet is still perfectly spherical, so
            // so the normal of each vertex is just like the vertex
            // itself: pointing away from the origin.
            normals[i * 3 + 0] = m_Vertices[poly.m_Vertices[0]];
            normals[i * 3 + 1] = m_Vertices[poly.m_Vertices[1]];
            normals[i * 3 + 2] = m_Vertices[poly.m_Vertices[2]];
        }
        terrainMesh.vertices = vertices;
        terrainMesh.normals = normals;
        terrainMesh.colors32 = colors;
        terrainMesh.SetTriangles(indices, 0); MeshFilter terrainFilter = m_PlanetMesh.AddComponent<MeshFilter>();
        terrainFilter.mesh = terrainMesh;
    }

    public void GenerateMeshSquare()
    {
        if (m_PlanetMesh)
        {
            Destroy(m_PlanetMesh);
        }

        m_PlanetMesh = new GameObject("Planet Mesh");

        MeshRenderer surfaceRenderer = m_PlanetMesh.AddComponent<MeshRenderer>();
        surfaceRenderer.material = m_Material;

        Mesh terrainMesh = new Mesh();

        int vertexCount = m_Polygons.Count * 6;

        int[] indices = new int[vertexCount];

        Vector3[] vertices = new Vector3[vertexCount];
        Vector3[] normals = new Vector3[vertexCount];
        Color32[] colors = new Color32[vertexCount];

        Color32 green = new Color32(20, 255, 30, 255);
        Color32 brown = new Color32(220, 150, 70, 255);

        for (int i = 0; i < m_Polygons.Count; i++)
        {
            var poly = m_Polygons[i];

            indices[i * 6 + 0] = i * 6 + 0;
            indices[i * 6 + 1] = i * 6 + 3;
            indices[i * 6 + 2] = i * 6 + 1;

            indices[i * 6 + 3] = i * 6 + 1;
            indices[i * 6 + 4] = i * 6 + 3;
            indices[i * 6 + 5] = i * 6 + 2;

            vertices[i * 6 + 0] = m_Vertices[poly.m_Vertices[0]];
            vertices[i * 6 + 1] = m_Vertices[poly.m_Vertices[1]];
            vertices[i * 6 + 2] = m_Vertices[poly.m_Vertices[2]];
            vertices[i * 6 + 3] = m_Vertices[poly.m_Vertices[3]];

            // Here's where we assign each polygon a random color.
            Color32 polyColor = Color32.Lerp(green, brown, Random.Range(0.0f, 1.0f));

            colors[i * 6 + 0] = polyColor;
            colors[i * 6 + 1] = polyColor;
            colors[i * 6 + 2] = polyColor;

            colors[i * 6 + 3] = polyColor;
            colors[i * 6 + 4] = polyColor;
            colors[i * 6 + 5] = polyColor;
        }

        terrainMesh.vertices = vertices;
        terrainMesh.normals = normals;
        terrainMesh.colors32 = colors;

        terrainMesh.SetTriangles(indices, 0);

        MeshFilter terrainFilter = m_PlanetMesh.AddComponent<MeshFilter>();
        terrainFilter.mesh = terrainMesh;
        terrainFilter.mesh.RecalculateNormals();
    }

}
