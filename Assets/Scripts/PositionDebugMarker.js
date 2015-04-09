#pragma strict

var zoom : float;
var hiBoy : float;

function Awake () {
	zoom = Camera.main.transform.position.z;
}

function Start () {

}

function Update () {
	zoom = Camera.main.transform.position.z;
	var mouseWorldPosition : Vector3 = Camera.main.ScreenToWorldPoint(Vector3(Input.mousePosition.x,Input.mousePosition.y,-zoom));
	transform.position = mouseWorldPosition;
	//transform.position.z = 0;
}

