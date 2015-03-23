var target : Transform;
var zpos : float;
var vlight : Transform;

function Start () {
	vlight = transform.Find("ViewLight");
}

function Update () {
	if(target){
		transform.position.x = target.position.x;
		transform.position.y = target.position.y+(zpos/(-6));
		transform.position.z = zpos;
		//light.position.x = target.position.x;
		//light.position.y = target.position.y;
		//vlight.position.z = (-25);
	}
}

function zoomCamera(zoomAmount : float) {
	zpos += zoomAmount;
}