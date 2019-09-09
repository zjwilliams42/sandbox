/***
* This file is sort of a combo from my code and the code
* from the below url.
* https://medium.com/@peter_winslow/creating-procedural-planets-in-unity-part-1-df83ecb12e91
*
* The goal here is to procedurally create planets.
* Zachary Williams
* Created: 9/8/2019
*/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/***
* This is the class that will be each individual polygon
* of our planet. The tutorial has this as a triangle, we
* will eventually make it a square.
*
* TODO make this a square.
*/
public class Polygon
{
	public List<int> m_Vertices;

	public Polygon(int a, int b, int c)
	{
		m_Vertices = new List<int> () {a, b, c};
	}
}

public class PlanetFactory : MonoBehaviour
{

	public List<Polygon> m_Polygons;
	public List<Vector3> m_Vertices;
	public GameObject m_PlanetMesh;
	public Material m_Material;

  // Start is called before the first frame update
  void Start()
  {
		InitAsIcosahedron();
		GenerateMesh(); 
  }

  // Update is called once per frame
  void Update()
  {
       
  }

	public void InitAsIcosahedron ()
	{
		m_Polygons = new List<Polygon> ();
		m_Vertices = new List<Vector3> ();

		float t = (1.0f + Mathf.Sqrt (5.0f)) / 2.0f;
	
		m_Vertices.Add (new Vector3 (-1, t, 0).normalized);
		m_Vertices.Add (new Vector3 (1, t, 0).normalized);
		m_Vertices.Add (new Vector3 (-1, -t, 0).normalized);
		m_Vertices.Add (new Vector3 (1, -t, 0).normalized);
		m_Vertices.Add (new Vector3 (0, -1, t).normalized);
		m_Vertices.Add (new Vector3 (0, 1, t).normalized);
		m_Vertices.Add (new Vector3 (0, -1, -t).normalized);
		m_Vertices.Add (new Vector3 (0, 1, -t).normalized);
		m_Vertices.Add (new Vector3 (t, 0, -1).normalized);
		m_Vertices.Add (new Vector3 (t, 0, 1).normalized);
		m_Vertices.Add (new Vector3 (-t, 0, -1).normalized);
		m_Vertices.Add (new Vector3 (-t, 0, 1).normalized);

    m_Polygons.Add (new Polygon(0, 11, 5));
    m_Polygons.Add (new Polygon(0, 5, 1));
    m_Polygons.Add (new Polygon(0, 1, 7));
    m_Polygons.Add (new Polygon(0, 7, 10));
    m_Polygons.Add (new Polygon(0, 10, 11));
    m_Polygons.Add (new Polygon(1, 5, 9));
    m_Polygons.Add (new Polygon(5, 11, 4));
    m_Polygons.Add (new Polygon(11, 10, 2));
    m_Polygons.Add (new Polygon(10, 7, 6));
    m_Polygons.Add (new Polygon(7, 1, 8));
    m_Polygons.Add (new Polygon(3, 9, 4));
    m_Polygons.Add (new Polygon(3, 4, 2));
    m_Polygons.Add (new Polygon(3, 2, 6));
    m_Polygons.Add (new Polygon(3, 6, 8));
    m_Polygons.Add (new Polygon(3, 8, 9));
    m_Polygons.Add (new Polygon(4, 9, 5));
    m_Polygons.Add (new Polygon(2, 4, 11));
    m_Polygons.Add (new Polygon(6, 2, 10));
    m_Polygons.Add (new Polygon(8, 6, 7));
    m_Polygons.Add (new Polygon(9, 8, 1));
	}

	public void GenerateMesh ()
	{
		if (m_PlanetMesh)
		{
			Destroy(m_PlanetMesh);
		}
	
		m_PlanetMesh = new GameObject("Planet Mesh");
	
		MeshRenderer surfaceRenderer = m_PlanetMesh.AddComponent<MeshRenderer> ();
		surfaceRenderer.material = m_Material;
		//GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //Renderer surfaceRenderer = cube.GetComponent<Renderer> ();
		//surfaceRenderer.material = new Material(Shader.Find("Specular"));	

		Mesh terrainMesh = new Mesh();
	
		int vertexCount = m_Polygons.Count * 3;
	
		int[] indices = new int[vertexCount];
	
		Vector3[] vertices = new Vector3[vertexCount];
		Vector3[] normals = new Vector3[vertexCount];
		Color32[] colors =  new Color32[vertexCount];
	
		Color32 green = new Color32(20, 255, 30, 255);
		Color32 brown = new Color32(220, 150, 70, 255);
	
		for (int i = 0; i < m_Polygons.Count; i++)
	  {
	    var poly = m_Polygons[i];
			indices[i * 3 + 0] = i * 3 + 0;
	    indices[i * 3 + 1] = i * 3 + 1;
	    indices[i * 3 + 2] = i * 3 + 2;
			vertices[i * 3 + 0] = m_Vertices[poly.m_Vertices[0]];
	    vertices[i * 3 + 1] = m_Vertices[poly.m_Vertices[1]];
	    vertices[i * 3 + 2] = m_Vertices[poly.m_Vertices[2]];
	    Color32 polyColor = Color32.Lerp(green, brown, Random.Range(0.0f, 1.0f)); // Here's where we assign each polygon a random color.
			colors[i * 3 + 0] = polyColor;
	    colors[i * 3 + 1] = polyColor;
	    colors[i * 3 + 2] = polyColor;
	
			// For now our planet is still perfectly spherical, so
	    // so the normal of each vertex is just like the vertex
	    // itself: pointing away from the origin.    normals[i * 3 + 0] = m_Vertices[poly.m_Vertices[0]];
	    normals[i * 3 + 1] = m_Vertices[poly.m_Vertices[1]];
	    normals[i * 3 + 2] = m_Vertices[poly.m_Vertices[2]];
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
