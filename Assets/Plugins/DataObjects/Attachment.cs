using UnityEngine;
using System.Collections;
using System;

namespace DataObjects
{
	public class Attachment
	{
		public String name; //to describe this specific attachment point
		public ItemTypes attachmentType; //used to limit attachments
		public StorableObject currentEquipped; //when something is attached
		public Vector3 localPosition; //the offsets from this attachment's parent
		public Vector3 localRotation; //as above
		public bool equipped = false;
		public float stability; //strength of connection




		public Attachment ()
		{
		}

		public bool attackConnection(float force, float damage)
		{
			stability -= damage; //perminant
			if (stability < force) { //temporary
				detach ();
				return true;
			} else
				return false;
		}
		
		public bool isCompatable (ItemTypes thisType)
		{
			if (thisType == attachmentType)
				return true;
			else
				return false;
		}
		
		public bool attach(StorableObject newObject, ItemLibrary items)
		{
			if (equipped)
				return false;

			ItemInfo info = new ItemInfo ();
			if (items.getItemInfo (info, newObject.item.id)) {
				if (isCompatable (info.type)) {
					currentEquipped = newObject;
					equipped = true;
					return true;
				}
			}

			return false;
		}

		public bool detach()
		{
			if (!equipped)
				return false;

			currentEquipped = null;
			equipped = false;
			return true;
		}
		
	}
}
