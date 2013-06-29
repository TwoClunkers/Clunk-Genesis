var target : Transform;
var zpos : float;

function Update () {
	if(target){
		transform.position.x = target.position.x;
		transform.position.y = target.position.y;
		transform.position.z = zpos;
	}
}