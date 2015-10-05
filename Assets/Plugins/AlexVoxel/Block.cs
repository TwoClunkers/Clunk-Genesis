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

	public int material = 1;
	public int varientx = 0;
	public int varienty = 0;
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
		damage = target.damage;
		offx = target.offx;
		offy = target.offy;
		offz = target.offz;

	}
	public virtual Tile TexturePosition(Direction direction)
	{
		Tile tile = new Tile();
		
		//tile.x = varientx;
		//tile.y = varienty;
		
		if(material < 5) {
			tile.x = varientx;
			tile.y = varienty+(material-1)*8;
		}
		else {
			tile.x = varientx+8;
			tile.y = varienty+(material-5)*8;
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
	public virtual Boolean DamageBlock (WorldPos pos, float amount, Vector3 direction)
	{
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

    public virtual MeshData Blockdata
     (Chunk chunk, int x, int y, int z, MeshData meshData)
    {


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
		meshData.uv.AddRange(FaceUVs(Direction.up, new Vector2(points [0].x, points [0].z), 
		                             new Vector2 (points [1].x, points [1].z), 
		                             new Vector2 (points [2].x, points [2].z), 
		                             new Vector2 (points [3].x, points [3].z)));
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
		meshData.uv.AddRange(FaceUVs(Direction.east, new Vector2(points [0].y, points [0].z), 
		                             new Vector2 (points [1].y, points [1].z), 
		                             new Vector2 (points [2].y, points [2].z), 
		                             new Vector2 (points [3].y, points [3].z)));
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
        return meshData;
    }

 

    public virtual bool IsSolid(Direction direction)
    {
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