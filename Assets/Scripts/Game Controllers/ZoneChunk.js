#pragma strict

public var chunk : Chunk; //our container class
var control : WorldController;

function Awake () {
	chunk.data = new int[256];
	//clear(); //clears the layout to zeros
}
function Start () {

}

function Update () {

}

function show () { //this creates the objects to allow interaction
	var boxtype : int;
	var thisbox : GameObject;
	var boxscript : PlacedBlock;
	
	for(var b : int = 0; b < 16; b += 1) {
		for(var a : int = 0; a < 16; a += 1) {
			boxtype = chunk.data[a*16+b]; //we need to convert the chunk data to something usable
			if(boxtype < 1) continue;
			thisbox = control.placeBlock(boxtype, Vector3(transform.position.x+(b-8)*control.globalBlockScale, transform.position.y+(a-8)*control.globalBlockScale, 0) );
			boxscript = thisbox.GetComponent("PlacedBlock");
			boxscript.mydata = chunk.data;
			boxscript.myx = b;
			boxscript.myy = a;
			boxscript.mychunk = chunk.id;
		}
	}

}

function initialize (posx : int, posy : int) {
	control = transform.parent.GetComponent("WorldController");
	//this creates the internal date for this chunk based on the position and Seed

	chunk.id = posx + posy*16;
	chunk.strata = 0;  //NOT IMPLEMENTED
	chunk.zone = 0; //NOT IMPLEMENTED
	chunk.feature = GetIntFromSeed(chunk.id, 3);  //used for drawing shapes that match up across chunks
	clear(); //clears the layout to ones
	
	
	//TO DO: use chunk strata and zone to draw our chunk
	
	if(chunk.feature > 0) { //we have a feature
		var start : Vector2 = GetPointFromSeed(chunk.id);
		var end : Vector2;
		var bordertype : int;
		var w : int;
				
		//get border
		bordertype = GetIntFromSeed(chunk.id+1,3);
		if(bordertype > 0) { //there is rift in right frame
			bordertype = Mathf.Min(bordertype,chunk.feature); //the lesser feature is drawn
			end = GetPointFromSeed(chunk.id+1);
			if(bordertype > 2) drawCavity(start,end,1,1);
			if(bordertype > 1) w = 5;
			else w = 1;
			drawRift(start, end, 1, w);
		}
		bordertype = GetIntFromSeed(chunk.id-1,3);
		if(bordertype > 0) { //there is rift in right frame	
			bordertype = Mathf.Min(bordertype,chunk.feature);
			end = GetPointFromSeed(chunk.id-1);
			if(bordertype > 2) drawCavity(start, end, -1, 1);
			if(bordertype > 1) w = 5;
			else w = 1;		
			drawRift(start, end, -1, w);
		}
		bordertype = GetIntFromSeed(chunk.id+16,3);
		if(bordertype > 0) {//there is rift below
			bordertype = Mathf.Min(bordertype,chunk.feature);
			end = GetPointFromSeed(chunk.id+16);
			if(bordertype > 2) drawCavity(start, end, 16, 1);
			if(bordertype > 1) w = 5;
			else w = 1;	
			drawRift(start, end, 16, w);
		}
		bordertype = GetIntFromSeed(chunk.id-16,3);
		if(bordertype > 0) {//there is rift above
			bordertype = Mathf.Min(bordertype,chunk.feature);
			end = GetPointFromSeed(chunk.id-16);
			if(bordertype > 2) drawCavity(start, end, -16, 1);
			if(bordertype > 1) w = 5;
			else w = 1;	
			drawRift(start, end, -16, w);
		}
	}

}

function GetIntFromSeed(number : float, limit : int) : int {
	var answer : int = Mathf.FloorToInt((control.iSeed)/(number+1))&limit;//((Mathf.FloorToInt(control.iSeed)>>(Mathf.FloorToInt(number-(Mathf.FloorToInt(number/7)*7))))&limit);
	return answer;
}

function GetBoolFromSeed(number : float) : int {
	var answer : int = ((Mathf.FloorToInt(control.bSeed)>>(Mathf.FloorToInt(number-(Mathf.FloorToInt(number/9)*9))))&3);
	if(answer>0) return 1;
	else return -1;
}

