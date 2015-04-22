using UnityEngine;
using System.Collections;

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
		quantity -= Mathf.Max(0,Mathf.Min(amount,quantity)); //get the amount we actually can remove
		if(quantity < 1) id = 0;
		return quantity;
	}

}

