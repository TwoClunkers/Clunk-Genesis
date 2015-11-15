using UnityEngine;
using System.Collections;
using AssemblyCSharpfirstpass;


public class Effect : MonoBehaviour
{
	Beam mybeam;
	public float starttime;
	public float endtime;
	public float duration;
	float elapsed = 0.0f;

	public Transform origen;
	public Transform foci;
	public bool moving = false;
	public Vector3 targetPoint;
	public Vector3 startPoint;
	public Vector3 offset;

	MeshFilter filter;
	MeshCollider coll;
	Renderer rend;

	bool changed = false;
	bool z_clamp = false;
	float clamp;
	public float power;
	
	void Start ()
	{
		filter = gameObject.GetComponent<MeshFilter>();
		coll = gameObject.GetComponent<MeshCollider>();
		rend = GetComponent<Renderer>();
		starttime = Time.time;
		duration = 1.0f;
		endtime = starttime + duration;
		mybeam = new Beam();
		clamp = 0.0f;
		power = 1.0f;
		offset = new Vector3 (0.0f, 0.5f, 0.0f);

	}

	void Update ()
	{
		if(foci) targetPoint = foci.position;
		if (!origen)
			destroyEffect ();

		startPoint = origen.position;
		gameObject.transform.position = origen.position;

		if (z_clamp) {
			targetPoint.z = 1.5f;
			startPoint.z = 1.5f;
		}
		if (power < 0)
			destroyEffect ();

		updateEffect(); 

		if (Time.time > endtime)
			destroyEffect ();
		else
			power -= Time.deltaTime*3;
	}
	public void setEffect(Transform newOrigen, Vector3 endPoint, bool restart, bool setclamp) {
		origen = newOrigen;
		targetPoint = endPoint;
		startPoint = origen.position;
		changed = true;
		if (setclamp) {
			z_clamp = true;
			clamp = startPoint.z;
		}
		if (restart) {
			starttime = Time.time;
			endtime = starttime + duration;
		}

	}

	Vector3 spin_x(Vector3 direction, float amount) 
	{
		amount = Random.value * 360;
		//direction = Quaternion.Euler(0, 0, 90) * direction;
		direction = Quaternion.Euler(Time.deltaTime*amount, 0, 90) * direction;

		return direction;
	}
	public void addPower(float powerIn) {
		power += powerIn;
		duration += powerIn;
		endtime += powerIn;
	}
	void updateEffect() {
		elapsed += Time.deltaTime;
		MeshData meshData = new MeshData ();

		float beamlength = Vector3.Distance (startPoint, targetPoint);

		//meshData = mybeam.getBeamMesh (startPoint, targetPoint, Random.value/2+1.0f-(elapsed/duration), meshData);
		float i = beamlength/8;
		for (; i < Mathf.Min(10,power); i+=(beamlength/8)) {
			meshData = mybeam.getBeamMesh (startPoint, targetPoint, 1 + (2*Random.value/(beamlength)), meshData);
		}
		meshData = mybeam.getBeamMesh (startPoint, targetPoint, 1 + ((2*(power-i)*Random.value)/(beamlength)), meshData);


		RenderMesh(meshData);
	
	}
	public void destroyEffect() {
		Destroy (gameObject);
	}
	void RenderMesh(MeshData meshData)
	{
		filter.mesh.Clear();
		filter.mesh.vertices = meshData.vertices.ToArray();
		filter.mesh.triangles = meshData.triangles.ToArray();
		
		filter.mesh.uv = meshData.uv.ToArray();
		filter.mesh.RecalculateNormals();
		
		coll.sharedMesh = null;
		Mesh mesh = new Mesh();
		mesh.vertices = meshData.colVertices.ToArray();
		mesh.triangles = meshData.colTriangles.ToArray();
		mesh.RecalculateNormals();
		
		coll.sharedMesh = mesh;
	}

}

