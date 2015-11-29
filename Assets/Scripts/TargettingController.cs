using UnityEngine;
using System.Collections;

public class TargettingController : MonoBehaviour
{
	public Transform Origin;
	public float scanRange;
	public float attackRange;
	public float rangeToTarget;
	public Vector3 angleToTarget = new Vector3 (0, 0, 0);
	public Vector3 targetPoint = new Vector3 (0, 0, 0);
	public Transform TargetLock = null;
	public bool ready = false;
	public bool target = false;

	float processCooldown = 1.0f;
	float activateTime = 0;

	// Use this for initialization
	void Start ()
	{
		activateTime = Time.time + processCooldown;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (activateTime < Time.time) {
			updateTargeting();
			activateTime = Time.time + processCooldown;
		}
	}

	void updateTargeting() 
	{
		if (!target) { //no active target given from scanner
			ready = false;
			TargetLock = null;
			targetPoint = Vector3.zero;
			return; //no action as we have no target
		}

		if (TargetLock) { //we know target position exactly
			targetPoint = TargetLock.position;
		}

		targetPoint.Set (Input.mousePosition.x, Input.mousePosition.y, transform.position.z);

		rangeToTarget = Vector3.Distance(this.transform.position,targetPoint);

		Vector3 targetDir = targetPoint - transform.position;
		targetDir.y = 0;
		Vector3 forward = transform.forward;
		forward.y = 0;
		angleToTarget.y = Vector3.Angle(targetDir, forward);

		float angle = Mathf.LerpAngle(transform.eulerAngles.x, angleToTarget.x, Time.deltaTime/500);
		transform.eulerAngles = new Vector3(angle, 0, 0);

		if (rangeToTarget < attackRange) {
			ready = true;
		} else { //not in range
			ready = false;
		}
	}
}

