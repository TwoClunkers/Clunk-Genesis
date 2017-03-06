using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DataObjects;

public class BrushButtonScript : MonoBehaviour {

	//Mode designation in lower left
	public Mode brushMode = Mode.voxel;
	public Text modeText;

	//Snap is written in upper left
	public bool snapActive = false;
	public Vector3 snapPosition = new Vector3 (0.0f, 0.0f, 0.0f);
	public Text xSnapText;
	public Text ySnapText;
	public Text zSnapText;

	//Shape is an image in center bottom right
	public Shape brushShape = Shape.cube;
	public Image shapeImage;
	public Sprite sphereSprite;
	public Sprite cubeSprite;
	public Sprite voxelSprite;

	//Size or Scale is bottom right
	public float brushSize = 0.8f;
	public Text sizeText;

	//Application is written in top right
	public App brushApp = App.additive;
	public Text appText;

	//Material or Object image in center top left
	public int brushMaterial = 0;
	public Image materialImage;
	public Texture2D materialTex;
	public Sprite materialSprite;

	//this is used to notify us to rewrite all our stuff
	public bool changed = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (changed) {
			UpdateMode ();
			UpdateSnap ();
			UpdateShape ();
			UpdateSize ();
			UpdateApplication ();
			UpdateMaterial ();

			changed = false;
		}


	}

	public void UpdateMode () { //update the mode textbox
		if (brushMode == Mode.plane) {
			modeText.text = "P";
		} else if (brushMode == Mode.articles) {
			modeText.text = "A";
		} else {
			brushMode = Mode.voxel;
			modeText.text = "V";
		}
	}

	public void UpdateSnap () {

		snapPosition = Vector3.Min(Vector3.Max (snapPosition, Vector3.zero), new Vector3(10.0f, 10.0f, 10.0f));

		if (!snapActive) {
			xSnapText.text = "";
			ySnapText.text = "";
			zSnapText.text = "";
		} else {
			xSnapText.text = "x " + snapPosition.x.ToString ();
			ySnapText.text = "y " + snapPosition.y.ToString ();
			zSnapText.text = "z " + snapPosition.z.ToString ();
		}
	}

	public void UpdateShape () {
		if (brushShape == Shape.cube) {
			shapeImage.sprite = cubeSprite;
		} else if (brushShape == Shape.sphere) {
			shapeImage.sprite = sphereSprite;
		} else { //reset to voxel if we fail our above options
			brushShape = Shape.voxel; 
			shapeImage.sprite = voxelSprite;
		}
	}

	public void UpdateSize () {
		brushSize = Mathf.Clamp(brushSize, 0.02f, 50.0f);
		sizeText.text = brushSize.ToString ();
	}

	public void UpdateApplication () {
		if (brushApp == App.additive) {
			appText.text = "+";
		} else if (brushApp == App.subtractive) {
			appText.text = "-";
		} else {
			brushApp = App.blend;
			appText.text = "Bl";
		}
	}

	public void UpdateMaterial () {
		//this may be a little more tricky as we need to get the right pic from many different materials...

		//this calculation is mostly only going to work for the voxel materials
		brushMaterial = Mathf.Clamp(brushMaterial, 0, 63);
		int xindex = brushMaterial/8;
		int yindex = brushMaterial - (xindex * 8);

		float width = (materialTex.width / 8);
		float height = (materialTex.height / 8);
		float xpos = width * xindex;
		float ypos = height * yindex;


		materialImage.sprite =  Sprite.Create(materialTex, new Rect(xpos,ypos,width,height), new Vector2(0.5f,0.5f), 100.0f);
	}

	public void SetMode(Mode newMode) {
		brushMode = newMode;
		changed = true;
	}

	public void SetSnapPosition(Vector3 newSnap) {
		snapPosition = newSnap;
		changed = true;
	}

	public void SetSnapActive(bool newState) {
		snapActive = newState;
		changed = true;
	}

	public void SetShape(Shape newShape) {
		brushShape = newShape;
		changed = true;
	}
		
	public void SetSize(float newSize) {
		brushSize = newSize;
		changed = true;
	}

	public void SetApp(App newApp) {
		brushApp = newApp;
		changed = true;
	}

	public void SetMaterial(int newMaterial) { //this will need to change to allow us non-voxel materials
		brushMaterial = newMaterial;
		changed = true;
	}
		
}
