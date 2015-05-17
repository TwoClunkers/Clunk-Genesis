using UnityEngine;
using System.Collections;
using DataObjects;
using AssemblyCSharpfirstpass;
using UnityEngine.UI;

public class ContainerUI : MonoBehaviour
{
	public GameObject buttonPrefab;
	public Container storage;
	public int selectedSlot;
	private Button[] buttonList;

	//public int rows; NOTE: perhaps we could enum different setups?
	
	void Start ()
	{
		Button[] buttonList = new Button[storage.size];

		RectTransform slotRect = buttonPrefab.GetComponent<RectTransform> ();
		RectTransform containerRect = gameObject.GetComponent<RectTransform> ();

		float width = slotRect.rect.width * storage.size;
		float height = slotRect.rect.height;

		//adjust container height
		containerRect.offsetMin = new Vector2 (containerRect.offsetMin.x, 0);
		containerRect.offsetMax = new Vector2 (containerRect.offsetMin.x + width, containerRect.offsetMin.y + height);

		for (int i = 0; i < storage.size; i += 1) {
			GameObject newSlot = Instantiate(buttonPrefab) as GameObject;
			newSlot.transform.parent = gameObject.transform;

			//reposition
			RectTransform thisRect = newSlot.GetComponent<RectTransform> ();
			thisRect.offsetMin = new Vector2(containerRect.offsetMin.x + (slotRect.rect.width * i), 0);
			thisRect.offsetMax = new Vector2(containerRect.offsetMin.x + (slotRect.rect.width * i) + slotRect.rect.width, 0);
			thisRect.localScale = new Vector3(1, 1, 1);

			Button thisButton = newSlot.GetComponent<Button>();
			buttonList[i] = thisButton; //save for later
			int capture = i;
			thisButton.onClick.AddListener(() => setSelectedSlot(capture));

		}
		
	}

	public void setSelectedSlot(int slotNumber)
	{
		selectedSlot = slotNumber;
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

