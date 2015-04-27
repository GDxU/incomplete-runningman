using UnityEngine;
using System.Collections;

public class Locator : MonoBehaviour
{
	public GameObject theCurrentBoat;
	public Vector3 theEarth;
	private Vector3 surfaceNormal; // current surface normal
	public float radius;
	// Use this for initialization
	void Start ()
	{
		radius = GetComponent<SphereCollider> ().radius;
	}
	//get the direction from the current object to the sphere
	Vector3 getNormal (GameObject fab)
	{
		Vector3 t = theEarth - fab.transform.position;
		float distance = t.magnitude;
		return t / distance;
	}

	void Create (GameObject fab, float deep = 0f)
	{

		Ray ray;
		RaycastHit hit;
		
		if (Input.GetButtonDown ("Jump")) { // jump pressed:
			ray = new Ray (fab.transform.position, getNormal(fab));
			if (Physics.Raycast (ray, out hit, 10)) { // wall ahead?
//				JumpToWall (hit.point, hit.normal); // yes: jump to the wall
//			} else if (isGrounded) { // no: if grounded, jump up
//				GetComponent<Rigidbody> ().velocity += jumpSpeed * myNormal;
			//	Instantiate(fab, hit.point, Quaternion.LookRotation(getNormal(fab)));
			}
		}
	}
	// Update is called once per frame
	void Update ()
	{
	
	}
}
