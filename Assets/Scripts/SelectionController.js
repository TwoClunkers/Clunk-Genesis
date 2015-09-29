#pragma strict

import DataObjects;

private var TerrainScript : Terra;

var positionhit : WorldPos;
var positionclick : WorldPos;
var blockhit : Block;
var blockset : Block;
var depth : float;
var toolmode : int;
var brushRadius : float;

var scrWorld : World;

var obCubeMarker : GameObject;
var obMarker : GameObject;
var trCamera : Transform;

var obPlayer : GameObject;
var inventory : PlayerInventory;

var obCanvas : GameObject;
var obUI : GameObject;
var obTool : GameObject;

var scrMarker : SelectedCube;
var targetPosition : Vector3;
var spritePlace : Sprite;
var spriteMine : Sprite;
var spriteEdit : Sprite;

function Start () {
	obPlayer = this.transform.parent.gameObject;
	inventory = obPlayer.GetComponent("PlayerInventory");
	
	obMarker = GameObject.Instantiate(obCubeMarker,transform.position,Quaternion.identity);
	scrMarker = obMarker.GetComponent("SelectedCube");
	trCamera = Camera.main.transform;
	obUI = GameObject.FindWithTag("UI");
	obTool = obUI.Find("Tool");
	scrWorld = GameObject.FindWithTag("world").GetComponent("World");
	
	
	toolmode = 1;
	obTool.GetComponent.<UnityEngine.UI.Image>().sprite = spriteMine;
	for(var point : GameObject in scrMarker.corner)
	{
		point.gameObject.SetActive(false);
	}
	obMarker.SetActive(false);
	
}

function Update () {

	if(Input.GetKeyDown(KeyCode.Q)) {
		toolmode += 1;
		if(toolmode>2) toolmode = 0;
		if(toolmode == 0) {
			obMarker.SetActive(true);
			for(var point : GameObject in scrMarker.corner) 
			{
				point.gameObject.SetActive(true);
			}
			obTool.GetComponent.<UnityEngine.UI.Image>().sprite = spriteEdit;
		}
		else {
			for(var point : GameObject in scrMarker.corner)
			{
				point.gameObject.SetActive(false);
			}
			obMarker.SetActive(false);
		}
		if(toolmode == 1) obTool.GetComponent.<UnityEngine.UI.Image>().sprite = spriteMine;
		if(toolmode == 2) obTool.GetComponent.<UnityEngine.UI.Image>().sprite = spritePlace;
	}
		
	
	if(Input.GetMouseButton(0)) {
		
		if(Input.GetKey(KeyCode.LeftShift)) {
			depth = 3;
			//ray.origin.z = 0;
		}
		else depth = 2;
		
		//create a target where we are pointing
		targetPosition = Camera.main.ScreenToWorldPoint(Vector3(Input.mousePosition.x,Input.mousePosition.y,depth-(Camera.main.transform.position.z)));
		targetPosition.z = depth; //
		
		positionhit = Terra.GetBlockPos(targetPosition);
    	blockhit = scrWorld.GetBlock(positionhit.x,positionhit.y,positionhit.z);

		scrMarker.targetPosition = targetPosition;
		scrMarker.pos = positionhit;
		
		//point to our target
//		var ray : Ray;
//		ray.origin = transform.position;
//		var rayLength : float = Vector3.Distance(targetPosition, transform.position);
//		ray.direction = targetPosition - ray.origin;
		
		//do a raycast
//		var hit : RaycastHit; 
		if(true) { //Vector3.Distance(targetPosition, transform.position)<8) {
			if(toolmode == 1) {
					pushVoxels(targetPosition);
		        	if(false) { //blockhit.material > 0) { //hit  DELETE
						if(Terra.DamageBlock(scrWorld.GetChunk(positionhit.x,positionhit.y,positionhit.z), positionhit, 10.0, transform.forward)) {
							var newPickup : Pickup = new Pickup();
							newPickup.reset(blockhit.material, 1);
							newPickup.setPosition(Vector3(positionhit.x, positionhit.y, 1.5), transform.rotation);
							GameObject.FindGameObjectWithTag("world").GetComponent(World).createPickUp(newPickup);
							GameObject.FindGameObjectWithTag("mc").GetComponent(NetworkView).RPC("playSound", RPCMode.All, "blockDestroyed", Vector3(positionhit.x,positionhit.y,positionhit.z));
						}
					
					//obMarker.transform.position = Vector3(positionhit.x, positionhit.y, positionhit.z);
					}
			}
			else if(toolmode == 2) { //we did not hit anything? PLACE
		        	if(blockhit.material == 0) {
						var item : InventoryItem = inventory.containerScript.pullCurrent(1);
						if(item.id > 0) { //not an air block Yay!
							blockset = new Block();
							blockset.material = item.id;
							blockset.changed = true;
							//blockset.Tile.y = item.id;
							Terra.SetBlock(scrWorld.GetChunk(positionhit.x,positionhit.y,positionhit.z), positionhit, blockset);
						}
					}
			}
    	}

	}
}

function pushVoxels (center : Vector3)
	{
		var pos_hit : WorldPos;
		var b_hit : Block;
		var distanceVtoC : float;
		var directionVtoC : Vector3;
		var radius : float = 4;
		
		var sizeX : int = (radius * 2) + 1;
		var sizeY : int = (radius * 2) + 1;
		
		for(var a : int = 0; a < sizeX; a+=1)
			for(var b : int = 0; b < sizeY; b+=1)
				for(var c : int = 0; c < sizeX; c+=1)
			{
				pos_hit = Terra.GetBlockPos(Vector3(center.x+a-(sizeX/2),center.y+b-(sizeY/2),center.z+c-(sizeX/2)));
    			b_hit = scrWorld.GetBlock(pos_hit.x,pos_hit.y,pos_hit.z);
    			//var Vpos : Vector3 = new Vector3(b_hit.offx + pos_hit.x, b_hit.offy +pos_hit.y, b_hit.offz + pos_hit.z);
    			var Vpos : Vector3 = new Vector3(0.5 + pos_hit.x, 0.5 +pos_hit.y, 0.5 + pos_hit.z);
    			distanceVtoC = Vector3.Distance(Vpos, center);
    			var push : float = radius/distanceVtoC;
    			
    			if(push > 1) {
    				var Vpush : Vector3 = (Vpos-center)*push;
    				Vpos = center+Vpush;
    				b_hit.offx = Mathf.Clamp(Vpos.x - pos_hit.x, 0.0, 1.0);
    				b_hit.offy = Mathf.Clamp(Vpos.y - pos_hit.y, 0.0, 1.0);
    				b_hit.offz = Mathf.Clamp(Vpos.z - pos_hit.z, 0.0, 1.0);
    				Terra.SetBlock(scrWorld.GetChunk(pos_hit.x,pos_hit.y,pos_hit.z), pos_hit, b_hit);
    			}
    			
    		}
	}
//		float distanceVtoC;
//		float radius = 1;
//		
//		int sizeX = radius x 2;
//		int sizeY = radius x 2;
//		
//		for(int a = 0; a < sizeX; a+=1)
//			for(int b = 0; b < sizeY; b+=1)
//			{
//				positionhit = Terra.GetBlockPos(Vector3(center.x+a,center.y+b,center.z));
//    			blockhit = scrWorld.GetBlock(positionhit.x,positionhit.y,positionhit.z);
//	}

	
