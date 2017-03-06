using UnityEngine;
using System.Collections;
using DataObjects;

public class PlayerInterfaceController : MonoBehaviour
{



	public WorldPos positionhit;
	public WorldPos positionclick;
	public Block blockhit;
	public Block blockset;
	public float depth;
	public int toolmode;
	public int brushMaterial;
	public float brushRadius;
	public int brushType;

	public World scrWorld;


//	public GameObject obCubeMarker;
//	public GameObject obMarker;
	Transform trCamera;

	GameObject obPlayer;
	PlayerInventory inventory;

	GameObject obCanvas;
	GameObject obUI;
	Sprite spriteTool;

	public SelectedCube scrMarker;
	public Vector3 targetPosition;
	public Sprite spritePlace;
	public Sprite spriteMine;
	public Sprite spriteEdit;

	//related to Beam effect
	GameObject EffectPrefab;
	GameObject OurBeam;
	Effect beamScript;
	bool beamActive = false;

	//related to selecting an area
	public SelectedArea AreaMarker;
	public GameObject AreaObject;
	public GameObject thisArea;
	bool makingArea = false;
	public MouseHandler mouseController;
	public BrushButtonScript Brush;
	public EditingToolbar Toolbar;

	// Use this for initialization
	void Start ()
	{
		mouseController = GameObject.FindWithTag ("mc").GetComponent<MouseHandler> ();

		obPlayer = GameObject.FindWithTag ("Player");
		if(obPlayer) inventory = obPlayer.GetComponent<PlayerInventory> ();

//		obMarker = GameObject.Instantiate(obCubeMarker,transform.position,Quaternion.identity) as GameObject;
//		scrMarker = obMarker.GetComponent<SelectedCube> ();

		trCamera = Camera.main.transform;
		obUI = GameObject.FindWithTag ("UI");
		Toolbar = obUI.GetComponent<EditingToolbar> () as EditingToolbar;
		Brush = Toolbar.CurrentBrush.GetComponent<BrushButtonScript> () as BrushButtonScript;

		scrWorld = GameObject.FindWithTag ("world").GetComponent<World> ();
		thisArea = GameObject.Instantiate(AreaObject, Vector3.zero, Quaternion.identity) as GameObject;
		AreaMarker = thisArea.GetComponent<SelectedArea> ();
		makingArea = false;

		toolmode = 1;
		brushRadius = 3.8f;


	}
	
	// Update is called once per frame
	void Update ()
	{
		Ray ray = new Ray();
		float rayLength;
		RaycastHit hit = new RaycastHit();

		if(mouseController.incRadius) brushRadius += 0.2f;
		else if(mouseController.decRadius) brushRadius -= 0.2f;
		if(brushRadius < 0.8f) brushRadius = 0.8f;

		if(Input.GetKeyDown(KeyCode.Q)) {
			toolmode += 1;
			if(toolmode>2) toolmode = 0;
			if (toolmode == 0) {
			}


		}

		//roving target
		targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,depth-(Camera.main.transform.position.z)));
		ray.origin = Camera.main.transform.position;
		rayLength = 100;
		ray.direction = targetPosition - ray.origin;
		if(Physics.Raycast(ray, out hit)) {
			if(hit.collider != null) {
				targetPosition = hit.point;
				positionhit = Terra.GetBlockPos(hit, true);
				blockhit = scrWorld.GetBlock(positionhit.x,positionhit.y,positionhit.z);
				Vector3 blockoffset = blockhit.getoffset();
				Vector3 hitoffset = new Vector3 (hit.point.x - positionhit.x, hit.point.y - positionhit.y, hit.point.z - positionhit.z);
				positionhit = Terra.GetAdjustedPos(positionhit, hitoffset, blockoffset);

				blockhit = scrWorld.GetBlock(positionhit.x,positionhit.y,positionhit.z);
				if (blockhit.GetMaterial() != 0) {
//					blockhit.material = 30;
//					//blockhit.setoffset (new Vector3 (0.5f, 0.5f, 0.5f));
//					blockhit.changed = true;
//					Terra.SetBlock (scrWorld.GetChunk (positionhit.x, positionhit.y, positionhit.z), positionhit, blockhit);
				}
			}
		}
	



		if(Input.GetMouseButtonDown(0)) {
			if(toolmode == 0) { //edit mode  lets use a mouse to screen ray to find the block
				//create a target where we are pointing
				targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,depth-(Camera.main.transform.position.z)));
				ray.origin = Camera.main.transform.position;
				rayLength = Vector3.Distance(targetPosition, ray.origin);
				ray.direction = targetPosition - ray.origin;

				if(rayLength<50) {
					if(Physics.Raycast(ray, out hit)) {
						if(hit.collider != null) {
							targetPosition = hit.point;
						}
						else return;
					}
					else return;
				}	

//				if(!makingArea) { 
//					makingArea = true;
//					AreaMarker.setStartCorner(targetPosition);
//					AreaMarker.update = true;
//				}
//				else { 
//					AreaMarker.setEndCorner(targetPosition);
//					AreaMarker.update = true;
//				}

//				obMarker.transform.position = targetPosition;
				return;
			}

			else { //if not mode 0
				if(Input.GetKey(KeyCode.LeftShift)) {
					depth = 2;
				}
				else depth = 1;

				if(toolmode == 1) {
					if (hit.collider != null) {
						brushMaterial = 0;
						Terra.applySphere (scrWorld, targetPosition, brushRadius, brushMaterial);
						//Debug.Log ("do?");
					}
				}
				else if(toolmode == 2) { //we did not hit anything? PLACE
					if(blockhit.GetMaterial() == 0) {
						brushMaterial = 4;
						Terra.applySphere (scrWorld, targetPosition, brushRadius, brushMaterial);
//						InventoryItem item = new InventoryItem();//inventory.containerScript.pullCurrent(1);
//						item.setInvItem(3, 1);
//						if(item.id > 0) { //not an air block Yay!
//							blockset = new Block ();
//							blockset.material = item.id;
//							blockset.changed = true;
//							blockset.setoffset (new Vector3 (0.5f, 0.5f, 0.5f));
//							blockset.setvariant (positionhit.x,positionhit.y,positionhit.z);
//							Terra.SetCube(scrWorld.GetChunk(positionhit.x,positionhit.y,positionhit.z), positionhit);
//							Terra.SetBlock(scrWorld.GetChunk(positionhit.x,positionhit.y,positionhit.z), positionhit, blockset);
						//}
					}
				}
			}
		}





	}
}

