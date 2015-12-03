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
		if((z < 1) || ((z > 4) && (z < 8))) { //air is entered so we see our chunks facing out
			for (int y = chunk.pos.y; y < chunk.pos.y + Chunk.chunkSize; y++)
			{
				float noise = GetNoise( x, y, z, 0.03f, 6);
				float remainder = (noise - (int)noise);
				Block block = new Block();
				block.material = 0;
				block.setoffset(remainder,remainder,Random.value/8);
				//block.setoffset (Random.value,0.5f,Random.value/8);
				chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);
			}
			return chunk;
		}

        for (int y = chunk.pos.y; y < chunk.pos.y + Chunk.chunkSize; y++)
        {
			float noise = GetNoise( x, y, z, 0.03f, 6);
			float remainder = (noise - (int)noise);

			if(noise < 2) 
			{
				Block block = new Block();
				block.setvariant(x,y);
				block.material = 0;
				block.setoffset(remainder,remainder,Random.value/8);
				//block.setoffset(remainder,remainder,Random.value/4+0.38f);
				//block.setoffset (Random.value/4+0.38f,0.5f,Random.value/4+0.38f);
				//block.setoffset(Random.Range (-0.2F,0.2F),Random.Range (-0.0F,0.0F),Random.Range (-0.0F,0.0F));
				chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);
			}
			else if(noise < 4)
			{
				Block block = new Block();
				block.setvariant(x,y);
				block.material = Mathf.FloorToInt(noise-1);
				block.setoffset(remainder,remainder,Random.value/8);
				//block.varient = Random.Range(0 , 3);
				//block.setoffset(remainder,remainder,Random.value/4+0.38f);
				//block.setoffset (Random.value/4+0.38f,0.5f,Random.value/2+0.5f);
				//block.setoffset (Random.Range (-0.2F,0.2F),Random.Range (-0.0F,0.0F),Random.Range (-0.0F,0.0F));
				chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);
			}
			else if(noise < 6)
			{
				Block block = new Block();
				block.material = Mathf.FloorToInt(noise-1);
				block.setvariant(x,y);
				block.setoffset(remainder,remainder,Random.value/8);
				//block.varient = Random.Range(0 , 3);
				//block.setoffset(remainder,remainder,Random.value/4+0.38f);
				//block.setoffset (Random.value,0.5f,Random.value/2+0.5f);
				//block.setoffset (Random.Range (-0.2F,0.2F),Random.Range (-0.0F,0.0F),Random.Range (-0.0F,0.0F));
				chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);
			}
			else 
			{
				Block block = new Block();
				block.material = Mathf.FloorToInt(Mathf.Min(4.0f, noise-1));
				block.setvariant(x,y);
				block.setoffset(remainder,remainder,Random.value/8);
				//block.varient = Random.Range(0 , 3);
				//block.setoffset (remainder,remainder,Random.value/4+0.38f);
				//block.setoffset (Random.value,0.5f,Random.value/2+0.5f);
				//block.setoffset(Random.Range (-0.2F,0.2F),Random.Range (-0.0F,0.0F),Random.Range (-0.0F,0.0F));
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