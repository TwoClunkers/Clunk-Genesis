#pragma strict

var items : AllItems;
var activeInventorySlot : int = -1;
var activeOverlayTexture : Texture;
var inventory : Container;

function Start(){
	items = GameObject.FindGameObjectWithTag("mc").GetComponent(ItemController).items;
	inventory = transform.GetComponent(Container);
	inventory.origenPos = Vector3(10,80,0);
	inventory.size = 10;
}

function Update() {

	
	
	
}

function OnGUI(){
	if(Event.current.isKey){
		if(Input.GetKeyUp("i")){
			if(!GameObject.FindGameObjectWithTag("mc").GetComponent(MasterScript).chatInputOpen){
				inventory.isOpen = !inventory.isOpen;
			}
		}
		
		var slotID : int = activeInventorySlot;
		
		if(Input.GetKeyUp(KeyCode.Alpha1)){
			slotID = 0;
		} else if(Input.GetKeyUp(KeyCode.Alpha2)){
			slotID = 1;
		} else if(Input.GetKeyUp(KeyCode.Alpha3)){
			slotID = 2;
		} else if(Input.GetKeyUp(KeyCode.Alpha4)){
			slotID = 3;
		} else if(Input.GetKeyUp(KeyCode.Alpha5)){
			slotID = 4;
		} else if(Input.GetKeyUp(KeyCode.Alpha6)){
			slotID = 5;
		} else if(Input.GetKeyUp(KeyCode.Alpha7)){
			slotID = 6;
		} else if(Input.GetKeyUp(KeyCode.Alpha8)){
			slotID = 7;
		} else if(Input.GetKeyUp(KeyCode.Alpha9)){
			slotID = 8;
		} else if(Input.GetKeyUp(KeyCode.Alpha0)){
			slotID = 9;
		}
		
		//GUI.DrawTexture(Rect(10+50*slotID, 180, 50, 50), activeOverlayTexture, 1);
		activeInventorySlot = slotID;
	}
	if(inventory.isOpen) GUI.DrawTexture(Rect(10+40*activeInventorySlot, Screen.height-60, 40, 40), activeOverlayTexture, 1);
}
