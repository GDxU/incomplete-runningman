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

	public void rightHit ()
	{
		if (rOn) {
			//	bartemp += upSpeedPress;
			rOn = false;
			lOn = true;
			bartemp += 2f;
		}
	}

	public void leftHit ()
	{
		if (lOn) {
			//bartemp += upSpeedPress;
			rOn = true;
			lOn = false;
			bartemp += 2f;
		}
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
		rOn = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (bartemp > maxLimit) {
			bartemp = maxLimit;
		}
		percentage = bartemp / maxLimit;
		tmp.setExternalSpeedCurrent (percentage*real_move_system);
		if (bartemp > 0) {
			bartemp -= percentage * threadhold;
		}
		int b = (int)(percentage * 100);
		hb.setVal (b);
	}
}
