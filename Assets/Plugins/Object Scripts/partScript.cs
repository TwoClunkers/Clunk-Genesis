using UnityEngine;
using System.Collections;
using PathologicalGames;
using DataObjects;

public class partScript : MonoBehaviour
{
	public Part partData;
	
	public ItemLibrary itemLibrary;

	void Start ()
	{
		itemLibrary = GameObject.FindGameObjectWithTag ("mc").GetComponent<ItemLibrary> ();
	}
	
	void Update()
	{
		
	}
	
	public virtual void initialize (InventoryItem keyItem)
	{
		partData = new Part ();
		partData.createFromItem (keyItem, itemLibrary);
	}
	
	public void destroyMe()
	{
		PoolManager.Pools["parts"].Despawn(transform);
		//	try{ Network.RemoveRPCs(networkView.viewID); } catch(e){}
		//	try{ Network.Destroy(networkView.viewID); } catch(e){}
	}
}

