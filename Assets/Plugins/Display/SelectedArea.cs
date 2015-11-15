using UnityEngine;
using System.Collections;
using AssemblyCSharpfirstpass;
using System.Collections.Generic;

public class SelectedArea : MonoBehaviour
{
	public Vector3 startCorner = new Vector3();
	public Vector3 endCorner = new Vector3();
	
	public bool snap;
	public bool update;
	public bool rendered;
	public bool mousehold = false;

	public World world;

	MeshFilter filter;
	MeshData meshMain;
	public List<Block> current_selected = new List<Block> ();

	// Use this for initialization
	void Start ()
	{
		gameObject.transform.position.Set (0, 0, 0);
		startCorner = Vector3.zero;
		endCorner.Set(10,10,10);
		update = true;
		snap = true;
		meshMain = new MeshData ();
		filter = gameObject.GetComponent<MeshFilter>();

		GameObject tempgo = GameObject.FindWithTag ("world");
		world = tempgo.GetComponent("World") as World;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (update) {
			buildMesh ();
			update = false;
		}
	}
	public void clearSelected()
	{
		for (int i = 0; current_selected.Count > 0; i++) {
			Block thisBlock = current_selected [0];
			thisBlock.isSelected = false;
			current_selected.RemoveAt (0);
		}
	}

	public void markSelected()
	{
		float sizex = Mathf.FloorToInt(endCorner.x - startCorner.x);
		float sizey = Mathf.FloorToInt(endCorner.y - startCorner.y);
		float sizez = Mathf.FloorToInt(endCorner.z - startCorner.z);
		float signx = Mathf.Sign (sizex);
		float signy = Mathf.Sign (sizey);
		float signz = Mathf.Sign (sizez);
		sizex = Mathf.Abs (sizex);
		sizey = Mathf.Abs (sizey);
		sizez = Mathf.Abs (sizez);

		WorldPos pos;
		Block current_block;

		for (int x = 0; x < sizex; x += 1)
			for (int y = 0; y < sizey; y += 1)
				for (int z = 0; z < sizez; z+= 1) {
					pos = Terra.GetBlockPos (new Vector3 (startCorner.x + (x*signx), startCorner.y + (y*signy), startCorner.z + (z*signz)));
					current_block = world.GetBlock (pos.x, pos.y, pos.z);
					current_block.isSelected = true;
					world.SetBlock(pos.x, pos.y,pos.z, current_block);
					current_selected.Add(current_block);
				}



	}

	public void setStartCorner (Vector3 point)
	{
		if(snap) startCorner.Set (Mathf.Floor (point.x) + 0.5f, Mathf.Floor (point.y) + 0.5f, Mathf.Floor (point.z) + 0.5f);
		else startCorner.Set (point.x, point.y, point.z);
		update = true;
	}

	public void setEndCorner (Vector3 point)
	{
		if(snap) endCorner.Set (Mathf.Floor (point.x) + 0.5f, Mathf.Floor (point.y) + 0.5f, Mathf.Floor (point.z) + 0.5f);
		else endCorner.Set (point.x, point.y, point.z);
		update = true;
		markSelected ();
	}

	public void buildMesh ()
	{
		Vector3 start = new Vector3 ();
		Vector3 end = new Vector3 ();

		meshMain.ClearAll ();

		if (snap) { //we are rounding to closest block
			start.Set (Mathf.Floor (startCorner.x) + 0.5f, Mathf.Floor (startCorner.y) + 0.5f, Mathf.Floor (startCorner.z) + 0.5f);
			end.Set (Mathf.Floor (endCorner.x) + 0.5f, Mathf.Floor (endCorner.y) + 0.5f, Mathf.Floor (endCorner.z) + 0.5f);
		} else {
			start = startCorner;
			end = endCorner;
		}
	
		meshMain = buildFaceUp (start, end, meshMain);
		meshMain = buildFaceDown (start, end, meshMain);
		meshMain = buildFaceNorth (start, end, meshMain);
		meshMain = buildFaceEast (start, end, meshMain);
		meshMain = buildFaceSouth (start, end, meshMain);
		meshMain = buildFaceWest (start, end, meshMain);

		RenderMesh (meshMain);
	}

