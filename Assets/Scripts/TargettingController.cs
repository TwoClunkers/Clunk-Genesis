using UnityEngine;
using System.Collections;

public class TargettingController : MonoBehaviour
{
	public Transform Origin;
	public float scanRange;
	public float attackRange;
	public float rangeToTarget;
	public Vector3 targetPoint;
	public Transform TargetLock;

	float processCooldown = 1;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

