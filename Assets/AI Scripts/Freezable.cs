using UnityEngine;
using System.Collections;

public class Freezable : MonoBehaviour 
{
	public bool isFrozen = false;

	public void Freeze()
	{
		isFrozen = true;
		rigidbody.angularVelocity = Vector3.zero;
		rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | 
								RigidbodyConstraints.FreezeRotationY | 
								RigidbodyConstraints.FreezeRotationZ | 
								RigidbodyConstraints.FreezePositionX |
								RigidbodyConstraints.FreezePositionY | 
								RigidbodyConstraints.FreezePositionZ;GetComponent<Respawnable>().Respawn();
	}

	public void UnFreeze()
	{
		isFrozen = false;
		rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | 
								RigidbodyConstraints.FreezeRotationY | 
								RigidbodyConstraints.FreezeRotationZ | 
								RigidbodyConstraints.FreezePositionY;
	}
}
