using UnityEngine;
using System.Collections;
using System;
namespace DataObjects
{
	[Serializable]
	public class Part : StorableObject
	{
		public PartTag tag; //used by a Schematic to organize parts
		public Node[] nodes;
		public ItemTypes type;

		//need to load from item
		public StatBlock baseStats;

		//states need to be saved
		public bool active = false;
		public float currentPower;
		public float currentHealth;
		public float powerIn; //we will take this if needed
		public float powerOut; //we will put power here if we have extra



		public Part ()
		{
		}

		public Part getCopy()
		{
			Part newPart = new Part ();
			newPart.tag = new PartTag ();
			newPart.tag.set (tag.index, tag.level, tag.parent, tag.node);
			//newPart.nodes = new Node[nodes.Length];
			newPart.nodes = nodes.Clone() as Node[];
			newPart.type = type;

			newPart.currentPower = currentPower;
			newPart.active = active;
			newPart.powerIn = powerIn; 
			newPart.powerOut = powerOut; 
			
			newPart.item = invItem();
			
			newPart.localx = localx;
			newPart.localy = localy;
			newPart.localz = localz;
			
			newPart.rotatex = rotatex;
			newPart.rotatey = rotatey;
			newPart.rotatez = rotatez;
			
			newPart.thisRotation = thisRotation;
			newPart.thisPos = thisPos;


			return newPart;
		}

		public Node getNode(int nodeIndex)
		{
			//verifies the index

			if (nodes.Length == 0) //no nodes created
				return null;
			if ((nodes.Length-1) < nodeIndex) //outside our range
				return null;
			if (nodeIndex < 0) //again outside our range
				return null;

			return nodes [nodeIndex];
		}

		public bool clearNode(int nodeIndex)
		{
			Node targetNode = getNode (nodeIndex);
			if (targetNode == null)
				return false;
			else {
				targetNode.detach ();
				return true;
			}
		}

		public Node getCompatableNode(Part newPart)
		{
			for (int i=0; i < nodes.Length; i+=1) {
				if(nodes[i].validate(newPart)) {
					return nodes[i];
				}
			}
			//if we are here, then nothing validated
			return null;
		}

		public void createFromItem (InventoryItem keyItem, ItemLibrary items)
		{
			item = new InventoryItem ();
			item.setInvItem (keyItem.id, keyItem.quantity);

			items.getItemAttachments (keyItem.id, out nodes);
			items.getItemStats (keyItem.id, out baseStats);

			currentPower = 0.0f;
			currentHealth = baseStats.stability;
			powerIn = 0.0f; //we will take this if needed
			powerOut = 0.0f; //we will put power here if we have extra
			type = items.getItemType (keyItem.id);
			
			Debug.Log ("LLLAAAMMAAASS");
		}



	}
}

