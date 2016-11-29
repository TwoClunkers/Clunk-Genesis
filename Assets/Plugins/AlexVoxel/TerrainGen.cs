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
	
		float planitary = GetNoise (x, z, 1000, 0.005f, 10);
		for (int y = chunk.pos.y; y < chunk.pos.y + Chunk.chunkSize; y++) {
			float metallic = GetNoise (x-1000, y-1000, z-1000, 0.005f, 20);
			float softness = GetNoise (x+1000, y, z, 0.05f, 10);
			float water = GetNoise (x+1000, y, z, 0.005f, 10);
			float bio = GetNoise (x+100, y, z+100, 0.005f, 20);
			float magnitude = (0.045f); 
			float zonenoise = GetNoise (x+100, y+200, z+400, 0.005f, 10);
			float substrate = planitary + softness - (water+1f);
			float biome;

			if (zonenoise > 5.0f) {
				biome = substrate + metallic;
				if ((metallic < 8) && (metallic > 3)) {
					if (water > 5)
						biome += bio;
					else
						biome -= bio;
				}
			} else {
				biome = substrate - metallic;
				if ((metallic < 8) && (metallic > 3)) {
					if (water < 5)
						biome += water;
					else
						biome -= water;
				}
			}


			//fidelity
			float prevx = GetNoise (x-1, y*2, z, magnitude, 6); 
			float noise = GetNoise (x, y*2, z, magnitude, 6);
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
			block.setoffset (xoff, yoff, remainder);




			if (noise + softness < 7.0f) {
				block.material = 0;
			} else if (metallic > 17.0f) {
				block.material = 19;
			} else if (bio > 17.07f) {
				block.material = 32;
			} else if (substrate < 4.07f) {
				block.material = 5;
			} else if (biome < 12.07f) {
				block.material = 35;
			} else if (biome < 18.07f) {
				block.material = 38;
			} else {
				block.material = 40;
			}

			if ((z < 1) || ((z > 4) && (z < 8))) { //air is entered so we see our chunks facing out
				block.material = 0;
				block.faces = 0;
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