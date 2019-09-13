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
    #region Terrain Types
    // These won't be a perminant solution.
    private static Color32 WATER = new Color32(64, 164, 223, 255);
    private static Color32 SAND = new Color32(194, 178, 128, 255);
    private static Color32 GRASS = new Color32(20, 255, 30, 255);
    #endregion

    #region Public Methods
    /***
     * This function generates and return a Planet object.
     * 
     * @param "subdivide" this is how many time we subdivide the original shape.
     * @param "type" decided the original shape we are subdividing, default is 0. (0 = cube, 1, = icosohedron)
     */
    static public GameObject GeneratePlanet(int seed, int type = 0)
    {
        #region Generate Random/Noise Based on Seed
        LibNoise.Generator.Perlin perlin = new LibNoise.Generator.Perlin();
        Random.InitState(seed);
        perlin.Seed = seed;
        #endregion

        // TODO I want this to be able to generate a subdivide 6 planet, but it can't.
        #region Generate Subdivide
        float subdivide_seed = Random.value;
        int subdivide = 5;
        if (subdivide <= 0.3) { subdivide = 4; }
        else if (subdivide <= 0.8) { subdivide = 5; }
        Debug.Log("subdivide: " + subdivide);
        #endregion

        #region Initialize Primitive
        Primitive primitive = null;
        if (type == 0)
        {
            primitive = InitAsCube();
            primitive = SubdivideCube(primitive, subdivide);
            
        }
        else if (type == 1)
        {
            primitive = InitAsIcosohedron();
            primitive = SubdivideIcosohedron(primitive, subdivide);
        }
        else
        { return null; }
        #endregion

        #region Create GameObject
        GameObject planet = CreateGameObject(primitive, perlin);
        #endregion

        #region Generate Axis Tilt
        int sign = (Random.value >= 0.5) ? 1 : -1;
        planet.GetComponent<Planet>().SetAxis(sign * Random.value * 10);
        #endregion

        return planet;
    }
    #endregion

    #region Utility Methods

    #region Initialize
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
    #endregion

    #region Subdivide
    private static Primitive SubdivideIcosohedron(Primitive icosohedron, int recursions)
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

                int ab = GetMidPointIndex(midPointCache, a, b, icosohedron);
                int bc = GetMidPointIndex(midPointCache, b, c, icosohedron);
                int ca = GetMidPointIndex(midPointCache, c, a, icosohedron);

                newPolys.Add(new Polygon(a, ab, ca));
                newPolys.Add(new Polygon(b, bc, ab));
                newPolys.Add(new Polygon(c, ca, bc));
                newPolys.Add(new Polygon(ab, bc, ca));
            }
            icosohedron.m_Polygons = newPolys;
        }

        return icosohedron;
    }

    private static Primitive SubdivideCube(Primitive cube, int recursions)
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

                int ab = GetMidPointIndex(midPointCache, a, b, cube);
                int bc = GetMidPointIndex(midPointCache, b, c, cube);
                int cd = GetMidPointIndex(midPointCache, c, d, cube);
                int da = GetMidPointIndex(midPointCache, d, a, cube);
                int abcd = GetMidPointIndex(midPointCache, ab, cd, cube);

                newPolys.Add(new Polygon(a, ab, abcd, da));
                newPolys.Add(new Polygon(ab, b, bc, abcd));
                newPolys.Add(new Polygon(abcd, bc, c, cd));
                newPolys.Add(new Polygon(da, abcd, cd, d));
            }
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
    #endregion

    #region Create Mesh
    private static GameObject CreateGameObject(Primitive primitive, LibNoise.Generator.Perlin perlin)
    {
        #region Create Planet Object
        GameObject planet = new GameObject("Planet");
        planet.AddComponent<Planet>();
        planet.GetComponent<Planet>().m_Polygons = primitive.m_Polygons;
        planet.GetComponent<Planet>().Perlin = perlin;

        MeshCollider meshCollider = planet.AddComponent<MeshCollider>();
        MeshFilter meshFilter = planet.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = planet.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Particles/Standard Surface"));
        #endregion

        #region Generate Terrain
        List<float> p_TerrainHeight = new List<float>();
        List<Color32> p_TerrainType = new List<Color32>();
        GenerateTerrain(p_TerrainHeight, p_TerrainType);
        planet.GetComponent<Planet>().p_TerrainHeight = p_TerrainHeight;
        planet.GetComponent<Planet>().p_TerrainType = p_TerrainType;
        #endregion

        #region Local Variables
        int vertexCount = (primitive.m_Polygons[0].type == 0) ? primitive.m_Polygons.Count * 6 : primitive.m_Polygons.Count * 3;
        int[] indices = new int[vertexCount];
        Vector3[] vertices = new Vector3[vertexCount];
        Color32[] colors = new Color32[vertexCount];
        #endregion

        #region Create Mesh Data
        for (int i = 0; i < primitive.m_Polygons.Count; i++)
        {
            #region Get/Set Polygon Data
            var poly = primitive.m_Polygons[i];

            int index = i;
            if (poly.type == 0)
            {
                index = index * 6;
                primitive.m_Polygons[i].indices.Add(index);
                primitive.m_Polygons[i].indices.Add(index + 3);

                primitive.m_Polygons[i].t_Vertices.Add(index + 0);
                primitive.m_Polygons[i].t_Vertices.Add(index + 2);
                primitive.m_Polygons[i].t_Vertices.Add(index + 1);
                primitive.m_Polygons[i].t_Vertices.Add(index + 4);
            }
            else
            {
                index = index * 3;
                primitive.m_Polygons[i].indices.Add(index);

                primitive.m_Polygons[i].t_Vertices.Add(index + 0);
                primitive.m_Polygons[i].t_Vertices.Add(index + 2);
                primitive.m_Polygons[i].t_Vertices.Add(index + 1);
            }
            #endregion

            #region Set Mesh Indices
            indices[index + 0] = index + 0;
            indices[index + 1] = index + 1;
            indices[index + 2] = index + 2;

            if (poly.type == 0)
            {
                indices[index + 3] = index + 3;
                indices[index + 4] = index + 4;
                indices[index + 5] = index + 5;
            }
            #endregion

            #region Set Mesh Vertices
            vertices[index + 0] = primitive.m_Vertices[poly.m_Vertices[0]];
            vertices[index + 1] = primitive.m_Vertices[poly.m_Vertices[2]];
            vertices[index + 2] = primitive.m_Vertices[poly.m_Vertices[1]];
            if (poly.type == 0)
            {
                vertices[index + 3] = primitive.m_Vertices[poly.m_Vertices[0]];
                vertices[index + 4] = primitive.m_Vertices[poly.m_Vertices[3]];
                vertices[index + 5] = primitive.m_Vertices[poly.m_Vertices[2]];
            }
            #endregion

            #region Get Polgon Color (Legacy)
            /*
            double value0 = Mathf.Abs((float)perlin.GetValue(vertices[index].x, vertices[index].y, vertices[index].z));
            double value1 = Mathf.Abs((float)perlin.GetValue(vertices[index + 1].x, vertices[index + 1].y, vertices[index + 1].z));
            double value2 = Mathf.Abs((float)perlin.GetValue(vertices[index + 2].x, vertices[index + 2].y, vertices[index + 2].z));
            double value3 = Mathf.Abs((float)perlin.GetValue(vertices[index + 3].x, vertices[index + 3].y, vertices[index + 3].z));
            double value = (value1 + value2 + value3 + value0) / 4;
            Color32 polyColor = p_TerrainType[p_TerrainType.Count - 1];
            for (int range = 0; range < p_TerrainHeight.Count; range++)
            {
                if (value <= p_TerrainHeight[range])
                {
                    polyColor = p_TerrainType[range];
                    break;
                }
            }
            */
            #endregion

            #region Set Colors (Legacy)
            /*
            colors[index] = polyColor;
            colors[index + 1] = polyColor;
            colors[index + 2] = polyColor;

            if (poly.type == 0)
            {
                colors[index + 3] = polyColor;
                colors[index + 4] = polyColor;
                colors[index + 5] = polyColor;
            }
            */
            #endregion

            #region Set Color
            int j_max = (primitive.m_Polygons[i].type == 0) ? 6 : 4;
            for (int j = 0; j < j_max; j++)
            {
                double value = Mathf.Abs((float)perlin.GetValue(vertices[index + j].x, vertices[index + j].y, vertices[index + j].z));
                colors[index + j] = p_TerrainType[p_TerrainType.Count - 1];
                for (int range = 0; range < p_TerrainHeight.Count; range++)
                {
                    if (value <= p_TerrainHeight[range])
                    {
                        colors[index + j] = p_TerrainType[range];
                        break;
                    }
                }
            }
            #endregion

        }
        #endregion

        #region Create Mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.colors32 = colors;
        mesh.SetTriangles(indices, 0);
        meshFilter.mesh = mesh;
        meshFilter.mesh.RecalculateNormals();
        meshCollider.sharedMesh = mesh;
        #endregion

        return planet;
    }

    private static void GenerateTerrain(List<float> p_TerrainHeight, List<Color32> p_TerrainType)
    {
        p_TerrainHeight.Add(0.5f);
        p_TerrainHeight.Add(0.55f);

        p_TerrainType.Add(WATER);
        p_TerrainType.Add(SAND);
        p_TerrainType.Add(GRASS);
    }
    #endregion

    #region Generate Planet Data
    private static float getAxis(LibNoise.Generator.Perlin perlin)
    {
        return (float) perlin.GetValue(0, 0, 1) * 10;
    }
    #endregion

    #endregion

    #region Utility Classes
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
    #endregion
}

#region Polygon Class
/***
 * This is just our class to store each polygon in.
 * @method "Polygon(int,int,int,int)" This lets us initialize a square, simply rendered as 2 triangles.
 * @method 'Polygon(int,int,int)" This lets us initialize a triangle.
 */
public class Polygon
{
    public List<int> m_Vertices;
    public List<int> indices;
    public List<int> t_Vertices;
    public int type;

    public Polygon(int a, int b, int c, int d)
    {
        m_Vertices = new List<int>() { a, b, c, d };
        indices = new List<int>();
        t_Vertices = new List<int>();
        type = 0;
    }

    public Polygon(int a, int b, int c)
    {
        m_Vertices = new List<int>() { a, b, c };
        indices = new List<int>();
        type = 1;
    }
}
#endregion
