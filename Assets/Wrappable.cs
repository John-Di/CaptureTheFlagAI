using UnityEngine;
using System.Collections;

public class Wrappable : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerExit(Collider coll)
	{
		Vector3 pos = coll.gameObject.transform.position;
		Vector3 col = collider.bounds.max;

		if(pos.x < -(col.x - 8))
		{
			coll.gameObject.transform.position = new Vector3((col.x - 8), pos.y, pos.z);
		}
		else if(pos.x > (col.x - 8))
		{
			coll.gameObject.transform.position = new Vector3(-(col.x - 8), pos.y, pos.z);
		}
		
		if(pos.z < -(col.z - 8))
		{
			coll.gameObject.transform.position = new Vector3(pos.x, pos.y, (col.z - 8));
		}
		else if(pos.z > (col.z - 8))
		{
			coll.gameObject.transform.position = new Vector3(pos.x, pos.y, -(col.z - 8));
		}

		pos = coll.gameObject.transform.position;
	}
}
