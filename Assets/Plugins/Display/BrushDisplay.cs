using UnityEngine;
using System.Collections;
using System;
using DataObjects;

public class BrushDisplay : MonoBehaviour
{
	public bool update = false;
	public bool rendered;
	MeshFilter filter;

	public World world;
	public WorldPos pos; //inworld
	public int mode;
	public Chunk hostChunk;
	public Vector3 targetPosition;
	BlockSelect displayBlock;

	public int depth;
	public bool mousehold = false;
	public Camera camera;
	public GameObject mainCanvas;

	// Use this for initialization
	void Start ()
	{
		GameObject tempgo = GameObject.FindWithTag ("world");
		world = tempgo.GetComponent("World") as World;

		camera = Camera.main;
		mainCanvas = GameObject.FindWithTag ("UI");

		displayBlock = new BlockSelect();
		filter = gameObject.GetComponent<MeshFilter>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (mousehold) {
			if (!Input.GetMouseButton (0))
				mousehold = false;
		}
		if ((!mousehold)) { //&& Input.GetMouseButton (0)) {
			//mousehold = true;
			if (Input.GetKey (KeyCode.LeftShift)) {
				depth = 2;
			} else
				depth = 1;

			//create a target where we are pointing
			Vector3 mouse = Input.mousePosition;
			mouse.z = depth; // - (Camera.main.transform.position.z)-1;

			targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,depth-(Camera.main.transform.position.z)));

			pos = new WorldPos (targetPosition);

			hostChunk = world.GetChunk(pos.x,pos.y,pos.z);
			if (hostChunk) {
				pos.x = pos.x - hostChunk.pos.x;
				pos.y = pos.y - hostChunk.pos.y;
				pos.z = pos.z - hostChunk.pos.z;
			}
			update = true;
		}

		if (update)
		{
			update = false;
			UpdateBrush();
		}
	}

	void UpdateBrush()
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

		Mesh mesh = new Mesh();
		mesh.vertices = meshData.colVertices.ToArray();
		mesh.triangles = meshData.colTriangles.ToArray();
		mesh.RecalculateNormals();

	}

}

