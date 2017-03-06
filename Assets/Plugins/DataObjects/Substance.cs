using System;
using System.Collections;
using UnityEngine;

namespace DataObjects
{
	[Serializable]
	public class Substance
	{
		//public enum Direction { };
		public Texture2D mainTexture;
		//public Texture2D blendTexture;
		public Texture2D normTexture;
		public Rect mainRect; //set once atlas has been made
		public Rect normRect;
		public int tilesWide;
		public int tilesHigh;

		public string name;
		public int id;
		public int hardness;

		public Substance (int newId) 
		{
			id = newId;
			hardness = 50;
			name = id.ToString ();
		}

		public Texture2D getMainTexture()
		{
			return mainTexture;
		}

		public Texture2D getNormTexture()
		{
			return normTexture;
		}

		public void setMainRect(Rect newRect)
		{
			mainRect = newRect;
		}

		public void setNormRect(Rect newRect)
		{
			normRect = newRect;
		}

		public Block SetAsBlockMaterial(Block block)
		{
			block.SetMaterial (id, mainRect, tilesWide, tilesHigh);
			return block;
		}

	}
}
