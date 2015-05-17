using System;
using System.Collections;
using UnityEngine;



namespace DataObjects
{
	[Serializable]
	public class ItemLibrary : MonoBehaviour
	{
		public ItemInfo[] library; 

		public ItemLibrary()
		{
			library = new ItemInfo[40];
		}

		void Start()
		{
			//library = new ItemInfo[40];

//			for(int i = 0; i < library.Length ; i+=1) {
//				library[i] = new ItemInfo ();
//			}
		}

		public bool getItemInfo(ItemInfo info, int itemId)
		{
			if (itemId == 0)
				return false;

			info.id = library[itemId].id;
			info.name = library[itemId].name;
			info.task = library[itemId].task;
			info.info = library[itemId].info;
			info.type = library[itemId].type;
			info.material = library[itemId].material;
			info.mesh = library[itemId].mesh;
			info.maxHealth = library[itemId].maxHealth;
			info.itemPrefab = library[itemId].itemPrefab;
			info.buttonContent = library[itemId].buttonContent;
			info.construct = library[itemId].construct;
			//info.attributes = library[itemId].attributes;

			return true;
		}
	}
}

