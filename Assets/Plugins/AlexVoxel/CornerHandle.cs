using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using DataObjects;

namespace AssemblyCSharpfirstpass
{
	public class CornerHandle : MonoBehaviour
	{
		public WorldPos pos;
		public float offx;
		public float offy;
		public float offz;
		public Block prime;

		public bool isActive = true;
		public bool isSelected = false;
		public enum handledirection { NW_T, SW_T, SE_T, NE_T, NW_B, SW_B, SE_B, NE_B };
		public handledirection corner = 0;
		public Vector3 holdout = new Vector3 (0, 0, 0);

		public World world;

		void Start ()
		{
			pos.x = 0;
			pos.y = 0;
			pos.z = 0;
			offx = 0.5f;
			offy = 0.5f;
			offz = 0.5f;
			corner = handledirection.NW_T;

			GameObject tempgo = GameObject.FindWithTag ("world");
			world = tempgo.GetComponent("World") as World;
		}
		void Update ()
		{
			updateHandle ();
		}
		public void assignVoxel (World thisworld, WorldPos position, int number)
		{
			prime = thisworld.GetBlock(position.x,position.y,position.z);

			corner = (handledirection) number;

			pos.x = position.x;
			pos.y = position.y;
			pos.z = position.z;
			offx = prime.offx;
			offy = prime.offy;
			offz = prime.offz;
			transform.position = Camera.main.WorldToScreenPoint(new Vector3 (pos.x+offx, pos.y+offy, pos.z+offz));

			setupHandle (number);
		}

		public void setupHandle( int thishandle) 
		{
			string ourtext;
			Vector3 vector = new Vector3 (1, 0, 0);
			Button button = gameObject.GetComponentInChildren<Button> ();
			ColorBlock tempcolor = button.colors;
			Color mix = new Color(0.0F, 0.0F, 0.0F, 0.4F);//

			if (thishandle < 1) {	
				ourtext = "NW";	
				vector = Quaternion.Euler(0, 0, 90+23) * vector;
				tempcolor.normalColor = new Color(0.0F, 0.0F, 0.0F, 0.4F);//Color.black;
			} else if (thishandle < 2) {
				ourtext = "NE";
				vector = Quaternion.Euler(0, 0, 90-23) * vector;
				tempcolor.normalColor = new Color(0.0F, 0.0F, 1.0F, 0.4F);//Color.blue;
			} else if (thishandle < 3) {	
				ourtext = "SE";	
				vector = Quaternion.Euler(0, 0, 23) * vector;
				tempcolor.normalColor = new Color(0.0F, 1.0F, 1.0F, 0.4F);//Color.cyan;
			} else if (thishandle < 4) {	
				ourtext = "SW";
				vector = Quaternion.Euler(0, 0, 180-23) * vector;
				tempcolor.normalColor = new Color(0.0F, 1.0F, 0.0F, 0.4F);//Color.green;
			} else if (thishandle < 5) {	
				ourtext = "SW";
				vector = Quaternion.Euler(0, 0, 180+23) * vector;
				tempcolor.normalColor = new Color(1.0F, 1.0F, 0.0F, 0.4F);//Color.yellow;
			} else if (thishandle < 6) {		
				ourtext = "SE";
				vector = Quaternion.Euler(0, 0, 360-23) * vector;
				tempcolor.normalColor = new Color(1.0F, 1.0F, 1.0F, 0.4F);//Color.white;
			} else if (thishandle < 7) {		
				ourtext = "NE";
				vector = Quaternion.Euler(0, 0, 270+23) * vector;
				tempcolor.normalColor = new Color(1.0F, 0.0F, 1.0F, 0.4F);//Color.magenta;
			} else if (thishandle < 8) {		
				ourtext = "NW";
				vector = Quaternion.Euler(0, 0, 270-23) * vector;
				tempcolor.normalColor = new Color(1.0F, 0.0F, 0.0F, 0.4F);//Color.red;
			} else
				ourtext = "fail!";


			gameObject.GetComponentInChildren<Text>().text = ourtext;
			button.colors = tempcolor;
			holdout.Set (vector.x, vector.y, vector.z);

		}

		public void updateVoxel ()
		{

		}

		public void updateHandle ()
		{
			transform.position = Camera.main.WorldToScreenPoint(new Vector3 (pos.x+offx, pos.y+offy, pos.z+offz) + holdout);

		}
	}
}

