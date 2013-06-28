var pickupblock : GameObject;
@System.NonSerializedAttribute
var blockHealth : int;
@System.NonSerializedAttribute
var maxHealth : int;
@System.NonSerializedAttribute
var blockMaterial : int;
@System.NonSerializedAttribute
var tickScaleAxis : int;
@System.NonSerializedAttribute
var startingZScale : float;

function Awake(){
	blockHealth = 100;
	maxHealth = 100;
	tickScaleAxis = 2;
}

function Update(){
	if(blockHealth <= 0){
		//destroy block, create pickup
		createPickUpBlock(transform.position);
		try{
			Network.RemoveRPCs(networkView.viewID);
			Network.Destroy(networkView.viewID);
		} catch(e){}
		GameObject.FindGameObjectWithTag("gui").GetComponent(NetworkView).RPC("playSound", RPCMode.All, "blockDestroyed", transform.position);
	}
}

function createPickUpBlock(atPosition : Vector3){
	var popDirection : Vector3;
	var oBlock : GameObject = Network.Instantiate(pickupblock, atPosition, Quaternion.identity, 0);
	popDirection = (GameObject.FindGameObjectWithTag("Player").transform.position - atPosition) * 3;
	oBlock.GetComponent(pickUpBlock).itemID = blockMaterial;
	oBlock.GetComponent(MeshFilter).mesh = GameObject.FindGameObjectWithTag("gui").GetComponent(guiScript).items.library[blockMaterial].mesh;
	oBlock.rigidbody.AddForce(popDirection,UnityEngine.ForceMode.VelocityChange);
	//Debug.Log("PickUpBlock ID: " + oBlock.networkView.viewID);
	GameObject.FindGameObjectWithTag("gui").GetComponent(NetworkView).RPC("setBlockMat", RPCMode.AllBuffered, oBlock.networkView.viewID, blockMaterial);
}

@RPC 
function setBlockValues(viewID : NetworkViewID, varToSet : int, setTo : Vector3){
	var vScale : Vector3;
	if( varToSet == 1){
		//set z scale to blockHealth/maxHealth
		try{
			if(networkView.Find(viewID) == null) return;
			var zSc : float = 0.25;
			if((1.0 * blockHealth) / (1.0 * maxHealth) > 0.25){
				zSc = (1.0 * blockHealth) / (1.0 * maxHealth);
			}
			zSc *= startingZScale;
			if(tickScaleAxis == 0){
				//scale on x axis
				vScale = Vector3(zSc,networkView.Find(viewID).transform.localScale.y, networkView.Find(viewID).transform.localScale.z);
			} else if(tickScaleAxis == 1) {
				//scale on y axis
				vScale = Vector3(networkView.Find(viewID).transform.localScale.x, zSc, networkView.Find(viewID).transform.localScale.z);
			} else {
				//scale on z axis
				vScale = Vector3(networkView.Find(viewID).transform.localScale.x, networkView.Find(viewID).transform.localScale.y, zSc);
			}
			networkView.Find(viewID).transform.localScale = vScale;
		}catch(e){}
	} else if(varToSet == 2) {
		//replace depth scale based tickScaleAxis
		if(tickScaleAxis == 0){
			//scale on x axis
			vScale = Vector3(setTo.z,setTo.x, setTo.x);
		} else if(tickScaleAxis == 1) {
			//scale on y axis
			vScale = Vector3(setTo.x,setTo.z, setTo.x);
		} else {
			//scale on z axis
			//vScale = Vector3(networkView.Find(viewID).transform.localScale.x,networkView.Find(viewID).transform.localScale.y, setTo.z);
			vScale = Vector3(setTo.x,setTo.x, setTo.z);
		}
		networkView.Find(viewID).transform.localScale = vScale;
		startingZScale = setTo.z;
	} else if(varToSet == 3) {
		//rotate x,y, or z by setTo.y
		if(setTo.x > 0.66){
			networkView.Find(viewID).transform.eulerAngles.x = setTo.y;
			if(setTo.y/90 == 1 | setTo.y/90 == 3) {
				//90|270 = y
				tickScaleAxis = 1;
			} else {
				//180 = z
				tickScaleAxis = 2;
			}
		} else if(setTo.x > 0.33){
			networkView.Find(viewID).transform.eulerAngles.y = setTo.y;
			if(setTo.y/90 == 1 | setTo.y/90 == 3) {
				//90|270 = y
				tickScaleAxis = 0;
			} else {
				//180 = z
				tickScaleAxis = 2;
			}
		} else {
			networkView.Find(viewID).transform.eulerAngles.z = setTo.y;
			tickScaleAxis = 2;
		}
	}
}

@RPC
function DamageBlock( viewID : NetworkViewID ,hp : int) {
	try{
		networkView.Find(viewID).GetComponent(blockClick).blockHealth -= hp;
	}catch(e){}
}