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

var EffectPrefab : GameObject;
var ourBeam : GameObject;
var beamScript : Effect;
var beamActive : boolean = false;
var AreaMarker : SelectedArea;
var AreaObject : GameObject;
var thisArea : GameObject;
var makingArea : boolean;
var mouseController : MouseHandler;

function Start () {
	
	mouseController = GameObject.FindWithTag("mc").GetComponent("MouseHandler");

	obPlayer = this.transform.parent.gameObject;
	inventory = obPlayer.GetComponent("PlayerInventory");
	
	obMarker = GameObject.Instantiate(obCubeMarker,transform.position,Quaternion.identity);
	scrMarker = obMarker.GetComponent("SelectedCube");
	trCamera = Camera.main.transform;
	obUI = GameObject.FindWithTag("UI");
	obTool = obUI.Find("Tool");
	scrWorld = GameObject.FindWithTag("world").GetComponent("World");
	thisArea = GameObject.Instantiate(AreaObject,Vector3.zero,Quaternion.identity);
	AreaMarker = thisArea.GetComponent("SelectedArea");
	makingArea = false;
	
	toolmode = 1;
	brushRadius = 0.8;
	obTool.GetComponent.<UnityEngine.UI.Image>().sprite = spriteMine;
	for(var point : GameObject in scrMarker.corner)
	{
		point.gameObject.SetActive(false);
	}
	obMarker.SetActive(false);
	
}

