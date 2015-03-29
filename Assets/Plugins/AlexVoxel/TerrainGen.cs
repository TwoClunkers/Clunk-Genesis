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
		if((z < 2) || ((z > 3) && (z < 14))) { //air is entered so we see our chunks facing out
			for (int y = chunk.pos.y; y < chunk.pos.y + Chunk.chunkSize; y++)
			{
				float noise = GetNoise( x, y, z, 0.03f, 15);
				float remainder = (noise - (int)noise);
				Block block = new BlockAir();
				block.offset.Set (1-remainder,1-remainder,Random.value/8);
				//block.offset.Set (Random.value,0.5f,Random.value/8);
				chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);
			}
			return chunk;
		}

        for (int y = chunk.pos.y; y < chunk.pos.y + Chunk.chunkSize; y++)
        {
			float noise = GetNoise( x, y, z, 0.0205f, 12);
			float remainder = (noise - (int)noise);

			if(noise < 3) 
			{
				Block block = new BlockAir();

				block.offset.Set (1-remainder,1-remainder,Random.value/4+0.38f);
				//block.offset.Set (Random.value/4+0.38f,0.5f,Random.value/4+0.38f);
				//block.offset.Set (Random.Range (-0.2F,0.2F),Random.Range (-0.0F,0.0F),Random.Range (-0.0F,0.0F));
				chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);
			}
			else if(noise < 8)
			{
				Block block = new Block();
				block.material = Mathf.FloorToInt(noise-2);
				block.varient = Random.Range(0 , 3);
				block.offset.Set (1-remainder,1-remainder,Random.value/4+0.38f);
				//block.offset.Set (Random.value/4+0.38f,0.5f,Random.value/2+0.5f);
				//block.offset.Set (Random.Range (-0.2F,0.2F),Random.Range (-0.0F,0.0F),Random.Range (-0.0F,0.0F));
				chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);
			}
			else if(noise < 10)
			{
				Block block = new Block();
				block.material = Mathf.FloorToInt(noise-2);
				block.varient = Random.Range(0 , 3);
				block.offset.Set (1-remainder,1-remainder,Random.value/4+0.38f);
				//block.offset.Set (Random.value,0.5f,Random.value/2+0.5f);
				//block.offset.Set (Random.Range (-0.2F,0.2F),Random.Range (-0.0F,0.0F),Random.Range (-0.0F,0.0F));
				chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);
			}
			else 
			{
				Block block = new Block();
				block.material = Mathf.FloorToInt(Mathf.Min(15.0f, noise-2));
				block.varient = Random.Range(0 , 3);
				block.offset.Set (1-remainder,1-remainder,Random.value/4+0.38f);
				//block.offset.Set (Random.value,0.5f,Random.value/2+0.5f);
				//block.offset.Set (Random.Range (-0.2F,0.2F),Random.Range (-0.0F,0.0F),Random.Range (-0.0F,0.0F));
				chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);
			}

        }

        return chunk;
    }

    public static float GetNoise(int x, int y, int z, float scale, int max)
    {
        return ((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
    }
}