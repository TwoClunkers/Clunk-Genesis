// Ross Edited this 6/27/2013  ...
import PathologicalGames;

var destroyAtThisTime : float;
var blockLifeTime : float;
var invItem : InventoryItem;
var targetable : Targetable;
var testit : boolean;

function Awake()
{
	targetable = this.GetComponent("Targetable");
}
function Start(){
	blockLifeTime = 100.0;
	destroyAtThisTime = Time.time + blockLifeTime; //seconds of lifetime for the pickup
	invItem.quantity = 1;
}
function InitializeBlock(){
	blockLifeTime = 100.0;
	destroyAtThisTime = Time.time + blockLifeTime; //seconds of lifetime for the pickup
	invItem.quantity = 1;
}

function OnTriggerStay(other : Collider){
	if(other.tag.Equals("Player")){
		GameObject.FindGameObjectWithTag("mc").GetComponent(NetworkView).RPC("playSound", RPCMode.All, "pickupBlock", transform.position);
		destroyMe();
		//TODO: turn into RPC?:
		other.GetComponent(PlayerInventory).inventory.addItem(invItem);
	} else if(other.tag.Equals("pickup")){
		if(invItem.quantity > 0 && other.GetComponent(pickUpBlock).invItem.quantity > 0 ){ //if both blocks contain items
			if(other.GetComponent(pickUpBlock).invItem.id.Equals(invItem.id)){ //if the blocks are the same
				var bCombineToOther : boolean;
				bCombineToOther = true;
				if(other.GetComponent(pickUpBlock).invItem.quantity > invItem.quantity){ //other block has more
					bCombineToOther = true;
				} else if(other.GetComponent(pickUpBlock).invItem.quantity < invItem.quantity) { //this block has more
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
					other.GetComponent(pickUpBlock).invItem.quantity += invItem.quantity;
					invItem.quantity = 0;
					other.GetComponent(pickUpBlock).destroyAtThisTime = Time.time + blockLifeTime;
					destroyAtThisTime = Time.time + 1.0;
				} else {
					invItem.quantity += other.GetComponent(pickUpBlock).invItem.quantity;
					other.GetComponent(pickUpBlock).invItem.quantity = 0;
					other.GetComponent(pickUpBlock).destroyAtThisTime = Time.time + 1.0;
					destroyAtThisTime = Time.time + blockLifeTime;
				}
			}
		}
	}
}

function Update(){
	if(invItem.quantity < 1) destroyMe();
	if(Time.time >= destroyAtThisTime) destroyMe();
}

function destroyMe(){
	blockLifeTime = 300.0;
	PoolManager.Pools["drops"].Despawn(transform);
//	try{ Network.RemoveRPCs(networkView.viewID); } catch(e){}
//	try{ Network.Destroy(networkView.viewID); } catch(e){}
}
