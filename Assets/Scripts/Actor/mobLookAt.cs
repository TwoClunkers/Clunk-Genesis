using UnityEngine;
using System.Collections;

public class mobLookAt : MonoBehaviour {
	public float lookSpeed = 1.0f;
	GameObject target;
	Vector3 targetPosition;
	GameObject player;
	float viewRange = 110f;
	Transform headBone;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		headBone = transform.Find("Armature/pelvis/spine/chest/neck/head");
		if (headBone == null)
			Debug.Log ("head null");
	}

	void LateUpdate () {
		if (player == null) player = GameObject.FindGameObjectWithTag("Player");
		if (player == null) {
			return;
		}
		targetPosition = player.transform.position;

		float lookAngle = Vector3.Angle (targetPosition - headBone.position,headBone.TransformDirection(Vector3.left));
		Debug.DrawRay (headBone.position, headBone.TransformDirection (Vector3.left),Color.blue);
		if (lookAngle <= viewRange/2) {
			//TODO: smooth head rotation
			headBone.LookAt (targetPosition);
			headBone.Rotate (-90, 90, 0);
		}
	}

	void OnTriggerStay ()
	{

	}
}
