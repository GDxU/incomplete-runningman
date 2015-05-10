using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SocialPlatforms;
public class GameControl : MonoBehaviour
{	
	public Canvas quitMenu;
	public Button startText;
	public Button exitText;
	public float maxLimit = 20f;
	public float bartemp;
	public float downPressure;
	public float upSpeedPress;
	public float threadhold = 0.1f;
	public float real_move_system = 1f;
	//[HideInInspector]
	private bool controlable;
	private float percentage;
	public controlNormal tmp;
	public PlayerHealth hb;
	public GameObject gameoverbutton;
	private int score, incremental;

	//http://stackoverflow.com/questions/19675676/calculating-actual-angle-between-two-vectors-in-unity3d
	public float SignedAngleBetween (Vector3 a, Vector3 b, Vector3 n)
	{
		// angle in [0,180]
		float angle = Vector3.Angle (a, b);
		float sign = Mathf.Sign (Vector3.Dot (n, Vector3.Cross (a, b)));
		
		// angle in [-179,180]
		float signed_angle = angle * sign;
		
		// angle in [0,360] (not used but included here for completeness)
		//float angle360 =  (signed_angle + 180) % 360;
		
		return signed_angle;
	}

	public void rightHit ()
	{
		if (incremental % 2 == 0 && controlable) {
			incremental++;
			bartemp += 2f;
			score += (int)bartemp;
		}
	}

	public void leftHit ()
	{
		if (incremental % 2 == 1 && controlable) {
			incremental++;
			bartemp += 2f;
			score += (int)bartemp;
		}
	}

	public int getScore ()
	{
		return score;
	}

	void Awake ()
	{

	}
	// Use this for initialization
	void Start ()
	{
//		quitMenu = quitMenu.GetComponent<Canvas> ();
//		startText = startText.GetComponent<Button> ();
//		exitText = exitText.GetComponent<Button> ();
		//tmp = this.GetComponent<normalwalk2> ();
		//hb = this.GetComponent<PlayerHealth> ();
		upSpeedPress = 0;
		score = 0;
		incremental = 0;
		controlable = true;
	}

	public void StopControl ()
	{

		// Select the Google Play Games platform as our social platform implementation
		GooglePlayGames.PlayGamesPlatform.Activate();

		controlable = false;
		gameoverbutton.SetActive (true);
	}

	private void keyboard ()
	{
		if (controlable) {
			if (Input.GetButtonDown ("Fire1")) {
				leftHit ();
			} else if (Input.GetButtonDown ("Fire2")) {
				rightHit ();
			}
		}
	}
	// Update is called once per frame
	void Update ()
	{
		keyboard ();
		if (bartemp > maxLimit) {
			bartemp = maxLimit;
		}
		percentage = bartemp / maxLimit;
		tmp.setExternalSpeedCurrent (percentage * real_move_system);
		if (bartemp > 0) {
			bartemp -= percentage * threadhold;
		}
		int b = (int)(percentage * 100);
		hb.setVal (b);
	}

	public void exitGame ()
	{
		Application.LoadLevel (0);
	}
}
