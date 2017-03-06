using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DataObjects;

public class EditingToolbar : MonoBehaviour {

	public bool retracted = false;
	public bool extended = false;

	public float positionTarget;
	public float positionOffset;

	public GameObject BrushPrefab;
	public GameObject CurrentBrush;

	// Use this for initialization
	void Start () {

		ChangeBrushSize (1.0f);
		positionOffset = -10.0f;
		positionTarget = -10.0f;

		CurrentBrush = GameObject.Instantiate (BrushPrefab, new Vector3 (transform.position.x+160, transform.position.y-405, 0), Quaternion.identity, transform);
	}
	
	// Update is called once per frame
	void Update () {
	

		if (!(retracted || extended)) { //we are moving 
			positionOffset = Mathf.Lerp (positionOffset, positionTarget, Time.deltaTime*8.0f);

			if (positionOffset < -9.99f) {
				retracted = true;
				positionOffset = -10.0f;
			} else if (positionOffset > 109.99) {
				extended = true;
				positionOffset = positionTarget;
			}

			Vector3 position = new Vector3 ();
			position = GetComponent<RectTransform> ().localPosition;
			position.x = positionOffset - 519.0f;
			GetComponent<RectTransform> ().localPosition = position;
		}
			
	}

	public void ChangeBrushSize(float f) {

	}

	public void ToggleExtention() {
		if (retracted) {
			ExtendToolbar ();
		} else {
			if (extended)
				RetractToolbar ();
		}
	}


	public void RetractToolbar() {
		retracted = false;
		extended = false;
		positionTarget = -10.0f;


	}

	public void ExtendToolbar() {
		retracted = false;
		extended = false;
		positionTarget = 110.0f;
	}

	public void SetVoxelMode() {
		CurrentBrush.GetComponent<BrushButtonScript> ().SetMode (Mode.voxel);
	}

	public void SetPlaneMode() {
		CurrentBrush.GetComponent<BrushButtonScript> ().SetMode (Mode.plane);
	}

	public void SetAdditiveApp() {
		CurrentBrush.GetComponent<BrushButtonScript> ().SetApp (App.additive);
	}

	public void SetSubtractiveApp() {
		CurrentBrush.GetComponent<BrushButtonScript> ().SetApp (App.subtractive);
	}

}
