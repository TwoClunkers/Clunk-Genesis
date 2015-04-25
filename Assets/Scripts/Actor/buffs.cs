using UnityEngine;
using System.Collections;

enum buffTypes{
	additive, 
	multiply, 
	func
}

public class buffs : MonoBehaviour {
	buffTypes type;
	ArrayList stats;
	ArrayList amounts;
	float duration;
	
	void FixedUpdate () {
		switch(type){
		case buffTypes.additive:
			//TODO: add amount to specified stat(s)
			break;
		case buffTypes.multiply:
			//TODO: multiply specified stat(s) by amount
			break;
		case buffTypes.func:
			//TODO: call function
			break;
		}
	}

	// Use this for initialization
	void Start () {
	
	}

}