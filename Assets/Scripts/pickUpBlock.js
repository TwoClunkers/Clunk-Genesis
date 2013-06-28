// Ross Edited this 6/27/2013  ...
var itemID : int;
var itemQty : int;
var destroyAtThisTime : float;
var blockLifeTime : float;

function Start(){
	blockLifeTime = 300.0;
	destroyAtThisTime = Time.time + blockLifeTime; //seconds of lifetime for the pickup
	itemQty = 1;
}

function OnTriggerStay(other : Collider){
	if(other.tag.Equals("Player")){
		GameObject.FindGameObjectWithTag("gui").GetComponent(NetworkView).RPC("playSound", RPCMode.All, "pickupBlock", transform.position);
		destroyMe();
		//TODO: turn into RPC?:
		other.GetComponent(PlayerInventory).giveItem(itemID,itemQty);
	} else if(other.tag.Equals("pickup")){
		if(itemQty > 0 && other.GetComponent(pickUpBlock).itemQty > 0 ){ //if both blocks contain items
			if(other.GetComponent(pickUpBlock).itemID.Equals(itemID)){ //if the blocks are the same
				var bCombineToOther : boolean;
				bCombineToOther = true;
				if(other.GetComponent(pickUpBlock).itemQty > itemQty){ //other block has more
					bCombineToOther = true;
				} else if(other.GetComponent(pickUpBlock).itemQty < itemQty) { //this block has more
					bCombineToOther = false;
				} else {
					// blocks have equal quantity, combine to the one dying later
					if(other.GetComponent(pickUpBlock).destroyAtThisTime > destroyAtThisTime){ //combine to other
						bCombineToOther = true;
					} else { //combine to this block
						bCombineToOther = false;
					}
				}
				
				if(bCombineToOther) {
					other.GetComponent(pickUpBlock).itemQty += itemQty;
					itemQty = 0;
					other.GetComponent(pickUpBlock).destroyAtThisTime = Time.time + blockLifeTime;
					destroyAtThisTime = Time.time + 1.0;
				} else {
					itemQty += other.GetComponent(pickUpBlock).itemQty;
					other.GetComponent(pickUpBlock).itemQty = 0;
					other.GetComponent(pickUpBlock).destroyAtThisTime = Time.time + 1.0;
					destroyAtThisTime = Time.time + blockLifeTime;
				}
			}
		}
	}
}

function Update(){
	if(Time.time >= destroyAtThisTime) destroyMe();
}

function destroyMe(){
	try{ Network.RemoveRPCs(networkView.viewID); } catch(e){}
	try{ Network.Destroy(networkView.viewID); } catch(e){}
}
