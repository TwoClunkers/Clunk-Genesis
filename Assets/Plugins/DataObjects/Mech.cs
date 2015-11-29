using UnityEngine;
using System.Collections;
using System;

namespace DataObjects
{
	[Serializable]
	public class Mech : StorableObject
	{
		public float currentPower;
		public bool active;
		public Attachment[] slots;

		public Attachment coupler; //pointer to where we attach

		public float powerIn; //we will take this if needed
		public float powerOut; //we will put power here if we have extra

		public float heat; 
		public float heatResistance;

		public float armor;

		public Mech ()
		{


		}
	}
}
