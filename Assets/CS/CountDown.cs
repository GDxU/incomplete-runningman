using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {
	
	public float timeRemaining = 60f;
	public Text countdown;
	void Start()
	{
		countdown  = countdown.GetComponent<Text>();
	//	InvokeRepeating("decreaseTimeRemaining", 1.0, 1.0);
	}
	
	void Update()
	{
		if (timeRemaining == 0)
		{
			//sendMessageUpward("timeElapsed");
		}
		
		//GuiText.text = timeRemaining + " Seconds remaining!";
		countdown.text = timeRemaining + " Seconds remaining!";
	}
	
	void decreaseTimeRemaining()
	{
		timeRemaining --;
	}
	
	//may not be needed, left it in there
	void timeElapsed()
	{}
}



