using UnityEngine;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;

[System.Serializable]
public class itemConf
{
	public GameObject itemPrefab;
	public int randomCounts;
	public bool justOnSurface = false;
}

public class RezzerOnSphere : MonoBehaviour
{  

	public float angleControl = 90F, minDistance, maxDistance, surfaceDistanceRadius;
	//public int objectCountMin, objectCountMax;
	[SerializeField]
	public List<itemConf>
		fab = new List<itemConf> (1);


	// Use this for initialization
	void Start ()
	{
//		if (objectCountMin > objectCountMax) {
//			Debug.LogError ("objectCountMin needs to be smaller than objectCountMax");
//			return;
//		}
//		if (objectCountMax < 1) {
//			Debug.LogError ("objectCountMax needs to be bigger than zero;");
//			return;
//		}
		if(surfaceDistanceRadius == null){
			Debug.LogError ("surfaceDistanceRadius needs to be bigger than zero;");
			return;
		}
		for (int h = 0; h<fab.Count; h++) {
			itemConf item = fab [h];
			for (int i = 0; i<item.randomCounts; i++) {
				rez (item.itemPrefab, item.justOnSurface);
			}
		}
	}

	protected void rez (GameObject prefab, bool on)
	{
		Vector3 V = GenerateRandomPos (on);
		Quaternion rot = Quaternion.Euler (getNormal (V)); 
		GameObject openobject = Instantiate (prefab, V, Quaternion.identity) as GameObject;
		openobject.transform.rotation = Quaternion.FromToRotation (-openobject.transform.up, getNormal (V)) * openobject.transform.rotation;
		openobject.transform.parent = GameObject.FindGameObjectWithTag ("Ground").transform;
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

	public Vector3 GenerateRandomPos (bool onSurface)
	{

		float onSurfaceDistance = onSurface ? surfaceDistanceRadius : Random.Range (minDistance, maxDistance);
		Vector3 newPos = Random.onUnitSphere * onSurfaceDistance;
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
