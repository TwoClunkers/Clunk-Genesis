using UnityEngine;
using System.Collections;
using System;

[Serializable]
public struct WorldPos
{
    public int x, y, z;

    public WorldPos(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

	public WorldPos(Vector3 position)
	{
		this.x = Mathf.FloorToInt(position.x);
		this.y = Mathf.FloorToInt(position.y);
		this.z = Mathf.FloorToInt(position.z);
	}

	public WorldPos(float xf, float yf, float zf)
	{
		this.x = Mathf.FloorToInt(xf);
		this.y = Mathf.FloorToInt(yf);
		this.z = Mathf.FloorToInt(zf);
	}

	public override int GetHashCode()
	{
		unchecked
		{
			int hash = 47;
			hash = hash * 227 + x.GetHashCode ();
			hash = hash * 227 + y.GetHashCode ();
			hash = hash * 227 + z.GetHashCode ();
			return hash;
		}
	}

    public override bool Equals(object obj)
    {
		if (GetHashCode () == obj.GetHashCode ())
			return true;
		return false;
    }
}