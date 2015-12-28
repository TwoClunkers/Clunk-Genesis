using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BlockAnchor : Block
{
	public BlockAnchor()
			: base()
	{
		material = 7;
	}
	
	public override void Resolve
		(Chunk chunk, int x, int y, int z)
	{
		chunk.GetBlock (x - 1, y, z).setoffset (new Vector3 (0.5f, 0.5f, 0.5f));
		chunk.GetBlock (x, y, z - 1).setoffset (new Vector3 (0.5f, 0.5f, 0.5f));
		chunk.GetBlock (x - 1, y, z - 1).setoffset (new Vector3 (0.5f, 0.5f, 0.5f));
		chunk.GetBlock (x - 1, y - 1, z).setoffset (new Vector3 (0.5f, 0.5f, 0.5f));
		chunk.GetBlock (x, y - 1, z - 1).setoffset (new Vector3 (0.5f, 0.5f, 0.5f));
		chunk.GetBlock (x - 1, y - 1, z - 1).setoffset (new Vector3 (0.5f, 0.5f, 0.5f));
		chunk.GetBlock (x, y - 1, z).setoffset (new Vector3 (0.5f, 0.5f, 0.5f));
	}
}
