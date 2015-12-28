using UnityEngine;
using System.Collections;
using PathologicalGames;
using DataObjects;

public class coreScript : partScript
{
	public Actor characterData = new Actor();
	
	private World world;
	
	void Start ()
	{
		itemLibrary = GameObject.FindGameObjectWithTag ("mc").GetComponent<ItemLibrary> ();
		//world = GameObject.FindGameObjectWithTag ("world").GetComponent<World> ();
	}
	
	void Update()
	{
		
	}
	
	public override void initialize (InventoryItem keyItem)
	{
		base.initialize (keyItem);

	}
	

}

