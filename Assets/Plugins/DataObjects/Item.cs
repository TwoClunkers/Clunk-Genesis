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
		public String task;
		public String info;
		public ItemTypes type;
		public Material material;
		public Sprite sprite; //as inventory
		public Mesh mesh; //as pickup
		public int maxHealth;
		public GameObject itemPrefab;
		public Split construct;
		//public ItemAttribute[] attributes;

		public ItemInfo()
		{

		}
	}
}
