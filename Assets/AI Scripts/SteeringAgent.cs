using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public sealed class SteeringAgent : AIAgent
{
	public Vector3 Acceleration { get; private set; }
	float MaxAcceleration = 5f;
    
    void Start()
    {

    }

	void FixedUpdate()
	{
		if(!gameObject.GetComponent<Freezable>().isFrozen)
		{
			halt = false;
			
			UpdateVelocities(Time.deltaTime);
			
			UpdateRotation(Time.deltaTime);
			
			UpdatePosition(Time.deltaTime);

		}
		else
		{
			halt = true;
		}
	}

    public void ResetVelocities()
    {
        Velocity = Vector3.zero;
        AngularVelocity = 0f;
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
			Evade();
		}		
		else if(behaviour == 4)
		{
			Pursue();
		}

		if(!halt)
		{
			Velocity += Acceleration * deltaTime;
		}
		else
		{
			Velocity = Vector3.zero;
		}
		
		Velocity = Velocity.normalized * MaxVelocity;
		Velocity = new Vector3(Velocity.x, transform.forward.y, Velocity.z);
		
		Debug.DrawLine(transform.position + new Vector3(0,1,0), transform.position + Velocity +  new Vector3(0,1,0), Color.cyan);
		Debug.DrawLine(transform.position + Velocity + new Vector3(0,1,0), (target != null) ? target.transform.position : wayPoint + new Vector3(0,1,0), targetColor);

		if(halt)
		{
			Velocity = Vector3.zero;
		}
		else
		{			
			Velocity = (Velocity.magnitude < MaxVelocity) ? Velocity : Velocity.normalized * MaxVelocity;
		}
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
		float step = Time.deltaTime * 5f;
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
		Vector3 acceleration = MaxAcceleration * directionVec.normalized;
		
		Acceleration = acceleration;
	}
	
	public void Flee()
	{				
		targetColor = Color.blue;

		Vector3 directionVec = transform.position - target.transform.position;
		Vector3 acceleration = MaxAcceleration * directionVec.normalized;
		
		Acceleration = acceleration;
	}
	
	public void Pursue()
	{						
		targetColor = Color.red;

		float timeTillTarget = (target.transform.position - transform.position).magnitude / MaxAcceleration;
		Vector3 directionVec = (target.transform.position + target.GetComponent<SteeringAgent>().Velocity * timeTillTarget) - transform.position;
		Vector3 acceleration = MaxAcceleration * directionVec.normalized;
		
		Acceleration = acceleration;
	}

	public void Evade()
	{					
		targetColor = Color.blue;

		float timeTillTarget = (transform.position - target.transform.position).magnitude / MaxAcceleration;
		Vector3 directionVec = transform.position - (target.transform.position + target.GetComponent<SteeringAgent>().Velocity * timeTillTarget);
		Vector3 acceleration = MaxAcceleration * directionVec.normalized;
		
		Acceleration = acceleration;
	}
	
	public void Arrive()
	{			
		targetColor = Color.magenta;

		Vector3 directionVec = target.transform.position - transform.position;
		Vector3 acceleration = MaxAcceleration * directionVec.normalized;
		
		Acceleration = (directionVec.magnitude <= slowRadius) ? -acceleration : acceleration;
		
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
		Vector3 acceleration = MaxAcceleration * directionVec.normalized;
		
		Acceleration = acceleration;
	}
}
