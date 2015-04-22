using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class StorableObject
{
	public float localx;
	public float localy;
	public float localz;

	public Quaternion thisRotation;

	public int itemID;

	private WorldPos thisPos;

	// Use this for initialization
	public StorableObject ()
	{
	
	}
	
	public virtual void setRelative (float x, float y, float z, Quaternion rot)
	{
		localx = x;
		localy = y;
		localz = z;
		thisRotation = rot;
	}

	public virtual void setPosition (int x, int y, int z)
	{
		thisPos.x = x;
		thisPos.y = y;
		thisPos.z = z;
	}

}
