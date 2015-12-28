using UnityEngine;
using System.Collections;
using PathologicalGames;
using DataObjects;

//The Drive Object will turn in the direction of travel. This is set by the Core object. 
//Since most everything will be managed by the Core, we will have little to do here. 

public class driveScript : partScript
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public override void initialize (InventoryItem keyItem)
	{
		base.initialize (keyItem);

	}

}

