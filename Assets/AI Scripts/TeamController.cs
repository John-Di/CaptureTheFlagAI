using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TeamController : MonoBehaviour 
{
	public List<GameObject> allies;
	public GameObject enemyFlag;

	public List<GameObject> defense, enemyOffense, beingRescued;
	public Dictionary<GameObject, float> teamGoals;

	bool enemySeekerAssigned = false;

	public int score = 0;

	void Start()
	{
		defense = new List<GameObject>();
		enemyOffense = new List<GameObject>();
		beingRescued = new List<GameObject>();
		teamGoals = new Dictionary<GameObject, float>();
	}

	GameObject Seeker
	{
		get
		{
			foreach(GameObject ally in allies)
			{
				GoalManager gm = ally.GetComponent<GoalManager>();

				if(gm.target != null && (gm.target == enemyFlag || gm.hasFlag))
				{
					return ally;
				}
			}

			return null;

		}
	}

	GameObject EnemySeeker
	{
		get
		{
			foreach(GameObject enemy in enemyOffense)
			{
				GoalManager gm = enemy.GetComponent<GoalManager>();
				
				if(gm.target != null && (gm.target.tag == "Flag" || gm.hasFlag))
				{
					return enemy;
				}
			}

			enemySeekerAssigned = false;
			return null;
			
		}
	}



	void Update()
	{
		UpdateSeekers();
		UpdateFrozen();
		UpdateEscaped();
		//UpdateFlee();

		AssignGoals();
	}

	void AssignGoals()
	{
		foreach(GameObject ally in allies)
		{
			GoalManager gm = ally.GetComponent<GoalManager>();

			if(teamGoals.Count > 0)
			{
				KeyValuePair<GameObject, float> first = teamGoals.OrderBy(key => key.Value).First();

				GameObject goal = first.Key;
				float priority = first.Value;

				if(ally != goal && priority < gm.priority && !ally.GetComponent<Freezable>().isFrozen)
				{
					if(gm.target != null)
					{
						if(teamGoals.ContainsKey(gm.target))
						{
							teamGoals[gm.target] = gm.priority;
						}
						else
						{
							teamGoals.Add(gm.target, gm.priority);
						}
					}

					gm.UpdateGoal(goal, priority);
					teamGoals.Remove(goal);

					if(goal.name == "NPC" && goal.tag == tag)
					{
						beingRescued.Add(goal);
					}

					if(goal == EnemySeeker)
					{
						enemySeekerAssigned = true;
					}
				}
			}
		}
	}

	void UpdateSeekers()
	{
		if(Seeker == null && !teamGoals.ContainsKey(enemyFlag))
		{
			teamGoals.Add(enemyFlag, 3f);
		}

		if(EnemySeeker != null && !teamGoals.ContainsKey(EnemySeeker) && !enemySeekerAssigned)
		{
			teamGoals.Add(EnemySeeker, 0f);
		}
	}

	void UpdateFrozen()
	{
		foreach(GameObject ally in allies)
		{
			if(ally.GetComponent<Freezable>().isFrozen && !teamGoals.ContainsKey(ally) && !beingRescued.Contains(ally))
			{
				teamGoals.Add(ally, 1f);

				GameObject target = ally.GetComponent<GoalManager>().target;
				float priority = ally.GetComponent<GoalManager>().priority;

				if(target != null && !teamGoals.ContainsKey(target) && priority >= 0)
				{
					teamGoals.Add (target, priority);

					if(target.name == "NPC" && target.tag != tag)
					{
						target.GetComponent<GoalManager>().UpdateGoal(null, 0f);
					}
				}

				ally.GetComponent<GoalManager>().UpdateGoal(null, 0f);

			}
			else if(!ally.GetComponent<Freezable>().isFrozen && beingRescued.Contains(ally))
			{
				ally.GetComponent<GoalManager>().UpdateGoal(null, Mathf.Infinity);
				beingRescued.Remove(ally);
			}
		}
	}

	void UpdateEscaped()
	{
		foreach(GameObject ally in allies)
		{
			GameObject target = ally.GetComponent<GoalManager>().target;
			float priority = ally.GetComponent<GoalManager>().priority;

			bool isEnemy = target != null && target.name == "NPC" && target.tag != tag;

			if(isEnemy && !enemyOffense.Contains(target) && !teamGoals.ContainsKey(target) && priority >= 0)
			{
				ally.GetComponent<GoalManager>().UpdateGoal(null, 0f);
			}
		}

	}

	void OnTriggerEnter(Collider coll)
	{
		GameObject obj = coll.gameObject;

		if(obj.name == "NPC")
		{
			if(obj.tag != tag)
			{
				if(!enemyOffense.Contains(obj))
				{
					enemyOffense.Add(obj);
				}

				if(!teamGoals.ContainsKey(obj))
				{
					float priority;
					GoalManager gm = obj.GetComponent<GoalManager>();
					
					if(gm.target != null && gm.target.tag == "Flag")
					{
						priority = 0f;
					}
					else
					{
						priority = 2f;
					}

					teamGoals.Add(obj, priority);
				}
			}
			else if(obj.tag == tag && !defense.Contains(obj))
			{
				defense.Add(obj);
				GoalManager gm = obj.GetComponent<GoalManager>();

				if(gm.target == null)
				{
					gm.UpdateGoal(null, 0f);
				}
			}
		}
	}

	void OnTriggerExit(Collider coll)
	{
		GameObject obj = coll.gameObject;
		
		if(obj.name == "NPC")
		{
			if(obj.tag != tag)
			{
				if(enemyOffense.Contains(obj))
				{
					enemyOffense.Remove(obj);
				}
				
				if(teamGoals.ContainsKey(obj))
				{
					teamGoals.Remove(obj);
				}

			}
			else if(obj.tag == tag && defense.Contains(obj))
			{
				defense.Remove(obj);

				GoalManager gm = obj.GetComponent<GoalManager>();

				if(gm.target != null && gm.target.tag != "Flag")
				{
					teamGoals.Remove(gm.target);
					gm.UpdateGoal(null, Mathf.Infinity);
				}
			}
		}
	}
}