	protected virtual MeshData buildFaceUp(Vector3 start, Vector3 end, MeshData meshData) 
	{
		meshData.AddVertex (new Vector3(end.x, start.y, start.z));
		meshData.AddVertex (new Vector3(start.x, start.y, start.z));
		meshData.AddVertex (new Vector3(start.x, start.y, end.z));
		meshData.AddVertex (new Vector3(end.x, start.y, end.z));
		meshData.AddQuadTriangles();
		meshData.uv.AddRange (FlatUVs ());
		return meshData;

	}
	protected virtual MeshData buildFaceDown(Vector3 start, Vector3 end, MeshData meshData) 
	{
		meshData.AddVertex (new Vector3(end.x, end.y, end.z));
		meshData.AddVertex (new Vector3(start.x, end.y, end.z));
		meshData.AddVertex (new Vector3(start.x, end.y, start.z));
		meshData.AddVertex (new Vector3(end.x, end.y, start.z));
		meshData.AddQuadTriangles();
		meshData.uv.AddRange (FlatUVs ());
		return meshData;
		
	}
	protected virtual MeshData buildFaceNorth(Vector3 start, Vector3 end, MeshData meshData) 
	{
		return meshData;

//		meshData.AddVertex (end.x, end.y, end.z);
//		meshData.AddVertex (start.x, end.y, end.z);
//		meshData.AddVertex (start.x, end.y, start.z);
//		meshData.AddVertex (end.x, end.y, start.z);
		//		meshData.AddQuadTriangles();
//		meshData.uv.AddRange (FlatUVs ());
//		return meshData;
		
	}
		
	protected virtual MeshData buildFaceEast(Vector3 start, Vector3 end, MeshData meshData) 
	{
		meshData.AddVertex (new Vector3(start.x, start.y, end.z));
		meshData.AddVertex (new Vector3(start.x, start.y, start.z));
		meshData.AddVertex (new Vector3(start.x, end.y, start.z));
		meshData.AddVertex (new Vector3(start.x, end.y, end.z));
		meshData.AddQuadTriangles();
		meshData.uv.AddRange (FlatUVs ());
		return meshData;
		
	}
		
	protected virtual MeshData buildFaceSouth(Vector3 start, Vector3 end, MeshData meshData) 
	{
		meshData.AddVertex (new Vector3(end.x, start.y, end.z));
		meshData.AddVertex (new Vector3(start.x, start.y, end.z));
		meshData.AddVertex (new Vector3(start.x, end.y, end.z));
		meshData.AddVertex (new Vector3(end.x, end.y, end.z));
		meshData.AddQuadTriangles();
		meshData.uv.AddRange (FlatUVs ());
		return meshData;
		
	}
	protected virtual MeshData buildFaceWest(Vector3 start, Vector3 end, MeshData meshData) 
	{
		meshData.AddVertex (new Vector3(end.x, start.y, start.z));
		meshData.AddVertex (new Vector3(end.x, start.y, end.z));
		meshData.AddVertex (new Vector3(end.x, end.y, end.z));
		meshData.AddVertex (new Vector3(end.x, end.y, start.z));
		meshData.AddQuadTriangles();
		meshData.uv.AddRange (FlatUVs ());
		return meshData;
		
	}

	Vector2[] FlatUVs()
	{
		Vector2[] UVs = new Vector2[4];
		
		UVs[0] = new Vector2(0, 0);
		
		UVs[1] = new Vector2(0, 1);
		
		UVs[2] = new Vector2(1, 1);
		
		UVs[3] = new Vector2(1, 0);
		
		return UVs;
	}

	void RenderMesh(MeshData meshData)
	{
		filter.mesh.Clear();
		filter.mesh.vertices = meshData.vertices.ToArray();
		filter.mesh.triangles = meshData.triangles.ToArray();
		
		filter.mesh.uv = meshData.uv.ToArray();
		filter.mesh.RecalculateNormals();
		
	}
}

