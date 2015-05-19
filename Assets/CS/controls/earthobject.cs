using UnityEngine;
using System.Collections;

public class earthobject : baseEarthMove
{
	public MODE objectType = MODE.ADDPOINTOBJECT;
	public float default_speed_drifting = 0.2f;
	public int taken_score = 1;
	public CountDown uiControl;
	public enum MODE
	{
		ADDPOINTOBJECT,
		DAMAGEOBJECT,
		MOVEOBJECT
	}
	protected override void controlDirecitonTurns ()
	{

	}

	protected override float getControlSpeedNow ()
	{
		return default_speed_drifting;
	}

	protected override void checkhp ()
	{

	}

	void OnCollisionEnter (Collision collision)
	{
		if (uiControl != null) {
			if (objectType == MODE.ADDPOINTOBJECT) {
				if (collision.gameObject.CompareTag ("Player")) {
					foreach (ContactPoint contact in collision.contacts) {
						Debug.DrawRay (contact.point, contact.normal, Color.white);
						uiControl.AddScore(taken_score);
						Destroy(gameObject);
					}
				}
			}

		}else{
			uiControl = GameObject.FindGameObjectWithTag("canvas").GetComponent<CountDown>();
		}
	}
}
