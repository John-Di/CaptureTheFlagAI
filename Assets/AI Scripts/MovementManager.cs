using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MovementManager : MonoBehaviour 
{
	public List<GameObject> npcs;
	public string agent = "Kinematic";

	void Awake()
	{
		foreach(GameObject npc in npcs)
		{
			npc.GetComponent<Animation>().Play ("Walk");
		}
	}

	public void ToggleAIAgent()
	{
		foreach(GameObject npc in npcs)
		{
			AIAgent prev;
			AIAgent next;

			if(agent == "Kinematic")
			{
				prev = npc.GetComponent<KinematicAgent>();
				npc.AddComponent<SteeringAgent>();
				next = npc.GetComponent<SteeringAgent>();
			}
			else
			{
				prev = npc.GetComponent<SteeringAgent>();
				npc.AddComponent<KinematicAgent>();
				next = npc.GetComponent<KinematicAgent>();
			}

			next.UpdateBehaviour(prev.target, prev.behaviour);
			next.Velocity = prev.Velocity;

			Destroy(prev);

		}
		
		if(agent == "Kinematic")
		{
			agent = "Steering";
		}
		else if(agent == "Steering")
		{			
			agent = "Kinematic";
		}
		
		gameObject.GetComponent<Text>().text = agent + " on";
	}


}
