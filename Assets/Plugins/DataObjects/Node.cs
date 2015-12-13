using UnityEngine;
using System.Collections;
using System;
namespace DataObjects
{
	[Serializable]
	public class Node
	{
		public Vector3 localPosition; //the offsets from this attachment's parent
		public Vector3 localRotation; //as above
		public ItemTypes typeRestriction; //what can go here

		PartTag attachedPart;
		public bool equipped = false;

		public Node ()
		{
		}

		public PartTag getPartTag()
		{
			if (equipped)
				return attachedPart;
			else
				return null;
		}

		public Node getCopy()
		{
			Node newNode = new Node ();
			newNode.localPosition.Set(localPosition.x, localPosition.y, localPosition.z);
			newNode.localRotation.Set (localRotation.x, localRotation.y, localRotation.z);
			newNode.attachedPart = new PartTag ();
			newNode.attachedPart.set (attachedPart.index, attachedPart.level, attachedPart.parent, attachedPart.node);
			newNode.equipped = equipped;
			newNode.typeRestriction = typeRestriction;

			return newNode;
		}

		public bool attach(PartTag tag)
		{
			if (equipped)
				return false;
			
			else {
				attachedPart = tag;
				equipped = true;
				return true;
			}
		}

		public void detach()
		{
			equipped = false;
			attachedPart = null;
		}

		public bool validate(Part newPart)
		{
			if (equipped)
				return false;
			if (!(newPart.type == typeRestriction)) //likely will need to be a more complex check
				return false;

			return true; //this node is empty and accepts this part type
		}

	}
}

