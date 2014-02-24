#pragma strict

var globalBlockScale : float;
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



function Awake () {
	globalBlockScale = 0.5;
}

function Start () {
	items = transform.GetComponent(ItemController).items;
	spawnpos = transform.GetComponent(MasterScript).spawnObject.position;
	
	cordx = 0;
	cordy = 0;
	seed = Mathf.RoundToInt(Random.value*1000000);
	xSeed = Mathf.RoundToInt(Random.value*1000000);
	ySeed = Mathf.RoundToInt(Random.value*1000000);
	bSeed = Mathf.RoundToInt(Random.value*1000000);
	iSeed = Mathf.RoundToInt(Random.value*1000000);
	
}

function Update () {

}

function buildWorld(type : int){
	var r : float;
	var c : float;
	var oZone : GameObject;
	var zonescript : ZoneChunk;
	var newzone : Chunk;
	newzone = new Chunk();
	zoneMap = new Chunk[80];
	var top : int;
	var bottom : int;
	var right : int;
	var left : int;
	var z_count : int;
	var c_count : int;
	var r_count : int;
	var map_width : int;
	var map_height : int;
	
	z_count = 0;
	c_count = 0;
	r_count = 0;
	map_width = 4;
	map_height = 10;
	
	for (r = 0; r < 120; r += (globalBlockScale*16)) {
		if((z_count/map_width) > (map_height+1)) continue;
		c_count = 0;
		
		for (c = 0; c < 30; c += (globalBlockScale*16)) {
			if(c_count > map_width) continue;
			//newzone = zoneMap[z_count];
			newzone.strata = (Mathf.RoundToInt(r/10));
			oZone = createZone(Vector3(c-(2*globalBlockScale*16),r-(5*globalBlockScale*16),0));
			zonescript = oZone.GetComponent("ZoneChunk");
			zonescript.initialize(c, r);
			zonescript.show();
			
			z_count += 1;
			c_count += 1;
		}
		r_count += 1;
	}
}

function getShape(row : int, column : int) {

}

function getStrata(row : int, column : int) {

}

function createZone(position : Vector3) {
	var oZone : GameObject = Network.Instantiate(zoneObj, position, Quaternion.identity,0);
	oZone.transform.parent = transform;
	return oZone;
}
//
//	var r : float;
//	var c : float;
//	var bMat : int;
//	var randVal : float;
//	var oZone : GameObject = Network.Instantiate(zoneObj, position, Quaternion.identity,0);
//	var zScale : float;
//	var randAngle : float;
//	var dist : float;
//	var newBlock : GameObject;
//	
//				randVal = Random.value;
//
//				//if(randVal > .8){
//					//bMat = 1;
//				//}else if(randVal > .6){
//					//bMat = 2;
//				//}else 
//				if(randVal > .4){
//					bMat = 1;
//				}else if(randVal > .2){
//					bMat = 2;
//				}else {
//					bMat = 3;
//				}
//	
//	if(1) {
//		for (r = 0; r < 16; r += 1) {
//			for (c = 0; c < 16; c += 1) {	
//				dist = Vector3.Distance(spawnpos,Vector3(position.x+globalBlockScale*c,position.y+globalBlockScale*r,0));
//				if(dist < 3) continue; //dont create next to spawn
//
//
//				
//				newBlock = placeBlock(bMat, Vector3(position.x+globalBlockScale*c,position.y+globalBlockScale*r,0));
//				newBlock.transform.parent = oZone.transform;
//				
//				if(zone.strata == StrataType.Magma) newBlock.renderer.material.color = Color.yellow;
//				else if(zone.strata == StrataType.Mantle) newBlock.renderer.material.color = Color.gray;
//				else newBlock.renderer.material.color = Color.white;
//
//			}
//		}
//	}
//	return oZone;
//}

function placeBlock(id : int, position : Vector3) {
	//create and initialize a block into world
	var randVal : float;
	var randAngle : float;
	var newBlock : GameObject = Network.Instantiate(block, position, Quaternion.identity,0);
	
	newBlock.networkView.RPC("setBlockMat", RPCMode.AllBuffered,newBlock.networkView.viewID, id);
	randVal = Random.value;
	if(randVal < 0.4) randVal+=0.5;
	newBlock.networkView.RPC("setBlockValues", RPCMode.AllBuffered,newBlock.networkView.viewID,2,Vector3(globalBlockScale,globalBlockScale,randVal));
	
	return newBlock;
}

function createPickUpBlock(id : int, position : Vector3, direction : Vector3){
	var oBlock : GameObject = Network.Instantiate(pickupblock, position, Quaternion.identity, 0);
	oBlock.GetComponent(pickUpBlock).invItem.id = id;
	oBlock.GetComponent(MeshFilter).mesh = GameObject.FindGameObjectWithTag("mc").GetComponent(ItemController).items.library[id].mesh;
	oBlock.rigidbody.AddForce(direction,UnityEngine.ForceMode.VelocityChange);
	//Debug.Log("PickUpBlock ID: " + oBlock.networkView.viewID);
	//TODO:      oBlock.GetComponent(NetworkView).RPC("setBlockMat", RPCMode.AllBuffered, oBlock.networkView.viewID, id);
}