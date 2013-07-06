class EquipmentGroupController {
	var slots : Container[];
	var isVisible : boolean;
	function EquipmentGroupController() {
		slots = new Container[12];
	}
	
	function Update () {
	}
	
	function UpdateEquipment () {
		//TODO: parse each slot and show/hide other slots based on what is equipped
	}
	
	function Show() { isVisible = true; }
	function Hide() { isVisible = false; }
}