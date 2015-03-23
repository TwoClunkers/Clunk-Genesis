#pragma strict

import PathologicalGames;

var master : GameObject;
var cam : GameObject;

var oldx : float;
var oldy : float;

var globalBlockScale : float;
var globalChunkSize : float;
var CL : float;
var ZL = 512;
var block : GameObject;
var zoneObj : GameObject;
var pickupblock : GameObject;
var items : AllItems;
var spawnpos : Vector3;
var zoneMap : Chunk[];

var seed : float; //seeds used for different purporses
var cordx : float;
var cordy : float;
var xSeed : float;
var ySeed : float;
var bSeed : float;
var iSeed : float;

var r_start : int;
var r_length : int;
var c_start : int;
var c_length : int;

var scale_a : float;
var scale_b : float;
var scale_x : float;
var scale_y : float;

var offset_a : float;
var offset_b : float;

var testcount : int;
var testcreate : int;
var zonecount : int;

function Awake () {
	globalBlockScale = 1;
	
}

function Start () {
	items = transform.GetComponent(ItemController).items;
	spawnpos = transform.GetComponent(MasterScript).spawnObject.position;
	master = GameObject.FindGameObjectWithTag("mc");
	cam = GameObject.FindGameObjectWithTag("MainCamera");
	oldx = cam.transform.position.x;
	oldy = cam.transform.position.y;
	
	
	cordx = 0;
	cordy = 0;
	
	testcount = 0;
	testcreate = 0;
	
	CL = globalBlockScale*globalChunkSize;
}

function Update () {
	var px : float = cam.transform.position.x;
	var py : float = cam.transform.position.y;
	
//	if((Mathf.Abs(oldx-px)>8) || (Mathf.Abs(oldy-py)>8)) {
//		testcount += 1;
//		//test for and create new chunks
//		expandZones(Vector2(px-(CL*2),py-(CL*2)),Vector2(px+(CL*2),py+(CL*2)));
//		//we have updated our position, lets save it 
//		oldx = px;
//		oldy = py;
//	}
}

function expandZones(start : Vector2, end : Vector2) {
//	var r_start : int;
//	var r_length : int;
//	var c_start : int;
//	var c_length : int;
	var existingzones : GameObject[];
	//var zoneframe : int[];
	var oZone : Transform;
	var zonescript : ZoneChunk;	
	
	
	//lets convert this into chunk units
	r_start = Mathf.FloorToInt(start.x/CL);
	r_length = Mathf.FloorToInt(end.x/CL)-r_start;
	c_start = Mathf.FloorToInt(start.y/CL);
	c_length = Mathf.FloorToInt(end.y/CL)-c_start;
	var zoneframe : int[] = new int[((c_length+1)*(r_length+1))];
	
	//first lets scan to see what is already made
	existingzones = GameObject.FindGameObjectsWithTag("chunk");
	for (var zo : GameObject in existingzones)  {
		zonescript = zo.GetComponent("ZoneChunk");
		
		//hide zone entities more than 3 zones to left and right
		var posx : int = Mathf.FloorToInt((zo.transform.position.x/CL)-r_start);
		if(posx < 0) {
			//zonescript.hideBlocks();
			continue;
		}
		else if(posx > r_length) {
			//zonescript.hideBlocks();
			continue;
		}
		//hide zone entities more than 3 zones above or below
		var posy : int = Mathf.FloorToInt((zo.transform.position.y/CL)-c_start);
		if(posy < 0) {
			//zonescript.hideBlocks();
			continue;
		}
		else if(posy > c_length) {
			//zonescript.hideBlocks();
			continue;
		}

		//if we are here, then we fell in range
		//so the zone entity should mark it's position in the frame so we don't re-create it
		zonescript.showBlocks();  	
		testcreate = posx;	
		zoneframe[(posx) + (posy)*r_length] = 1; 
	} 


	//now, lets check the array. if we don't have a zone there, we will make it.
	for (var a : int = 0; a < r_length; a += 1) {
		for (var b : int = 0; b < c_length; b += 1) {
			testcount += 1;
			if(zoneframe[a+(b*c_length)] == 1) ; //it already exists and is showing 
			
			else {
			//but if not, we should create it
			oZone = createZone(Vector3((r_start+a)*CL,(c_start+b)*CL,0));
			//we initialize the zone to the start values at the corner and show it
			zonescript = oZone.GetComponent("ZoneChunk");
			
			zonescript.initialize(r_start+a, c_start+b); //this should be moved to within the create function
			
			zonescript.showBlocks();
			}
		}
	}	

}

