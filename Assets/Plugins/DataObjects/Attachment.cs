using UnityEngine;
using System.Collections;
using System;

namespace DataObjects
{
	[Serializable]
	public class Attachment
	{
		public GameObject owner;
		public GameObject equippedObject;
		public bool equipped = false;
		public Vector3 localPosition; //the offsets from this attachment's parent
		public Vector3 localRotation; //as above
		public float stability; //strength of connection
		public ItemTypes attachmentType; //used to limit attachments
		public String name; //to describe this specific attachment point

		public Attachment ()
		{
			localPosition = new Vector3 ();
			localRotation = new Vector3 ();
			stability = new float();
			attachmentType = ItemTypes.bot_core;
			name = "empty";
		}

		public void setAttachment(GameObject newOwner, Attachment source)
		{
			equipped = false;
			owner = newOwner;

			localPosition = source.localPosition;
			localRotation = source.localRotation;
			stability = source.stability;
			attachmentType = source.attachmentType;
			name = source.name;
		}



		public bool attackConnection(float force)
		{
			if (stability < force) {
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
		
		public bool attach(GameObject newObject, Mech mech, ItemTypes thisType)
		{
			if (equipped)
				return false;

			if (isCompatable (thisType)) {
				newObject.transform.SetParent(owner.transform);
				mech.setAttached(this);
				equippedObject = newObject;
				equipped = true;
				return true;
			}

			return false;
		}

		public bool detach()
		{
			if (!equipped)
				return false;
			equippedObject = null;
			equipped = false;
			return true;
		}
		
	}
}