function GetPointFromSeed(number : int) : Vector2 {
	var temp1 : int;
	var temp2 : int;
	temp1 = Mathf.FloorToInt((control.xSeed)/(number+1))&15;
	temp2 = Mathf.FloorToInt((control.ySeed)/(number+1))&15;
	//temp1 = ((Mathf.FloorToInt(control.xSeed)>>(Mathf.FloorToInt(number-(Mathf.FloorToInt(number/13)*13))))&15);
	//temp2 = ((Mathf.FloorToInt(control.ySeed)>>(Mathf.FloorToInt(number-(Mathf.FloorToInt(number/13)*13))))&15);
	return Vector2(temp1,temp2);
}

function clear()
{
	for(var a : int = 0; a < 16; a += 1) {
		for(var b : int = 0; b < 16; b += 1) chunk.data[a*16+b] = 3;
	}
}

function drawRift(startRift : Vector2, endRift : Vector2, direction : int, maxWidth : int) {
	var length : float = 0;
	var width : float;
	var posx : int;
	var posy : int;
	var a : float;
	var b : float;
	var temp : Vector2;
	
	if(direction == 1) { //end is in the frame to our right
		endRift.y = (Mathf.Lerp(startRift.y,endRift.y,0.5)); //get the mid point
		endRift.x = 16;
		length = Mathf.RoundToInt(endRift.x-startRift.x); //length to the border
		for(a=0; a < length; a += 1) {
			posx = startRift.x+a;
			posy = Mathf.RoundToInt(Mathf.Lerp(startRift.y,endRift.y,a/length));
			width = (Random.Range(1,maxWidth));
			for(b=0; b < width; b += 1) chunk.data[Mathf.Clamp(posy+b,0,15)*16+Mathf.Clamp(posx,0,15)] = 0;
			width = (Random.Range(1,maxWidth));
			for(b=0; b < width; b += 1) chunk.data[Mathf.Clamp(posy-b,0,15)*16+Mathf.Clamp(posx,0,15)] = 0;
		}
	}
	else if(direction == -1) { //end is in the frame to our left
		endRift.y = (Mathf.Lerp(startRift.y,endRift.y,0.5)); //get the mid point
		endRift.x = 0;
		temp = endRift; endRift = startRift; startRift = temp; //we are gonna still draw from left to right
		length = endRift.x; //length to the border
		for(a=0; a < length; a += 1) {
			posx = startRift.x+a;
			posy = Mathf.RoundToInt(Mathf.Lerp(startRift.y,endRift.y,a/length));
			width = (Random.Range(1,maxWidth));
			for(b=0; b < width; b += 1) chunk.data[Mathf.Clamp(posy+b,0,15)*16+Mathf.Clamp(posx,0,15)] = 0;
			width = (Random.Range(1,maxWidth));
			for(b=0; b < width; b += 1) chunk.data[Mathf.Clamp(posy-b,0,15)*16+Mathf.Clamp(posx,0,15)] = 0;
		}
	}
	else if(direction == 16) { //end is in the frame above?
		endRift.x = (Mathf.Lerp(startRift.x,endRift.x,0.5)); //get the mid point for the border
		endRift.y = 16;
		length = Mathf.RoundToInt(endRift.y-startRift.y); //length to the border 
		for(a=0; a < length; a += 1) {
			posx = Mathf.RoundToInt(Mathf.Lerp(startRift.x,endRift.x,a/length));
			posy = startRift.y+a;
			width = (Random.Range(1,maxWidth));
			for(b=0; b < width; b += 1) chunk.data[Mathf.Clamp(posy,0,15)*16+Mathf.Clamp(posx+b,0,15)] = 0;
			width = (Random.Range(1,maxWidth));
			for(b=0; b < width; b += 1) chunk.data[Mathf.Clamp(posy,0,15)*16+Mathf.Clamp(posx-b,0,15)] = 0;
		}		
	}
	else if(direction == -16) { //end is in the frame below?
		endRift.x = (Mathf.Lerp(startRift.x,endRift.x,0.5)); //get the mid point for the border
		endRift.y = 0;
		temp = endRift; endRift = startRift; startRift = temp; //we are gonna still draw from bottom to top
		length = endRift.y; //length to the border 
		for(a=0; a < length; a += 1) {
			posx = Mathf.RoundToInt(Mathf.Lerp(startRift.x,endRift.x,a/length));
			posy = startRift.y+a;
			width = (Random.Range(1,maxWidth));
			for(b=0; b < width; b += 1) chunk.data[Mathf.Clamp(posy,0,15)*16+Mathf.Clamp(posx+b,0,15)] = 0;
			width = (Random.Range(1,maxWidth));
			for(b=0; b < width; b += 1) chunk.data[Mathf.Clamp(posy,0,15)*16+Mathf.Clamp(posx-b,0,15)] = 0;
		}		
	}

}

