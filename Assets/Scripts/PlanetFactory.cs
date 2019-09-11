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


public class PlanetFactory : MonoBehaviour
{
    /***
     * This function generates and return a Planet object.
     * 
     * @param "subdivide" this is how many time we subdivide the original shape.
     * @param "type" decided the original shape we are subdividing, default is 0. (0 = cube, 1, = icosohedron)
     */
    static public GameObject GeneratePlanet(Material material, int subdivide, int type = 0)
    {
        if (type == 0)
        {
            Primitive cube = InitAsCube();
            cube.m_Material = material;
            cube = SubdivideSquare(cube, subdivide);
            LibNoise.Generator.Perlin perlin = new LibNoise.Generator.Perlin();
            perlin.Seed = Random.Range(0, 999999);
            GameObject planet = GenerateMeshSquare(cube, perlin);
            planet.AddComponent<MeshCollider>();
            planet.AddComponent<Planet>();
            planet.GetComponent<Planet>().m_Polygons = cube.m_Polygons;
            return planet;
        }
        else if (type == 1)
        {
            Primitive icsosohedron = InitAsIcosohedron();
            icsosohedron.m_Material = material;
            icsosohedron = SubdivideTriangle(icsosohedron, subdivide);
            LibNoise.Generator.Perlin perlin = new LibNoise.Generator.Perlin();
            perlin.Seed = Random.Range(0,999999);
            GameObject planet = GenerateMeshTriangle(icsosohedron);
            planet.AddComponent<SphereCollider>();
            planet.AddComponent<Planet>();
            planet.GetComponent<Planet>().m_Polygons = icsosohedron.m_Polygons;
            return planet;
        }
        else
        { return null; }
    }

    private static Primitive InitAsIcosohedron()
    {
        Primitive icosohedron = new Primitive();
        //m_Polygons = new List<Polygon>();
        //m_Vertices = new List<Vector3>();

        float t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;

        icosohedron.m_Vertices.Add(new Vector3(-1, t, 0).normalized);
        icosohedron.m_Vertices.Add(new Vector3(1, t, 0).normalized);
        icosohedron.m_Vertices.Add(new Vector3(-1, -t, 0).normalized);
        icosohedron.m_Vertices.Add(new Vector3(1, -t, 0).normalized);
        icosohedron.m_Vertices.Add(new Vector3(0, -1, t).normalized);
        icosohedron.m_Vertices.Add(new Vector3(0, 1, t).normalized);
        icosohedron.m_Vertices.Add(new Vector3(0, -1, -t).normalized);
        icosohedron.m_Vertices.Add(new Vector3(0, 1, -t).normalized);
        icosohedron.m_Vertices.Add(new Vector3(t, 0, -1).normalized);
        icosohedron.m_Vertices.Add(new Vector3(t, 0, 1).normalized);
        icosohedron.m_Vertices.Add(new Vector3(-t, 0, -1).normalized);
        icosohedron.m_Vertices.Add(new Vector3(-t, 0, 1).normalized);

        icosohedron.m_Polygons.Add(new Polygon(0, 11, 5));
        icosohedron.m_Polygons.Add(new Polygon(0, 5, 1));
        icosohedron.m_Polygons.Add(new Polygon(0, 1, 7));
        icosohedron.m_Polygons.Add(new Polygon(0, 7, 10));
        icosohedron.m_Polygons.Add(new Polygon(0, 10, 11));
        icosohedron.m_Polygons.Add(new Polygon(1, 5, 9));
        icosohedron.m_Polygons.Add(new Polygon(5, 11, 4));
        icosohedron.m_Polygons.Add(new Polygon(11, 10, 2));
        icosohedron.m_Polygons.Add(new Polygon(10, 7, 6));
        icosohedron.m_Polygons.Add(new Polygon(7, 1, 8));
        icosohedron.m_Polygons.Add(new Polygon(3, 9, 4));
        icosohedron.m_Polygons.Add(new Polygon(3, 4, 2));
        icosohedron.m_Polygons.Add(new Polygon(3, 2, 6));
        icosohedron.m_Polygons.Add(new Polygon(3, 6, 8));
        icosohedron.m_Polygons.Add(new Polygon(3, 8, 9));
        icosohedron.m_Polygons.Add(new Polygon(4, 9, 5));
        icosohedron.m_Polygons.Add(new Polygon(2, 4, 11));
        icosohedron.m_Polygons.Add(new Polygon(6, 2, 10));
        icosohedron.m_Polygons.Add(new Polygon(8, 6, 7));
        icosohedron.m_Polygons.Add(new Polygon(9, 8, 1));

        return icosohedron;
    }

