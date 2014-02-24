#pragma strict

var leftplan : GameObject;
var connectedLeft : GameObject;
var mountLeft : Vector3 = Vector3(0.6,0,0);
var driveplan : GameObject;
var connectedDrive : GameObject;
var mountDrive : Vector3 = Vector3(0,-1,0);
var controller : CharacterController;
var spotlight : GameObject;

function Start () {
	leftplan = Resources.Load("Gun");
	connectedLeft = Network.Instantiate(leftplan, (transform.position + mountLeft),Quaternion.identity, 0);
	connectedLeft.transform.parent = transform;
	driveplan = Resources.Load("Drive");
	connectedDrive = Network.Instantiate(driveplan, (transform.position + mountDrive),Quaternion.identity, 0);
	connectedDrive.transform.parent = transform;
	controller = transform.GetComponent(CharacterController);
	controller.height = 2;
	controller.center = Vector3(0,-0.5,0);
	spotlight = transform.Find("NarrowLight").gameObject;
}

function Update () {
	var mouseWorldPosition : Vector3 = Camera.main.ScreenToWorldPoint(Vector3(Input.mousePosition.x,Input.mousePosition.y,-Camera.main.transform.position.z));
	mouseWorldPosition.z = transform.position.z;
	spotlight.transform.LookAt(mouseWorldPosition);
}