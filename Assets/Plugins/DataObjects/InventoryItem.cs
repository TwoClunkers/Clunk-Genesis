using UnityEngine;
using System.Collections;

namespace DataObjects
{
	public class InventoryItem 
	{
		public int id;
		public int quantity;

		public InventoryItem () 
		{
			id = 0;
			quantity = 0;
		}
		public void setInvItem(int newID, int newQty)
		{
			id = newID;
			quantity = newQty;
		}
		public bool IsEmpty () 
		{
			if(quantity < 1) return true;
			else return false;
		}
		public int Remove (int amount)
		{
			int moved = Mathf.Clamp(amount, 0, quantity); //get the amount we actually can remove
			quantity -= moved;
			amount -= moved;
			if(quantity < 1) id = 0;
			return amount; //this return value is what is still needed.
		}
		public int Add (int amount)
		{
			//NOTE: This limits stacks to 200 - we could insert a different compare here 
			int moved = Mathf.Clamp(amount, 0, 200 - quantity); //get the amount we actually can remove
			quantity += moved;
			amount -= moved;
			return amount; //this return value is what is still needed.
		}
	}
}

