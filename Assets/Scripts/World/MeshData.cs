using UnityEngine;
using System.Collections.Generic;

public class MeshData
{
	public List<Vector3> vertices = new List<Vector3>();
	public List<SubmeshTriangles> triangles = new List<SubmeshTriangles>();

	public List<Vector2> uv = new List<Vector2>();

	public List<Vector3> colVertices = new List<Vector3>();
	public List<int> colTriangles = new List<int>();

	public bool useRenderDataForCollision;

	public MeshData()
    {
        // Submesh 0 - all
        triangles.Add(new SubmeshTriangles());
        // Submesh 1 - second material
        triangles.Add(new SubmeshTriangles()); 
	}

	public void AddQuadTriangles(int submesh)
	{
		triangles[submesh].Add(vertices.Count - 4);
		triangles[submesh].Add(vertices.Count - 3);
		triangles[submesh].Add(vertices.Count - 2);
		
		triangles[submesh].Add(vertices.Count - 4);
		triangles[submesh].Add(vertices.Count - 2);
		triangles[submesh].Add(vertices.Count - 1);

        // If true we use the same data for rendering and for the collision mesh
        if (useRenderDataForCollision) {
			colTriangles.Add(colVertices.Count - 4);
			colTriangles.Add(colVertices.Count - 3);
			colTriangles.Add(colVertices.Count - 2);

			colTriangles.Add(colVertices.Count - 4);
			colTriangles.Add(colVertices.Count - 2);
			colTriangles.Add(colVertices.Count - 1);
		}
	}

	public void AddVertex(Vector3 vertex)
    {
		vertices.Add(vertex);

		if(useRenderDataForCollision) {
			colVertices.Add(vertex);
		}
	}

	public void AddTriangle(int tri, int submesh)
    {
		triangles[submesh].Add(tri);

		if(useRenderDataForCollision) {
			colTriangles.Add(tri - (vertices.Count - colVertices.Count));
		}
	}

	public class SubmeshTriangles
    {
		public List<int> triangles = new List<int>();

		public void Add(int i)
        {
			triangles.Add(i);
		}
	}
}
