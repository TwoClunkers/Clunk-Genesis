using UnityEngine;
using System.Collections;
using System;

public class SelectedCube : MonoBehaviour
{
	public bool update = false;
	public bool rendered;

	MeshFilter filter;
	MeshCollider coll;
	
	public World world;
	public WorldPos pos; //inworld
	public int mode;
	public Chunk hostChunk;
	public Vector3 targetPosition;
	BlockSelect displayBlock;

	public int depth;
	public bool mousehold = false;

	// Use this for initialization
	void Start ()
	{
		world = GameObject.FindWithTag("world").GetComponent("World") as World;

		displayBlock = new BlockSelect();
		filter = gameObject.GetComponent<MeshFilter>();
		coll = gameObject.GetComponent<MeshCollider>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (mousehold) {
			if (!Input.GetMouseButton (0))
				mousehold = false;
		}
		if ((!mousehold) && Input.GetMouseButton (0)) {
			mousehold = true;
			if (Input.GetKey (KeyCode.LeftShift)) {
				depth = 3;
			} else
				depth = 2;

			//create a target where we are pointing
//			Vector3 mouse = Input.mousePosition;
//			mouse.z = depth - (Camera.main.transform.position.z)-1;
//
//			targetPosition = Camera.main.ScreenToWorldPoint (mouse);
//			targetPosition.z = depth-1;

			hostChunk = world.GetChunk(pos.x,pos.y,pos.z);
//			pos.x = pos.x - hostChunk.pos.x;
//			pos.y = pos.y - hostChunk.pos.y;
//			pos.z = pos.z - hostChunk.pos.z;

			update = true;
		}

		if (update)
		{
			update = false;
			UpdateCube();
		}

	}

	void UpdateCube()
	{
		rendered = true;
		MeshData meshData = new MeshData();

		meshData = displayBlock.Blockdata(hostChunk, pos.x,pos.y,pos.z, meshData);
	
		RenderMesh(meshData);
		Vector3 newpos = new Vector3 (hostChunk.pos.x, hostChunk.pos.y, hostChunk.pos.z);
		transform.position = newpos;
	}

	void RenderMesh(MeshData meshData)
	{
		filter.mesh.Clear();
		filter.mesh.vertices = meshData.vertices.ToArray();
		filter.mesh.triangles = meshData.triangles.ToArray();
		
		filter.mesh.uv = meshData.uv.ToArray();
		filter.mesh.RecalculateNormals();
		Debug.Log ("hmm");
		coll.sharedMesh = null;
		Mesh mesh = new Mesh();
		mesh.vertices = meshData.colVertices.ToArray();
		mesh.triangles = meshData.colTriangles.ToArray();
		mesh.RecalculateNormals();
		
		coll.sharedMesh = mesh;
	}
}


