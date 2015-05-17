using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using DataObjects;

[Serializable]
public class Save
{
    public Dictionary<WorldPos, Block> blocks = new Dictionary<WorldPos, Block>();
	public Dictionary<WorldPos, Pickup> articles = new Dictionary<WorldPos, Pickup>();
	private GameObject[] pickups;
	
    public Save(Chunk chunk)
    {
		//pull our block data from chunk and store it in dictionary only if changed
		for (int x = 0; x < Chunk.chunkSize; x++)
        {
			for (int y = 0; y < Chunk.chunkSize; y++)
            {
				for (int z = 0; z < Chunk.chunkSize; z++)
                {
                    if (!chunk.blocks[x, y, z].changed) //if this block was changed, lets save the block and related articles
                        continue;

                    WorldPos pos = new WorldPos(x, y, z);
                    blocks.Add(pos, chunk.blocks[x, y, z]);
                }
            }
        }
		//pickup blocks should always be saved
		pickups = GameObject.FindGameObjectsWithTag("pickup");
		if(pickups.Length > 0) {
			Rect rbounds = new Rect(chunk.pos.x, chunk.pos.y, Chunk.chunkSize, Chunk.chunkSize);
			foreach (GameObject pickupObj in pickups)
			{
				if(rbounds.Contains(pickupObj.transform.position, true) )
				{
					//set position within the data structure before saving
					Pickup thisPickup = pickupObj.GetComponent<Pickup>();
					thisPickup.setPosition(pickupObj.transform.position, pickupObj.transform.rotation);
					articles.Add(thisPickup.getWorldPos(), thisPickup);
				}
			}
		}

    }
}