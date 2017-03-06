using UnityEngine;
using System.Collections;
using System;


[Serializable]
public class BlockSelect : Block
{
	public BlockSelect()
		: base()
	{
		SetMaterial(1);
		tileSize = 1.0f;
		offx = 0.5f;
		offy = 0.5f;
		offz = 0.5f;
	}

	public override MeshData Blockdata
		(Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		meshData.useRenderDataForCol = false;
		Vector3[] normMasks = GetNormMask (chunk, x, y, z);

		meshData = FaceDataUp(chunk, x, y, z, meshData, normMasks);
		meshData = FaceDataDown(chunk, x, y, z, meshData, normMasks);
		meshData = FaceDataNorth(chunk, x, y, z, meshData, normMasks);
		meshData = FaceDataSouth(chunk, x, y, z, meshData, normMasks);
		meshData = FaceDataEast(chunk, x, y, z, meshData, normMasks);
		meshData = FaceDataWest(chunk, x, y, z, meshData, normMasks);

		return meshData;
		
	}
		
	public override Tile TexturePosition(Direction direction)
	{
		Tile tile = new Tile();
		
		tile.x = 0;
		tile.y = 0;
		
		return tile;
	}
	
	public override Vector2[] FaceUVs(Direction direction, Vector2 point0, Vector2 point1, Vector2 point2, Vector2 point3)
	{
		Vector2[] UVs = new Vector2[4];

		UVs[0] = new Vector2(0, 0);
		
		UVs[1] = new Vector2(0, 1);
		
		UVs[2] = new Vector2(1, 1);
		
		UVs[3] = new Vector2(1, 0);
		
		return UVs;
	}
}

