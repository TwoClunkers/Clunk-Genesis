#pragma strict

var target : Vector3;
var range : float;
var laserNade : GameObject;
var selector : GameObject;
//private var line : LineRenderer;

private var TerrainScript : Terra;

function Start () {
	range = 20;
	selector = GameObject.FindGameObjectWithTag("selector");
	//line = GetComponent(LineRenderer);
}

function Update () {

	//transform.LookAt(selector.transform); 
	
	if(Input.GetMouseButtonDown(0)) { //!!!Warning, we should also check to see there is a parent, right?
		//Laser();
		//Blast();
		//var thisone : GameObject;
		//thisone = Network.Instantiate(laserNade,transform.position,Quaternion.identity,0);
		//thisone.GetComponent(Rigidbody).AddForce(transform.forward * 20, ForceMode.Impulse );
		//thisone.transform.position.z = 0;
		//Physics.IgnoreCollision(thisone.transform.collider, transform.parent.collider);
	}
}

function Laser () {
    var hit : RaycastHit;
	var raystart : Vector3 = transform.position;
	//raystart.z = selector.z;
	transform.LookAt(selector.transform); 
    if (Physics.Raycast(raystart , transform.forward, hit, 100))
    {
    	Debug.DrawLine (raystart, hit.point, Color.red, 1, false);
        TerrainScript.SetBlock(hit, new BlockAir(),false);
    }
}

function Blast () {
	var ray : Ray;
	var startPoint : Vector3 = transform.parent.position;
	var hitBlock : GameObject;
	//var hitPosition : Vector3;
	var trueHit : RaycastHit;
	
	ray = Ray(Vector3(startPoint.x,startPoint.y), transform.forward); //starting from center of player, but in direction of gun...
	var layMask : LayerMask;
	layMask = (1<<0);
	if(Physics.Raycast(ray, trueHit, range, layMask)){
	  // Call a damage function on all objects caught in the radius
	  	//line.enabled = true;
		//line.SetPosition(0, transform.position);
		//line.SetPosition(1, trueHit.point);
        var hitColliders = Physics.OverlapSphere(trueHit.point, 1.25);
        for (var i = 0; i < hitColliders.Length; i++) {
        	hitBlock = hitColliders[i].gameObject;
			if(hitBlock.tag.Equals("breakable")) {
				hitBlock.GetComponent.<NetworkView>().RPC("setBlockValues",RPCMode.All, hitBlock.GetComponent.<NetworkView>().viewID,1,Vector3(.0,.0,.3));
				hitBlock.GetComponent.<NetworkView>().RPC("DamageBlock",RPCMode.All, hitBlock.GetComponent.<NetworkView>().viewID, 50, transform.forward);
				GameObject.FindGameObjectWithTag("mc").GetComponent(NetworkView).RPC("playSound", RPCMode.All, "tickAudio", hitBlock.transform.position);	
			}
        }
    }
}