using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Block
{
    public enum Direction { north, east, south, west, up, down };

    public struct Tile { public int x; public int y;}
    public float tileSize = 0.03125f; //one 32th of the pic
    public bool changed = true;
	public bool isSelected = false;
	public int material = 1;
	public int varientx = 0;
	public int varienty = 0;
	public int varientz = 0;
	public float damage = 100;
	public float offx;
	public float offy;
	public float offz;



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

		if (material < 9) { //first 8 materials have large maps
			if (material < 5) { //first 4 materials
				xregion = 0;
				yregion = (material - 1) * 8;
			} else { //second 4 materials
				xregion = 8;
				yregion = (material - 5) * 8;
			}
		} else if (material < 17) { //next 8 matierals are medium size
			xregion = 16;
			yregion = (material - 9) * 4;
			varientx = Mathf.Clamp (varientx, 0, 2);
			varienty = Mathf.Clamp (varienty, 0, 2);
			varientz = Mathf.Clamp (varientz, 0, 2);
		} else if (material < 49) { //next 32 matirials are rectangles
			if (material < 33) { //first row of materials
				xregion = 20;
				yregion = (material - 17) * 2;
			} else { //second 16 materials
				xregion = 24;
				yregion = (material - 33) * 2;
			}
			varientx = Mathf.Clamp (varientx, 0, 2);
			varienty = 0;
			varientz = 0;
		}

		switch (direction)
		{
			case Direction.south:
				tile.x = varientx + xregion;
				tile.y = varienty + yregion;
				return tile;
			case Direction.down:
				tile.x = varientx + xregion;
				tile.y = varientz + yregion;
				return tile;
			case Direction.up:
				tile.x = varientx + xregion;
				tile.y = varientz + yregion;
				return tile;
			case Direction.east:
				tile.x = varientz + xregion;
				tile.y = varienty + yregion;
				return tile;
			case Direction.west:
				tile.x = 7-varientz + xregion;
				tile.y = varienty + yregion;
				return tile;
		}

		
		return tile;
	}
	
	public virtual Vector2[] FaceUVs(Direction direction, Vector2 point0, Vector2 point1, Vector2 point2, Vector2 point3)
	{
		Vector2[] UVs = new Vector2[4];
		Tile tilePos = TexturePosition(direction);
		
		
		
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

	public virtual Vector2[] FaceUV1 ()
	{
		Vector2[] UVs = new Vector2[4];

		if (isSelected) {

			UVs [0] = new Vector2 (0.0f, 0.0f);
		
			UVs [1] = new Vector2 (1.0f, 0.0f);
		
			UVs [2] = new Vector2 (1.0f, 1.0f); 
		
			UVs [3] = new Vector2 (0.0f, 1.0f);
		} else {
			UVs [0] = new Vector2 (0.1f, 0.1f);
			
			UVs [1] = new Vector2 (0.9f, 0.1f);
			
			UVs [2] = new Vector2 (0.9f, 0.9f); 
			
			UVs [3] = new Vector2 (0.1f, 0.9f);
		}
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

    public virtual MeshData Blockdata
     (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
		if(material < 1) return meshData;

        meshData.useRenderDataForCol = true;

        if (!chunk.GetBlock(x, y + 1, z).IsSolid(Direction.down))
        {
            meshData = FaceDataUp(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y - 1, z).IsSolid(Direction.up))
        {
            meshData = FaceDataDown(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y, z + 1).IsSolid(Direction.south))
        {
            meshData = FaceDataNorth(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y, z - 1).IsSolid(Direction.north))
        {
            meshData = FaceDataSouth(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x + 1, y, z).IsSolid(Direction.west))
        {
            meshData = FaceDataEast(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x - 1, y, z).IsSolid(Direction.east))
        {
            meshData = FaceDataWest(chunk, x, y, z, meshData);
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

	public void setvariant (float x, float y, float z)
	{
		int vposx = (int)(x % 7);
		int vposy = (int)(y % 7);
		int vposz = (int)(z % 7);

		if (vposx < 0)
			vposx += 7;
		if (vposy < 0)
			vposy += 7;
		if (vposz < 0)
			vposz += 7;

		varientx = vposx;
		varienty = vposy;
		varientz = vposz;
	}

	//the getPoint# members should return a Vector3 that is a point relative to this block
	protected Vector3 getPoint1 (int x, int y, int z, Vector3 pos)
	{
		return new Vector3((x - 1.0f) + pos.x, pos.y + y + 0.0f, pos.z + z + 0.0f);
	}

	protected Vector3 getPoint2 (int x, int y, int z, Vector3 pos)
	{
		return new Vector3((x + 0.0f) + pos.x, pos.y + y + 0.0f, pos.z + z + 0.0f);
	}

	protected Vector3 getPoint3 (int x, int y, int z, Vector3 pos)
	{
		return new Vector3((x + 0.0f) + pos.x, pos.y + y + 0.0f, pos.z + z - 1.0f);
	}
	
	protected Vector3 getPoint4 (int x, int y, int z, Vector3 pos)
	{
		return new Vector3((x - 1.0f) + pos.x, pos.y + y + 0.0f, pos.z + z - 1.0f);
	}

	protected Vector3 getPoint5 (int x, int y, int z, Vector3 pos)
	{
		return new Vector3(x - 1.0f + pos.x, pos.y + y - 1.0f, pos.z + z - 1.0f);
	}
	
	protected Vector3 getPoint6 (int x, int y, int z, Vector3 pos)
	{
		return new Vector3(x + 0.0f + pos.x, pos.y + y - 1.0f, pos.z + z - 1.0f);
	}
	
	protected Vector3 getPoint7 (int x, int y, int z, Vector3 pos)
	{
		return new Vector3(x + 0.0f + pos.x, pos.y + y - 1.0f, pos.z + z + 0.0f);
	}
	
	protected Vector3 getPoint8 (int x, int y, int z, Vector3 pos)
	{
		return new Vector3(x - 1.0f + pos.x, pos.y + y - 1.0f, pos.z + z + 0.0f);
	}

    protected virtual MeshData FaceDataUp
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
		Vector3[] points = new Vector3[4]; 
		points [0] = chunk.GetBlock (x - 1, y, z).getoffset ();
		meshData.AddVertex(getPoint1 (x, y, z, points [0] ));
		points [1] = getoffset ();
		meshData.AddVertex(getPoint2 (x, y, z, points [1] ));
		points [2] = chunk.GetBlock(x, y, z - 1).getoffset ();
		meshData.AddVertex(getPoint3 (x, y, z, points [2] ));
		points [3] = chunk.GetBlock(x - 1, y, z - 1).getoffset ();
		meshData.AddVertex(getPoint4 (x, y, z, points [3] ));

        meshData.AddQuadTriangles();
		meshData.uv.AddRange(FaceUVs(Direction.up, new Vector2(points [0].z, points [0].x), 
		                             new Vector2 (points [1].z, points [1].x), 
		                             new Vector2 (points [2].z, points [2].x), 
		                             new Vector2 (points [3].z, points [3].x)));
		meshData.uv1.AddRange (FaceUV1 ());

        return meshData;
    }

    protected virtual MeshData FaceDataDown
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
		Vector3[] points = new Vector3[4]; 
		points [0] = chunk.GetBlock(x - 1, y - 1, z - 1).getoffset ();
		meshData.AddVertex(getPoint5 (x, y, z, points [0] ));
		points [1] = chunk.GetBlock(x, y - 1, z - 1).getoffset ();
		meshData.AddVertex(getPoint6 (x, y, z, points [1] ));
		points [2] = chunk.GetBlock(x, y - 1, z).getoffset ();
		meshData.AddVertex(getPoint7 (x, y, z, points [2] ));
		points [3] = chunk.GetBlock(x - 1, y - 1, z).getoffset ();
		meshData.AddVertex(getPoint8 (x, y, z, points [3] ));

        meshData.AddQuadTriangles();
		meshData.uv.AddRange(FaceUVs(Direction.down, new Vector2(points [0].x, points [0].z), 
		                             new Vector2 (points [1].x, points [1].z), 
		                             new Vector2 (points [2].x, points [2].z), 
		                             new Vector2 (points [3].x, points [3].z)));
		meshData.uv1.AddRange (FaceUV1 ());
        return meshData;
    }

    protected virtual MeshData FaceDataNorth
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        return meshData;
    }

    protected virtual MeshData FaceDataEast
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
		Vector3[] points = new Vector3[4]; 
		points [0] = chunk.GetBlock(x, y, z - 1).getoffset ();
		meshData.AddVertex(getPoint3 (x, y, z, points [0] ));
		points [1] = getoffset ();
		meshData.AddVertex(getPoint2 (x, y, z, points [1] ));
		points [2] = chunk.GetBlock(x, y - 1, z).getoffset ();
		meshData.AddVertex(getPoint7 (x, y, z, points [2] ));
		points [3] = chunk.GetBlock(x, y - 1, z - 1).getoffset ();
		meshData.AddVertex(getPoint6 (x, y, z, points [3] ));

		
        meshData.AddQuadTriangles();
		meshData.uv.AddRange(FaceUVs(Direction.east, new Vector2(points [0].z, points [0].y), 
		                             new Vector2 (points [1].z, points [1].y), 
		                             new Vector2 (points [2].z, points [2].y), 
		                             new Vector2 (points [3].z, points [3].y)));
		meshData.uv1.AddRange (FaceUV1 ());
        return meshData;
    }

    protected virtual MeshData FaceDataSouth
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
		Vector3[] points = new Vector3[4]; 

		points [0] = chunk.GetBlock(x - 1, y, z - 1).getoffset ();
		meshData.AddVertex(getPoint4 (x, y, z, points [0] ));

		points [1] = chunk.GetBlock(x, y, z - 1).getoffset ();
		meshData.AddVertex(getPoint3 (x, y, z, points [1] ));

		points [2] = chunk.GetBlock(x, y - 1, z - 1).getoffset ();
		meshData.AddVertex(getPoint6 (x, y, z, points [2] ));

		points [3] = chunk.GetBlock(x - 1, y - 1, z - 1).getoffset ();
		meshData.AddVertex(getPoint5 (x, y, z, points [3] ));

        meshData.AddQuadTriangles();
		meshData.uv.AddRange(FaceUVs(Direction.south, new Vector2(points [0].x, points [0].y), 
		                             new Vector2 (points [1].x, points [1].y), 
		                             new Vector2 (points [2].x, points [2].y), 
		                             new Vector2 (points [3].x, points [3].y)));
		meshData.uv1.AddRange (FaceUV1 ());
        return meshData;
    }

    protected virtual MeshData FaceDataWest
        (Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		Vector3[] points = new Vector3[4]; 
		points [0] = chunk.GetBlock(x - 1, y, z).getoffset ();
		meshData.AddVertex(getPoint1 (x, y, z, points [0] ));
		points [1] = chunk.GetBlock(x - 1, y, z - 1).getoffset ();
		meshData.AddVertex(getPoint4 (x, y, z, points [1] ));
		points [2] = chunk.GetBlock(x - 1, y - 1, z - 1).getoffset ();
		meshData.AddVertex(getPoint5 (x, y, z, points [2] ));
		points [3] = chunk.GetBlock(x - 1, y - 1, z).getoffset ();
		meshData.AddVertex(getPoint8 (x, y, z, points [3] ));


        meshData.AddQuadTriangles();
		meshData.uv.AddRange(FaceUVs(Direction.west, new Vector2(points [0].z, points [0].y), 
		                             new Vector2 (points [1].z, points [1].y), 
		                             new Vector2 (points [2].z, points [2].y), 
		                             new Vector2 (points [3].z, points [3].y)));
		meshData.uv1.AddRange (FaceUV1 ());
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