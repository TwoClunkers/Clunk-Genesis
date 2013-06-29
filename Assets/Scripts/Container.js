#pragma strict

var containerName : String = "Generic";
var block : GameObject;

static var mouseItem : InventoryItem; //this is to hold the mouse drag
static var mouseRect : Rect = Rect(0,0,-30,-30);

var size = 12;
var width = 4;
var empty = 0;
var isOpen : boolean = false;
var show : boolean = false;
var pollRecipe : Split;

var setItem : InventoryItem; //this allows us to set our global mouseItem at startup.
var contents : InventoryItem[] = new InventoryItem[12]; //this holds the item data
var buttonsData : GUIContent[] = new GUIContent[12]; //this holds the button info...
var items : AllItems;

var origenPos : Vector3 = Vector3(0,0,0); //this should be set by the controller script 
var offsetPos : Vector3 = Vector3(-50,50,0); //we need to set this to relative to origen
var positionRect : Rect = Rect(25,25,160,80); //this should hold our adjusted footprint


//var selectedSlot : int = 0;

function Start () {
	items = GameObject.FindGameObjectWithTag("gui").GetComponent(guiScript).items;
	mouseItem = setItem;
}

function Update () {
	countEmptySlots();
	//offset from origen of control display
	if(show&&isOpen) {
		if(Input.GetKeyDown(KeyCode.R)) {
			dropItem(mouseItem);
			mouseItem = InventoryItem(0,0);
		}
		origenPos += offsetPos;
		positionRect.x = origenPos.x;
		positionRect.y = Screen.height - origenPos.y;
		
	    size = Mathf.Min(size,contents.Length,buttonsData.Length); //cut our size to max of array length
	  	for(var i = 0; i < contents.Length; i++){
	  		buttonsData[i] = items.library[contents[i].id].buttonContent; //get textures and tooltips	
	  	}
  	}
  
}

function countEmptySlots () {
	empty = 0;
	for(var i=0;i<size;i++) {
		if(contents[i].quantity < 1) empty += 1;
	}
}

function pollInputSingle(process : ProcessTypes) { 
	//checks for a single stack that can be broken by the given process
	//returns a recipe if available, or -1 if no stacks can be broken with this process

	var checkItem : Item;
	for(var i=0;i<size;i++) {
		checkItem = items.library[contents[i].id]; //get the item info for this slot
		//check that we have the right process
		if(process == checkItem.construct.process) {
			if(checkItem.construct.product.quantity < (contents[i].quantity+1)) {
				pollRecipe = checkItem.construct; //we have supply and tool to make this recipe
				return true;
			}
		}
	}
	//Debug.Log("NO MATCH");
	return false; //no match after checking all slots
}

function pollOutputMultiple(checkRecipe : Split) {
	//checks to see if given recipe can fit in available space
	//returns true if it can, false if it cannot
	
	var needempty = 0;
	for(var i=0;i<checkRecipe.components.length;i++) {
		var match = false;
		for(var a=0;a<size;a++) {
			if(checkRecipe.components[i].id == contents[a].id) {
				if(checkRecipe.components[i].quantity < (200-contents[a].quantity)) {
					match = true; //same type with enough room
				}
			}
		}
		if(!match) needempty += 1; //we will need an empty for each that was not matched with a partial
	}
	if(empty < needempty) return false;
	else return true;
}

function addItem(itemstack : InventoryItem) {
	//add a single item stack to this container, return amount we were not able to add
	
	var firstempty = -1;
	var quantityleft = itemstack.quantity;
	var quantitymoved = 0;
	for(var i=0;i<size;i++) {
		if(itemstack.id == contents[i].id) {
			quantitymoved = Mathf.Min(200-contents[i].quantity, quantityleft);
			contents[i].quantity += quantitymoved;
			quantityleft -= quantitymoved;
		}
		else if((contents[i].quantity < 1)&&(firstempty<0)) firstempty = i;
		if(quantityleft < 1) { //no more left
			return 0;
		}
	}
	//we either did not find a match, or had more than would fit
	if(firstempty<0) return quantityleft; //no place left to put it
	else { //we did find an empty slot
		contents[firstempty].id = itemstack.id;
		contents[firstempty].quantity += quantityleft;
		return 0; //we put the rest in the empty slot
	}	
 
}

function subtractItem(itemstack : InventoryItem) {
	//subtract a single item stack from this container, return amount we could not subtract

	var quantityleft = itemstack.quantity;
	var quantitymoved = 0;
	for(var i=0;i<size;i++) {
		if(itemstack.id == contents[i].id) {
			quantitymoved = Mathf.Min(contents[i].quantity, quantityleft);
			contents[i].quantity -= quantitymoved;
			quantityleft -= quantitymoved;
		}
		if(quantityleft < 1) { //no more left
			return 0;
		}
	}
	return quantityleft;
}

function dropItem(itemstack : InventoryItem) {
	
	if(itemstack.id > 0) {
		//create instance of this material in world
		var position : Vector3 = transform.position+Vector3(0,0,-1);
		var popDirection : Vector3 = Vector3(0,0,0);
		var pickupblock : GameObject;
		
		var oBlock : GameObject = Network.Instantiate(block, position, Quaternion.identity, 0);
		popDirection = Vector3(1,1,0); //(GameObject.FindGameObjectWithTag("Player").transform.position - position) * 3;
		oBlock.rigidbody.AddForce(popDirection,UnityEngine.ForceMode.VelocityChange);
		
		oBlock.GetComponent(pickUpBlock).itemID = itemstack.id;
		oBlock.GetComponent(pickUpBlock).itemQty = 5;//itemstack.quantity;
		oBlock.GetComponent(MeshFilter).mesh = items.library[itemstack.id].mesh;
		
		//Debug.Log("PickUpBlock ID: " + oBlock.networkView.viewID);
		GameObject.FindGameObjectWithTag("gui").GetComponent(NetworkView).RPC("setBlockMat", RPCMode.AllBuffered, oBlock.networkView.viewID, itemstack.id);
	}
}

