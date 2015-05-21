
using System;
using DataObjects;
using UnityEngine;

namespace AssemblyCSharpfirstpass
{
	[Serializable]
	public class Container
	{
		string name = "Generic";
		//containerType; used to limit what can be stored.

		public int size;
		public InventoryItem[] contents;

		public Container (int newsize)
		{
			size = newsize;
			contents = new InventoryItem[size];
		}
			
		public InventoryItem getItem (int slot)
		{
			if (size > slot)
				return contents [slot];
			else
				return new InventoryItem ();

		}

		public int getEmptySlot () 
		{
			for(var i=0;i<size;i++) {
				if(contents[i].quantity < 1) return i;
			}
			return -1;
		}

		public int getMatchingSlot (int compareID)
		{
			for(var i=0;i<size;i++) {
				if(contents[i].id == compareID) return i;
			}
			return -1;
		}

		public int pullItem (InventoryItem requestedItem)
		{
			//subtract a single item stack from this container, return amount we could not subtract
			int quantityneeded = requestedItem.quantity;
			int quantitymoved = 0;
						
			for (var i=0; i<size; i++) {
				if (requestedItem.id == contents [i].id) {
					quantityneeded -= contents [i].Remove(quantityneeded);
				
					//if we got all we needed, we can exit
					if (quantityneeded < 1)
						return 0;
				}
			}
			//we did not grab it all
			return quantityneeded;
		}

		public int addItem(InventoryItem itemstack)
		{
			//this code makes the assumption that quantity is limited to a single stack
			int quantityleft = itemstack.quantity;
			int quantitymoved = 0;
			for (var i=0; i<size; i++) {
				if (itemstack.id == contents [i].id) {
					Debug.Log ("Match Found");
					quantityleft = contents [i].Add(quantityleft);
				}
			}
			if (quantityleft > 0) {
				int firstempty = getEmptySlot ();
				while (firstempty > -1) {
					quantityleft = contents [firstempty].Add(quantityleft);
					//if we placed it all, we can exit here
					if (quantityleft < 1)
						return 0;
					else
						firstempty = getEmptySlot ();
				}
				Debug.Log ("Too much!");
				//we could not find an more empty slots
				return quantityleft;
			}
			return 0;
		}

		public int pullSlot(int slot, int amount)
		{
			//limits pull to specified slot
			if (size > slot) {
				InventoryItem thisStack = contents [slot];
				if (thisStack.IsEmpty ())
					return amount;
				else
					return thisStack.Remove (amount);//this return value is what is still needed.
			}
			Debug.Log ("Error: slot out of range");
			return amount;//this return value is what is still needed.
		}

		public int addSlot(int slot, InventoryItem itemstack)
		{
			//limits pull to specified slot
			if (size > slot) {
				InventoryItem thisStack = contents [slot];
				if (thisStack.IsEmpty ()) thisStack.id = itemstack.id;
				if (thisStack.id == itemstack.id) {
					int amount = thisStack.Add(itemstack.quantity);
					return amount;
				}
				Debug.Log ("Error: mismatched id");
				return itemstack.quantity;
			}
			Debug.Log ("Error: slot out of range");
			return itemstack.quantity;
		}

		public InventoryItem swapSlot(int slot, InventoryItem swapItem)
		{
			InventoryItem thisStack = new InventoryItem ();
			if ((size > slot) && (slot > -1)) {
				thisStack = contents [slot];
				contents [slot] = swapItem;
			}
			return thisStack;
		}
	}


//			function pollInputSingle(process : ProcessTypes) { 
//				//checks for a single stack that can be broken by the given process
//				//returns a recipe if available, or -1 if no stacks can be broken with this process
//				
//				var checkItem : ItemInfo;
//				for(var i=0;i<size;i++) {
//					checkItem = items.library[contents[i].id]; //get the item info for this slot
//					//check that we have the right process
//					//		if(process == checkItem.construct.process) {
//					//			if(checkItem.construct.product.quantity < (contents[i].quantity+1)) {
//					//				pollRecipe = checkItem.construct; //we have supply and tool to make this recipe
//					//				return true;
//					//			}
//					//		}
//				}
//				//Debug.Log("NO MATCH");
//				return false; //no match after checking all slots
//			}
//			
//			function pollOutputMultiple(checkRecipe : Split) {
//				//checks to see if given recipe can fit in available space
//				//returns true if it can, false if it cannot
//				
//				var needempty = 0;
//				for(var i=0;i<checkRecipe.components.length;i++) {
//					var match = false;
//					for(var a=0;a<size;a++) {
//						if(checkRecipe.components[i].id == contents[a].id) {
//							if(checkRecipe.components[i].quantity < (200-contents[a].quantity)) {
//								match = true; //same type with enough room
//							}
//						}
//					}
//					if(!match) needempty += 1; //we will need an empty for each that was not matched with a partial
//				}
//				if(empty < needempty) return false;
//				else return true;
//			}
//			

//				
//			}
//			





}

