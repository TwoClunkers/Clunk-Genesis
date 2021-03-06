using UnityEngine;
using System.Collections;
using System;

namespace DataObjects
{
	[Serializable]
	public class StorableObject
	{
		public InventoryItem item;

		public float localx;
		public float localy;
		public float localz;

		public float rotatex;
		public float rotatey;
		public float rotatez;

		[NonSerialized]
		public Quaternion thisRotation;
		public WorldPos thisPos;
		public Vector3 extentMax;
		public Vector3 extentMin;

		// Use this for initialization
		public StorableObject ()
		{

		}

		public InventoryItem invItem()
		{
			InventoryItem inv = new InventoryItem();
			inv.setInvItem( item.id, item.quantity);
			
			return inv;
		}

		public virtual void setPosition (Vector3 position, Quaternion rotation) 
		{
			thisPos = new WorldPos (
				Mathf.RoundToInt (position.x),
				Mathf.RoundToInt (position.y),
				Mathf.RoundToInt (position.z)
			);
			localx = position.x - thisPos.x;
			localy = position.y - thisPos.y;
			localz = position.z - thisPos.z;

			thisRotation = rotation;

			Vector3 rot3 = thisRotation.eulerAngles;
			rotatex = rot3.x;
			rotatey = rot3.y;
			rotatez = rot3.z;

		}

		public virtual void setPosition (int x, int y, int z)
		{
			thisPos.x = x;
			thisPos.y = y;
			thisPos.z = z;
		}
		
		public virtual void setRelative (float x, float y, float z, Quaternion rotation)
		{
			localx = x;
			localy = y;
			localz = z;

			thisRotation = rotation;
			Vector3 rot3 = thisRotation.eulerAngles;
			rotatex = rot3.x;
			rotatey = rot3.y;
			rotatez = rot3.z;
		}

		public virtual WorldPos getWorldPos ()
		{
			return thisPos;
		}

		public virtual Vector3 getPosition ()
		{
			return new Vector3 (localx + thisPos.x, localy + thisPos.y, localz + thisPos.z);
		}

		public virtual Quaternion getRotation ()
		{
			Quaternion temp = new Quaternion();
			temp = thisRotation;
			return temp;
		}

		public virtual bool getExtent(ItemLibrary items) 
		{
			ItemInfo info = new ItemInfo ();
			if (items.getItemInfo (info, item.id)) {
				extentMax = Vector3.one * info.size;
				extentMin = Vector3.one * (-info.size);
				return true;
			} else
				return false;
		}
	}

}
