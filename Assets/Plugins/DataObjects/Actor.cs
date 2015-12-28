using UnityEngine;
using System.Collections;
using System;
namespace DataObjects
{
	[Serializable]
	public class Actor
	{
		public Schematic schema = new Schematic();

		//this dataobject is to save a copy of the entire character and also to provide
		//a templet for creating MOBs

		//I dont think it needs to derive from the Part class. Since we have a "Core" part, we have a starting 
		//point for saving location data. 

		public Actor ()
		{

		}

		//probably should have code for saving and loading

	}
}

