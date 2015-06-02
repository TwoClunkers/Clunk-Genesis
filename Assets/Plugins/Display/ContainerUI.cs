using UnityEngine;
using System.Collections;
using AssemblyCSharpfirstpass;
using UnityEngine.UI;
using DataObjects;

public class ContainerUI : MonoBehaviour
{
	public GameObject buttonPrefab;
	public Container storage;
	public int selectedSlot;
	public Button[] buttonList;
	public ItemLibrary itemLibrary;

	//public int rows; NOTE: perhaps we could enum different setups?
	
	void Start ()
	{
		InitializeButtons ();
	}

	//call after new container data is assigned
	void InitializeButtons () 
	{
		itemLibrary = GameObject.FindGameObjectWithTag ("mc").GetComponent<ItemLibrary> ();
		buttonList = new Button[storage.size];

		RectTransform slotRect = buttonPrefab.GetComponent<RectTransform> ();
		RectTransform containerRect = gameObject.GetComponent<RectTransform> ();

		float width = slotRect.rect.width * storage.size;
		float height = slotRect.rect.height;

		//adjust container height to fit buttons
		containerRect.offsetMin = new Vector2 (containerRect.offsetMin.x, 0);
		containerRect.offsetMax = new Vector2 (containerRect.offsetMin.x + width, containerRect.offsetMin.y + height);

		for (int i = 0; i < storage.size; i += 1) {
			//create our prefab slot
			GameObject newSlot = Instantiate(buttonPrefab) as GameObject;
			newSlot.transform.parent = gameObject.transform;

			//reposition
			RectTransform thisRect = newSlot.GetComponent<RectTransform> ();
			thisRect.offsetMin = new Vector2(containerRect.offsetMin.x + (slotRect.rect.width * i), 0);
			thisRect.offsetMax = new Vector2(containerRect.offsetMin.x + (slotRect.rect.width * i) + slotRect.rect.width, 0);
			thisRect.localScale = new Vector3(1, 1, 1);

			//save a reference to this slot and add a handler
			Button thisButton = newSlot.GetComponent<Button>();
			buttonList[i] = thisButton; //save for later
			int capture = i;
			thisButton.onClick.AddListener(() => leftclickSlot(capture));

			//refresh image and number
			refreshItem(i);

		}
		
	}

	public void remarkSelected() //aint workin!
	{
		Button thisButton = buttonList [selectedSlot];
		thisButton.Select ();
	}

	public void refreshItem(int slot) 
	{
		if (storage.size > slot) {
			ItemInfo ourInfo = new ItemInfo ();
			InventoryItem thisItem = storage.contents [slot];
			itemLibrary.getItemInfo (ourInfo, thisItem.id);
			Button thisButton = buttonList[slot];
			thisButton.transform.GetChild(0).GetComponent<Image>().sprite = ourInfo.sprite;
			Text thisText = thisButton.transform.GetChild(1).GetComponent<Text>();
			if(thisItem.quantity > 0) {
				thisText.text = thisItem.quantity.ToString();
				thisText.enabled = true;
			}
			else thisText.enabled = false;
		}
	}

	public void leftclickSlot(int slotNumber)
	{
		selectedSlot = slotNumber;
		GameObject holding = GameObject.FindGameObjectWithTag ("inhand");
		InHand inHand = holding.GetComponent<InHand> ();
		Container stuff = inHand.holder;
		stuff.contents [0] = storage.swapSlot (slotNumber, stuff.contents [0]);
		inHand.refreshItem ();
		refreshItem (slotNumber);


	}
	
	void Update ()
	{
//		if (storage.size > selectedSlot) {
//			Button selectedButton = buttonList[selectedSlot];
//			selectedButton.Select();
//		}
	}



	public InventoryItem pullCurrent(int placementSize) 
	{
		InventoryItem pulled = new InventoryItem ();

		if (selectedSlot > -1) {
			pulled = storage.getItem (selectedSlot);
			if (pulled.id > 0) {
				pulled.quantity = 1 - storage.pullSlot (selectedSlot, 1);
			}
		}

		return pulled;
	}

}

