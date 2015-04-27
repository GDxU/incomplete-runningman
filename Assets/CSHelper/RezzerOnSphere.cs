using UnityEngine;
using System.Collections;

public class RezzerOnSphere : MonoBehaviour
{

	public float angleControl = 90F, minDistance, maxDistance;
	public int objectCountMin, objectCountMax;
	public GameObject fab;
	// Use this for initialization
	void Start ()
	{
		if (objectCountMin > objectCountMax) {
			Debug.LogError ("objectCountMin needs to be smaller than objectCountMax");
			return;
		}
		if (objectCountMax < 1) {
			Debug.LogError ("objectCountMax needs to be bigger than zero;");
			return;
		}
		for (int i = 0; i<50; i++) {
			// Vector3 V = GetPointOnUnitSphereCap (transform.rotation, angleControl);
			Vector3 V = GenerateRandomPos ();
			// Debug.DrawRay (transform.position, V, Color.red);
			if (fab != null) {
				Quaternion rot = Quaternion.Euler (getNormal (V)); 
				//* Vector3.up;

				GameObject openobject = Instantiate (fab, V, Quaternion.identity) as GameObject;
				//Quaternion currentRot = T.localRotation;
				openobject.transform.rotation = Quaternion.FromToRotation (-openobject.transform.up, getNormal (V)) * openobject.transform.rotation;
				//T.transform.rotation = Quaternion.Euler (getNormal (V)) * -T.transform.up;
				//Quaternion.LookRotation(Vector3.up * getNormal(V)
				//openobject.transform.LookAt (getNormal (V));
				openobject.transform.parent = GameObject.FindGameObjectWithTag ("Ground").transform;
				//T.transform.TransformPoint (Quaternion.Euler (getNormal (V)) * Vector3.up);
			}
		}
	}

	public Vector3 getNormal (Vector3 vpos)
	{
		Vector3 t = GetComponent<Transform> ().position - vpos;
		float distance = t.magnitude;
		return t / distance;
	}
	// Update is called once per frame
	void Update ()
	{

   

	}

	public Vector3 GenerateRandomPos ()
	{
		Vector3 newPos = Random.onUnitSphere * Random.Range (minDistance, maxDistance);
		// newPos.y = originalPos.y;
		return newPos;
	}

	public Vector3 GetPointOnUnitSphereCap (Quaternion targetDirection, float angle)
	{
		float angleInRad = Random.Range (0.0F, angle) * Mathf.Rad2Deg;
		Vector3 pointoncircle = (Random.insideUnitCircle.normalized) * Mathf.Sin (angleInRad);
		float distance = Mathf.Tan (angleInRad);
		Vector2 PointOnCircle = Random.insideUnitCircle;
		//Mathf.Cos(angleInRad)
		Vector3 V = new Vector3 (PointOnCircle.x, PointOnCircle.y, distance);
		return targetDirection * V;
	}
}