    private static Primitive InitAsCube()
    {
        //m_Polygons = new List<Polygon>();
        //m_Vertices = new List<Vector3>();
        Primitive cube = new Primitive();

        cube.m_Vertices.Add(new Vector3(-0.5f, -0.5f, -0.5f).normalized);
        cube.m_Vertices.Add(new Vector3(0.5f, -0.5f, -0.5f).normalized);
        cube.m_Vertices.Add(new Vector3(0.5f, 0.5f, -0.5f).normalized);
        cube.m_Vertices.Add(new Vector3(-0.5f, 0.5f, -0.5f).normalized);
        cube.m_Vertices.Add(new Vector3(-0.5f, -0.5f, 0.5f).normalized);
        cube.m_Vertices.Add(new Vector3(0.5f, -0.5f, 0.5f).normalized);
        cube.m_Vertices.Add(new Vector3(0.5f, 0.5f, 0.5f).normalized);
        cube.m_Vertices.Add(new Vector3(-0.5f, 0.5f, 0.5f).normalized);


        cube.m_Polygons.Add(new Polygon(0, 1, 2, 3)); // front
        cube.m_Polygons.Add(new Polygon(3, 2, 6, 7)); // top
        cube.m_Polygons.Add(new Polygon(1, 5, 6, 2)); // right
        cube.m_Polygons.Add(new Polygon(4, 0, 3, 7)); // left
        cube.m_Polygons.Add(new Polygon(5, 4, 7, 6)); // back
        cube.m_Polygons.Add(new Polygon(4, 5, 1, 0)); // bottom

        return cube;
    }

    private static Primitive SubdivideTriangle(Primitive icosohedron, int recursions)
    {
        var midPointCache = new Dictionary<int, int>();

        for (int i = 0; i < recursions; i++)
        {
            var newPolys = new List<Polygon>();
            foreach (var poly in icosohedron.m_Polygons)
            {
                int a = poly.m_Vertices[0];
                int b = poly.m_Vertices[1];
                int c = poly.m_Vertices[2];

                // Use GetMidPointIndex to either create a
                // new vertex between two old vertices, or
                // find the one that was already created.
                int ab = GetMidPointIndex(midPointCache, a, b, icosohedron);
                int bc = GetMidPointIndex(midPointCache, b, c, icosohedron);
                int ca = GetMidPointIndex(midPointCache, c, a, icosohedron);

                // Create the four new polygons using our original
                // three vertices, and the three new midpoints.
                newPolys.Add(new Polygon(a, ab, ca));
                newPolys.Add(new Polygon(b, bc, ab));
                newPolys.Add(new Polygon(c, ca, bc));
                newPolys.Add(new Polygon(ab, bc, ca));
            }

            // Replace all our old polygons with the new set of
            // subdivided ones.
            icosohedron.m_Polygons = newPolys;
        }

        return icosohedron;
    }

