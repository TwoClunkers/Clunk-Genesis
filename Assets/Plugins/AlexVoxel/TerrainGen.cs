using UnityEngine;
using System.Collections;
using SimplexNoise;

public class TerrainGen
{

    float stoneBaseHeight = -24;
    float stoneBaseNoise = 0.05f;
    float stoneBaseNoiseHeight = 4;

    float stoneMountainHeight = 48;
    float stoneMountainFrequency = 0.03f;
    float stoneMinHeight = -12;

    float dirtBaseHeight = 1;
    float dirtNoise = 0.04f;
    float dirtNoiseHeight = 3;

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
		if((z < 2) || ((z > 3) && (z < 14))) { //air is entered so we see out chunks facing out
			for (int y = chunk.pos.y; y < chunk.pos.y + Chunk.chunkSize; y++)
			{
				chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new BlockAir());
			}
			return chunk;
		}

        for (int y = chunk.pos.y; y < chunk.pos.y + Chunk.chunkSize; y++)
        {
			int noise = GetNoise( x, y, z, stoneMountainFrequency, 9);

			if(noise < 3) 
			{
				chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new BlockAir());
			}
			else if(noise < 5)
			{
				Block block = new Block();
				block.material = noise-2;
				chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);
			}
			else 
			{
				Block block = new Block();
				block.material = Mathf.Min(7, noise-2);
				chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);
			}

        }

        return chunk;
    }

    public static int GetNoise(int x, int y, int z, float scale, int max)
    {
        return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
    }
}