using System;
using System.Collections;
using System.Collections.Generic;
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

		public int findItem(ItemTypes typeToFind, int startingIndex)
		{
			if (startingIndex < 0)
				startingIndex = 0;

			for (int i = startingIndex; i < library.Length; i+=1) {
				if (library [i].type == typeToFind)
					return i;
			}
			return -1;
		}

		public int[] findAll(ItemTypes typeToFind, int startingIndex)
		{
			List<int> itemList = new List<int>();
			int foundItem = -1;
			int nextIndex = startingIndex;

			while(nextIndex < library.Length) {
				foundItem = findItem(typeToFind, nextIndex);
				if(foundItem > -1) {
					itemList.Add(foundItem);
					nextIndex = foundItem+1;
				}
				else nextIndex += 1;
			}

			return itemList.ToArray ();
		}

		public int findRandom(ItemTypes typeToFind)
		{
			int[] possibles = findAll (typeToFind, 0);

			int listSize = possibles.Length;
			if (listSize < 1)
				return -1;
			else {
				return UnityEngine.Random.Range (0, listSize);
			}
		}

		public bool getItemInfo(ItemInfo info, int itemIndex)
		{
			//if (itemId == 0)
			//	return false;

			info.id = library[itemIndex].id;
			info.name = library[itemIndex].name;
			info.info = library[itemIndex].info;
			info.group = library[itemIndex].group;
			info.type = library[itemIndex].type;
			info.material = library[itemIndex].material;
			info.sprite = library[itemIndex].sprite;
			info.mesh = library[itemIndex].mesh;
			info.maxHealth = library[itemIndex].maxHealth;
			info.itemPrefab = library[itemIndex].itemPrefab;
			info.construct = library[itemIndex].construct;
			info.size = library [itemIndex].size;

			return true;
		}

		public ItemTypes getItemType (int itemId)
		{
			return library [itemId].type;
		}
		public Node[] getItemAttachments(int itemId)
		{
			if (library.Length > itemId) {

				return library [itemId].attachments.Clone () as Node[];
			} else {
				return null;
			}
		}

		public StatBlock getItemStats (int itemId)
		{
			if (library.Length > itemId) {
				return library [itemId].baseStats.getCopy ();
			} else {
				return null;
			}
		}
	}
}

