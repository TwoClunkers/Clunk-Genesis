using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;
using DataObjects;

public class World : MonoBehaviour {

    public Dictionary<WorldPos, Chunk> chunks = new Dictionary<WorldPos, Chunk>();
    public GameObject chunkPrefab;
	public GameObject pickupPrefab;
	public GameObject controllerPrefab;
	public GameObject master;
	public string worldName = "mech";

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
			Block block = new Block();
			block.material = 0;
            return block;
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

	public GameObject createController(float x, float y, float z)
	{
		return Instantiate (controllerPrefab, new Vector3 (x, y, z), Quaternion.identity) as GameObject;
	}
	
	public bool createPickUp(Pickup pickup) //creates an instance that holds the data object
	{		
		//get the pickup info from library
		ItemInfo info = new ItemInfo();
		if (!itemLibrary.getItemInfo (info, pickup.item.id)) {
			Debug.Log("Oh Noo Mr Billlll");
			return false;
		}
		Transform oPickup = PoolManager.Pools["drops"].Spawn(pickupPrefab, pickup.getPosition(), pickup.thisRotation);

		oPickup.GetComponent<MeshFilter>().mesh = info.mesh;
		oPickup.GetComponent<MeshCollider> ().sharedMesh = info.mesh;
		oPickup.GetComponent<MeshRenderer> ().material = info.material;
		//okay, we kinda need to populate the  new object with the pickup data...
		pickUpScript sPickup = oPickup.GetComponent("pickUpScript") as pickUpScript; 

		if (sPickup.pickup.copyPickup (pickup)) {

			return true;
		}
		else
			return false;
	}
	public Transform createPart(Part newPart) //creates an instance that holds the data object
	{		
		if (newPart == null)
			return null;
	
		//get the part info from library
		ItemInfo info = new ItemInfo();
		if (!itemLibrary.getItemInfo (info, newPart.item.id)) {

			return null;
		} 

		Transform oPart = PoolManager.Pools["parts"].Spawn(info.itemPrefab, newPart.getPosition(), newPart.thisRotation);

		oPart.GetComponent<MeshFilter>().mesh = info.mesh;
		oPart.GetComponent<MeshCollider> ().sharedMesh = info.mesh;
		oPart.GetComponent<Renderer>().material = info.material;
			
		//okay, we kinda need to populate the  new object with the pickup data...
		//this would mean that whatever the prefab just used, hopefully it has a partScript
		partScript sPart = oPart.GetComponent("partScript") as partScript; 
			
		//perhaps in the future we could see how to attach a script from the item data, which would correspond
		//directly to the type of object. 
		sPart.partData = newPart.getCopy ();

		return oPart;
	}
	public Transform createActor(Schematic schema) //builds from schema
	{		
		if (schema == null)
			return null;

		if (schema.parts.Count < 1)
			return null;

		Transform core = createPart (schema.getPart (0));

		if (core == null)
			return null;

		assembleActor (core, schema, 0); 
		return core;
	
	}
	public void assembleActor(Transform parentPart, Schematic schema, int index)
	{

		//reads node data to create child parts
		Part thisPart = schema.getPart (index);
		if (thisPart == null)
			return;
		else {
			for(int i=0; i<thisPart.nodes.Length; i+=1) {

				Node thisNode = thisPart.getNode(i);
				if(thisNode == null) continue;

				if(thisNode.equipped) {
					Debug.Log("node:yes");
					PartTag attachedTag = thisNode.getPartTag ();

					if (attachedTag == null)
						continue; //nothing attached

					Part attachedPart = schema.getPart (attachedTag.index);

					//now that we pulled the data we need..
					Transform partObject = createPart(attachedPart);
					if(partObject) {
						partObject.SetParent(parentPart);
						partObject.localPosition.Set (thisNode.localPosition.x,thisNode.localPosition.y,thisNode.localPosition.z);
						partObject.localRotation = Quaternion.Euler(thisNode.localRotation.x,thisNode.localRotation.y,thisNode.localRotation.z);
						assembleActor(partObject, schema, attachedTag.index);
					}
					else Debug.Log("Gag! no part!");
				}

			}

		}
	}
}
