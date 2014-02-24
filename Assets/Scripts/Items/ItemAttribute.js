class ItemAttribute {
	var name : String;
	var amount : float;
	var math : MathTypes;
	function ItemAttribute (newName : String, newAmount : float, newMath : MathTypes) {
		name = newName;
		amount = newAmount;
		math = newMath;
	}
	function getValue(base : float) {
		if(math == 0) return (base+amount);
		if(math == 1) return (base-amount);
		if(math == 2) return (base*amount);
		if(math == 4) {
			if(amount > 0) return (base/amount);
			else return amount;
		}
		if(math == 5) return Mathf.Max(base, amount);
		if(math == 6) return Mathf.Min(base, amount);
	}
}