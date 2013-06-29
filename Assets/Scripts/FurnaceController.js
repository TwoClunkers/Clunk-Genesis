#pragma strict

var contIsOpen : boolean = true;
var contIsSelected : boolean = true;
var contIsPowered : boolean = true;
var contIsProcessing : boolean = false;
var contHasSupply : boolean = false;
var contHasRoom : boolean = false;
var contCanActivate : boolean = false;
var contIsAutoActive : boolean = false;
var contIsAutoEject : boolean = false;
var contProcessType : ProcessTypes = ProcessTypes.smelt;
public var contRecipe : Split;



var contents : InventoryItem[] = new InventoryItem[10];
var items : AllItems;

var positionRect : Rect = Rect(25,25,140,95); //this should hold our adjusted footprint
var offsetPos : Vector3 = Vector3(-70, 145, 0);
var containers : Container[];
var inputContainer : Container;
var outputContainer : Container;

var activeContainerSlot : int = -1;
var activeOverlayTexture : Texture;
var powerOnTexture : Texture;
var powerOffTexture : Texture;
var powerState : Texture;
var activeReadyTexture : Texture;
var activeOffTexture : Texture;
var activeProcessTexture : Texture;
var activeState : Texture;
var autorunOnTexture : Texture;
var autorunOffTexture : Texture;
var autorunState : Texture;
var autoejectOnTexture : Texture;
var autoejectOffTexture : Texture;
var autoejectState : Texture;

var myFont : Font;
var styleName : GUIStyle;

public var cycleState = 0; //empty, pending, active, finished
public var itemSlot = 1; //inventory = item slots * capacity

private var processCount = 0; //current process timer
public var recipeDuration = 1000; //length a process will take
var cooldown = 50;
private var processSpeed = 1; //steps per update for this machine
private var newColor = Color.magenta;
private var flashColor = Color.black;
private var flashFrame = 0;
private var processIntensity = 2.00;
private var processLevel = 0.00;
var distance : float;

function Start () {
	items = GameObject.FindGameObjectWithTag("mc").GetComponent(ItemController).items;
	containers = gameObject.GetComponents.<Container>();
	inputContainer = containers[0];
	outputContainer = containers[1];
}

function OnMouseUpAsButton () { //toggle whether this object is selected
	contIsSelected = !contIsSelected;
	inputContainer.show = !inputContainer.show;
	outputContainer.show = !outputContainer.show;
}


function Update () {

	if(contIsSelected) { //we only need to calculate this is we are going to show it in GUI layer
		//below variables and calculations to find screen origen for Gui Display
		//this is based on the containing object position and the direction to player
		var screenPos : Vector3;
		var heading : Vector3;
		var playerHandle : GameObject = GameObject.FindGameObjectWithTag("Player");
		
		if(playerHandle) { //offset for player position
			
			heading = (this.gameObject.transform.position - playerHandle.transform.position);
			heading.z = 0;
			distance = heading.magnitude;
			if(distance > 3) { //out of range, so turn off our GUI
				contIsSelected = false;
				inputContainer.show = false;
				outputContainer.show = false;
			}
			else screenPos = Camera.main.WorldToScreenPoint(this.gameObject.transform.position+(heading/distance));
		}
		else screenPos = Camera.main.WorldToScreenPoint(this.gameObject.transform.position);
	
		inputContainer.origenPos = screenPos;
		outputContainer.origenPos = screenPos;
		//offset from origen point of control display
		screenPos += offsetPos;
		positionRect.x = screenPos.x;
		positionRect.y = Screen.height - screenPos.y;
	}

	
	//act if someone opened us - make the graphical changes
	//TO DO: Generalize this to work in other machines
	if(contIsOpen) {
		transform.FindChild("Gate").GetComponent(Moveit).isOpen=1;
		inputContainer.isOpen = true;
		outputContainer.isOpen = true;
	}
	else {
		transform.FindChild("Gate").GetComponent(Moveit).isOpen=0;	
		inputContainer.isOpen = false;
		outputContainer.isOpen = false;
	}
	//this is our graphical furnace fire ramp when turned off or on.
	processIntensity = Mathf.Lerp(processIntensity, processLevel, Time.deltaTime);
	if(processIntensity < 0.01) processIntensity = 0;
	else if(processIntensity > 1.99) processIntensity = 2;
	transform.FindChild("FurnaceFire").light.intensity = processIntensity;
	
	if(cycleState == 1) { 
		contCanActivate = true;
		if(contIsAutoActive) {
			contIsProcessing = true;
		}
	}                         
	else contCanActivate = false;	
	
	if(cycleState < 1) { //furnace is empty
		//check if our container meets criteria for processing and pick the recipe
		contHasSupply = inputContainer.pollInputSingle(contProcessType);
		
		//now to check if we have enough room to store a run
		if(contHasSupply) {
			contRecipe = inputContainer.pollRecipe;
			contHasRoom = outputContainer.pollOutputMultiple(contRecipe);
		}
		if(!contHasRoom) { //no room for this recipe
			newColor = Color.red;
			setIndicator();	
		}	
		if(contHasSupply && contHasRoom) { //items were placed
			cycleState = 1; //pending
			newColor = Color.green;
			setIndicator();
		}
	}
	else if(cycleState > 2) { //cycle is complete
		processCount += processSpeed;
		if(processCount>cooldown) {
			contHasSupply = inputContainer.pollInputSingle(contProcessType);
			if(contHasSupply) {
					contRecipe = inputContainer.pollRecipe;
					contHasRoom = outputContainer.pollOutputMultiple(contRecipe);
			}
			else {
				cycleState = 0; //ready for more material
				newColor = Color.Lerp(Color.blue, Color.white, .2);
				setIndicator();
			}
			if(!contHasRoom) { //no room for this recipe
				cycleState = 0;
				newColor = Color.red;
				setIndicator();	
			}
			if(contHasSupply && contHasRoom) { //items were placed
				cycleState = 1; //pending
				newColor = Color.green;
				setIndicator();
			}
		}
	}
	else if(cycleState == 1) { //furnace is pending, which means we have feed material in it, but have not activated it
		contHasSupply = inputContainer.pollInputSingle(contProcessType);
		if(!contHasSupply) { //items were removed
			cycleState = 0; ////we opened it up, no feed (does not mean we took everything out-may have some finished stuff)
			newColor = Color.Lerp(Color.blue, Color.white, .2);
			setIndicator();
		}
		else if(contIsProcessing) { //activated through the control or by mouse
			//close the door and fire up the oven
			processLevel = 2;
			contIsOpen = false;
			cycleState = 2; //now running
			newColor = Color.yellow;
			setIndicator();
			processCount=0;
		}
	}
	else if(cycleState == 2) { //cycle is in process
	
		//Countdown timer 
		processCount += processSpeed;
		newColor = Color.Lerp(newColor, flashColor, Time.deltaTime);
		setIndicator();
		
		if(processCount>recipeDuration) { //done processing - push to "finished" cycle
			finishProcess(contRecipe);
			processCount=0;
			contIsProcessing = false;
			processLevel = 0;
			cycleState = 3;
			newColor = Color.green;
			setIndicator();
		}
		else { //we are still processing - lets flash our light
			flashFrame += 1;
			if(flashFrame>100) { //end of "half a flash" lerp back other way
				flashFrame=0;
				if(flashColor == Color.yellow) {
					flashColor = Color.black;
				}
				else {
					flashColor = Color.yellow;
				}
			}
		}
	}	
	
}

