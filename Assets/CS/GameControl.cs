using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
	public bool rOn = false, lOn = false;
	private float percentage;
	public normalwalk2 tmp;

	public void rightHit ()
	{
		if (rOn) {
			bartemp += upSpeedPress;
			rOn = false;
			lOn = true;
			upSpeedPress += 0.1f;
		}
	}

	public void leftHit ()
	{
		if (lOn) {
			bartemp += upSpeedPress;
			rOn = true;
			lOn = false;
			upSpeedPress += 0.1f;
		}
	}
	// Use this for initialization
	void Start ()
	{
//		quitMenu = quitMenu.GetComponent<Canvas> ();
//		startText = startText.GetComponent<Button> ();
//		exitText = exitText.GetComponent<Button> ();
		tmp = this.GetComponent<normalwalk2> ();
		upSpeedPress = 0;
		rOn = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		percentage = bartemp / maxLimit;
		tmp.setExternalSpeedCurrent (bartemp);
		if (bartemp > 0) {
			bartemp-=0.01f;
		}
	}
}