function OnGUI() {

	if(show&&isOpen) {
		var e : Event = Event.current;
		var maxtosend : int = 0;
		var tempItem : InventoryItem;
		GUI.Box(Rect(positionRect.x-5,positionRect.y,width*40+10, Mathf.Ceil(size/width)*40+25), "");
		GUI.Label(Rect(positionRect.x,positionRect.y,100, 50), containerName);
		
		
		var i=0;
		for(var r = 0;(i<size) && (r<20);r++) {
			for(var w = 0;w < width;w++) {
				if(i<size) {
					if(GUI.Button(Rect(positionRect.x+(w*40),positionRect.y+(r*40)+20,40,40),buttonsData[i] )) { //true if clicked
						if(mouseItem.quantity > 0) { // we are holding item(s)
							if(contents[i].quantity > 0) { //the clicked container slot also has item(s)
								if(contents[i].id == mouseItem.id) { //we are holding a matching item
									if(e.button == 0) { //left click
										maxtosend = Mathf.Min(mouseItem.quantity, 200-contents[i].quantity); //amount recieving will hold
									}
									else if(e.button == 1) { //right click
										maxtosend = Mathf.Min(mouseItem.quantity, 1, 200-contents[i].quantity);
									}
									else { //middle click
										maxtosend = Mathf.Min(Mathf.Round(mouseItem.quantity/2),200-contents[i].quantity);
									}
									contents[i].quantity += maxtosend;
									mouseItem.quantity -= maxtosend;
									if(mouseItem.quantity < 1) mouseItem.id = 0;
								}
								else { //the items do not match we swap using a temp
									tempItem = contents[i];
									contents[i] = mouseItem;
									mouseItem = tempItem;
								}
							}
							else { //the container slot is empty so we can put it all in 
								if(e.button == 0) { //left click
									maxtosend = mouseItem.quantity;
								}
								else if(e.button == 1) { //right click
									maxtosend = 1;
								}
								else { //middle click
									maxtosend = Mathf.Round(mouseItem.quantity/2);
								}
								contents[i].quantity += maxtosend;
								if(maxtosend) contents[i].id = mouseItem.id;
								mouseItem.quantity -= maxtosend;
								if(mouseItem.quantity < 1) mouseItem.id = 0;
							}
						}
						else { // we are not holding anything
							if(e.button == 0) { //left click
		 						maxtosend = contents[i].quantity;
							}
							else if(e.button == 1) { //right click
		 						maxtosend = Mathf.Min(1, contents[i].quantity);
							}
							else { //middle click
		 						maxtosend = Mathf.Round(contents[i].quantity/2);
							}
							contents[i].quantity -= maxtosend;
							mouseItem.quantity += maxtosend;
							mouseItem.id = contents[i].id;
							if(contents[i].quantity < 1) contents[i].id = 0;
						}
	
					}
					if(contents[i].quantity>0) GUI.Label(Rect(positionRect.x+(w*40)+2,positionRect.y+(r*40)+18,40,40),contents[i].quantity+"" );
		 			i++;
		 		}
		 	}
		} 
		var screenPos : Vector2 = Event.current.mousePosition;
	    mouseRect.x = screenPos.x;
	    mouseRect.y = screenPos.y;
		if(mouseItem.quantity>0) {
		    GUI.DrawTexture(mouseRect,items.library[mouseItem.id].buttonContent.image);
		    GUI.Label(Rect(mouseRect.x-20,mouseRect.y-20,30,30),mouseItem.quantity+"");
	    }
		GUI.Label(Rect(mouseRect.x+10,mouseRect.y+10,100,20),GUI.tooltip);
	}
}
	/*
	if( Event.current.type == EventType.MouseDown && PointIsWithinRect( Event.current.MousePosition, myDraggableObject.rect ) )
    {
        currentlyDragged = myDraggableObject;
    }
    else if( Event.current.type == EventType.MouseDrag && currentlyDragged != null )
    {
        currentlyDragged.rect = new Rect( currentlyDragged.rect.x + Event.current.mousePosition.x, currentlyDragged.rect.y + Event.current.mousePosition.y, currentlyDragged.rect.width, currentlyDragged.rect.height );
    }
    else if( Event.current.type == EventType.MouseUp )
    {
        currentlyDragged = null;
    }
 
    
    //	
			if(contIsOpen){ //draw inventory window and contents
				GUI.Box(Rect(Screen.width/2+100, Screen.height/2-22,73,73)," ");
				GUI.Box(Rect(Screen.width/2+177, Screen.height/2-22,73,73)," ");
				GUI.SelectionGrid(Rect(Screen.width/2+177, Screen.height/2-22,73,73),0,selStrings,2, "box");
				//draw items in the inventory
				for(var i : int = 0; i < contents.Length; i++){
					if(contents[i].quantity > 0 ){ //draw items in the inventory
						GUI.DrawTexture(Rect(Screen.width/2+100+(i*35),Screen.height/2-22,30,30),items.library[contents[i].id].material.mainTexture);
						if( activeContainerSlot != -1 && i == activeContainerSlot) { //mark the selected slot
							GUI.DrawTexture(Rect(Screen.width/2+95+(i*35),Screen.height/2-22,40,40),activeOverlayTexture);
						}
						GUI.Label(Rect(Screen.width/2+100+(i*35),Screen.height/2-18,30,20),contents[i].quantity + "");	
 */