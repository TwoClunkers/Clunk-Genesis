using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Block
{
    public enum Direction { north, east, south, west, up, down };

    public struct Tile { public int x; public int y;}
    const float tileSize = 0.125f; //one eighth of the pic
    public bool changed = true;

	public int material = 1;
	public int varient = 0;
	public float damage = 100;
	public Vector3 offset;

    //Base block constructor
    public Block()
    {
		offset.Set (0.5f, 0.5f, 0.5f);
    }
	public virtual Boolean DamageBlock (WorldPos pos, float amount, Vector3 direction)
	{
		damage = Mathf.Max(0, damage-amount);
		if(damage < 1) {

			return true;
		}
		else return false;
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
		//Vector3 modified = getPoint1 (x, y, z, offset); 

		meshData.AddVertex(getPoint1 (x, y, z, chunk.GetBlock(x - 1, y, z).offset));
		meshData.AddVertex(getPoint2 (x, y, z, offset));
		meshData.AddVertex(getPoint3 (x, y, z, chunk.GetBlock(x, y, z - 1).offset));
		meshData.AddVertex(getPoint4 (x, y, z, chunk.GetBlock(x - 1, y, z - 1).offset));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.up));
        return meshData;
    }

    protected virtual MeshData FaceDataDown
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
		meshData.AddVertex(getPoint5 (x, y, z, chunk.GetBlock(x - 1, y - 1, z - 1).offset));
		meshData.AddVertex(getPoint6 (x, y, z, chunk.GetBlock(x, y - 1, z - 1).offset));
		meshData.AddVertex(getPoint7 (x, y, z, chunk.GetBlock(x, y - 1, z).offset));
		meshData.AddVertex(getPoint8 (x, y, z, chunk.GetBlock(x - 1, y - 1, z).offset));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.down));
        return meshData;
    }

    protected virtual MeshData FaceDataNorth
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
		meshData.AddVertex(getPoint7 (x, y, z, chunk.GetBlock(x, y - 1, z).offset));
		meshData.AddVertex(getPoint2 (x, y, z, offset));
		meshData.AddVertex(getPoint1 (x, y, z, chunk.GetBlock(x - 1, y, z).offset));
		meshData.AddVertex(getPoint8 (x, y, z, chunk.GetBlock(x - 1, y - 1, z).offset));
//		meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
//        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
//        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
//        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.north));
        return meshData;
    }

    protected virtual MeshData FaceDataEast
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
		meshData.AddVertex(getPoint6 (x, y, z, chunk.GetBlock(x, y - 1, z - 1).offset));
		meshData.AddVertex(getPoint3 (x, y, z, chunk.GetBlock(x, y, z - 1).offset));
		meshData.AddVertex(getPoint2 (x, y, z, offset));
		meshData.AddVertex(getPoint7 (x, y, z, chunk.GetBlock(x, y - 1, z).offset));
//		meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
//        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
//        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
//        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.east));
        return meshData;
    }

    protected virtual MeshData FaceDataSouth
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
		meshData.AddVertex(getPoint5 (x, y, z, chunk.GetBlock(x - 1, y - 1, z - 1).offset));
		meshData.AddVertex(getPoint4 (x, y, z, chunk.GetBlock(x - 1, y, z - 1).offset));
		meshData.AddVertex(getPoint3 (x, y, z, chunk.GetBlock(x, y, z - 1).offset));
		meshData.AddVertex(getPoint6 (x, y, z, chunk.GetBlock(x, y - 1, z - 1).offset));
//		meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
//        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
//        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
//        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.south));
        return meshData;
    }

    protected virtual MeshData FaceDataWest
        (Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		meshData.AddVertex(getPoint8 (x, y, z, chunk.GetBlock(x - 1, y - 1, z).offset));
		meshData.AddVertex(getPoint1 (x, y, z, chunk.GetBlock(x - 1, y, z).offset));
		meshData.AddVertex(getPoint4 (x, y, z, chunk.GetBlock(x - 1, y, z - 1).offset));
		meshData.AddVertex(getPoint5 (x, y, z, chunk.GetBlock(x - 1, y - 1, z - 1).offset));
//        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
//        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
//        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
//        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.west));
        return meshData;
    }

    public virtual Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();
		tile.x = varient;
		tile.y = material;

        return tile;
    }

    public virtual Vector2[] FaceUVs(Direction direction)
    {
        Vector2[] UVs = new Vector2[4];
        Tile tilePos = TexturePosition(direction);

        UVs[0] = new Vector2(tileSize * tilePos.x + tileSize,
            tileSize * tilePos.y);
        UVs[1] = new Vector2(tileSize * tilePos.x + tileSize,
            tileSize * tilePos.y + tileSize);
        UVs[2] = new Vector2(tileSize * tilePos.x,
            tileSize * tilePos.y + tileSize);
        UVs[3] = new Vector2(tileSize * tilePos.x,
            tileSize * tilePos.y);

        return UVs;
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