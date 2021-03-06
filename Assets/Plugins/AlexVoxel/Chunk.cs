﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataObjects;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class Chunk : MonoBehaviour
{

    public Block[, ,] blocks = new Block[chunkSize, chunkSize, chunkSize];
	public List<Pickup> pickups = new List<Pickup> ();
	public List<GameObject> processes = new List<GameObject> ();

    public static int chunkSize = 16;
    public bool update = false;
    public bool rendered;

    MeshFilter filter;
	MeshCollider coll;
	public MeshRenderer rend;

    public World world;
    public WorldPos pos;

    void Start()
    {
        filter = gameObject.GetComponent<MeshFilter>();

		rend = gameObject.GetComponent<MeshRenderer>();
        coll = gameObject.GetComponent<MeshCollider>();
    }

    //Update is called once per frame
    void Update()
    {
        if (update)
        {
            update = false;
            UpdateChunk();
        }
    }

	public void OnSpawned()
	{
		update = true;
	}

    public Block GetBlock(int x, int y, int z)
    {
        if (InRange(x) && InRange(y) && InRange(z))
            return blocks[x, y, z];
        return world.GetBlock(pos.x + x, pos.y + y, pos.z + z);
    }

	public bool IsClear(int x, int y, int z, int endx, int endy, int endz)
	{
		for (int a = x; a < endx; a++)
		{
			for (int b = y; b < endy; b++)
			{
				for (int c = z; c < endz; c++)
				{
					Block check = GetBlock (a, b, c);
					if(check.IsHard()) return false;
				}
			}
		}
		return true;
	}
    public static bool InRange(int index)
    {
        if (index < 0 || index >= chunkSize)
            return false;

        return true;
    }

    public void SetBlock(int x, int y, int z, Block block)
    {

        if (InRange(x) && InRange(y) && InRange(z))
        {
            blocks[x, y, z] = block;
        }
        else
        {
            world.SetBlock(pos.x + x, pos.y + y, pos.z + z, block);
        }
    }

    public void SetBlocksUnmodified()
    {
        foreach (Block block in blocks)
        {
            block.changed = false;
        }
    }

    // Updates the chunk based on its contents
    void UpdateChunk()
    {
        rendered = true;
        MeshData meshData = new MeshData();

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
					Block thisBlock = blocks[x, y, z];
					thisBlock.Resolve(this, x, y, z);
					if(thisBlock is BlockAnchor) {
						GameObject response = processes.Find(obj=>obj.transform.position == new Vector3(pos.x + x, pos.y + y, pos.z + z));
						if(!response) {
							response = world.createController(x,y,z);
							processes.Add(response);
						}
					}
                }
            }
        }
		for (int x = 0; x < chunkSize; x++)
		{
			for (int y = 0; y < chunkSize; y++)
			{
				for (int z = 0; z < chunkSize; z++)
				{
					meshData = blocks[x, y, z].Blockdata(this, x, y, z, meshData);
				}
			}
		}
        RenderMesh(meshData);

		SpawnPickups();

    }

	void SpawnPickups()
	{
		for (int i = 0; pickups.Count > 0; i++) 
		{
			Pickup pickupData = new Pickup();
			pickupData.copyPickup(pickups[0]);
			//Debug.Log(pickupData.quantity.ToString() + "<- after serialize");
			pickups.RemoveAt(0);

			if(world.createPickUp(pickupData))
			{
				//Debug.Log("Success!");
			}
			else Debug.Log ("Fail :(");
		}
	}
    // Sends the calculated mesh information
    // to the mesh and collision components
    void RenderMesh(MeshData meshData)
    {
        filter.mesh.Clear();
		filter.mesh.subMeshCount = 2;
        filter.mesh.vertices = meshData.vertices.ToArray();
		filter.mesh.SetTriangles(meshData.triangles.ToArray(), 0);
		filter.mesh.SetTriangles(meshData.blendtriangles.ToArray(), 1);

        filter.mesh.uv = meshData.uv.ToArray();
		filter.mesh.uv2 = meshData.uv1.ToArray ();
		filter.mesh.normals = meshData.normals.ToArray ();
		//filter.mesh.colors32 = meshData.colors.ToArray ();

        //filter.mesh.RecalculateNormals();

        coll.sharedMesh = null;
        Mesh mesh = new Mesh();
        mesh.vertices = meshData.colVertices.ToArray();
        mesh.triangles = meshData.colTriangles.ToArray();
        mesh.RecalculateNormals();

        coll.sharedMesh = mesh;
    }

}