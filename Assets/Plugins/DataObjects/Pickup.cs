using UnityEngine;
using System.Collections;
using System;

namespace DataObjects
{
	[Serializable]
	public class Pickup : StorableObject
	{
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
			item = new InventoryItem();
			item.setInvItem (newID, newQuant);
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

		public bool stackCheck( float checktime )
		{
			if ((destroyAtThisTime - checktime) < 80)
				return true;
			else
				return false;
		}

		public bool copyPickup (Pickup source)
		{
			if (source == null)
				return false;


			destroyAtThisTime = source.destroyAtThisTime;
			
			lifeTime = source.lifeTime;
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

		public bool combinePickups(Pickup otherPickup)
		{
			if (item.quantity > 0 && otherPickup.item.quantity > 0) { //if both blocks contain items
				if (otherPickup.item.id == item.id) { //if the blocks are the same
					bool bCombineToOther = true;
					if (otherPickup.item.quantity > item.quantity) { //other block has more
						bCombineToOther = true;
					} else if (otherPickup.item.quantity < item.quantity) { //this block has more
						bCombineToOther = false;
					} else { // blocks have equal quantity, combine to the one dying later
						if (otherPickup.destroyAtThisTime > destroyAtThisTime) { //combine to other
							bCombineToOther = true;
						} else { //combine to this block
							bCombineToOther = false;
						}
					}
			
					if (bCombineToOther) {
						otherPickup.item.quantity += item.quantity;
						item.quantity = 0;
						otherPickup.destroyAtThisTime = Time.time + lifeTime;
						destroyAtThisTime = Time.time + 1.0f;
						return true;
					} else {
						item.quantity += otherPickup.item.quantity;
						otherPickup.item.quantity = 0;
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

