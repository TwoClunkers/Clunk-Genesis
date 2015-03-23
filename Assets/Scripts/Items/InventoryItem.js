class InventoryItem {
	var id : int;
	var quantity : int;
	function InventoryItem (newID : int, newQty : int) {
		id = newID;
		quantity = newQty;
	}
	function IsEmpty () : boolean {
		if(quantity < 1) return true;
		else return false;
	}
	function Remove (amount : int) : int {
		quantity -= Mathf.Max(0,Mathf.Min(amount,quantity)); //get the amount we actually can remove
		if(quantity < 1) id = 0;
		return quantity;
	}
}