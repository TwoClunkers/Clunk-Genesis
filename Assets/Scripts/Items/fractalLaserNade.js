#pragma strict
private var lifeEndTime : float;
var laserDuration : float;
private var laserStartTime : float;
private var isFiring : boolean;
var laserCount : int;
var laserDamage : int;
var laserTickDuration : float;
var laserRange : float;
private var laserTargetEnd : Vector3[];
private var laserTargetNow : Vector3[];
var smooth : float;
private var lasers : Array;
private var lasersReady : boolean;
var laserBeamObject : GameObject;
private var fixedPosition : Vector3;
var smokePuff : GameObject;

function Start () {
	smooth = 1.0;
	laserDamage = 200;
	laserRange = 3.5;
	laserTickDuration = 0.01;
	laserCount = 5; // number of lasers to shoot
	lifeEndTime = Time.time + 6.0; // life time of the grenade
	laserDuration = 0.5; // how long each laser is active
	laserStartTime = Time.time + 2.0; // delay from spawn time that lasers start
	isFiring = false;
	lasersReady = false;
	
	laserTargetEnd = new Vector3[laserCount];
	laserTargetNow = new Vector3[laserCount];
	
	var i : int;
	
	lasers = new Array();
	for( i = 0;i < (laserCount );i++){
		lasers.Add(Network.Instantiate(laserBeamObject, GetComponent.<Collider>().bounds.center, Quaternion.identity, 0));
	}

	for( i = 0; i < laserCount ; i++){ // randomize laser start and end points
		laserTargetNow[i].x = Random.Range(-50.0,50.0);
		laserTargetNow[i].y = Random.Range(-50.0,50.0);
		laserTargetNow[i].z = 0.0;
		laserTargetEnd[i].x = Random.Range(-50.0,50.0);
		laserTargetEnd[i].y = Random.Range(-50.0,50.0);
		laserTargetEnd[i].z = 0.0;
	}
}

function Update () {
	var i : int;
	if(isFiring){
		if(Time.time < lifeEndTime){
			//shoot lasers
			var aLaser : GameObject;
			for( i = 0;i < (laserCount);i++){
				//Debug.Log("index " + i);
				laserTargetNow[i] = Vector3.Lerp(laserTargetNow[i],transform.position + laserTargetEnd[i], smooth * Time.deltaTime);
				aLaser = lasers[i];
				aLaser.GetComponent(laserBeam).InitLaser(transform.position, laserTargetNow[i], laserDamage, laserRange, laserTickDuration, laserDuration, aLaser.GetComponent.<Renderer>().material);
				aLaser.GetComponent(laserBeam).FireLaser();	
			}
		} else {
			//destroy grenade
			Network.Instantiate(smokePuff, transform.position,Quaternion.identity,0);
			destroyMe();
		}
	} else {
		if(Time.time > laserStartTime){
			fixedPosition = transform.position;
			GetComponent.<Rigidbody>().isKinematic = true;
			isFiring = true;
		}
	}
}


function destroyMe(){
	try{ Network.RemoveRPCs(GetComponent.<NetworkView>().viewID); } catch(e){}
	try{ Network.Destroy(GetComponent.<NetworkView>().viewID); } catch(e){}
}