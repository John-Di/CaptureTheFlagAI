using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public sealed class KinematicAgent : AIAgent
{	
	void Start()
	{
		wayPoint = transform.position;
	}

    void FixedUpdate()
    {
		if(!gameObject.GetComponent<Freezable>().isFrozen)
		{
			halt = false;

			UpdateVelocities(Time.deltaTime);

			UpdateRotation(Time.deltaTime);

			if(Vector3.Angle (Velocity, transform.forward) < 30f)
			{
				UpdatePosition(Time.deltaTime);
			}
		}
		else
		{
			halt = true;
		}
    }

	private void UpdateVelocities(float deltaTime)
	{		
		if(behaviour == 0)
		{
			Wander();
		}
		else if(behaviour == 1)
		{
			Seek();
		}
		else if(behaviour == 2)
		{
			Arrive();
		}
		else if(behaviour == 3)
		{
			Flee();
		}		
		else if(behaviour == 4)
		{
			Pursue();
		}
		
		if(!halt)
		{
			Velocity *= deltaTime;
		}
		else
		{
			Velocity = Vector3.zero;
		}
		if(!halt)
		{
			Velocity *= deltaTime;
		}
		else
		{
			Velocity = Vector3.zero;
		}
		
		Velocity = Velocity.normalized * MaxVelocity;
		Velocity = new Vector3(Velocity.x, transform.forward.y, Velocity.z);
		
		Debug.DrawLine(transform.position + new Vector3(0,1,0), transform.position + Velocity +  new Vector3(0,1,0), Color.cyan);
		Debug.DrawLine(transform.position + Velocity + new Vector3(0,1,0), (target != null) ? target.transform.position : wayPoint + new Vector3(0,1,0), targetColor);
	}

    private void UpdatePosition(float deltaTime)
    {
		if(target == null || (target.transform.position - transform.position).magnitude >= 0.5f)
		{
			transform.position += Velocity * deltaTime;
		}
		else
		{
			transform.position = target.transform.position;
		}
    }

    private void UpdateRotation(float deltaTime)
    {		
		float step = Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, Velocity, step, 0.0F);
		transform.rotation = Quaternion.LookRotation(newDir);
    }

	public override void UpdateBehaviour(GameObject target, int b)
	{
		behaviour = b;
		this.target = target;
	}

	public void Seek()
	{		
		targetColor = Color.cyan;

		Vector3 directionVec = target.transform.position - transform.position;
		Vector3 velocity = MaxVelocity * directionVec.normalized;
		
		Velocity = velocity;
	}

	public void Flee()
	{	
		targetColor = Color.blue;

		Vector3 directionVec = transform.position - target.transform.position;
		Vector3 velocity = MaxVelocity * directionVec.normalized;
		
		Velocity = velocity;
	}

	public void Pursue()
	{	
		targetColor = Color.red;

		float timeTillTarget = (target.transform.position - transform.position).magnitude / MaxVelocity;
		Vector3 directionVec = (target.transform.position + target.GetComponent<KinematicAgent>().Velocity * timeTillTarget) - transform.position;
		Vector3 velocity = MaxVelocity * directionVec.normalized;
		
		Velocity = velocity;
	}

	public void Arrive()
	{
		targetColor = Color.magenta;

		Vector3 directionVec = target.transform.position - transform.position;
		Vector3 velocity = MaxVelocity * directionVec.normalized;
		
		Velocity = (directionVec.magnitude <= slowRadius) ? -velocity : velocity;

		halt = ((target.transform.position - transform.position).magnitude <= arriveRadius);
	}

	public void Wander()
	{			
		targetColor = Color.green;

		Vector3 wanderCircle = transform.forward * wanderDistance + transform.position,
		wanderTarget = UnityEngine.Random.insideUnitCircle.normalized * wanderRadius;				
		wanderTarget = new Vector3(wanderTarget.x, transform.position.y, wanderTarget.y);
		
		if((wayPoint - transform.position).magnitude <= updateRange || timeStuck <= 0.0)
		{
			wayPoint = wanderCircle + wanderTarget;
			wayPoint.y = 0.0f;
			timeStuck = 3.0f;
		}

		timeStuck -= Time.deltaTime;
		
		Vector3 directionVec = wayPoint - transform.position;
		Vector3 velocity = MaxVelocity * directionVec.normalized;
		
		Velocity = velocity;
	}
}
