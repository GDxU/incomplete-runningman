using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpeedControl : MonoBehaviour
{
	public Color flashColour = new Color (1f, 0f, 0f, 0.1f);
	public Slider speedControlSlider, directionslider;
	public controlNormal shipcontrolengine;
	public SphereChaseCam chasecam;
	public Button a, b;
	public float adjustmentSpeed = 5.1f;
	// Use this for initialization
	void Start ()
	{
		if (speedControlSlider != null) {
			speedControlSlider.onValueChanged.AddListener (onSpeedChange);
		}
		if (directionslider != null) {
			directionslider.onValueChanged.AddListener (onDirectionChange);
		}
	}

	private bool releasedSliderUp = false;
	/**
	 * to receive the slider up event
*/
	public void SlideDrageUp ()
	{
		releasedSliderUp = true;
	}

	public void SliderDragOn ()
	{
		releasedSliderUp = false;

	}

	float directionsliderv ;

	private void sliderRuntimeAdjustment ()
	{
		if (releasedSliderUp) {
			float t = Time.deltaTime * adjustmentSpeed;
			directionsliderv = directionslider.value;
			directionslider.value = Mathf.SmoothStep (directionsliderv, 0.5f, t);
		}
	}

	private void clicknormal ()
	{
		chasecam.CamState = SphereChaseCam.CamStates.Behind;
	}

	private void clicksurround ()
	{
		chasecam.CamState = SphereChaseCam.CamStates.AutoSurround;
	}

	private void onSpeedChange (float val)
	{
		if (shipcontrolengine != null) {
			shipcontrolengine.setExternalSpeedCurrent (val / 100);
		}
	}

	private void onDirectionChange (float val)
	{
		float normal = val - 0.5f;
		shipcontrolengine.inputDirection (normal / 0.5f);
	}

	void Update ()
	{
		sliderRuntimeAdjustment ();
	}

	private void onDown ()
	{
		Debug.Log ("onDown");
	}

	private void onUp ()
	{
		Debug.Log ("up");
	}


}
