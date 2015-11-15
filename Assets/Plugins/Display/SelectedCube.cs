using UnityEngine;
using System.Collections;
using System;
using AssemblyCSharpfirstpass;
using DataObjects;

public class SelectedCube : MonoBehaviour
{
	public bool update = false;
	public bool rendered;

	MeshFilter filter;
	//MeshCollider coll;
	
	public World world;
	public WorldPos pos; //inworld
	public int mode;
	public Chunk hostChunk;
	public Vector3 targetPosition;
	BlockSelect displayBlock;
	public GameObject handleUI;
	public GameObject[] corner;

	public int depth;
	public bool mousehold = false;
	public Camera camera;
	public GameObject mainCanvas;
	public CornerHandle scriptHandle;

	// Use this for initialization
	void Start ()
	{
		GameObject tempgo = GameObject.FindWithTag ("world");
		world = tempgo.GetComponent("World") as World;

		camera = Camera.main;
		mainCanvas = GameObject.FindWithTag ("UI");

		corner = new GameObject[8];
		for(int a = 0; a < 8; a+=1) 
		{
			//this creates a circle for each corner to act as a handle to drag
			GameObject point = corner[a];
			point = Instantiate (handleUI, new Vector3 (targetPosition.x, targetPosition.y, targetPosition.z), Quaternion.identity) as GameObject;
			point.transform.SetParent(mainCanvas.transform);

			corner[a] = point;
			setcorner(point, a);

		}
			

		displayBlock = new BlockSelect();
		filter = gameObject.GetComponent<MeshFilter>();
		//coll = gameObject.GetComponent<MeshCollider>();


	}

	void setcorner(GameObject marker, int num) {

		scriptHandle = marker.GetComponent ("CornerHandle") as CornerHandle;
		WorldPos thisPos;
		thisPos.x = pos.x;
		thisPos.y = pos.y;
		thisPos.z = pos.z;

		if (num < 1) {	
			thisPos.x = pos.x - 1;	
		}
		else if (num < 2) {		}
		else if (num < 3) {	
			thisPos.z = pos.z - 1;	
		}
		else if (num < 4) {	
			thisPos.x = pos.x - 1;
			thisPos.z = pos.z - 1;
		}
		else if (num < 5) {	
			thisPos.x = pos.x - 1;
			thisPos.y = pos.y - 1;
			thisPos.z = pos.z - 1;
		}
		else if (num < 6) {		
			thisPos.y = pos.y - 1;
			thisPos.z = pos.z - 1;
		}
		else if (num < 7) {		
			thisPos.y = pos.y - 1;
		}
		else if (num < 8) {		
			thisPos.y = pos.y - 1;
			thisPos.x = pos.x - 1;
		}

		scriptHandle.assignVoxel (world, thisPos, num);

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
			pos.x = pos.x - hostChunk.pos.x;
			pos.y = pos.y - hostChunk.pos.y;
			pos.z = pos.z - hostChunk.pos.z;

			update = true;
		}

		if (update)
		{
			update = false;
			UpdateCube();
		}
		for(int a = 0; a < 8; a+=1) 
		{
			GameObject point = corner[a];
			setcorner(point, a);
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

		Mesh mesh = new Mesh();
		mesh.vertices = meshData.colVertices.ToArray();
		mesh.triangles = meshData.colTriangles.ToArray();
		mesh.RecalculateNormals();

	}
}


