using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Block
{
    public enum Direction { north, east, south, west, up, down, topwest, topeast };

    public struct Tile { public int x; public int y;}
    public float tileSize = 0.03125f; //one 32th of the pic
    public bool changed = true;
	public bool isSelected = false;
	public int material = 1;
	public int varientx = 0;
	public int varienty = 0;
	public int varientz = 0;
	public float damage = 30;
	public float offx;
	public float offy;
	public float offz;
	public uint faces = 0;



    //Base block constructor
    public Block()
    {
		offx = 0.5f;
		offy = 0.5f;
		offz = 0.5f;
    }
	public virtual void cloneBlock (Block target)
	{
		changed = true;
		
		material = target.material;
		varientx = target.varientx;
		varienty = target.varienty;
		varientz = target.varientz;
		damage = target.damage;
		offx = target.offx;
		offy = target.offy;
		offz = target.offz;

	}
	public virtual Tile TexturePosition(Direction direction)
	{
		Tile tile = new Tile();
		int xregion = 0;
		int yregion = 0;

		if (material < 49) { //first 40 materials have medium maps
			xregion = Mathf.FloorToInt((material-1)/8)*4;
			yregion = ((material-1) % 8)* 4;
			varientx = (varientx % 3);
			varienty = (varienty % 3);
			varientz = (varientz % 3);
		} else if (material < 57) { //next 32 matirials are rectangles
			if (material < 33) { //first row of materials
				xregion = 20;
				yregion = (material - 17) * 2;
			} else { //second 16 materials
				xregion = 24;
				yregion = (material - 33) * 2;
			}
			varientx = (varienty+varientx+varientz) % 3;
			varienty = 0;
			varientz = 0;
		}

		switch (direction)
		{
			case Direction.south:
				tile.x = xregion + ((varientx) % 3);
				tile.y = yregion + ((varienty) % 3);
				return tile;
			case Direction.down:
				tile.x = xregion + ((varientx) % 3);
				tile.y = yregion + ((varientz) % 3);
				return tile;
			case Direction.up:
				tile.x = xregion + ((varientx) % 3);
				tile.y = yregion + ((varientz) % 3);
				return tile;
			case Direction.east:
				tile.x = xregion + ((varientz) % 3);
				tile.y = yregion + ((varienty) % 3);
				return tile;
			case Direction.west:
				tile.x = xregion + ((varientz) % 3);
				tile.y = yregion + ((varienty) % 3);
				return tile;
			case Direction.topeast:
				tile.x = xregion + ((varientx) % 3);
				tile.y = yregion + ((varientz) % 3);
				return tile;
			case Direction.topwest:
				tile.x = xregion + ((varientx) % 3);
				tile.y = yregion + ((varientz) % 3);
				return tile;
		}

		
		return tile;
	}
	
	public virtual Vector2[] FaceUVs(Direction direction, Vector2 point0, Vector2 point1, Vector2 point2, Vector2 point3)
	{
		Vector2[] UVs = new Vector2[4];
		Tile tilePos = TexturePosition(direction); //this is the start of the texture for our material type
		
		
		if (direction == Direction.up) {
			UVs[0] = new Vector2(tileSize * tilePos.x + tileSize*point0.x,
				tileSize * tilePos.y + tileSize*point0.y + tileSize);

			UVs[1] = new Vector2(tileSize * tilePos.x + tileSize*point1.x + tileSize,
				tileSize * tilePos.y + tileSize*point1.y + tileSize);

			UVs[2] = new Vector2(tileSize * tilePos.x + tileSize*point2.x + tileSize,
				tileSize * tilePos.y + tileSize*point2.y);

			UVs[3] = new Vector2(tileSize * tilePos.x + tileSize*point3.x,
				tileSize * tilePos.y + tileSize*point3.y);

			return UVs;
		}
		UVs[0] = new Vector2(tileSize * tilePos.x + tileSize*point0.x,
		                     tileSize * tilePos.y + tileSize*point0.y + tileSize);
		
		UVs[1] = new Vector2(tileSize * tilePos.x + tileSize*point1.x + tileSize,
		                     tileSize * tilePos.y + tileSize*point1.y + tileSize);
		
		UVs[2] = new Vector2(tileSize * tilePos.x + tileSize*point2.x + tileSize,
		                     tileSize * tilePos.y + tileSize*point2.y);
		
		UVs[3] = new Vector2(tileSize * tilePos.x + tileSize*point3.x,
		                     tileSize * tilePos.y + tileSize*point3.y);
		
		return UVs;
	}

	public virtual Vector2[] SplitFaceUVs( Vector2[] faceUVs )
	{
		//take 4 points and split them into 6 to make two triangles
		Vector2[] newUVs = new Vector2[6];

		newUVs [0] = faceUVs [0];
		newUVs [1] = faceUVs [1];
		newUVs [2] = faceUVs [3];
		newUVs [3] = faceUVs [1];
		newUVs [4] = faceUVs [2];
		newUVs [5] = faceUVs [3];

		return newUVs;
	}

	public virtual Vector2[] FaceUV1 (Direction direction, Vector2 point0, Vector2 point1, Vector2 point2, Vector2 point3)
	{
		Vector2[] UVs = new Vector2[4];
		Tile tilePos = TexturePosition(direction); //this is the start of the texture for our material type
		//we are going to have to find a different way to sort this mess out...



		UVs[0] = new Vector2(tileSize * tilePos.x + tileSize*point1.x + tileSize,
			tileSize * tilePos.y + tileSize*point1.y + tileSize);
		UVs[1] = new Vector2(tileSize * tilePos.x + tileSize*point2.x + tileSize,
			tileSize * tilePos.y + tileSize*point2.y);
		UVs[2] = new Vector2(tileSize * tilePos.x + tileSize*point3.x,
			tileSize * tilePos.y + tileSize*point3.y);
		UVs[3] = new Vector2(tileSize * tilePos.x + tileSize*point0.x,
			tileSize * tilePos.y + tileSize*point0.y + tileSize);

		return UVs;
	}

	public virtual Boolean DamageBlock (WorldPos pos, float amount, Vector3 direction)
	{
		if (material < 1)
			return false;

		damage = Mathf.Max(0, damage-amount);
		if (damage < 1) {

			return true;
		} else {
			//offx = direction.x;
			//offy = direction.y;
			//offz = direction.z;
		}
		return false;
	}

	public virtual void Resolve
		(Chunk chunk, int x, int y, int z)
	{

	}

	public virtual Vector3[] GetNormMask( Chunk chunk, int x, int y, int z)
	{
		//I have broken this test out of the Blockdata routine as I think we need to know
		//the exposed faces for later normal calculations.
		//This way we won't have to call 6 times again!
		Vector3[] normMasks = new Vector3[8];
		faces = 0;

		for (int a = 0; a < 8; a++) {
			normMasks [a] = new Vector3 (1.0f, 1.0f, 1.0f);
		}

		if (!chunk.GetBlock (x, y + 1, z).IsSolid (Direction.down)) {
			faces += 1;
		} else {
			normMasks [0].y = 0.0f;
			normMasks [1].y = 0.0f;
			normMasks [2].y = 0.0f;
			normMasks [3].y = 0.0f;
		}

		if (!chunk.GetBlock (x, y - 1, z).IsSolid (Direction.up)) {
			faces += 2;
		} else {
			normMasks [4].y = 0.0f;
			normMasks [5].y = 0.0f;
			normMasks [6].y = 0.0f;
			normMasks [7].y = 0.0f;
		}

		if (!chunk.GetBlock (x, y, z + 1).IsSolid (Direction.south)) {
			faces += 4;
		} else {
			normMasks [0].z = 0.0f;
			normMasks [1].z = 0.0f;
			normMasks [6].z = 0.0f;
			normMasks [7].z = 0.0f;
		}

		if (!chunk.GetBlock (x, y, z - 1).IsSolid (Direction.north)) {
			faces += 8;
		} else {
			normMasks [2].z = 0.0f;
			normMasks [3].z = 0.0f;
			normMasks [4].z = 0.0f;
			normMasks [5].z = 0.0f;
		}

		if (!chunk.GetBlock (x + 1, y, z).IsSolid (Direction.west)) {
			faces += 16;
		} else {
			normMasks [1].x = 0.0f;
			normMasks [2].x = 0.0f;
			normMasks [5].x = 0.0f;
			normMasks [6].x = 0.0f;
		}

		if (!chunk.GetBlock (x - 1, y, z).IsSolid (Direction.east)) {
			faces += 32;
		} else {
			normMasks [0].x = 0.0f;
			normMasks [3].x = 0.0f;
			normMasks [4].x = 0.0f;
			normMasks [7].x = 0.0f;
		}

		return normMasks;
	}

    public virtual MeshData Blockdata
     (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
		if(material < 1) return meshData;

		Vector3[] normMasks = GetNormMask (chunk, x, y, z);


        meshData.useRenderDataForCol = true;

		if ((faces&1) != 0)
		{
			meshData = FaceDataUp(chunk, x, y, z, meshData, normMasks);
		}

		if ((faces&2) != 0)
		{
			meshData = FaceDataDown(chunk, x, y, z, meshData, normMasks);
		}

		if ((faces&4) != 0)
		{
			meshData = FaceDataNorth(chunk, x, y, z, meshData, normMasks);
		}

		if ((faces&8) != 0)
		{
			meshData = FaceDataSouth(chunk, x, y, z, meshData, normMasks);
		}

		if ((faces&16) != 0)
		{
			meshData = FaceDataEast(chunk, x, y, z, meshData, normMasks);
		}

		if ((faces&32) != 0)
		{
			meshData = FaceDataWest(chunk, x, y, z, meshData, normMasks);
		}

        return meshData;

    }

	public Vector3 getoffset ()
	{
		return new Vector3 (offx, offy, offz);
	}

	public void setoffset (float x, float y, float z)
	{
		offx = x;
		offy = y;
		offz = z;
	}

	public void setoffset (Vector3 offset)
	{
		offx = offset.x;
		offy = offset.y;
		offz = offset.z;
	}

	public void SnapOffset (Vector3 offset)
	{
		offx = Mathf.Max(Mathf.Min(offset.x, 1.0f), 0.0f);
		offy = Mathf.Max(Mathf.Min(offset.y, 1.0f), 0.0f);
		offz = Mathf.Max(Mathf.Min(offset.z, 1.0f), 0.0f);
	}

	public void ClipOffset (Vector3 offset)
	{
		offx = (offset.x+1f) % 1.0f;
		offy = (offset.y+1f) % 1.0f;
		offz = (offset.z+1f) % 1.0f;
	}

	public void setvariant (float x, float y, float z)
	{
		//this gives us a repeatin 0-3 pattern
		int vposx = (int)((x) % 3);
		int vposy = (int)((y) % 3);
		int vposz = (int)(z % 3);

		//this is needed in case it is a negative number
		if (vposx < 0)
			vposx += 3;
		if (vposy < 0)
			vposy += 3;
		if (vposz < 0)
			vposz += 3;

		varientx = vposx;
		varienty = vposy;
		varientz = vposz;
	}

	public Vector3 getvariant (float x, float y, float z)
	{
		int vposx = (int)(x % 3);
		int vposy = (int)(y % 3);
		int vposz = (int)(z % 3);

		if (vposx < 0)
			vposx += 3;
		if (vposy < 0)
			vposy += 3;
		if (vposz < 0)
			vposz += 3;

		return new Vector3 (vposx, vposy, vposz);
	}

	//the getPoint# members return relative to world 
	public Vector3 getPoint1 (int x, int y, int z, Vector3 pos)
	{
		return new Vector3((x - 1.0f) + pos.x, pos.y + y + 0.0f, pos.z + z + 0.0f);
	}

	public Vector3 getPoint2 (int x, int y, int z, Vector3 pos)
	{
		return new Vector3((x + 0.0f) + pos.x, pos.y + y + 0.0f, pos.z + z + 0.0f);
	}

	public Vector3 getPoint3 (int x, int y, int z, Vector3 pos)
	{
		return new Vector3((x + 0.0f) + pos.x, pos.y + y + 0.0f, pos.z + z - 1.0f);
	}
	
	public Vector3 getPoint4 (int x, int y, int z, Vector3 pos)
	{
		return new Vector3((x - 1.0f) + pos.x, pos.y + y + 0.0f, pos.z + z - 1.0f);
	}

	public Vector3 getPoint5 (int x, int y, int z, Vector3 pos)
	{
		return new Vector3(x - 1.0f + pos.x, pos.y + y - 1.0f, pos.z + z - 1.0f);
	}
	
	public Vector3 getPoint6 (int x, int y, int z, Vector3 pos)
	{
		return new Vector3(x + 0.0f + pos.x, pos.y + y - 1.0f, pos.z + z - 1.0f);
	}
	
	public Vector3 getPoint7 (int x, int y, int z, Vector3 pos)
	{
		return new Vector3(x + 0.0f + pos.x, pos.y + y - 1.0f, pos.z + z + 0.0f);
	}
	
	public Vector3 getPoint8 (int x, int y, int z, Vector3 pos)
	{
		return new Vector3(x - 1.0f + pos.x, pos.y + y - 1.0f, pos.z + z + 0.0f);
	}

	protected Vector3 GetNormFromPoint (Vector3 point, Vector3 origen, Vector3 mask)
	{
		Vector3 normal = new Vector3 ();
		normal = Vector3.Scale(point - origen, mask);

		return normal;
	}

    protected virtual MeshData FaceDataUp
	(Chunk chunk, int x, int y, int z, MeshData meshData, Vector3[] normMasks)
    {
		//The top surface. We want to see if we can "round" off the corners by adjusting the normals


		//get the local offsets for each point
		Vector3[] offsets = new Vector3[4]; 
		offsets [0] = chunk.GetBlock (x - 1, y, z).getoffset ();
		offsets [1] = getoffset ();
		offsets [2] = chunk.GetBlock(x, y, z - 1).getoffset ();
		offsets [3] = chunk.GetBlock(x - 1, y, z - 1).getoffset ();

		Vector3[] points = new Vector3[4]; 
		points [0] = getPoint1 (x, y, z, offsets [0]);
		points [1] = getPoint2 (x, y, z, offsets [1]);
		points [2] = getPoint3 (x, y, z, offsets [2]);
		points [3] = getPoint4 (x, y, z, offsets [3]);

		meshData = AddQuadFlat (points, meshData); //use our points to calculate normals and faces

		meshData.uv.AddRange(SplitFaceUVs( FaceUVs(Direction.up, new Vector2(offsets [0].x, offsets [0].z), 
			new Vector2 (offsets [1].x, offsets [1].z), 
			new Vector2 (offsets [2].x, offsets [2].z), 
			new Vector2 (offsets [3].x, offsets [3].z))));

        return meshData;
    }

    protected virtual MeshData FaceDataDown
	(Chunk chunk, int x, int y, int z, MeshData meshData, Vector3[] normMasks)
    {
		//get the local offsets for each point
		Vector3[] offsets = new Vector3[4]; 
		offsets [0] = chunk.GetBlock(x - 1, y - 1, z - 1).getoffset ();
		offsets [1] = chunk.GetBlock(x, y - 1, z - 1).getoffset ();
		offsets [2] = chunk.GetBlock(x, y - 1, z).getoffset ();
		offsets [3] = chunk.GetBlock(x - 1, y - 1, z).getoffset ();

		Vector3[] points = new Vector3[4]; 
		points [0] = getPoint5 (x, y, z, offsets [0]);
		points [1] = getPoint6 (x, y, z, offsets [1]);
		points [2] = getPoint7 (x, y, z, offsets [2]);
		points [3] = getPoint8 (x, y, z, offsets [3]);

		meshData = AddQuadFlat (points, meshData); //use our points to calculate normals and faces

		meshData.uv.AddRange(SplitFaceUVs( FaceUVs(Direction.down, new Vector2(offsets [0].x, offsets [0].z), 
			new Vector2 (offsets [1].x, offsets [1].z), 
			new Vector2 (offsets [2].x, offsets [2].z), 
			new Vector2 (offsets [3].x, offsets [3].z))));
		
        return meshData;
    }

    protected virtual MeshData FaceDataNorth
	(Chunk chunk, int x, int y, int z, MeshData meshData, Vector3[] normMasks)
    {
        return meshData;
    }

    protected virtual MeshData FaceDataEast
	(Chunk chunk, int x, int y, int z, MeshData meshData, Vector3[] normMasks)
    {
		//get the local offsets for each point
		Vector3[] offsets = new Vector3[4]; 
		offsets [0] = chunk.GetBlock(x, y, z - 1).getoffset ();
		offsets [1] = getoffset ();
		offsets [2] = chunk.GetBlock(x, y - 1, z).getoffset ();
		offsets [3] = chunk.GetBlock(x, y - 1, z - 1).getoffset ();

		Vector3[] points = new Vector3[4]; 
		points [0] = getPoint3 (x, y, z, offsets [0]);
		points [1] = getPoint2 (x, y, z, offsets [1]);
		points [2] = getPoint7 (x, y, z, offsets [2]);
		points [3] = getPoint6 (x, y, z, offsets [3]);

		meshData.uv.AddRange(SplitFaceUVs( FaceUVs(Direction.east, new Vector2 (offsets [0].z, offsets [0].y), 
			new Vector2 (offsets [1].z, offsets [1].y), 
			new Vector2 (offsets [2].z, offsets [2].y), 
			new Vector2 (offsets [3].z, offsets [3].y))));
		
		if ((faces & 1) != 0) {
			meshData = AddQuadFlat (points, meshData); //use our points to calculate normals and faces
			meshData = AddQuadFlatBlend (points, meshData); //reuse our points to make a blending face
			meshData.uv.AddRange (SplitFaceUVs( FaceUVs(Direction.topeast, new Vector2 (offsets [1].z, offsets [1].y), 
			new Vector2 (offsets [2].z, offsets [2].y),
			new Vector2 (offsets [3].z, offsets [3].y),
			new Vector2 (offsets [0].z, offsets [0].y))));
		} else {
			meshData = AddQuadFlat (points, meshData); //use our points to calculate normals and faces
		}

        return meshData;
    }

    protected virtual MeshData FaceDataSouth
	(Chunk chunk, int x, int y, int z, MeshData meshData, Vector3[] normMasks)
    {
		//get the local offsets for each point
		Vector3[] offsets = new Vector3[4]; 
		offsets [0] = chunk.GetBlock(x - 1, y, z - 1).getoffset ();
		offsets [1] = chunk.GetBlock(x, y, z - 1).getoffset ();
		offsets [2] = chunk.GetBlock(x, y - 1, z - 1).getoffset ();
		offsets [3] = chunk.GetBlock(x - 1, y - 1, z - 1).getoffset ();

		Vector3[] points = new Vector3[4]; 
		points [0] = getPoint4 (x, y, z, offsets [0]);
		points [1] = getPoint3 (x, y, z, offsets [1]);
		points [2] = getPoint6 (x, y, z, offsets [2]);
		points [3] = getPoint5 (x, y, z, offsets [3]);

		meshData.uv.AddRange(SplitFaceUVs( FaceUVs(Direction.south, new Vector2(offsets [0].x, offsets [0].y), 
			new Vector2 (offsets [1].x, offsets [1].y), 
			new Vector2 (offsets [2].x, offsets [2].y), 
			new Vector2 (offsets [3].x, offsets [3].y))));

		if ((faces & 1) != 0) {
			meshData = AddQuadFlat (points, meshData); //use our points to calculate normals and faces
			meshData = AddQuadFlatBlend (points, meshData); //reuse our points to make a blending face
			meshData.uv.AddRange (SplitFaceUVs( FaceUVs(Direction.up, new Vector2(offsets [0].x, offsets [0].y), 
				new Vector2 (offsets [1].x, offsets [1].y), 
				new Vector2 (offsets [2].x, offsets [2].y), 
				new Vector2 (offsets [3].x, offsets [3].y))));
		} else {
			meshData = AddQuadFlat (points, meshData); //use our points to calculate normals and faces
		}
			
        return meshData;
    }

    protected virtual MeshData FaceDataWest
	(Chunk chunk, int x, int y, int z, MeshData meshData, Vector3[] normMasks)
	{
		//get the local offsets for each point
		Vector3[] offsets = new Vector3[4]; 
		offsets [0] = chunk.GetBlock(x - 1, y, z).getoffset ();
		offsets [1] = chunk.GetBlock(x - 1, y, z - 1).getoffset ();
		offsets [2] = chunk.GetBlock(x - 1, y - 1, z - 1).getoffset ();
		offsets [3] = chunk.GetBlock(x - 1, y - 1, z).getoffset ();

		Vector3[] points = new Vector3[4]; 
		points [0] = getPoint1 (x, y, z, offsets [0]);
		points [1] = getPoint4 (x, y, z, offsets [1]);
		points [2] = getPoint5 (x, y, z, offsets [2]);
		points [3] = getPoint8 (x, y, z, offsets [3]);

		meshData.uv.AddRange(SplitFaceUVs( FaceUVs(Direction.west, new Vector2(offsets [0].z, offsets [0].y), 
			new Vector2 (offsets [1].z, offsets [1].y), 
			new Vector2 (offsets [2].z, offsets [2].y), 
			new Vector2 (offsets [3].z, offsets [3].y))));

		if ((faces & 1) != 0) {
			meshData = AddQuadFlat (points, meshData); //use our points to calculate normals and faces
			meshData = AddQuadFlatBlend (points, meshData); //reuse our points to make a blending face
			meshData.uv.AddRange (SplitFaceUVs (FaceUVs (Direction.topwest, new Vector2 (offsets [0].z, offsets [0].y), 
				new Vector2 (offsets [1].z, offsets [1].y), 
				new Vector2 (offsets [2].z, offsets [2].y), 
				new Vector2 (offsets [3].z, offsets [3].y))));
		} else {
			meshData = AddQuadFlat (points, meshData); //use our points to calculate normals and faces
		}
			
        return meshData;
    }

	public virtual MeshData AddQuadFlatBlend( Vector3[] points, MeshData meshData) 
	{
		//Takes 4 points, calculates two normals for two faces
		//Adds the points and triangles to the mesh
		Vector3 flatnorm1 = new Vector3();
		flatnorm1 = Vector3.Cross (points [1] - points [0], points [3] - points [0]);
		Vector3 flatnorm2 = new Vector3();
		flatnorm2 = Vector3.Cross (points [3] - points [2], points [1] - points [2]);

		//set the vertices
		meshData.AddVertex(points [0], flatnorm1);
		meshData.AddVertex(points [1], flatnorm1);
		meshData.AddVertex(points [3], flatnorm1);
		meshData.AddBlendTriangle ();

		meshData.AddVertex(points [1], flatnorm2);
		meshData.AddVertex(points [2], flatnorm2);
		meshData.AddVertex(points [3], flatnorm2);
		meshData.AddBlendTriangle ();

		Color32[] vcolors = new Color32[6];
		vcolors [2] = Color.clear;
		vcolors [4] = Color.clear;
		vcolors [5] = Color.clear;
		vcolors [0] = Color.white;
		vcolors [1] = Color.white;
		vcolors [3] = Color.white;
		meshData.colors.AddRange (vcolors);

		return meshData;
	}

	public virtual MeshData AddQuadFlat( Vector3[] points, MeshData meshData) 
	{
		//Takes 4 points, calculates two normals for two faces
		//Adds the points and triangles to the mesh
		Vector3 flatnorm1 = new Vector3();
		flatnorm1 = Vector3.Cross (points [1] - points [0], points [3] - points [0]);
		Vector3 flatnorm2 = new Vector3();
		flatnorm2 = Vector3.Cross (points [3] - points [2], points [1] - points [2]);

		//set the vertices
		meshData.AddVertex(points [0], flatnorm1);
		meshData.AddVertex(points [1], flatnorm1);
		meshData.AddVertex(points [3], flatnorm1);
		meshData.AddTriangle();

		meshData.AddVertex(points [1], flatnorm2);
		meshData.AddVertex(points [2], flatnorm2);
		meshData.AddVertex(points [3], flatnorm2);
		meshData.AddTriangle();

		Color32[] vcolors = new Color32[6];
		vcolors [2] = Color.white;
		vcolors [4] = Color.white;
		vcolors [5] = Color.white;
		vcolors [0] = Color.white;
		vcolors [1] = Color.white;
		vcolors [3] = Color.white;

		meshData.colors.AddRange (vcolors);

		return meshData;
	}

	public virtual MeshData AddQuadRoundBlend( Vector3[] points, MeshData meshData) 
	{
		//Takes 4 points, calculates two normals for two faces
		//Adds the points and triangles to the mesh
		Vector3 flatnorm1 = new Vector3();
		flatnorm1 = Vector3.Cross (points [1] - points [0], points [3] - points [0]);
		Vector3 flatnorm2 = new Vector3();
		flatnorm2 = Vector3.Cross (points [3] - points [2], points [1] - points [2]);

		//set the vertices
		meshData.AddVertex(points [0], ((points[0]-points[2])+flatnorm1));
		meshData.AddVertex(points [1], ((points[1]-points[3])+flatnorm1));
		meshData.AddVertex(points [3], ((points[3]-points[1])+flatnorm1));
		meshData.AddBlendTriangle ();

		meshData.AddVertex(points [1], ((points[1]-points[3])+flatnorm2));
		meshData.AddVertex(points [2], ((points[2]-points[0])+flatnorm2));
		meshData.AddVertex(points [3], ((points[3]-points[1])+flatnorm2));
		meshData.AddBlendTriangle ();

		Color32[] vcolors = new Color32[6];
		vcolors [2] = Color.clear;
		vcolors [4] = Color.clear;
		vcolors [5] = Color.clear;
		vcolors [0] = Color.white;
		vcolors [1] = Color.white;
		vcolors [3] = Color.white;
		meshData.colors.AddRange (vcolors);

		return meshData;
	}

	public virtual MeshData AddQuadRound( Vector3[] points, MeshData meshData) 
	{
		//Takes 4 points, calculates two normals for two faces
		//Adds the points and triangles to the mesh
		Vector3 flatnorm1 = new Vector3();
		flatnorm1 = Vector3.Cross (points [1] - points [0], points [3] - points [0]);
		Vector3 flatnorm2 = new Vector3();
		flatnorm2 = Vector3.Cross (points [3] - points [2], points [1] - points [2]);

		//set the vertices
		meshData.AddVertex(points [0], ((points[0]-points[2])+flatnorm1));
		meshData.AddVertex(points [1], ((points[1]-points[3])+flatnorm1));
		meshData.AddVertex(points [3], ((points[3]-points[1])+flatnorm1));
		meshData.AddBlendTriangle ();

		meshData.AddVertex(points [1], ((points[1]-points[3])+flatnorm2));
		meshData.AddVertex(points [2], ((points[2]-points[0])+flatnorm2));
		meshData.AddVertex(points [3], ((points[3]-points[1])+flatnorm2));
		meshData.AddTriangle();

		Color32[] vcolors = new Color32[6];
		vcolors [2] = Color.white;
		vcolors [4] = Color.white;
		vcolors [5] = Color.white;
		vcolors [0] = Color.white;
		vcolors [1] = Color.white;
		vcolors [3] = Color.white;

		meshData.colors.AddRange (vcolors);

		return meshData;
	}

    public virtual bool IsSolid(Direction direction)
    {
		if (material < 1)
			return false;

        switch (direction)
        {
            case Direction.north:
                return true;
            case Direction.east:
                return true;
            case Direction.south:
                return true;
            case Direction.west:
                return true;
            case Direction.up:
                return true;
            case Direction.down:
                return true;
        }

        return false;
    }
	
}