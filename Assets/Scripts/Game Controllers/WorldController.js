#pragma strict

var globalBlockScale : float;
var block : GameObject;
var pickupblock : GameObject;
var items : AllItems;
var spawnpos : Vector3;

function Awake () {
	globalBlockScale = 0.5;
}

function Start () {
	items = transform.GetComponent(ItemController).items;
	spawnpos = transform.GetComponent(MasterScript).spawnObject.position;
}

function Update () {

}

function buildWorld(type : int){
	var r : float;
	var c : float;
	var bMat : int;
	var randVal : float;
	var oBlock : GameObject;
	var zScale : float;
	var randAngle : float;
	var dist : float;
	
	if(type > 0) {
		for (r = (-15); r < 15; r += globalBlockScale) {
			for (c = (-15); c < 15; c += globalBlockScale) {	
				dist = Vector3.Distance(spawnpos,Vector3(c,r,0));
				if(dist < 3) continue; //dont create next to spawn
				
				oBlock = Network.Instantiate(block, Vector3(c,r,0),Quaternion.identity, 0);
				randVal = Random.value;
				//bMat = 1 + Mathf.Round(randVal*5);
				//oBlock.networkView.RPC("setBlockMat", RPCMode.AllBuffered,oBlock.networkView.viewID, bMat);
				
				if(randVal > .8){
					//bMat = 1;
					//oBlock.networkView.RPC("setBlockMat", RPCMode.AllBuffered,oBlock.networkView.viewID, bMat);
				}else if(randVal > .6){
					//bMat = 2;
					//oBlock.networkView.RPC("setBlockMat", RPCMode.AllBuffered,oBlock.networkView.viewID, bMat);
				}else if(randVal > .4){
					bMat = 1;
					oBlock.networkView.RPC("setBlockMat", RPCMode.AllBuffered,oBlock.networkView.viewID, bMat);
				}else if(randVal > .2){
					bMat = 2;
					oBlock.networkView.RPC("setBlockMat", RPCMode.AllBuffered,oBlock.networkView.viewID, bMat);
				}else {
					bMat = 3;
					oBlock.networkView.RPC("setBlockMat", RPCMode.AllBuffered,oBlock.networkView.viewID, bMat);
				}
				/**/
				randAngle =  90.0 * Random.Range(1,3);
				oBlock.networkView.RPC("setBlockValues", RPCMode.AllBuffered,oBlock.networkView.viewID,3,Vector3(randVal,randAngle,globalBlockScale));
				if(randVal < 0.4) randVal += 0.5;
				oBlock.networkView.RPC("setBlockValues", RPCMode.AllBuffered,oBlock.networkView.viewID,2,Vector3(globalBlockScale,globalBlockScale,randVal));
			}
		}
	}
	else {
		for (r = (1.0 - (globalBlockScale / 2.0)); r < 30; r += globalBlockScale) {
			for (c = (0.0 - (globalBlockScale / 2.0)); c < 30; c += globalBlockScale) {	
				oBlock = Network.Instantiate(block, Vector3(c,r,0),Quaternion.identity, 0);
				randVal = Random.value;
				//bMat = 1 + Mathf.Round(randVal*5);
				//oBlock.networkView.RPC("setBlockMat", RPCMode.AllBuffered,oBlock.networkView.viewID, bMat);
				
				if(randVal > .8){
					//bMat = 1;
					//oBlock.networkView.RPC("setBlockMat", RPCMode.AllBuffered,oBlock.networkView.viewID, bMat);
				}else if(randVal > .6){
					//bMat = 2;
					//oBlock.networkView.RPC("setBlockMat", RPCMode.AllBuffered,oBlock.networkView.viewID, bMat);
				}else if(randVal > .4){
					bMat = 1;
					oBlock.networkView.RPC("setBlockMat", RPCMode.AllBuffered,oBlock.networkView.viewID, bMat);
				}else if(randVal > .2){
					bMat = 2;
					oBlock.networkView.RPC("setBlockMat", RPCMode.AllBuffered,oBlock.networkView.viewID, bMat);
				}else {
					bMat = 3;
					oBlock.networkView.RPC("setBlockMat", RPCMode.AllBuffered,oBlock.networkView.viewID, bMat);
				}
				/**/
				randAngle =  90.0 * Random.Range(1,3);
				oBlock.networkView.RPC("setBlockValues", RPCMode.AllBuffered,oBlock.networkView.viewID,3,Vector3(randVal,randAngle,globalBlockScale));
				if(randVal < 0.4) randVal += 0.5;
				oBlock.networkView.RPC("setBlockValues", RPCMode.AllBuffered,oBlock.networkView.viewID,2,Vector3(globalBlockScale,globalBlockScale,randVal));
			}
		}
	}
	
}

function placeBlock(id : int, position : Vector3) {
	//create and initialize a block into world
	var newBlock : GameObject = Network.Instantiate(block, position, Quaternion.identity,0);
	newBlock.networkView.RPC("setBlockMat", RPCMode.AllBuffered,newBlock.networkView.viewID, id);
	//newBlock.networkView.RPC("setBlockValues", RPCMode.AllBuffered,newBlock.networkView.viewID,3,Vector3(randVal,0.0,globalBlockScale));
	newBlock.networkView.RPC("setBlockValues", RPCMode.AllBuffered,newBlock.networkView.viewID,2,Vector3(globalBlockScale,globalBlockScale,1.0));
}

function createPickUpBlock(id : int, position : Vector3){
	var popDirection : Vector3;
	var oBlock : GameObject = Network.Instantiate(pickupblock, position, Quaternion.identity, 0);
	popDirection = (GameObject.FindGameObjectWithTag("Player").transform.position - position) * 3;
	oBlock.GetComponent(pickUpBlock).invItem.id = id;
	oBlock.GetComponent(MeshFilter).mesh = GameObject.FindGameObjectWithTag("mc").GetComponent(ItemController).items.library[id].mesh;
	oBlock.rigidbody.AddForce(popDirection,UnityEngine.ForceMode.VelocityChange);
	//Debug.Log("PickUpBlock ID: " + oBlock.networkView.viewID);
	//TODO:      oBlock.GetComponent(NetworkView).RPC("setBlockMat", RPCMode.AllBuffered, oBlock.networkView.viewID, id);
}