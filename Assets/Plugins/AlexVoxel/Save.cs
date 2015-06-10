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
	
    public Save(Chunk chunk)
    {
		Debug.Log ("saveattempt blocks");
		//pull our block data from chunk and store it in dictionary only if changed
		for (int x = 0; x < Chunk.chunkSize; x++)
        {
			for (int y = 0; y < Chunk.chunkSize; y++)
            {
				for (int z = 0; z < Chunk.chunkSize; z++)
                {
                    if (chunk.blocks[x, y, z].changed) //if this block was changed, lets save the block and related articles
					{
	                    WorldPos pos = new WorldPos(x, y, z);
	                    blocks.Add(pos, chunk.blocks[x, y, z]);
					}
                }
            }
        }
		//pickup blocks should always be saved
		GameObject[] drops;
		drops = GameObject.FindGameObjectsWithTag("pickup");
		if(drops.Length > 0) {
			Debug.Log ("saveattempt pickups");
			Rect rbounds = new Rect(chunk.pos.x, chunk.pos.y, Chunk.chunkSize, Chunk.chunkSize);
			foreach (GameObject pickupObj in drops)
			{

				if(rbounds.Contains(pickupObj.transform.position, true) )
				{
					 
						//Debug.Log ("saveattempt single pickup");
						
					
					//set position within the data structure before saving
					Pickup thisPickup = new Pickup();
					thisPickup.copyPickup(pickupObj.GetComponent<pickUpScript>().pickup);

					thisPickup.setPosition(pickupObj.transform.position, pickupObj.transform.rotation);
					//Debug.Log(thisPickup.quantity.ToString());
					//Debug.Log(thisPickup.itemID.ToString() + "<- added");
					articles.Add(thisPickup.getWorldPos(), thisPickup);
				}
			}
		}

    }
}