function drawCavity(startRift : Vector2, endRift : Vector2, direction : int, maxWidth : int) {
	var length : float = 0;
	var width : float;
	var posx : int;
	var posy : int;
	var a : float;
	var b : float;
	var temp : Vector2;
	
	if(direction == 1) { //end is in the frame to our right
		endRift.x = 16;
		length = 9; //length to the border
		for(a=0; a < length; a += 1) {
			width = Mathf.Clamp(Random.Range(a/2,2+a/2),2,7);
			posx = 7+a;
			posy = 7;
			for(b=0; b < width; b += 1) chunk.data[Mathf.Clamp(posy+b,0,15)*16+Mathf.Clamp(posx,0,15)] = 0;
			width = Mathf.Clamp(Random.Range(a/2,2+a/2),2,7);
			for(b=0; b < width; b += 1) chunk.data[Mathf.Clamp(posy-b,0,15)*16+Mathf.Clamp(posx,0,15)] = 0;
		}
	}
	if(direction == -1) { //end is in the frame to our left
		endRift.x = 0;
		length = 9; //length to the border
		for(a=0; a < length; a += 1) {
			width = Mathf.Clamp(Random.Range(7-a/2,5-a/2),2,7);
			posx = 0+a;
			posy = 7;
			for(b=0; b < width; b += 1) chunk.data[Mathf.Clamp(posy+b,0,15)*16+Mathf.Clamp(posx,0,15)] = 0;
			width = Mathf.Clamp(Random.Range(7-a/2,5-a/2),2,7);
			for(b=0; b < width; b += 1) chunk.data[Mathf.Clamp(posy-b,0,15)*16+Mathf.Clamp(posx,0,15)] = 0;
		}
	}
	if(direction == 16) { //end is in the frame above?
		endRift.y = 16;
		length = 9; //length to the border 
		for(a=0; a < length; a += 1) {
			width = Mathf.Clamp(Random.Range(a/2,2+a/2),2,7);
			posx = 7;
			posy = 7+a;
			for(b=0; b < width; b += 1) chunk.data[Mathf.Clamp(posy,0,15)*16+Mathf.Clamp(posx+b,0,15)] = 0;
			width = Mathf.Clamp(Random.Range(a/2,2+a/2),2,7);
			for(b=0; b < width; b += 1) chunk.data[Mathf.Clamp(posy,0,15)*16+Mathf.Clamp(posx-b,0,15)] = 0;
		}		
	}
	if(direction == -16) { //end is in the frame below?
		endRift.y = 0;
		length = 9; //length to the border 
		for(a=0; a < length; a += 1) {
			width = Mathf.Clamp(Random.Range(7-a/2,5-a/2),2,7);
			posx = 7;
			posy = 0+a;
			for(b=0; b < width; b += 1) chunk.data[Mathf.Clamp(posy,0,15)*16+Mathf.Clamp(posx+b,0,15)] = 0;
			width = Mathf.Clamp(Random.Range(7-a/2,5-a/2),2,7);
			for(b=0; b < width; b += 1) chunk.data[Mathf.Clamp(posy,0,15)*16+Mathf.Clamp(posx-b,0,15)] = 0;
		}		
	}

}