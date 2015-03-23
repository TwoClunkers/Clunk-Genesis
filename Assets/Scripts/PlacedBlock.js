var pickupblock : GameObject;
var mychunk : int;
var myx : int;
var myy : int;
var mydata : int[];
var myvert : int;
var mymat : int;
var position : Vector3;
var matchange : int;
var mymaterial : Material;

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
@System.NonSerializedAttribute
var forceDirection : Vector3;

function Awake(){
	blockHealth = 100;
	maxHealth = 100;
	tickScaleAxis = 2;
	matchange = 0;
			var mesh : Mesh = GetComponent(MeshFilter).mesh;
		var vertices : Vector3[] = mesh.vertices;
		myvert = vertices.Length;
}

function Update(){
	if(blockHealth == 100){
	//I created this "initialize" spot to mess with mesh vertices. - it did not work much
//		var mesh : Mesh = GetComponent(MeshFilter).mesh;
//		var vertices : Vector3[] = mesh.vertices;
//
//		vertices[5] += Vector3.up*.35;
//		vertices[7] += Vector3.up*.35;
//		vertices[3] += Vector3.down*.35;
//		vertices[2] += Vector3.down*.35;
//		vertices[5] += Vector3.left*.35;
//		vertices[7] += Vector3.right*.35;
//		vertices[2] += Vector3.left*.35;
//		vertices[3] += Vector3.right*.35;
//		vertices[5] += Vector3.back*.35;
//		vertices[7] += Vector3.back*.35;
//		vertices[3] += Vector3.back*.35;
//		vertices[2] += Vector3.back*.35;
//		vertices[0] += Vector3.forward*.35;
//		vertices[1] += Vector3.forward*.35;
//		vertices[4] += Vector3.forward*.35;
//		vertices[6] += Vector3.forward*.35;
//		myvert = vertices.Length;
//		//for (var i = 0; i < vertices.Length; i++)
//		//	vertices[i] += Vector3.up * Time.deltaTime;
//
//		mesh.vertices = vertices;
//		mesh.RecalculateBounds();
		blockHealth = 99;
	}
	if(blockHealth <= 0){
		transform.parent = null;
		


		//destroy block, create pickup
		GameObject.FindGameObjectWithTag("mc").GetComponent(WorldController).createPickUpBlock(blockMaterial, transform.position, forceDirection);
		
//		try{
//			Network.RemoveRPCs(networkView.viewID);
//			Network.Destroy(networkView.viewID);
//		} catch(e){}
		GameObject.FindGameObjectWithTag("mc").GetComponent(NetworkView).RPC("playSound", RPCMode.All, "blockDestroyed", transform.position);
		
		
		blockHealth = 100;
		
				//remove our backround block last
		PoolManager.Pools["blocks"].Despawn(transform);
		
		
	}
	else forceDirection = Vector3(0,0,0);
}

@RPC
function setBlockMat(viewID : NetworkViewID, mat : int){
	var newitem : Item = GameObject.FindGameObjectWithTag("mc").GetComponent(ItemController).items.library[mat];
	matchange = 1;
	mymat = mat;
	blockMaterial = mat;
	blockHealth = newitem.maxHealth;
	maxHealth = newitem.maxHealth;
	mymaterial = newitem.material;
	this.renderer.material = newitem.material;
	matchange += 2;
//	try{
//	var oBlockView : NetworkView = networkView.Find(viewID);
//		oBlockView.renderer.material = newitem.material;
//		oBlockView.GetComponent(PlacedBlock).blockMaterial = mat;
//		oBlockView.GetComponent(PlacedBlock).blockHealth = newitem.maxHealth;
//		oBlockView.GetComponent(PlacedBlock).maxHealth = newitem.maxHealth;
//		oBlockView.GetComponent(MeshFilter).mesh = newitem.mesh;
//		matchange = 2;
//	}catch(e){ matchange = -1;}
}

@RPC 
function setBlockValues(viewID : NetworkViewID, varToSet : int, setTo : Vector3){
	if( !viewID.isMine ) return;
	var vScale : Vector3;
	if( varToSet == 1){
		//set z scale to blockHealth/maxHealth
		try{
			var zSc : float = 0.25;
			if((1.0 * blockHealth) / (1.0 * maxHealth) > 0.25){
				zSc = (1.0 * blockHealth) / (1.0 * maxHealth);
			}
			zSc *= startingZScale;
			if(tickScaleAxis == 0){
				//scale on x axis
				vScale = Vector3(zSc,transform.localScale.y, transform.localScale.z);
			} else if(tickScaleAxis == 1) {
				//scale on y axis
				vScale = Vector3(transform.localScale.x, zSc, transform.localScale.z);
			} else {
				//scale on z axis
				vScale = Vector3(transform.localScale.x, transform.localScale.y, zSc);
			}
			transform.localScale = vScale;
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
			vScale = Vector3(setTo.x,setTo.x, setTo.z);
		}
		transform.localScale = vScale;
		startingZScale = setTo.z;
	} else if(varToSet == 3) {
		//rotate x,y, or z by setTo.y
		if(setTo.x > 0.66){
			transform.eulerAngles.x = setTo.y;
			if(setTo.y/90 == 1 || setTo.y/90 == 3) {
				//90|270 = y
				tickScaleAxis = 1;
			} else {
				//180 = z
				tickScaleAxis = 2;
			}
		} else if(setTo.x > 0.33){
			transform.eulerAngles.y = setTo.y;
			if(setTo.y/90 == 1 || setTo.y/90 == 3) {
				//90|270 = y
				tickScaleAxis = 0;
			} else {
				//180 = z
				tickScaleAxis = 2;
			}
		} else {
			transform.eulerAngles.z = setTo.y;
			tickScaleAxis = 2;
		}
	}
}
@RPC
function setPosition( viewID : NetworkViewID , newPosition : Vector3) {
	if( !viewID.isMine ) return;
	
	position = newPosition;
}

@RPC
function DamageBlock( viewID : NetworkViewID ,hp : int, direction : Vector3) {
	if( !viewID.isMine ) return;
	/*try{ var nv : NetworkView = networkView.Find(viewID); } catch(e) {}
	try{
		nv.GetComponent(PlacedBlock).blockHealth -= hp;
	}catch(e){}*/
	blockHealth -= hp;
	forceDirection = (forceDirection + (direction*(hp/10)));
}