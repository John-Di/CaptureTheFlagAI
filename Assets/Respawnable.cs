using UnityEngine;
using System.Collections;

public class Respawnable : MonoBehaviour 
{
	public Vector3 respawnPosition;
	public Vector3 respawnRotation;

	// Use this for initialization
	void Start () 
	{
		respawnPosition = transform.position;
		respawnRotation = transform.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Respawn()
	{
		transform.position = respawnPosition;
		transform.eulerAngles = respawnRotation;
	}
}