function setIndicator() {
	var indicator = transform.FindChild("Indicator");
	indicator.renderer.material.color = newColor;
	indicator.FindChild("Point light").light.color = newColor;
}

function finishProcess (recipe : Split) {
	var drop : InventoryItem = InventoryItem(0, 0);
	inputContainer.subtractItem(recipe.product);
	for(var i = 0;i < recipe.components.length;i++) {
		if(contIsAutoEject) {
			outputContainer.dropItem(recipe.components[i]);
		}
		else {
			drop.quantity = outputContainer.addItem(recipe.components[i]);
			if(drop.quantity > 0) {
				drop.id = recipe.components[i].id;
				outputContainer.dropItem(drop);
			}
		}
	}

}			

function OnGUI() {

	if(contIsSelected){ //our parent is selected, so we need to draw
	
	    //Name and Type
	    GUI.Box(positionRect," ");
	    styleName.fontSize = 25;
		GUI.Label(Rect(positionRect.x+5,positionRect.y,50,25), "Ace BX30", styleName);
		styleName.fontSize = 15;
	  	GUI.Label(Rect(positionRect.x+5,positionRect.y+25,50,15), "Furnace", styleName);
	
		//Power Indicator
		if(contIsPowered) {
			powerState = powerOnTexture;
		}
		else powerState = powerOffTexture;
		GUI.DrawTexture(Rect(positionRect.x+5,positionRect.y+44,30,30),powerState);
		
		//Activate Button
		if(contIsProcessing) {
			activeState = activeProcessTexture;
		}
		else {
			if(!contCanActivate) activeState = activeOffTexture;
			else activeState = activeReadyTexture;
		}
		if(contCanActivate) {
			contIsProcessing = GUI.Toggle(Rect(positionRect.x+40,positionRect.y+44,30,30), contIsProcessing, "", "button");
		}
		GUI.DrawTexture(Rect(positionRect.x+40,positionRect.y+44,30,30), activeState);
		
		//AutoRun Toggle
		if(contIsAutoActive) {
			autorunState = autorunOnTexture;
		}
		else autorunState = autorunOffTexture;
		contIsAutoActive = GUI.Toggle(Rect(positionRect.x+75,positionRect.y+44,30,30), contIsAutoActive, autorunState, "button");
		GUI.DrawTexture(Rect(positionRect.x+75,positionRect.y+44,30,30), autorunState);
		
		//AutoEject Toggle
		if(contIsAutoEject) {
			autoejectState = autoejectOnTexture;
		}
		else autoejectState = autoejectOffTexture;
		contIsAutoEject = GUI.Toggle(Rect(positionRect.x+110,positionRect.y+44,30,30), contIsAutoEject, "", "button");	
		GUI.DrawTexture(Rect(positionRect.x+110,positionRect.y+44,30,30), autoejectState);	
		
		//Open/Close Inventory Toggle
		if(!contIsProcessing) { //cannot open while processing
			contIsOpen = GUI.Toggle(Rect(positionRect.x+35,positionRect.y+76,80,16), contIsOpen, "", "button");
			//in update, we will show our containers
		}
		GUI.DrawTexture(Rect(positionRect.x+55,positionRect.y+76,40,16), activeState);
	}
}