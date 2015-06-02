using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;
using DataObjects;

public class World : MonoBehaviour {

    public Dictionary<WorldPos, Chunk> chunks = new Dictionary<WorldPos, Chunk>();
    public GameObject chunkPrefab;
	public GameObject pickupPrefab;
	public GameObject master;
	public string worldName = "pickup";

	public ItemLibrary itemLibrary; 

	void Start()
	{
		itemLibrary = GameObject.FindGameObjectWithTag ("mc").GetComponent<ItemLibrary> ();
	}

    public void CreateChunk(int x, int y, int z)
    {
        WorldPos worldPos = new WorldPos(x, y, z);

        //Instantiate the chunk at the coordinates using the chunk prefab
        GameObject newChunkObject = Instantiate(
                        chunkPrefab, new Vector3(x, y, z),
                        Quaternion.Euler(Vector3.zero)
                    ) as GameObject;

        Chunk newChunk = newChunkObject.GetComponent<Chunk>(); //gets the script component

        newChunk.pos = worldPos; //rounds position to an int
        newChunk.world = this; //tells the chunk how to get back to mommy
		newChunk.tag = "breakable";

        //Add it to the chunks dictionary with the position as the key
        chunks.Add(worldPos, newChunk);

        TerrainGen terrainGen = new TerrainGen(); //this has some preset values for generating 
        newChunk = terrainGen.ChunkGen(newChunk); //this runs the generation code

        newChunk.SetBlocksUnmodified(); //this tells Serialization not to save as there was no changes

        Serialization.Load(newChunk);
    }

	public void SaveAll()
	{
		int savecount = 0;
		foreach(var objchunk in chunks) 
		{
			if(objchunk.Key.z != 0) continue;
			Chunk data = null;

			if (chunks.TryGetValue(new WorldPos(objchunk.Key.x, objchunk.Key.y, objchunk.Key.z), out data))
			{
				Serialization.SaveChunk(data);
				savecount+=1;
			}
		}
		Debug.Log("Count:" + savecount);
	}

    public void DestroyChunk(int x, int y, int z)
    {
        Chunk chunk = null;
        if (chunks.TryGetValue(new WorldPos(x, y, z), out chunk))
        {
            Serialization.SaveChunk(chunk);
            Object.Destroy(chunk.gameObject);
            chunks.Remove(new WorldPos(x, y, z));
        }
    }

    public Chunk GetChunk(int x, int y, int z)
    {
        WorldPos pos = new WorldPos();
        float multiple = Chunk.chunkSize;
        pos.x = Mathf.FloorToInt(x / multiple) * Chunk.chunkSize;
        pos.y = Mathf.FloorToInt(y / multiple) * Chunk.chunkSize;
        pos.z = Mathf.FloorToInt(z / multiple) * Chunk.chunkSize;

        Chunk containerChunk = null;

        chunks.TryGetValue(pos, out containerChunk);

        return containerChunk;
    }

    public Block GetBlock(int x, int y, int z)
    {
        Chunk containerChunk = GetChunk(x, y, z);

        if (containerChunk != null)
        {
            Block block = containerChunk.GetBlock(
                x - containerChunk.pos.x,
                y - containerChunk.pos.y,
                z - containerChunk.pos.z);

            return block;
        }
        else
        {
            return new BlockAir();
        }

    }

    public void SetBlock(int x, int y, int z, Block block)
    {
        Chunk chunk = GetChunk(x, y, z);

        if (chunk != null)
        {
			//we update the chunk that we are in
            chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);
            chunk.update = true;

			//if we alter a block at the beginning or end of a chunk, we update the adjacent chunk
            UpdateIfEqual(x - chunk.pos.x, 0, new WorldPos(x - 1, y, z));
            UpdateIfEqual(x - chunk.pos.x, Chunk.chunkSize - 1, new WorldPos(x + 1, y, z));
            UpdateIfEqual(y - chunk.pos.y, 0, new WorldPos(x, y - 1, z));
            UpdateIfEqual(y - chunk.pos.y, Chunk.chunkSize - 1, new WorldPos(x, y + 1, z));
            UpdateIfEqual(z - chunk.pos.z, 0, new WorldPos(x, y, z - 1));
            UpdateIfEqual(z - chunk.pos.z, Chunk.chunkSize - 1, new WorldPos(x, y, z + 1));
        
        }
    }

    void UpdateIfEqual(int value1, int value2, WorldPos pos)
    {
        if (value1 == value2)
        {
            Chunk chunk = GetChunk(pos.x, pos.y, pos.z);
            if (chunk != null)
                chunk.update = true;
        }
    }
	
	public bool createPickUp(Pickup pickup) //creates an instance that holds the data object
	{		
		//get the pickup info from library
		ItemInfo info = new ItemInfo();
		if (!itemLibrary.getItemInfo (info, pickup.itemID)) {
			Debug.Log("Oh Noo Mr Billlll");
			return false;
		}
		Transform oPickup = PoolManager.Pools["drops"].Spawn(pickupPrefab, pickup.getPosition(), pickup.thisRotation);


		//okay, we kinda need to populate the  new object with the pickup data...
		pickUpScript sPickup = oPickup.GetComponent("pickUpScript") as pickUpScript; 


		return true;
	}
	
}
