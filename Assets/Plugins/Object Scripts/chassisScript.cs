using UnityEngine;
using System.Collections;
using PathologicalGames;
using DataObjects;

//The chassis, or mid-chassis is responsible for turning in the direction of target
//However, we know the core may be turning to move in a particular direction

public class chassisScript : partScript
{
	public Vector3 targetRotation = new Vector3();

	//directionToTarget
	//parentRotation
	//angletoTarget = directionToTarget-parentRotation;
	//localRotation = slerp(localRotation, angletoTarget, something something time.deltatime);


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

