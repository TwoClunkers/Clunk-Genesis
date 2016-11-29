using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshData
{
    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();
	public List<int> blendtriangles = new List<int>();
    public List<Vector2> uv = new List<Vector2>();
	public List<Vector3> normals = new List<Vector3> ();

	public List<Vector2> uv1 = new List<Vector2>();
	public List<Color32> colors = new List<Color32> ();

    public List<Vector3> colVertices = new List<Vector3>();
    public List<int> colTriangles = new List<int>();

    public bool useRenderDataForCol;

    public MeshData() { }

	public void ClearAll() 
	{
		vertices.Clear();
		triangles.Clear();
		blendtriangles.Clear ();
		uv.Clear();
		uv1.Clear ();
		normals.Clear ();
		colors.Clear ();
		colVertices.Clear();
		colTriangles.Clear();
	}

    public void AddQuadTriangles()
    {
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 2);

        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 1);

        if (useRenderDataForCol)
        {
            colTriangles.Add(colVertices.Count - 4);
            colTriangles.Add(colVertices.Count - 3);
            colTriangles.Add(colVertices.Count - 2);
            colTriangles.Add(colVertices.Count - 4);
            colTriangles.Add(colVertices.Count - 2);
            colTriangles.Add(colVertices.Count - 1);
        }
    }

    public void AddVertex(Vector3 vertex, Vector3 normal)
    {
        vertices.Add(vertex);
		normals.Add (normal);

        if (useRenderDataForCol)
        {
            colVertices.Add(vertex);
        }

    }

	public void AddVertex(Vector3 vertex) //for backward compatability :)
	{
		vertices.Add(vertex);

		if (useRenderDataForCol)
		{
			colVertices.Add(vertex);
		}

	}

    public void AddTriangle()
    {
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);

        if (useRenderDataForCol)
        {
            //colTriangles.Add(tri - (vertices.Count - colVertices.Count));
			colTriangles.Add(colVertices.Count - 3);
			colTriangles.Add(colVertices.Count - 2);
			colTriangles.Add(colVertices.Count - 1);
        }
    }

	public void AddBlendTriangle()
	{
		blendtriangles.Add(vertices.Count - 3);
		blendtriangles.Add(vertices.Count - 2);
		blendtriangles.Add(vertices.Count - 1);
	}

	public void AddBlendQuad()
	{
		blendtriangles.Add(vertices.Count - 6);
		blendtriangles.Add(vertices.Count - 5);
		blendtriangles.Add(vertices.Count - 4);

		blendtriangles.Add(vertices.Count - 3);
		blendtriangles.Add(vertices.Count - 2);
		blendtriangles.Add(vertices.Count - 1);
	}	
}