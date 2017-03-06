using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadChunks : MonoBehaviour
{
    static WorldPos[] chunkPositions = {   new WorldPos( 0,  0, 0), new WorldPos(-1,    0, 0), new WorldPos( 0, -1, 0), new WorldPos( 0,  1, 0), new WorldPos( 1, 0,  0),
                             new WorldPos(-1, -1, 0), new WorldPos(-1,  1, 0), new WorldPos( 1, -1, 0), new WorldPos( 1,  1, 0), new WorldPos(-2,  0, 0),
                             new WorldPos( 0, -2, 0), new WorldPos( 2,  0, 0), new WorldPos( 2,  0, 0), new WorldPos(-2, -1, 0), new WorldPos(-2,  1, 0),
                             new WorldPos(-1, -2, 0), new WorldPos(-1,  2, 0), new WorldPos( 1, -2, 0), new WorldPos( 1,  2, 0), new WorldPos( 2, -1, 0),
                             new WorldPos( 2,  1, 0), new WorldPos(-2, -2, 0), new WorldPos(-2,  2, 0), new WorldPos( 2, -2, 0), new WorldPos( 2,  2, 0),
                             new WorldPos(-3,  0, 0), new WorldPos( 0, -3, 0), new WorldPos( 0,  3, 0), new WorldPos( 3,  0, 0), new WorldPos(-3, -1, 0),
                             new WorldPos(-3,  1, 0), new WorldPos(-1, -3, 0), new WorldPos(-1,  3, 0), new WorldPos( 1, -3, 0), new WorldPos( 1,  3, 0),
                             new WorldPos( 3, -1, 0), new WorldPos( 3,  1, 0), new WorldPos(-3, -2, 0), new WorldPos(-3,  2, 0), new WorldPos(-2, -3, 0),
                             new WorldPos(-2,  3, 0), new WorldPos( 2, -3, 0), new WorldPos( 2,  3, 0), new WorldPos( 3, -2, 0), new WorldPos( 3,  2, 0),
                             new WorldPos(-4,  0, 0), new WorldPos( 0, -4, 0), new WorldPos( 0,  4, 0), new WorldPos( 4,  0, 0), new WorldPos(-4, -1, 0),
                             new WorldPos(-4,  1, 0), new WorldPos(-1, -4, 0), new WorldPos(-1,  4, 0), new WorldPos( 1, -4, 0), new WorldPos( 1,  4, 0),
                             new WorldPos( 4, -1, 0), new WorldPos( 4,  1, 0), new WorldPos(-3, -3, 0), new WorldPos(-3,  3, 0), new WorldPos( 3, -3, 0),
                             new WorldPos( 3,  3, 0), new WorldPos(-4, -2, 0), new WorldPos(-4,  2, 0), new WorldPos(-2, -4, 0), new WorldPos(-2,  4, 0),
                             new WorldPos( 2, -4, 0), new WorldPos( 2,  4, 0), new WorldPos( 4, -2, 0), new WorldPos( 4,  2, 0), new WorldPos(-5,  0, 0),
                             new WorldPos(-4, -3, 0), new WorldPos(-4,  3, 0), new WorldPos(-3, -4, 0), new WorldPos(-3,  4, 0), new WorldPos( 0, -5, 0),
                             new WorldPos( 0,  5, 0), new WorldPos( 3, -4, 0), new WorldPos( 3,  4, 0), new WorldPos( 4, -3, 0), new WorldPos( 4,  3, 0),
                             new WorldPos( 5,  0, 0), new WorldPos(-5, -1, 0), new WorldPos(-5,  1, 0), new WorldPos(-1, -5, 0), new WorldPos(-1,  5, 0),
                             new WorldPos( 1, -5, 0), new WorldPos( 1,  5, 0), new WorldPos( 5, -1, 0), new WorldPos( 5,  1, 0), new WorldPos(-5, -2, 0),
                             new WorldPos(-5,  2, 0), new WorldPos(-2, -5, 0), new WorldPos(-2,  5, 0), new WorldPos( 2, -5, 0), new WorldPos( 2,  5, 0),
                             new WorldPos( 5, -2, 0), new WorldPos( 5,  2, 0), new WorldPos(-4, -4, 0), new WorldPos(-4,  4, 0), new WorldPos( 4, -4, 0),
                             new WorldPos( 4,  4, 0), new WorldPos(-5, -3, 0), new WorldPos(-5,  3, 0), new WorldPos(-3, -5, 0), new WorldPos(-3,  5, 0),
                             new WorldPos( 3, -5, 0), new WorldPos( 3,  5, 0), new WorldPos( 5, -3, 0), new WorldPos( 5,  3, 0), new WorldPos(-6,  0, 0),
                             new WorldPos( 0, -6, 0), new WorldPos( 0,  6, 0), new WorldPos( 6,  0, 0), new WorldPos(-6, -1, 0), new WorldPos(-6,  1, 0),
                             new WorldPos(-1, -6, 0), new WorldPos(-1,  6, 0), new WorldPos( 1, -6, 0), new WorldPos( 1,  6, 0), new WorldPos( 6, -1, 0),
                             new WorldPos( 6,  1, 0), new WorldPos(-6, -2, 0), new WorldPos(-6,  2, 0), new WorldPos(-2, -6, 0), new WorldPos(-2,  6, 0),
                             new WorldPos( 2, -6, 0), new WorldPos( 2,  6, 0), new WorldPos( 6, -2, 0), new WorldPos( 6,  2, 0), new WorldPos(-5, -4, 0),
                             new WorldPos(-5,  4, 0), new WorldPos(-4, -5, 0), new WorldPos(-4,  5, 0), new WorldPos( 4, -5, 0), new WorldPos( 4,  5, 0),
                             new WorldPos( 5, -4, 0), new WorldPos( 5,  4, 0), new WorldPos(-6, -3, 0), new WorldPos(-6,  3, 0), new WorldPos(-3, -6, 0),
                             new WorldPos(-3,  6, 0), new WorldPos( 3, -6, 0), new WorldPos( 3,  6, 0), new WorldPos( 6, -3, 0), new WorldPos( 6,  3, 0),
                             new WorldPos(-7,  0, 0), new WorldPos( 0, -7, 0), new WorldPos( 0,  7, 0), new WorldPos( 7,  0, 0), new WorldPos(-7, -1, 0),
                             new WorldPos(-7,  1, 0), new WorldPos(-5, -5, 0), new WorldPos(-5,  5, 0), new WorldPos(-1, -7, 0), new WorldPos(-1,  7, 0),
                             new WorldPos( 1, -7, 0), new WorldPos( 1,  7, 0), new WorldPos( 5, -5, 0), new WorldPos( 5,  5, 0), new WorldPos( 7, -1, 0),
                             new WorldPos( 7,  1, 0), new WorldPos(-6, -4, 0), new WorldPos(-6,  4, 0), new WorldPos(-4, -6, 0), new WorldPos(-4,  6, 0),
                             new WorldPos( 4, -6, 0), new WorldPos( 4,  6, 0), new WorldPos( 6, -4, 0), new WorldPos( 6,  4, 0), new WorldPos(-7, -2, 0),
                             new WorldPos(-7,  2, 0), new WorldPos(-2, -7, 0), new WorldPos(-2,  7, 0), new WorldPos( 2, -7, 0), new WorldPos( 2,  7, 0),
                             new WorldPos( 7, -2, 0), new WorldPos( 7,  2, 0), new WorldPos(-7, -3, 0), new WorldPos(-7,  3, 0), new WorldPos(-3, -7, 0),
                             new WorldPos(-3,  7, 0), new WorldPos( 3, -7, 0), new WorldPos( 3,  7, 0), new WorldPos( 7, -3, 0), new WorldPos( 7,  3, 0),
                             new WorldPos(-6, -5, 0), new WorldPos(-6,  5, 0), new WorldPos(-5, -6, 0), new WorldPos(-5,  6, 0), new WorldPos( 5, -6, 0),
                             new WorldPos( 5,  6, 0), new WorldPos( 6, -5, 0), new WorldPos( 6,  5, 0) };

    public World world;

    List<WorldPos> updateList = new List<WorldPos>();
    List<WorldPos> buildList = new List<WorldPos>();

    int timer = 0;
	int loadtimer = 0;

    // Update is called once per frame
    void Update()
    {
		if (DeleteChunks ()) //return early if a delete happened
			return;
        FindChunksToLoad();
        LoadAndRenderChunks();
    }

    void FindChunksToLoad()
    {
        //Get the position of this gameobject to generate around
        WorldPos playerPos = new WorldPos(
            Mathf.FloorToInt(transform.position.x / Chunk.chunkSize) * Chunk.chunkSize,
            Mathf.FloorToInt(transform.position.y / Chunk.chunkSize) * Chunk.chunkSize, 0
			//Mathf.FloorToInt(transform.position.z / Chunk.chunkSize) * Chunk.chunkSize
            );

        //If there aren't already chunks to generate
		if (updateList.Count == 0)
        {
            //Cycle through the array of positions
            for (int i = 0; i < chunkPositions.Length; i++)
            {
                //translate the player position and array position into chunk position
                WorldPos newChunkPos = new WorldPos(
                    chunkPositions[i].x * Chunk.chunkSize + playerPos.x,
					chunkPositions[i].y * Chunk.chunkSize + playerPos.y,
                    0
                    );

                //Get the chunk in the defined position
                Chunk newChunk = world.GetChunk(
                    newChunkPos.x, newChunkPos.y, 0);

                //If the chunk already exists and it's already
                //rendered or in queue to be rendered continue
                if (newChunk != null
                    && (newChunk.rendered || updateList.Contains(newChunkPos)))
                    continue;

				for (int x = newChunkPos.x - Chunk.chunkSize; x <= newChunkPos.x + Chunk.chunkSize; x += Chunk.chunkSize) {
					for (int y = newChunkPos.y - Chunk.chunkSize; y <= newChunkPos.y + Chunk.chunkSize; y += Chunk.chunkSize) {
						buildList.Add (new WorldPos (
							x, y, 0));
					}
				}
				updateList.Add (new WorldPos(
					newChunkPos.x, newChunkPos.y, 0));
            }
        }
    }

    void LoadAndRenderChunks()
    {

		if (buildList.Count != 0) 
		{
			for (int i = 0; i < buildList.Count && i < 8; i++)
			{
				BuildChunk(buildList[0]);
					buildList.RemoveAt(0);
			}
			//if chunks built, return early
			return;
		}
		if (updateList.Count != 0)
		{
			Chunk chunk = world.GetChunk(updateList[0].x, updateList[0].y, updateList[0].z);
			if (chunk != null)
				chunk.update = true;
			updateList.RemoveAt(0);
		}

    }

    void BuildChunk(WorldPos pos)
    {
 		if (world.GetChunk(pos.x, pos.y, pos.z) == null)
			world.CreateChunk(pos.x, pos.y, pos.z);
    }

    bool DeleteChunks()
    {

        if (timer == 10)
        {
            var chunksToDelete = new List<WorldPos>();
            foreach (var chunk in world.chunks)
            {
                float distance = Vector3.Distance(
					new Vector3(chunk.Value.pos.x, chunk.Value.pos.y, 0),
					new Vector3(transform.position.x, transform.position.y, 0));


                if (distance > 256) {
					//Debug.Log(distance);
                    chunksToDelete.Add(chunk.Key);
				}
            }

            foreach (var chunk in chunksToDelete)
                world.DestroyChunk(chunk.x, chunk.y, chunk.z);

            timer = 0;
			return true;
        }

        timer++;
		return false;
    }
}