using UnityEngine;
using System.Collections;

public class controlNormal : baseEarthMove
{
	public int hp = 100;
	public bool autodrive = false, collisionModed = false, debugMoveSpeed = false;
	public float speedinternal;
	private float inputsliderdriverfactor = 0;
	public GameObject fire;
	public MODE drivingMode = MODE.AUTODRIVE;
	public enum MODE
	{
		AUTODRIVE,
		KEYBOARD,
		SLIDERDRIVE
	}
//	public float turn_speed{
//		get{return this.turnSpeed;}
//		set{this.turnSpeed = value;}
//	}
//	protected override void Start (){
//
//	}
	public void inputDirection (float r)
	{
		inputsliderdriverfactor = r;
	}

	protected override void controlDirecitonTurns ()
	{
		switch (this.drivingMode) {
		case MODE.AUTODRIVE:
			myTransform.Rotate (0, turnSpeed * Time.deltaTime, 0);
			break;
		case MODE.KEYBOARD:
			myTransform.Rotate (0, Input.GetAxis ("Horizontal") * turnSpeed * Time.deltaTime, 0);
			break;
		case MODE.SLIDERDRIVE:
			myTransform.Rotate (0, inputsliderdriverfactor * turnSpeed * Time.deltaTime, 0);
			break;
		}
	
	}

	protected override void checkhp ()
	{
		if (fire != null) {
			if (hp < 20) {
				fire.SetActive (true);
			} else {
				if (fire.activeSelf) {
					fire.SetActive (false);
				}
			}
		}
	}

	protected override float getControlSpeedNow ()
	{
		return debugMoveSpeed ? speedinternal : external_control_speed;
	}


	#region Properties (public)

	public bool IsJump ()
	{
		return this.jumping || !this.isGrounded;
	}
	
	public Vector3 getNormal ()
	{
		return this.surfaceNormal;
	}
	#endregion

	void OnCollisionEnter (Collision collision)
	{
		if (collisionModed) {
			if (collision.gameObject.CompareTag ("Player")) {
				foreach (ContactPoint contact in collision.contacts) {
					Debug.DrawRay (contact.point, contact.normal, Color.white);
					if (hp > 0) {
						hp--;
					}
				}
			}
		}
	}
}
