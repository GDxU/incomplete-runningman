using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(PlayerHealth))]
[RequireComponent (typeof(normalwalk2))]
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
	private bool rOn = false, lOn = false;
	private float percentage;
	public normalwalk2 tmp;
	public PlayerHealth hb;
	private bool controlable;
	public GameObject gameoverbutton;
	private int score;

	public void rightHit ()
	{
		if (rOn && controlable) {
			//	bartemp += upSpeedPress;
			rOn = false;
			lOn = true;
			bartemp += 2f;
			score += (int)bartemp;
		}
	}

	public void leftHit ()
	{
		if (lOn && controlable) {
			//bartemp += upSpeedPress;
			rOn = true;
			lOn = false;
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
		tmp = this.GetComponent<normalwalk2> ();
		hb = this.GetComponent<PlayerHealth> ();
		upSpeedPress = 0;
		score = 0;
		rOn = true;
		controlable = true;
	}

	public void StopControl ()
	{
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
