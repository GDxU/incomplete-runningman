using UnityEngine;
using System.Collections;

public class controlNormal : baseEarthMove
{
	public int hp = 100;
	public bool autodrive = false, collisionModed = false, debugMoveSpeed = false;
	public float speedinternal = .05f;
	public GameObject fire;
//	public float turn_speed{
//		get{return this.turnSpeed;}
//		set{this.turnSpeed = value;}
//	}
//	protected override void Start (){
//
//	}
	protected override void controlDirecitonTurns ()
	{
		if (autodrive) {
			myTransform.Rotate (0, turnSpeed * Time.deltaTime, 0);
		} else {
			// movement code - turn left/right with Horizontal axis:
			myTransform.Rotate (0, Input.GetAxis ("Horizontal") * turnSpeed * Time.deltaTime, 0);
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
