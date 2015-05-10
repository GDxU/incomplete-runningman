using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
	
	public int timeRemaining = 60;
	public Text countdown, score;
	public GameControl thecontrol;

	void Start ()
	{
		thecontrol = this.GetComponent<GameControl> ();
		//countdown = countdown.GetComponent<Text> ();
		InvokeRepeating ("decreaseTimeRemaining", 1.0F, 1.0F);
	}
	
	void Update ()
	{
		score.text = thecontrol.getScore () + "";
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
		thecontrol.StopControl ();
		countdown.text = "-0-";
	}
}



