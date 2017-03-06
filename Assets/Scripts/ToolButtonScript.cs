using UnityEngine;
using System.Collections;

public class ToolButtonScript : MonoBehaviour {

	public enum Mode { voxel, plane, articles };
	public Mode ToolMode = Mode.voxel;

	public Vector3 snap = new Vector3 (0.0f, 0.0f, 0.0f);

	public enum Shape {	sphere, cube, voxel	};
	public Shape ToolShape = Shape.cube;

	public float ToolSize = 0.8f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}




}
