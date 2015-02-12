using UnityEngine;
using System.Collections;

public class AIAgent : MonoBehaviour 
{
	public GameObject target;
	public Vector3 wayPoint;
	public int unstuckAttempts = 3;
	
	//KinematicBehavior behaviour;
	
	public Vector3 Velocity { get; set; }
	public float AngularVelocity { get; set; }
	protected float MaxVelocity = 2.0f;	
	protected float MaxAngularVelocity = 0.0f;
	
	protected float slowRadius = 1f;
	protected float arriveRadius = 0f;
	
	protected float timeStuck = 0.0f;
	protected float wanderDistance = 6f, wanderRadius = 3f, updateRange= 2f;
	
	public Color targetColor = Color.green;

	public int behaviour = 0;

	public bool halt = false;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public virtual void UpdateBehaviour(GameObject target, int b)
	{

	}
}
