var target : Transform;
var zpos : float;
var vlight : Transform;
var camAdjustSpeed : float = 0.5;
private var lastZ : float;

function Start () {
	vlight = transform.Find("ViewLight");
}

function Update () {
	if(target){
		var newPosition : Vector3;
		newPosition.x = target.position.x;
		newPosition.y = (target.position.y-0.5f)+(zpos/(-5f));
		newPosition.z = zpos;
		
		//Ross: lerp camera to new position
		transform.position = Vector3.Lerp(transform.position, newPosition, camAdjustSpeed);
		
		vlight.position.z = lastZ;
		//light.position.x = target.position.x;
		//light.position.y = target.position.y;
		//vlight.position.z = (-25);
	}
}

function zoomCamera(zoomAmount : float) {
	zpos += zoomAmount;
}