using UnityEngine;
using System.Collections;
using DataObjects;
using PathologicalGames;
//using PlayerInventory;

public class pickUpScript : MonoBehaviour
{
	public Pickup pickup;

	void Start ()
	{
		pickup.initialize(0, 1);
	}

	void Update()
	{
		if(pickup.quantity < 1) destroyMe();
		if(pickup.destroyCheck(Time.time)) destroyMe();
	}
	
	void InitializePickup(int ID, int newQuant)
	{
		pickup.initialize(ID, newQuant);
	}

	void OnTriggerStay(Collider other)
	{
		if(other.tag.Equals("Player")){
			//GameObject.FindWithTag("mc").GetComponent(NetworkView).RPC("playSound", RPCMode.All, "pickupBlock", transform.position);
			destroyMe();
			//TODO: turn into RPC?:
			GameObject player = other.gameObject;
			//PlayerInventory 
			//player.GetComponent("PlayerInventory") as PlayerInventory; .inventory.addItem(pickup.invItem());
		} else if(other.tag.Equals("pickup")){ //combine data
			pickUpScript spup = other.GetComponent<pickUpScript>();
			pickup.combinePickups(spup.pickup);
		}
	}

	void destroyMe()
	{
		PoolManager.Pools["drops"].Despawn(transform);
		//	try{ Network.RemoveRPCs(networkView.viewID); } catch(e){}
		//	try{ Network.Destroy(networkView.viewID); } catch(e){}
	}
}

