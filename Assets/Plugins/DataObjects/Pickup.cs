using UnityEngine;
using System.Collections;
using System;

namespace DataObjects
{
	[Serializable]
	public class Pickup : StorableObject
	{
		public int quantity;
		public float destroyAtThisTime;
		
		private float lifeTime; //usually this is static anyway...

		// Use this for initialization
		public Pickup ()
			: base()
		{
			lifeTime = 100;
		}

		public void reset(int newID, int newQuant)
		{
			destroyAtThisTime = Time.time + lifeTime;
			itemID = newID;
			quantity = newQuant;
			//Debug.Log (itemID.ToString ()+"  reset");
		}

		public void OnLoad()
		{
			thisRotation = Quaternion.Euler (rotatex, rotatey, rotatez);

		}

		public bool destroyCheck( float checktime )
		{
			if (checktime >= destroyAtThisTime)
				return true;
			else
				return false; 
		}

		public InventoryItem invItem()
		{
			InventoryItem inv = new InventoryItem();
			inv.setInvItem( itemID, quantity);

			return inv;
		}

		public bool copyPickup (Pickup destination)
		{
			if (destination == null)
				return false;

			quantity = destination.quantity;
			destroyAtThisTime = destination.destroyAtThisTime;
			
			lifeTime = destination.lifeTime;
			itemID = destination.itemID;
			
			localx = destination.localx;
			localy = destination.localy;
			localz = destination.localz;
			
			rotatex = destination.rotatex;
			rotatey = destination.rotatey;
			rotatez = destination.rotatez;

			thisRotation = destination.thisRotation;
			thisPos = destination.thisPos;

			return true;
		}

		public bool combinePickups(Pickup otherPickup)
		{
			if (quantity > 0 && otherPickup.quantity > 0) { //if both blocks contain items
				if (otherPickup.itemID == itemID) { //if the blocks are the same
					bool bCombineToOther = true;
					if (otherPickup.quantity > quantity) { //other block has more
						bCombineToOther = true;
					} else if (otherPickup.quantity < quantity) { //this block has more
						bCombineToOther = false;
					} else { // blocks have equal quantity, combine to the one dying later
						if (otherPickup.destroyAtThisTime > destroyAtThisTime) { //combine to other
							bCombineToOther = true;
						} else { //combine to this block
							bCombineToOther = false;
						}
					}
			
					if (bCombineToOther) {
						otherPickup.quantity += quantity;
						quantity = 0;
						otherPickup.destroyAtThisTime = Time.time + lifeTime;
						destroyAtThisTime = Time.time + 1.0f;
						return true;
					} else {
						quantity += otherPickup.quantity;
						otherPickup.quantity = 0;
						otherPickup.destroyAtThisTime = Time.time + 1.0f;
						destroyAtThisTime = Time.time + lifeTime;
						return true;
					}
				}
				return false;
			}
			return false;
		}
	}

}

