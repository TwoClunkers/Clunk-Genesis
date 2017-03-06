var myGUI : GUISkin;
var playerPrefab : GameObject;
var spawnObject: Transform;
var myGameName : String;
var worldgen : int;

//audio
var audioBlockDestroyed : AudioClip;
var audioBlockPickup : AudioClip;
var tickAudio : AudioClip;


@System.NonSerializedAttribute
var chatInputOpen : boolean = false;
@System.NonSerializedAttribute
var chatText : String;
@System.NonSerializedAttribute
var chatLog : String[] = new String[3];

private var refreshing : boolean = false;
private var hostData : HostData[];

function Awake(){
	chatLog[0] = " ";
	chatLog[1] = " ";
	chatLog[2] = " ";
	chatText = " ";
	myGameName = "Onshuu_Clunk_NetworkGame";
	
}

function OnGUI() {
	GUI.skin = myGUI;
	if(GUI.Button(Rect(Screen.width - 20,0,20,20), "X")) {
		Application.Quit();
	}
	if(chatInputOpen){
		if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return)){
			chatInputOpen = false;
			if(chatText != "" && chatText != " "){
				GetComponent.<NetworkView>().RPC("chatMessage",RPCMode.All, chatText);
				chatText = " ";
			}
		}
		GUI.SetNextControlName("chatInputBox");
		chatText = GUI.TextField(Rect(0, 60, 300, 20), chatText, 40);
		GUI.FocusControl("chatInputBox");
	}
	//chatLog
	GUI.Label(Rect(0,0,300,20),chatLog[0]);
	GUI.Label(Rect(0,20,300,20),chatLog[1]);
	GUI.Label(Rect(0,40,300,20),chatLog[2]);
	GUI.Label(Rect(Screen.width - 300,Screen.height - 20,200,20),"press c to toggle camera");
	GUI.Label(Rect(Screen.width - 300,Screen.height - 40,200,40),"press i to toggle inventory");
	
	if(!Network.isClient && !Network.isServer){
		if(GUI.Button(Rect(20,Screen.height-50,100,20),"Start Server")) {
			startServer();
		}
		if(GUI.Button(Rect(20,Screen.height-30,100,20),"Refresh Hosts")) {
			refreshHostsList();
		}
		
		if(hostData){
			for(var i:int = 0; i < hostData.length; i++){
				var sHostButtonText : String = hostData[i].gameName + " " + hostData[i].ip[0];
				if(GUI.Button(Rect(0,(Screen.height-90) - (20 * i),sHostButtonText.Length * 7,20), sHostButtonText)){
					Network.Connect(hostData[i]);
				}
			}
		}
	}
}

function spawnPlayer(){
	var thePlayer : Transform = (Network.Instantiate(playerPrefab, spawnObject.position, Quaternion.identity, 0)).transform;
	GameObject.FindGameObjectWithTag("MainCamera").GetComponent(PlatformCam).target = thePlayer;
}

function refreshHostsList(){
	MasterServer.RequestHostList(myGameName);
	refreshing = true;
}

function Update(){
	if(refreshing){
		if(MasterServer.PollHostList().Length > 0) {
			refreshing = false;
			Debug.Log(MasterServer.PollHostList().Length);
			hostData = MasterServer.PollHostList();
		}
	}
	if(Input.GetKeyUp(KeyCode.Return)){
		if(!chatInputOpen) {
			if(Network.isClient || Network.isServer){
				chatInputOpen = true;
			} else {
				displayLocalMessage("You are not connected to a server.");
			}
		}
	}
	if(Input.GetKeyUp(KeyCode.C)){
		Camera.main.orthographic = !Camera.main.orthographic;
	}
	if(Input.GetKey(KeyCode.KeypadPlus)){
		Camera.main.GetComponent(PlatformCam).zoomCamera(0.2);
	}
	if(Input.GetKey(KeyCode.KeypadMinus)){
		Camera.main.GetComponent(PlatformCam).zoomCamera(-0.2);
	}
	if(Input.GetKeyUp(KeyCode.K)) {
		GameObject.FindGameObjectWithTag("world").GetComponent(World).SaveAll();
	} 
}

function OnConnectedToServer() {
	Debug.Log("Connected to server");
	spawnPlayer();
}

function startServer(){
	Debug.Log("starting server");
	Network.InitializeServer(32,25001,!Network.HavePublicAddress);
	MasterServer.RegisterHost(myGameName,"Clunk","Clunk game host");
}

function OnServerInitialized(){
	Debug.Log("Server Initialized");
	//transform.GetComponent(WorldController).buildWorld(worldgen); //we need to contruct a new building routine
	spawnPlayer();
}

function OnPlayerDisconnected(player:NetworkPlayer){
	if(Network.isServer){
		Debug.Log("Player left, cleaning up.");
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}
}

function OnDisconnectedFromServer(info : NetworkDisconnection){
	if(Network.isClient){
		if(info.Equals(NetworkDisconnection.Disconnected)) {
			Debug.Log("Server connection closed.");
		} else {
			Debug.Log("Lost connection to Server.");
		}
	}
}

function OnMasterServerEvent(mse:MasterServerEvent){
	if(mse == MasterServerEvent.RegistrationSucceeded){
		Debug.Log("registration succeeded");
	}
}

function displayLocalMessage(msg : String){
	chatLog[0] = chatLog[1];
	chatLog[1] = chatLog[2];
	chatLog[2] = msg;
}

@RPC
function chatMessage(msg : String){
	chatLog[0] = chatLog[1];
	chatLog[1] = chatLog[2];
	chatLog[2] = msg;
}

@RPC
function playSound(clipName : String, location : Vector3){
	//Debug.Log("playing sound:" + clipName);
	if(clipName.Equals("blockDestroyed")){
		AudioSource.PlayClipAtPoint(audioBlockDestroyed,location);
	} else if(clipName.Equals("pickupBlock")){
		AudioSource.PlayClipAtPoint(audioBlockPickup,location);
	} else if(clipName.Equals("tickAudio")){
		AudioSource.PlayClipAtPoint(tickAudio,location);
	}
}

