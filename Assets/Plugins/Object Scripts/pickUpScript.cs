using UnityEngine;
using System.Collections;
using PathologicalGames;
//using PlayerInventory;
using DataObjects;

public class pickUpScript : MonoBehaviour
{
	public Pickup pickup;
	public bool canStack = false;

	void Start ()
	{

	}

	void Update()
	{
		if (pickup.stackCheck (Time.time))
			canStack = true;

		if(pickup.item.quantity < 1) destroyMe();
		else if(pickup.destroyCheck(Time.time)) destroyMe();
	}
	
	void InitializePickup(int ID, int newQuant)
	{
		pickup.reset(ID, newQuant);
	}

	void OnTriggerStay(Collider other)
	{
		if(other.tag.Equals("Player")){
			//GameObject.FindWithTag("mc").GetComponent(NetworkView).RPC("playSound", RPCMode.All, "pickupBlock", transform.position);

			//TODO: turn into RPC?:
			GameObject player = other.gameObject;
			//PlayerInventory 
			PlayerInventory thisInv = player.GetComponent<PlayerInventory>();
			//need to run through ContainerUI
			int remainder = thisInv.containerData.addItem(pickup.invItem());
			thisInv.changed = true;
			if(remainder>0) {
				pickup.item.quantity = remainder;
			}
			else destroyMe();
		} else if(other.tag.Equals("pickup")){ //combine data
			if(canStack) {
				pickUpScript spup = other.GetComponent<pickUpScript>();
				if(spup.canStack) pickup.combinePickups(spup.pickup);
			}
		}
	}

	void destroyMe()
	{
		PoolManager.Pools["drops"].Despawn(transform);
		//	try{ Network.RemoveRPCs(networkView.viewID); } catch(e){}
		//	try{ Network.Destroy(networkView.viewID); } catch(e){}
	}
}

