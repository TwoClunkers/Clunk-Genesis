using UnityEngine;
using System.Collections;
using SimplexNoise;

public class TerrainGen
{

    public Chunk ChunkGen(Chunk chunk)
    {
        for (int x = chunk.pos.x; x < chunk.pos.x + Chunk.chunkSize; x++)
        {
            for (int z = chunk.pos.z; z < chunk.pos.z + Chunk.chunkSize; z++)
			//for (int z = 0; z < 4; z++)
            {
                chunk = ChunkColumnGen(chunk, x, z);
            }
        }
        return chunk;
    }

    public Chunk ChunkColumnGen(Chunk chunk, int x, int z)
    {
		float prevy = chunk.pos.y - 1;
		float xoff;
		float yoff;

		for (int y = chunk.pos.y; y < chunk.pos.y + Chunk.chunkSize; y++) {
			float prevx = GetNoise (x - 1, y, z, 0.03f, 8); 
			float noise = GetNoise (x, y, z, 0.03f, 8);
			float remainder = (noise - (int)noise);
			Block block = new Block ();

			if (noise > prevx)
				xoff = 1 - remainder;
			else
				xoff = remainder;
			if (noise > prevy)
				yoff = remainder;
			else
				yoff = 1 - remainder;

			prevy = noise;

			block.setvariant (x, y, z);
			block.setoffset ( xoff, yoff, Random.Range ( 0.4f, 0.6f) );

			if (noise < 2.50) {
				block.material = 0;
			} else if (noise < 4.25) {
				if((noise < 2.55) && (noise > 2.549)) {
					BlockAnchor blockA = new BlockAnchor();
					blockA.material = 31;
					blockA.damage = 450;
					blockA.setvariant (x, y, z);
					block = blockA;
				} else if((noise < 2.6) && (noise > 2.59)) {
					block.material = 32;
					block.damage = 50;
				} else block.material = 14;
			} else if (noise < 6.0) {
				block.material = Mathf.FloorToInt (noise - 1);
			} else {
				block.material = Mathf.FloorToInt (Mathf.Min (16.0f, noise - 1));
			}

			if ((z < 1) || ((z > 4) && (z < 8))) { //air is entered so we see our chunks facing out
				block.material = 0;
			}

			chunk.SetBlock (x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);
		}
        return chunk;
    }

    public static float GetNoise(int x, int y, int z, float scale, int max)
    {
		return ((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
    }
}