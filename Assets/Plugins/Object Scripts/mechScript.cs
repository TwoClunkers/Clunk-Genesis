using UnityEngine;
using System.Collections;
using PathologicalGames;
using DataObjects;

public class mechScript : MonoBehaviour
{
	public Mech mechData;

	// Use this for initialization
	void Start ()
	{
	
	}

	void Update()
	{

	}
	
	void InitializeMech(int ID, int newQuant)
	{
		mechData.reset(ID, newQuant);
	}
	
	void destroyMe()
	{
		PoolManager.Pools["mechs"].Despawn(transform);
		//	try{ Network.RemoveRPCs(networkView.viewID); } catch(e){}
		//	try{ Network.Destroy(networkView.viewID); } catch(e){}
	}
}