function Update () {
	var ray : Ray;
	var rayLength : float;
	var hit : RaycastHit;
	
	if(mouseController.incRadius) brushRadius += 0.2f;
	else if(mouseController.decRadius) brushRadius -= 0.2f;
	if(brushRadius < 0.2f) brushRadius = 0.2f;
	
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
				point.gameObject.SetActive(true);//false
			}
			obMarker.SetActive(true);//false
		}
		if(toolmode == 1) obTool.GetComponent.<UnityEngine.UI.Image>().sprite = spriteMine;
		if(toolmode == 2) obTool.GetComponent.<UnityEngine.UI.Image>().sprite = spritePlace;
	}
		
	if(Input.GetMouseButton(0)) {
		if(toolmode == 0) { //edit mode  lets use a mouse to screen ray to find the block
			//create a target where we are pointing
			targetPosition = Camera.main.ScreenToWorldPoint(Vector3(Input.mousePosition.x,Input.mousePosition.y,depth-(Camera.main.transform.position.z)));
			ray.origin = Camera.main.transform.position;
			rayLength = Vector3.Distance(targetPosition, ray.origin);
			ray.direction = targetPosition - ray.origin;
			
			if(rayLength<50) {
				if(Physics.Raycast(ray, hit)) {
					if(hit.collider != null) {
						targetPosition = hit.point;
					}
					else return;
				}
				else return;
			}	
			
			if(!makingArea) { 
				makingArea = true;
				AreaMarker.setStartCorner(targetPosition);
				AreaMarker.update = true;
			}
			else { 
				AreaMarker.setEndCorner(targetPosition);
				AreaMarker.update = true;
			}
			
			obMarker.transform.position = targetPosition;
			return;
		}
		
		else {
			if(Input.GetKey(KeyCode.LeftShift)) {
				depth = 4;
				//ray.origin.z = 0;
			}
			else depth = 2;
			
			//create a target where we are pointing
			targetPosition = Camera.main.ScreenToWorldPoint(Vector3(Input.mousePosition.x,Input.mousePosition.y,depth-(Camera.main.transform.position.z)));
			targetPosition.z = depth; //
			
			//point to our target
			ray.origin = transform.position;
			rayLength = Vector3.Distance(targetPosition, transform.position);
			ray.direction = (targetPosition - ray.origin) + (Vector3(0,0,Random.value-1.0f)*1.5);
			
			if(toolmode == 1) {
				if(rayLength<20) {
					//do a raycast
					if(Physics.Raycast(ray, hit)) {
						if(hit.collider != null) {
							targetPosition = hit.point;
						}
					}
				}
			}
			obMarker.transform.position = targetPosition;
			
			positionhit = Terra.GetBlockPos(targetPosition);
	    	blockhit = scrWorld.GetBlock(positionhit.x,positionhit.y,positionhit.z);

			scrMarker.targetPosition = targetPosition;
			scrMarker.pos = positionhit;
			
			
			if(toolmode == 1) {
					pushVoxels(targetPosition);
		        	//if(false) 
	//	        	{ //blockhit.material > 0) { //hit  DELETE
	//					if(Terra.DamageBlock(scrWorld.GetChunk(positionhit.x,positionhit.y,positionhit.z), positionhit, 10.0, transform.forward)) {
	//						var newPickup : Pickup = new Pickup();
	//						newPickup.reset(blockhit.material, 1);
	//						newPickup.setPosition(Vector3(positionhit.x, positionhit.y, 1.5), transform.rotation);
	//						GameObject.FindGameObjectWithTag("world").GetComponent(World).createPickUp(newPickup);
	//						GameObject.FindGameObjectWithTag("mc").GetComponent(NetworkView).RPC("playSound", RPCMode.All, "blockDestroyed", Vector3(positionhit.x,positionhit.y,positionhit.z));
	//					}
	//				
	//				//obMarker.transform.position = Vector3(positionhit.x, positionhit.y, positionhit.z);
	//				}
			}
			else if(toolmode == 2) { //we did not hit anything? PLACE
	        	if(blockhit.material == 0) {
					var item : InventoryItem = inventory.containerScript.pullCurrent(1);
					if(item.id > 0) { //not an air block Yay!
						blockset = new Block();
						blockset.material = item.id;
						blockset.changed = true;
						blockset.setoffset(0.5f, 0.5f, 0.5f);
						blockset.setvariant(positionhit.x,positionhit.y);
						//blockset.Tile.y = item.id;
						Terra.SetBlock(scrWorld.GetChunk(positionhit.x,positionhit.y,positionhit.z), positionhit, blockset);
					}
				}
			}
		}
    }
	else {
		if(beamActive) {
			if(ourBeam != null) {
				
				beamScript = ourBeam.GetComponent("Effect");
				beamScript.endtime = Mathf.Min(beamScript.endtime, Time.time + 0.1f);
				Debug.Log("beam on");
				//point to our target
				ray.origin = transform.position;
				rayLength = 200;
				var raydir : Vector3 = new Vector3();
				raydir = (scrMarker.targetPosition - ray.origin) + (Vector3(0,0,Random.value-1.0f)*1.5);
				ray.direction = raydir.normalized;
				//do a raycast
				if(Physics.Raycast(ray, hit, 200)) {
					Debug.Log("ray");
					Debug.DrawLine(ray.origin, hit.point);
					if(hit.collider != null) {
						Debug.Log(beamScript.power);
						targetPosition = hit.point;
						obMarker.transform.position = targetPosition;
					}
				}
				Terra.applyCircle(scrWorld, targetPosition, brushRadius, -2, 100);
				scrMarker.targetPosition = targetPosition;
			}
			else beamActive = false;
		}
		makingArea = false;
	}
}

function pushVoxels (center : Vector3)
	{
		//Terra.pushCircle(scrWorld, center, 2.0f, -2, 50);
		
		
		
		if(beamActive) {
			if(ourBeam != null) {
				Terra.applyCircle(scrWorld, targetPosition, brushRadius, -2, 50);
				beamScript = ourBeam.GetComponent("Effect");
				beamScript.addPower(0.2f);
			}
			else beamActive = false;
		}
		else {
			Terra.applyCircle(scrWorld, targetPosition, brushRadius, -2, 50);
			ourBeam = Instantiate(EffectPrefab, transform.position, Quaternion.identity);
			beamActive = true;
			//eo.transform.SetParent(this.transform);
			beamScript = ourBeam.GetComponent("Effect");
			beamScript.foci = obMarker.transform;
			beamScript.moving = true;
			beamScript.setEffect(this.transform, center, true, true);
		}
	}

