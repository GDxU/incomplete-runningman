using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
	
	public int timeRemaining;
	public Text countdown, score;
	private int score_now;

	void Start ()
	{
		InvokeRepeating ("decreaseTimeRemaining", 1.0F, 1.0F);
	}

	public void AddScore (int n)
	{
		if (score_now == null)
			score_now = 0;

		score_now += 1;
	}

	void Update ()
	{
		if (score_now != null) {
			score.text = score_now + "";
		}
		if (timeRemaining == 0) {
			SendMessageUpwards ("timeElapsed");
			timeElapsed ();
		} else {
			countdown.text = timeRemaining + "";
		}
	}
	
	void decreaseTimeRemaining ()
	{
		timeRemaining --;
	}
	
	//may not be needed, left it in there
	void timeElapsed ()
	{
		CancelInvoke ("decreaseTimeRemaining");
		countdown.text = "0";
	}
}



