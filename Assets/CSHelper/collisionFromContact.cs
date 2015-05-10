using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class collisionFromContact : MonoBehaviour
{

	controlNormal player;
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
				player = collision.gameObject.GetComponent<controlNormal> ();
				player.exploreFrom (contact.point, contact.normal, 10f);
			}
			if (collision.relativeVelocity.magnitude > 0.2f) {
				asr.Play ();
			}
		}
	}

}