function buildWorld(type : int){

	
	//expandZones(Vector2(-(16*2),-(16*2)),Vector2((16*2),(16*2)));
	
	//expandZones(Vector2(-(16*2),-(16*2)),Vector2((16*2),(16*2))); //test to see if we will find our chunks
	//expandZones(Vector2(-(globalChunkSize*3),-(globalChunkSize*3)),Vector2((globalChunkSize*3),(globalChunkSize*3)));
//	for (r = 0; r < 24; r += (globalBlockScale*16)) {
//		if((z_count/map_width) > (map_height+1)) continue;
//		c_count = 0;
//		
//		for (c = 0; c < 24; c += (globalBlockScale*16)) {
//			if(c_count > map_width) continue;
//			//newzone = zoneMap[z_count];
//			newzone.strata = (Mathf.RoundToInt(r/10));
//			oZone = createZone(Vector3(c-(1*globalBlockScale*16),r-(1*globalBlockScale*16),0));
//			zonescript = oZone.GetComponent("ZoneChunk");
//			zonescript.initialize(c, r);
//			zonescript.show();
//			
//			z_count += 1;
//			c_count += 1;
//		}
//		r_count += 1;
//	}
}

function getShape(row : int, column : int) {

}

function getStrata(row : int, column : int) {

}

function createZone(position : Vector3) {

	var oZone : Transform = PoolManager.Pools["chunks"].Spawn(zoneObj.transform, position, Quaternion.identity);
	
	return oZone;
}

function placeBlock(id : int, position : Vector3) {
	//create and initialize a block into world
	var randVal : float;
	var randAngle : float;
	var newBlock : Transform = PoolManager.Pools["blocks"].Spawn(block.transform, position, Quaternion.identity);
	//var newBlock : Transform = Instantiate(block.transform, position, Quaternion.identity);
	
	newBlock.networkView.RPC("setBlockMat", RPCMode.AllBuffered,newBlock.networkView.viewID, id);
	randVal = Random.value;
	if(randVal < 0.4) randVal+=0.5;
	newBlock.networkView.RPC("setBlockValues", RPCMode.AllBuffered,newBlock.networkView.viewID,2,Vector3(globalBlockScale*1,globalBlockScale*1,randVal*1));
	
	return newBlock;
}

function createPickUpBlock(id : int, position : Vector3, direction : Vector3){
	var oBlock : Transform = PoolManager.Pools["drops"].Spawn(pickupblock.transform, position, Quaternion.identity);
	oBlock.GetComponent(pickUpBlock).invItem.id = id;
	oBlock.GetComponent(pickUpBlock).invItem.quantity = 1;
	oBlock.GetComponent(pickUpBlock).InitializeBlock();
	oBlock.GetComponent(MeshFilter).mesh = master.GetComponent(ItemController).items.library[1].mesh;
	oBlock.rigidbody.AddForce(direction,UnityEngine.ForceMode.VelocityChange);
	oBlock.renderer.material = master.GetComponent(ItemController).items.library[id].material;
	//Debug.Log("PickUpBlock ID: " + oBlock.networkView.viewID);
	//TODO:      oBlock.GetComponent(NetworkView).RPC("setBlockMat", RPCMode.AllBuffered, oBlock.networkView.viewID, id);
}