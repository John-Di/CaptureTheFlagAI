using UnityEngine;
using System.Collections;

public class GoalManager : MonoBehaviour 
{
	public GameObject target;
	public float priority = Mathf.Infinity;

	public bool hasFlag = false;
	GameObject chest;

	public float GoalDistance
	{
		get
		{
			float distance;

			if(target != null)
			{
				distance = (target.transform.position - transform.position).magnitude;
			}
			else
			{
				distance = Mathf.Infinity;
			}

			return distance;
		}
	}


	void OnTriggerEnter(Collider obj)
	{
		GameObject coll = obj.gameObject;
		string collTag = obj.gameObject.tag;

		if(coll == target)
		{
			if(collTag == "Flag")
			{
				chest = coll;
				hasFlag = true;
				coll.transform.parent = transform;
				coll.transform.position = transform.position + new Vector3(0,3.0f,0f);
				UpdateGoal(GameObject.Find(tag + " Podium"), -1f);
			}		
			else if(coll.name == tag + " Podium")
			{
				ReturnChest();
				GameObject.Find(gameObject.tag + " Team Controller").GetComponent<TeamController>().score++;
				UpdateGoal(null, Mathf.Infinity);
			}	
			else if(coll.name == "NPC")
			{	
				if(collTag != tag)
				{
					GameObject tagged = coll.gameObject;
					GoalManager tagGoal = tagged.GetComponent<GoalManager>();

					if(tagGoal.hasFlag)
					{
						tagGoal.ReturnChest();
					}
					
					StartCoroutine("Freeze", coll.gameObject);	
				}
				else
				{	
					StartCoroutine("UnFreeze", coll.gameObject);
				}

				UpdateGoal(null, Mathf.Infinity);
			}
		}
	}	
	
	public void UpdateGoal(GameObject target, float priority)
	{		
		this.target = target;

		AIAgent ai = gameObject.GetComponent<AIAgent>();

		if(target != null)
		{
			if(target.name == "NPC")
			{
				if(target.tag != tag)
				{
					ai.UpdateBehaviour(target, 4);

					if(target.GetComponent<GoalManager>().target == null)
					{
						target.GetComponent<AIAgent>().UpdateBehaviour(gameObject, 3);
						target.GetComponent<GoalManager>().priority = -1f;
					}
				}
				else
				{
					ai.UpdateBehaviour(target, 1);
				}
			}
			else
			{
				ai.UpdateBehaviour(target, 2);
			}

			this.priority = priority;
		}
		else
		{
			ai.UpdateBehaviour(null, 0);
			this.priority = Mathf.Infinity;
		}
	}

	IEnumerator Freeze(GameObject dead) 
	{
		gameObject.GetComponent<Animation>().Play ("Attack");
		dead.GetComponent<Animation>().Play ("Dead");	
		dead.GetComponent<Freezable>().Freeze();
		yield return new WaitForSeconds(gameObject.GetComponent<Animation>()["Attack"].length + dead.GetComponent<Animation>()["Dead"].length);	
		gameObject.GetComponent<Animation>().Play ("Walk");
		dead.GetComponent<Respawnable>().Respawn();
	}

	IEnumerator UnFreeze(GameObject frozen) 
	{
		gameObject.GetComponent<Animation>().Play ("Attack");
		frozen.GetComponent<Animation>().Play ("Walk");
		yield return new WaitForSeconds(gameObject.GetComponent<Animation>()["Attack"].length);
		frozen.GetComponent<Freezable>().UnFreeze();
		gameObject.GetComponent<Animation>().Play ("Walk");
	}
	
	public void ReturnChest()
	{
		chest.transform.parent = null;
		chest.GetComponent<Respawnable>().Respawn();
		chest = null;
		hasFlag = false;
	}
}
