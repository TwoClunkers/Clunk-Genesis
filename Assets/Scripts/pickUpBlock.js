// Ross Edited this 6/27/2013  ...
import PathologicalGames;

var pickup : Pickup;

function Awake()
{

}
function Start(){
	pickup.initialize(0, 1);
}
function InitializePickup(ID : int, newQuant : int)
{
	pickup.initialize(ID, newQuant);
}

function OnTriggerStay(other : Collider)
{
	if(other.tag.Equals("Player")){
		GameObject.FindGameObjectWithTag("mc").GetComponent(NetworkView).RPC("playSound", RPCMode.All, "pickupBlock", transform.position);
		destroyMe();
		//TODO: turn into RPC?:
		other.GetComponent(PlayerInventory).inventory.addItem(pickup.invItem());
	} else if(other.tag.Equals("pickup")){ //combine data
		pickup.combinePickups(other.GetComponent(pickUpBlock).pickup);
	}
}

function Update()
{
	if(pickup.quantity < 1) destroyMe();
	if(pickup.destroyCheck(Time.time)) destroyMe();
}

function destroyMe()
{
	blockLifeTime = 100.0;
	PoolManager.Pools["drops"].Despawn(transform);
//	try{ Network.RemoveRPCs(networkView.viewID); } catch(e){}
//	try{ Network.Destroy(networkView.viewID); } catch(e){}
}
