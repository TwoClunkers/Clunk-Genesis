using System;
using System.Collections;
using UnityEngine;

namespace DataObjects
{
	[Serializable]
	public class ItemInfo
	{
		public int id;
		public String name;
		public String info;
		public ItemGroup group;
		public ItemTypes type;
		public Material material; //used for 3d object
		public Sprite sprite; //as inventory object in GUI
		public Mesh mesh; //as pickup or 3d object
		public int maxHealth; 
		public float size;
		public GameObject itemPrefab; //to hold script behaviour
		public Split construct; //to guide what makes this thing
		public Node[] attachments; //if things can be attached
		public StatBlock baseStats; //attributes, if applicable
		//public ItemAttribute[] attributes;

		public ItemInfo()
		{

		}
	}
}
