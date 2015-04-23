using UnityEngine;
using System.Collections;

public class physicinEarth : MonoBehaviour
{
	private GameObject mground;
	private Vector3 theEarth;
	private Vector3 surfaceNormal; // current surface normal
	//public float radius;
	// Use this for initialization
	public Quaternion rotOffSet;
	private Vector3 now;

	void Start ()
	{
		basic ();
	}

	void Awake ()
	{
		basic ();
	}

	private void basic ()
	{
		Quaternion currentRot = GetComponent<Transform> ().localRotation;
		now = GetComponent<Transform> ().position;
		theEarth = GameObject.FindGameObjectWithTag ("Ground").transform.position;
		//GetComponent<Transform> ().LookAt(theEarth);
		Ray ray;
		RaycastHit hit;
		ray = new Ray (now, getNormal ());
		if (Physics.Raycast (ray, out hit, 500)) { // wall ahead?
			//				JumpToWall (hit.point, hit.normal); // yes: jump to the wall
			//			} else if (isGrounded) { // no: if grounded, jump up
			//				GetComponent<Rigidbody> ().velocity += jumpSpeed * myNormal;
			//Instantiate (fab, hit.point, Quaternion.LookRotation (getNormal (fab)));

			GetComponent<Transform> ().position = hit.point;
		}
		//GetComponent<Transform> ().rotation = Quaternion.Euler (getNormal ());

		GetComponent<Transform> ().rotation = Quaternion.FromToRotation (-transform.up, getNormal ()) * GetComponent<Transform> ().rotation;

	}
	//get the direction from the current object to the sphere
	private Vector3 getNormal ()
	{
		Vector3 t = theEarth - now;
		float distance = t.magnitude;
		//float sqrLen = offset.sqrMagnitude;

		return t / distance;
	}

	void OnDrawGizmos ()
	{
		Gizmos.color = Color.yellow;
		Gizmos.color = Color.red;
		//Gizmos.DrawSphere(transform.position, 1);
		//	Gizmos.DrawLine (now, getNormal ());
		Gizmos.DrawRay (now, getNormal ());
	} 
}
