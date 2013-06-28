#pragma strict
var startPoint : Vector3;
var endPoint : Vector3;
var laserDamageAmount : int;
var sparks : GameObject;
var range : float;
var ray : Ray;
var line : LineRenderer;
var trueHit : RaycastHit;
var lastHitViewID : NetworkViewID;
var tickLength : float = 0.6;
var laserMaterial : Material;
var isFiring : boolean;
var durationOfLaser : float;
var lifeEndTime : float;
var hitSomething : boolean;
var wasActive : boolean;
var tickEndTime : float;
var halfTick : boolean;
var sparksDestroyed : boolean;
var sparksViewID : NetworkViewID;

function Start () {
	lifeEndTime = Time.time + 3.0;
	line = GetComponent(LineRenderer);
}

function Update () {
	if(isFiring){
		/////////////////////////////////////////////start here 
		ray = Ray(Vector3(startPoint.x,startPoint.y), (Vector3(endPoint.x,endPoint.y) - Vector3(startPoint.x,startPoint.y)).normalized);
		var layMask : LayerMask;
		layMask = (1<<0);
		if(Physics.Raycast(ray, trueHit, range, layMask)){
			//hit something
			hitSomething = true;
		} else {
			//didn't hit anything
			hitSomething = false;
			endPoint.Scale(Vector3(10.0,10.0,10.0));
			trueHit.point = Vector3.MoveTowards(startPoint, endPoint, range);
		}
		line.enabled = true;
		line.SetPosition(0, transform.position);
		line.SetPosition(1, trueHit.point);// + trueHit.normal);
		
		if(hitSomething){
			if(trueHit.collider.gameObject.tag.Equals("breakable")) {
				//start breaking
				if(wasActive && lastHitViewID.Equals(trueHit.collider.gameObject.networkView.viewID) & Time.time > tickEndTime) {
					doTick(trueHit.collider.gameObject, trueHit.point);
				}
				lastHitViewID = trueHit.collider.gameObject.networkView.viewID;
				wasActive = true;
			}
			Debug.DrawLine(startPoint, trueHit.point, Color.green,2.0);	
			if(halfTick){
				if(!sparksDestroyed) {
					Network.RemoveRPCs(sparksViewID);
					Network.Destroy(sparksViewID);
				}
				sparksDestroyed = true;
				var fxObj : GameObject = Network.Instantiate(sparks, trueHit.point, Quaternion.LookRotation((Vector3(transform.position.x,transform.position.y) - Vector3(trueHit.point.x,trueHit.point.y)).normalized),0);
				sparksViewID = fxObj.networkView.viewID;
				sparksDestroyed = false;
			}
			halfTick = !halfTick;
		}
	}
	if(Time.time > lifeEndTime) destroyMe();
}

function FireLaser(){
	lifeEndTime = Time.time + durationOfLaser;
	transform.position = startPoint;
	isFiring = true;
}

function InitLaser(laserStartPoint : Vector3, towardsPoint : Vector3, damagePerTick : int, laserRange : float, tickDuration : float, lifeTime : float, material : Material){
	startPoint = laserStartPoint;
	endPoint = towardsPoint;
	laserDamageAmount = damagePerTick;
	range = laserRange;
	tickLength = tickDuration;
	durationOfLaser = lifeTime;
	laserMaterial = material;
}

function doTick(hitBlock : GameObject, hitPosition : Vector3){
	hitBlock.networkView.RPC("DamageBlock",RPCMode.All, hitBlock.networkView.viewID, laserDamageAmount);
	GameObject.FindGameObjectWithTag("gui").GetComponent(NetworkView).RPC("playSound", RPCMode.All, "tickAudio", hitBlock.transform.position);
	hitBlock.networkView.RPC("setBlockValues",RPCMode.All, hitBlock.networkView.viewID,1,Vector3(.0,.0,.3));
	tickEndTime = Time.time + tickLength - Time.deltaTime;
	hitBlock.GetComponent(blockClick).createPickUpBlock(hitPosition);
}

function destroyMe(){
	try{ Network.RemoveRPCs(networkView.viewID); } catch(e){}
	try{ Network.Destroy(networkView.viewID); } catch(e){}
}

