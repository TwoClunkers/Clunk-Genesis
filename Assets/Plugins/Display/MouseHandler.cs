using UnityEngine;
using System.Collections;

public class MouseHandler : MonoBehaviour
{
	public float d = 0.0f;
	public bool incRadius = false;
	public bool decRadius = false;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		d = Input.GetAxis("Mouse ScrollWheel");
		if (d > 0.0f) {
			if (Input.GetKey (KeyCode.R)) {
				incRadius = true;
				decRadius = false;
			} 
		} else if (d < 0.0f) {	// scroll down
			if (Input.GetKey (KeyCode.R)) {
				decRadius = true;
				incRadius = false;
			} 
		} else {
			incRadius = false;
			decRadius = false;
		}
	}
}

