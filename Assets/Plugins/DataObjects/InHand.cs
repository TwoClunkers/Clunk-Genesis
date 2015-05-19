using UnityEngine;
using System.Collections;
using AssemblyCSharpfirstpass;
using UnityEngine.UI;
using DataObjects;

public class InHand : MonoBehaviour {

	public Container holder = new Container(1);
	public ItemLibrary itemLibrary;

	// Use this for initialization
	void Start () {
		itemLibrary = GameObject.FindGameObjectWithTag ("mc").GetComponent<ItemLibrary> ();
		refreshItem ();
	}

	void Update() {
		transform.position = Input.mousePosition;
	}
	
	public void refreshItem() 
	{
		ItemInfo ourInfo = new ItemInfo ();
		itemLibrary.getItemInfo (ourInfo, holder.contents [0].id);
		this.GetComponent<Image> ().sprite = ourInfo.sprite;
		Text thisText = this.transform.GetChild(0).GetComponent<Text>();
		if(holder.contents [0].quantity > 0) {
			thisText.text = holder.contents [0].quantity.ToString();
			thisText.enabled = true;
		}
		else thisText.enabled = false;
	}
}
