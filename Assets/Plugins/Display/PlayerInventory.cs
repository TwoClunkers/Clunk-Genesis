using UnityEngine;
using System.Collections;
using System;
using AssemblyCSharpfirstpass;

public class PlayerInventory : MonoBehaviour
{ 
	public GameObject container;
	public GameObject inventory;
	public GameObject mainCanvas;
	public ContainerUI containerScript;
	public Container containerData;
	public bool changed;

	// Use this for initialization
	void Start ()
	{
		inventory = Instantiate(container) as GameObject;
		mainCanvas = GameObject.FindGameObjectWithTag ("UI");
		inventory.transform.SetParent (mainCanvas.transform, false);
		inventory.transform.localPosition = new Vector3 (0, -150, 0);
		containerScript = inventory.GetComponent<ContainerUI> ();
		containerData = containerScript.storage;
		changed = false;
	}
	
	void Update()
	{
		if (changed) {
			for (int i=0; i< containerData.size; i+=1) {
				containerScript.refreshItem (i);
			}
		}
	}
}

