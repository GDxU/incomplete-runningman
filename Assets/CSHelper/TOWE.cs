using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class TOWE : physicinEarth
{
	AudioSource asr;
	Transform t;
	// Use this for initialization
	void Start ()
	{
		t = GetComponent<Transform> ();
		asr = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	

	}
	
	void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.CompareTag ("Player")) {
			foreach (ContactPoint contact in collision.contacts) {
				Debug.DrawRay (contact.point, contact.normal, Color.white);
			}
			if (collision.relativeVelocity.magnitude > 0.2f) {
				asr.Play ();
			}
		}
	}

}
