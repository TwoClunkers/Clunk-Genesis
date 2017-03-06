using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataObjects;

public class SubstanceManager : MonoBehaviour
{
	public Substance[] substances;

	public Texture2D[] mainTexures;
	public Texture2D[] blendTexures;
	public Texture2D[] normTexures;
	public Rect[] rectsMain; //set when textures are packed
	public Rect[] rectsBlend; 
	public Rect[] rectsNorm; 
	public Texture2D mainAtlas;
	public Texture2D blendAtlas;
	public Texture2D normAtlas;
	public bool atlasReady = false;

	public SubstanceManager ()
	{
		substances = new Substance[10];
	}

	void Start() {
		
//		for (int count = 0; count < substances.Length; count += 1) {
//			substances [count] = new Substance (count);
//		}

		mainAtlas = new Texture2D(8192, 8192);
		blendAtlas = new Texture2D(8192, 8192);
		normAtlas = new Texture2D(8192, 8192);

		GetSubstanceTextures ();
		CreateAtlas ();

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void CreateAtlas()
	{
		rectsMain = mainAtlas.PackTextures(mainTexures, 0, 8192);
		rectsBlend = blendAtlas.PackTextures(blendTexures, 0, 8192);
		rectsNorm = normAtlas.PackTextures(normTexures, 0, 8192);
		for (int i = 0; i < rectsMain.Length; i++) {
			substances [i].mainRect = rectsMain [i];
			substances [i].normRect = rectsNorm [i];
		}
		atlasReady = true;
	}

	void GetSubstanceTextures()
	{
		int size = substances.Length;

		mainTexures = new Texture2D[size];
		blendTexures = new Texture2D[size];
		normTexures = new Texture2D[size];

		for (int index = 0; index < size; index +=1)
		{
			mainTexures [index] = substances[index].getMainTexture();
			blendTexures [index] = substances[index].getMainTexture();
			normTexures [index] = substances[index].getNormTexture();
		}
	}
}

