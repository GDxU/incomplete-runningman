using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpeedControl : MonoBehaviour
{
	public Color flashColour = new Color (1f, 0f, 0f, 0.1f);
	public Slider speedControlSlider, directionslider;
	public controlNormal shipcontrolengine;
	public SphereChaseCam chasecam;
	public Button a,b;
	// Use this for initialization
	void Start ()
	{
		if (speedControlSlider != null) {
			speedControlSlider.onValueChanged.AddListener (onSpeedChange);
		}
		if (directionslider != null) {
			directionslider.onValueChanged.AddListener (onDirectionChange);
		
			//	directionslider.OnPointerDown (onDown);
			//	directionslider.OnPointerUp (onUp);
		}
		if(a!=null && chasecam!=null){
			a.onClick.AddListener(() => clicknormal());
		}
		if(b!=null && chasecam!=null){
			b.onClick.AddListener(() => clicksurround());
		}
	}

	private void clicknormal(){
		chasecam.CamState = SphereChaseCam.CamStates.Behind;
	}

	private void clicksurround(){
		chasecam.CamState = SphereChaseCam.CamStates.AutoSurround;
	}

	private void onDirectionChange (float val)
	{
		float normal = val - 0.5f;
		shipcontrolengine.inputDirection (normal / 0.5f);
	}

	void update ()
	{

		//	healthSlider.value = h;
	}

	private void onDown ()
	{
		Debug.Log ("onDown");
	}

	private void onUp ()
	{
		Debug.Log ("up");
	}

	private void onSpeedChange (float val)
	{
		if (shipcontrolengine != null) {
			shipcontrolengine.setExternalSpeedCurrent (val / 100);
		}
	}
}
