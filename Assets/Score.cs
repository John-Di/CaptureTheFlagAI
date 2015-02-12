using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour 
{
	public TeamController blue, red;
	public Text blueScore, RedScore; 
	public GameObject playAgain, winner;
	public int blueCount, redCount;
	
	bool win = false;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!win)
		{
			blueScore.text = (blue.score).ToString();
			RedScore.text = (red.score).ToString();
			
			blueCount = 0; redCount = 0;
			
			foreach(GameObject ally in blue.allies)
			{
				if(ally.GetComponent<Freezable>().isFrozen)
				{
					blueCount++;
				}
			}
			
			foreach(GameObject ally in red.allies)
			{
				if(ally.GetComponent<Freezable>().isFrozen)
				{
					redCount++;
				}
			}

			if(blueCount == blue.allies.Count || redCount == red.allies.Count)
			{
				if(blueCount == blue.allies.Count && redCount == red.allies.Count)
				{
					winner.GetComponent<Text>().text = "Draw";
					winner.GetComponent<Text>().color = new Color(1f, 0.5f, 0f);				
				}
				else if(blueCount == blue.allies.Count)
				{
					winner.GetComponent<Text>().text = "Red Team Wins";
					winner.GetComponent<Text>().color = new Color(1f, 0f, 0f);
					
				}
				else if(redCount == red.allies.Count)
				{				
					winner.GetComponent<Text>().text = "Blue Team Wins";
					winner.GetComponent<Text>().color = new Color(0f, 0f, 1f);
				}
				
				winner.SetActive(true);
				playAgain.SetActive(true);
				win = true;
			}

		}
		
	}

	public void PlayAgain()
	{
		Application.LoadLevel("Capture the Flag");

	}
}