    private static Primitive SubdivideSquare(Primitive cube, int recursions)
    {
        var midPointCache = new Dictionary<int, int>();

        for (int i = 0; i < recursions; i++)
        {
            var newPolys = new List<Polygon>();
            foreach (var poly in cube.m_Polygons)
            {
                int a = poly.m_Vertices[0];
                int b = poly.m_Vertices[1];
                int c = poly.m_Vertices[2];
                int d = poly.m_Vertices[3];

                // Use GetMidPointIndex to either create a
                // new vertex between two old vertices, or
                // find the one that was already created.
                int ab = GetMidPointIndex(midPointCache, a, b, cube);
                int bc = GetMidPointIndex(midPointCache, b, c, cube);
                int cd = GetMidPointIndex(midPointCache, c, d, cube);
                int da = GetMidPointIndex(midPointCache, d, a, cube);
                int abcd = GetMidPointIndex(midPointCache, ab, cd, cube);

                // Create the four new polygons using our original
                // three vertices, and the three new midpoints.
                newPolys.Add(new Polygon(a, ab, abcd, da));
                newPolys.Add(new Polygon(ab, b, bc, abcd));
                newPolys.Add(new Polygon(abcd, bc, c, cd));
                newPolys.Add(new Polygon(da, abcd, cd, d));
            }
            // Replace all our old polygons with the new set of
            // subdivided ones.
            cube.m_Polygons = newPolys;
        }

        return cube;
    }

    private static int GetMidPointIndex(Dictionary<int, int> cache, int indexA, int indexB, Primitive shape)
    {
        int smallerIndex = Mathf.Min(indexA, indexB);
        int greaterIndex = Mathf.Max(indexA, indexB);
        int key = (smallerIndex << 16) + greaterIndex;

        // if a midpoint is already defined, just return it.
        int ret;
        if (cache.TryGetValue(key, out ret))
            return ret;

        // otherwise...
        Vector3 p1 = shape.m_Vertices[indexA];
        Vector3 p2 = shape.m_Vertices[indexB];
        Vector3 middle = Vector3.Lerp(p1, p2, 0.5f).normalized;

        ret = shape.m_Vertices.Count;
        shape.m_Vertices.Add(middle);

        cache.Add(key, ret);
        return ret;
    }

    private static GameObject GenerateMeshTriangle(Primitive icosohedron)
    {
        GameObject m_PlanetMesh = new GameObject("Planet Mesh");

        Mesh terrainMesh = new Mesh();
        MeshRenderer surfaceRenderer = m_PlanetMesh.AddComponent<MeshRenderer>();
        surfaceRenderer.material = icosohedron.m_Material;

        int vertexCount = icosohedron.m_Polygons.Count * 3;
        int[] indices = new int[vertexCount];

        Vector3[] vertices = new Vector3[vertexCount];
        Vector3[] normals = new Vector3[vertexCount];
        Color32[] colors = new Color32[vertexCount];

        Color32 green = new Color32(20, 255, 30, 255);
        Color32 brown = new Color32(220, 150, 70, 255);

        for (int i = 0; i < icosohedron.m_Polygons.Count; i++)
        {
            var poly = icosohedron.m_Polygons[i];

            indices[i * 3 + 0] = i * 3 + 0;
            indices[i * 3 + 1] = i * 3 + 1;
            indices[i * 3 + 2] = i * 3 + 2;

            vertices[i * 3 + 0] = icosohedron.m_Vertices[poly.m_Vertices[0]];
            vertices[i * 3 + 1] = icosohedron.m_Vertices[poly.m_Vertices[1]];
            vertices[i * 3 + 2] = icosohedron.m_Vertices[poly.m_Vertices[2]];

            Color32 polyColor = Color32.Lerp(green, brown, Random.Range(0.0f, 1.0f));
            colors[i * 3 + 0] = polyColor;
            colors[i * 3 + 1] = polyColor;
            colors[i * 3 + 2] = polyColor;

            normals[i * 3 + 0] = icosohedron.m_Vertices[poly.m_Vertices[0]];
            normals[i * 3 + 1] = icosohedron.m_Vertices[poly.m_Vertices[1]];
            normals[i * 3 + 2] = icosohedron.m_Vertices[poly.m_Vertices[2]];
        }
        terrainMesh.vertices = vertices;
        terrainMesh.normals = normals;
        terrainMesh.colors32 = colors;

        terrainMesh.SetTriangles(indices, 0);

        MeshFilter terrainFilter = m_PlanetMesh.AddComponent<MeshFilter>();
        terrainFilter.mesh = terrainMesh;

        //return new Planet(m_PlanetMesh, icosohedron.m_Polygons);
        return m_PlanetMesh;
    }

