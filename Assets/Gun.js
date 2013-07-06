#pragma strict

var target : Vector3;
var range : float;
//private var line : LineRenderer;

function Start () {
	range = 20;
	//line = GetComponent(LineRenderer);
}

function Update () {
	var mouseWorldPosition : Vector3 = Camera.main.ScreenToWorldPoint(Vector3(Input.mousePosition.x,Input.mousePosition.y,-Camera.main.transform.position.z));
	 mouseWorldPosition.z = transform.position.z;
	transform.LookAt(mouseWorldPosition); //for this purpose, put the gun on same plane as target
	if(Input.GetMouseButtonDown(0)) {
		Blast();
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
				hitBlock.networkView.RPC("setBlockValues",RPCMode.All, hitBlock.networkView.viewID,1,Vector3(.0,.0,.3));
				hitBlock.networkView.RPC("DamageBlock",RPCMode.All, hitBlock.networkView.viewID, 75, transform.forward);
				GameObject.FindGameObjectWithTag("mc").GetComponent(NetworkView).RPC("playSound", RPCMode.All, "tickAudio", hitBlock.transform.position);	
			}
        }
    }
}