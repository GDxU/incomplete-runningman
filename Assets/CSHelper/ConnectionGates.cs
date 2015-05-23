using UnityEngine;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;

[System.Serializable]
public class itemGate
{
	readonly public GameObject itemPrefab;
	readonly public int num;
	readonly public Vector3 location;

	public itemGate (int i, Vector3 loc, GameObject host)
	{
		itemPrefab = host;
		num = i;
		location = loc;
	}
}

public class ConnectionGates : resBase
{

	public GameObject singleConnectedObject;
	public int nodePoints;
	public float angleControl = 25f, minDistance, maxDistance, surfaceDistanceRadius;
	private Transform parentHolder;
	[SerializeField]
	public List<itemGate>
		ram = new List<itemGate> (1);
	private float distance_object_from_center;
	// Use this for initialization
	protected void Start ()
	{
		parentHolder = GameObject.FindGameObjectWithTag ("Ground").transform;
		distance_object_from_center = minDistance;
		//Random.Range (minDistance, maxDistance);
		base.Start ();
	}

	protected override void init ()
	{
		initByConnectionsOnSphere (Vector3.zero, angleControl, nodePoints, distance_object_from_center);
	}

	protected override void createobjectsByConnectionsOnSphere (Vector3 position, Quaternion rot, int i)
	{
		GameObject openobject = Instantiate (singleConnectedObject, position, Quaternion.identity) as GameObject;
		openobject.transform.rotation = rot;
		openobject.name = "gate-" + i;
		openobject.transform.parent = parentHolder;
		itemGate g = new itemGate (i, position, openobject);
		ram.Add (g);
	}

	protected void DebugDrawRay ()
	{
		int k = ram.Count;
		if (k > 0) {
			for (int h = 0; h< k; h++) {
				if (h < k - 1) {
					Vector3 t = ram [h + 1].location - ram [h].location;
					Debug.DrawRay (ram [h].location, t.normalized, Color.red);
				}
			
			}
		}
	}

	void Update ()
	{
		DebugDrawRay ();
	}

}