    private static GameObject GenerateMeshSquare(Primitive cube, LibNoise.Generator.Perlin perlin)
    {
        GameObject m_PlanetMesh = new GameObject("Planet Mesh");

        MeshRenderer surfaceRenderer = m_PlanetMesh.AddComponent<MeshRenderer>();
        surfaceRenderer.material = cube.m_Material;

        Mesh terrainMesh = new Mesh();

        int vertexCount = cube.m_Polygons.Count * 6;

        int[] indices = new int[vertexCount];

        Vector3[] vertices = new Vector3[vertexCount];
        Vector3[] normals = new Vector3[vertexCount];
        Color32[] colors = new Color32[vertexCount];

        Color32 grass = new Color32(20, 255, 30, 255);
        Color32 water = new Color32(64, 164, 223, 255);
        Color32 sand = new Color32(194, 178, 128, 255);

        for (int i = 0; i < cube.m_Polygons.Count; i++)
        {
            var poly = cube.m_Polygons[i];

            cube.m_Polygons[i].indices.Add(i * 6);
            cube.m_Polygons[i].indices.Add(i * 6 + 3);

            indices[i * 6 + 0] = i * 6 + 0;
            indices[i * 6 + 1] = i * 6 + 3;
            indices[i * 6 + 2] = i * 6 + 1;

            indices[i * 6 + 3] = i * 6 + 1;
            indices[i * 6 + 4] = i * 6 + 3;
            indices[i * 6 + 5] = i * 6 + 2;

            vertices[i * 6 + 0] = cube.m_Vertices[poly.m_Vertices[0]];
            vertices[i * 6 + 1] = cube.m_Vertices[poly.m_Vertices[1]];
            vertices[i * 6 + 2] = cube.m_Vertices[poly.m_Vertices[2]];
            vertices[i * 6 + 3] = cube.m_Vertices[poly.m_Vertices[3]];

            double value = Mathf.Abs((float)perlin.GetValue(vertices[i * 6 + 0].x, vertices[i * 6 + 0].y, vertices[i * 6 + 0].z));

            // Here's where we assign each polygon a random color.
            //Color32 polyColor = Color32.Lerp(green, brown, Random.Range(0.0f, 1.0f));
            Color32 polyColor = (value >= 0.5) ? grass : water;

            if (value <= 0.5)
            {
                polyColor = water;
            }
            else if (value <= 0.55)
            {
                polyColor = sand;
            }
            else
            {
                polyColor = grass;
            }

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

        //return new Planet(m_PlanetMesh, cube.m_Polygons);
        return m_PlanetMesh;
    }

    /***
     * This simply stores our shape before being sudivided.
     */
    private class Primitive
    {
        public List<Polygon> m_Polygons;
        public List<Vector3> m_Vertices;
        public Material m_Material;

        public Primitive()
        {
            m_Polygons = new List<Polygon>();
            m_Vertices = new List<Vector3>();
        }
    }
}

/***
 * This is just our class to store each polygon in.
 * @method "Polygon(int,int,int,int)" This lets us initialize a square, simply rendered as 2 triangles.
 * @method 'Polygon(int,int,int)" This lets us initialize a triangle.
 */
public class Polygon
{
    public List<int> m_Vertices;
    public List<int> indices;

    public Polygon(int a, int b, int c, int d)
    {
        m_Vertices = new List<int>() { a, b, c, d };
        indices = new List<int>();
    }

    public Polygon(int a, int b, int c)
    {
        m_Vertices = new List<int>() { a, b, c };
        indices = new List<int>();
    }
}
