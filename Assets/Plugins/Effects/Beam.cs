using System;
using UnityEngine;

namespace AssemblyCSharpfirstpass
{
	public class Beam
	{

		public Beam ()
		{

		}

		public virtual MeshData getBeamMesh (Vector3 start, Vector3 end, float width, MeshData meshData)
		{
			Vector3[] points = new Vector3[3]; 
			Vector2[] UVs = new Vector2[3];

			Vector3 tip = end - start;
			float mag = tip.magnitude;
			Vector3 perp = new Vector3 (-tip.y, tip.x, tip.z) / mag;
			Vector3 base1 = (perp * width/4) + tip/mag;
			Vector3 base2 = (perp * -width/4) + tip/mag;
			//Vector3 base3 = Quaternion.Euler(300, 0, 0) * perp;


			UVs [2] = new Vector2 (0.0f, Math.Min (0.4f+width,1.0f));
			UVs [1] = new Vector2 (0.0f, Math.Max (0.6f-width,0.0f));
			UVs [0] = new Vector2 (0.1f, 0.5f);

			meshData.AddVertex(Vector3.zero);
			meshData.AddVertex(base1);
			meshData.AddVertex(base2);
			meshData.AddTriangle();
			meshData.uv.AddRange(UVs);

			UVs [1] = new Vector2 (0.0f, Math.Min (0.4f+width,1.0f));
			UVs [2] = new Vector2 (0.0f, Math.Max (0.6f-width,0.0f));
			UVs [0] = new Vector2 (0.1f, 0.5f);
			
			meshData.AddVertex(tip);
			meshData.AddVertex(base2);
			meshData.AddVertex(base1);
			meshData.AddTriangle();
			meshData.uv.AddRange(UVs);

			return meshData;
		}

	}
}

