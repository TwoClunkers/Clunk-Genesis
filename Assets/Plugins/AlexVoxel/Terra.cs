using UnityEngine;
using System.Collections;

public static class Terra
{
    public static WorldPos GetBlockPos(Vector3 pos)
    {
        WorldPos blockPos = new WorldPos(
            Mathf.RoundToInt(pos.x),
            Mathf.RoundToInt(pos.y),
            Mathf.RoundToInt(pos.z)
            );

        return blockPos;
    }

    public static WorldPos GetBlockPos(RaycastHit hit, bool adjacent = false)
    {
        Vector3 pos = new Vector3(
            MoveWithinBlock(hit.point.x, hit.normal.x, adjacent),
            MoveWithinBlock(hit.point.y, hit.normal.y, adjacent),
            MoveWithinBlock(hit.point.z, hit.normal.z, adjacent)
            );

        return GetBlockPos(pos);
    }

    static float MoveWithinBlock(float pos, float norm, bool adjacent = false)
    {
        if (pos - (int)pos == 0.5f || pos - (int)pos == -0.5f)
        {
//            if (adjacent)
//            {
//                pos += (norm / 2);
//            }
//            else
//            {
//                pos -= (norm / 2);
//            }
        }

        return (float)pos;
    }

	public static bool SetBlock(Chunk chunk, WorldPos pos, Block block)
	{
		if (chunk == null)
			return false;

		chunk.world.SetBlock(pos.x, pos.y, pos.z, block);
		
		return true;
	}

    public static bool SetBlock(RaycastHit hit, Block block, bool adjacent = false)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return false;

        WorldPos pos = GetBlockPos(hit, adjacent);

        chunk.world.SetBlock(pos.x, pos.y, pos.z, block);

        return true;
    }

	public static bool DamageBlock(Chunk chunk, WorldPos pos, float amount, Vector3 direction)
	{
		if (chunk == null)
			return false;
		
		Block block = chunk.world.GetBlock(pos.x, pos.y, pos.z);
		if(block.DamageBlock(pos,amount,direction)) {
			block = new BlockAir();
			chunk.world.SetBlock(pos.x, pos.y, pos.z, block);
			return true;
		}
		return false;
	}

	public static bool DamageBlock(RaycastHit hit, float amount, Vector3 direction)
	{
		Chunk chunk = hit.collider.GetComponent<Chunk>();
		if (chunk == null)
			return false;
		
		WorldPos pos = GetBlockPos(hit, false);
		
		Block block = chunk.world.GetBlock(pos.x, pos.y, pos.z);
		if(block.DamageBlock(pos,amount,direction)) {
			block = new BlockAir();
			chunk.world.SetBlock(pos.x, pos.y, pos.z, block);
			return true;
		}
		return false;
	}

    public static Block GetBlock(RaycastHit hit, bool adjacent = false)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return null;

        WorldPos pos = GetBlockPos(hit, adjacent);

        Block block = chunk.world.GetBlock(pos.x, pos.y, pos.z);

        return block;
    }
}