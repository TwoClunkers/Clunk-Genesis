#pragma strict
private var startPoint : Vector3;
private var endPoint : Vector3;
private var laserDamageAmount : float;
var sparks : GameObject;
private var range : float;
private var ray : Ray;
private var line : LineRenderer;
private var trueHit : RaycastHit;
private var lastHitViewID : NetworkViewID;
private var tickLength : float = 0.6;
var laserMaterial : Material;
private var isFiring : boolean;
private var durationOfLaser : float;
private var lifeEndTime : float;
private var hitSomething : boolean;
private var wasActive : boolean;
private var tickEndTime : float;
private var halfTick : boolean;
private var sparksDestroyed : boolean;
private var sparksViewID : NetworkViewID;

function Start () {
	lifeEndTime = Time.time + 3.0;
	line = GetComponent(LineRenderer);
}

function Update () {
	if(isFiring){
		/////////////////////////////////////////////start here 
		ray = Ray(Vector3(startPoint.x,startPoint.y, startPoint.z), (Vector3(endPoint.x,endPoint.y, endPoint.z) - Vector3(startPoint.x,startPoint.y, startPoint.z)).normalized);
		var layMask : LayerMask;
		layMask = (1<<0);
		//Debug.DrawLine(transform.position, startPoint, Color.yellow, 2.0);
		//Debug.DrawLine(transform.position, endPoint, Color.magenta, 2.0);
		if(Physics.Raycast(ray, trueHit, range, layMask)){
			//hit something
			hitSomething = true;
			//Debug.Log("hitSomething.");
		} else {
			//didn't hit anything
			hitSomething = false;
			wasActive = false;
			endPoint.Scale(Vector3(10.0,10.0,10.0));
			trueHit.point = Vector3.MoveTowards(startPoint, endPoint, range);
		}
		
		line.enabled = true;
		line.SetPosition(0, transform.position);
		line.SetPosition(1, trueHit.point);// + trueHit.normal);
		
		if(hitSomething){
			if(trueHit.collider.gameObject.tag.Equals("breakable")) {
				//start breaking
				if(wasActive && lastHitViewID.Equals(trueHit.collider.gameObject.GetComponent.<NetworkView>().viewID) && Time.time > tickEndTime) {
					doTick(trueHit.collider.gameObject, trueHit.point, trueHit);
				}
				lastHitViewID = trueHit.collider.gameObject.GetComponent.<NetworkView>().viewID;
				wasActive = true;
			}
			Debug.DrawLine(startPoint, trueHit.point, Color.green,2.0);	
			if(halfTick){
				/*if(!sparksDestroyed) {
					Network.RemoveRPCs(sparksViewID);
					Network.Destroy(sparksViewID);
				}*/
				sparksDestroyed = true;
				var fxObj : GameObject = Network.Instantiate(sparks, trueHit.point, Quaternion.LookRotation((Vector3(transform.position.x,transform.position.y) - Vector3(trueHit.point.x,trueHit.point.y)).normalized),0);
				sparksViewID = fxObj.GetComponent.<NetworkView>().viewID;
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

function InitLaser(laserStartPoint : Vector3, towardsPoint : Vector3, damagePerTick : float, laserRange : float, tickDuration : float, lifeTime : float, material : Material){
	startPoint = laserStartPoint;
	endPoint = towardsPoint;
	laserDamageAmount = damagePerTick;
	range = laserRange;
	tickLength = tickDuration;
	durationOfLaser = lifeTime;
	laserMaterial = material;
}

function doTick(hitBlock : GameObject, hitPosition : Vector3, actualRayCastHit : RaycastHit){
	//- legacy block system - hitBlock.GetComponent.<NetworkView>().RPC("setBlockValues",RPCMode.All, hitBlock.GetComponent.<NetworkView>().viewID,1,Vector3(.0,.0,.3));
	//- legacy block system - hitBlock.GetComponent.<NetworkView>().RPC("DamageBlock",RPCMode.All, hitBlock.GetComponent.<NetworkView>().viewID, laserDamageAmount, (transform.position-trueHit.point).normalized);
	//Debug.Log("tick");
	Terra.DamageBlock(actualRayCastHit, laserDamageAmount, transform.forward);  // RDM: It seems the correct block is not always found, possibly because of offsets
	//var blockPos : WorldPos = Terra.GetBlockPos(actualRayCastHit,false);
	//Debug.Log( blockPos.x + "," + blockPos.y + "," + blockPos.z );
	//- legacy block system - GameObject.FindGameObjectWithTag("mc").GetComponent(NetworkView).RPC("playSound", RPCMode.All, "tickAudio", hitBlock.transform.position);
	tickEndTime = Time.time + tickLength - Time.deltaTime;
	//this needs to be part of a block.damage routine
	//hitBlock.GetComponent(blockClick).createPickUpBlock(hitPosition);
}

function destroyMe(){
	try{ Network.RemoveRPCs(GetComponent.<NetworkView>().viewID); } catch(e){}
	try{ Network.Destroy(GetComponent.<NetworkView>().viewID); } catch(e){}
}

