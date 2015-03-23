#pragma strict
private var lifeEndTime : float;
var lifeTime : float;

function Start () {
	lifeEndTime = Time.time + lifeTime;
}

function Update () {
	if(Time.time > lifeEndTime) destroyMe();
}

function destroyMe(){
	try{ Network.RemoveRPCs(GetComponent.<NetworkView>().viewID); } catch(e){}
	try{ Network.Destroy(GetComponent.<NetworkView>().viewID); } catch(e){}
}