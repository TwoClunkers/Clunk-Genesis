#pragma strict

var leftplan : GameObject;
var connectedLeft : GameObject;
var mountLeft : Vector3 = Vector3(0.6,0,0);

var driveplan : GameObject;
var connectedDrive : GameObject;
var mountDrive : Vector3 = Vector3(0,-0.5,0);

var headplan : GameObject;
var connectedHead : GameObject;
var mountHead : Vector3 = Vector3(0,0.375,0);

var controller : CharacterController;
var spotlight : GameObject;
static var controllerDefaultHeight : float = 1.75;
static var controllerDefaultCenter : float = -0.125;
static var controllerDefaultRadius : float = 0.49;


function Start () {
	leftplan = Resources.Load("miningLaser09");
	connectedLeft = Network.Instantiate(leftplan, (transform.position + mountLeft),Quaternion.identity, 0);
	connectedLeft.transform.parent = transform;
	
	driveplan = Resources.Load("botA_Drive");
	connectedDrive = Network.Instantiate(driveplan, (transform.position + mountDrive),Quaternion.identity, 0);
	connectedDrive.transform.parent = transform;
	
	headplan = Resources.Load("botA_Head");
	connectedHead = Network.Instantiate(headplan, (transform.position + mountHead),Quaternion.identity, 0);
	connectedHead.transform.parent = transform;
	
	controller = transform.GetComponent(CharacterController);
	AutoResizeController();
	
	spotlight = transform.Find("NarrowLight").gameObject;
}

function Update () {
	var mouseWorldPosition : Vector3 = Camera.main.ScreenToWorldPoint(Vector3(Input.mousePosition.x,Input.mousePosition.y,-Camera.main.transform.position.z));
	mouseWorldPosition.z = transform.position.z;
	spotlight.transform.LookAt(mouseWorldPosition);
}

function AutoResizeController(){
	//TODO: look at drive, body, and head to resize height of collider and adjust vertical center
	var newControllerHeight : float;
	var newControllerCenter : float;
	
	newControllerHeight = controllerDefaultHeight;
	//newControllerHeight = controllerDefaultHeight - 0.375; //no head piece, reduce height by .375
	controller.height = newControllerHeight;
	
	newControllerCenter = controllerDefaultCenter - (controllerDefaultHeight - newControllerHeight);
	controller.center = Vector3(0,newControllerCenter,0);
}