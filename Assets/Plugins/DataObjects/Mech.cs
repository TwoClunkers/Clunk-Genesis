using UnityEngine;
using System.Collections;
using System;

namespace DataObjects
{
	[Serializable]
	public class Mech : StorableObject
	{

		//neet to load from item
		public Attachment[] slots;
		public StatBlock baseStats;

		private Attachment coupler; //pointer to where we attach

		//states need to be saved
		public bool active = false;
		public float currentPower;
		public float currentHealth;
		public float powerIn; //we will take this if needed
		public float powerOut; //we will put power here if we have extra


		public Mech ()
		{


		}

		public void setAttached(Attachment newAttachment)
		{
			coupler = newAttachment;
		}

		public void createFromItem (InventoryItem keyItem, ItemLibrary items)
		{
			slots = items.getItemAttachments (keyItem.id);
			baseStats = items.getItemStats (keyItem.id);
			currentPower = 0.0f;
			currentHealth = baseStats.stability;
			powerIn = 0.0f; //we will take this if needed
			powerOut = 0.0f; //we will put power here if we have extra

		}

		public bool copyMech (Mech source)
		{
			if (source == null)
				return false;

			currentPower = source.currentPower;
			active = source.active;
			slots = source.slots;
			
			coupler = source.coupler; 
			
			powerIn = source.powerIn; 
			powerOut = source.powerOut; 

			item = source.invItem();
			
			localx = source.localx;
			localy = source.localy;
			localz = source.localz;
			
			rotatex = source.rotatex;
			rotatey = source.rotatey;
			rotatez = source.rotatez;
			
			thisRotation = source.thisRotation;
			thisPos = source.thisPos;
			
			return true;
		}

		public void reset(int newID, int newQuant)
		{//we need to load this baby from our item library!
			item = new InventoryItem();
			item.setInvItem (newID, newQuant);
		}

		public override bool getExtent(ItemLibrary items) 
		{
			ItemInfo info = new ItemInfo ();
			if (items.getItemInfo (info, item.id)) {
				extentMax = Vector3.one * info.size;
				extentMin = Vector3.one * (-info.size);
			} else
				return false;

			return true;
		}

	}
}
