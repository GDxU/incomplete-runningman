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

public class RezzerOnSphere : resBase
{  

	public float angleControl = 90F, minDistance, maxDistance, surfaceDistanceRadius;
	//public int objectCountMin, objectCountMax;
	[SerializeField]
	public List<itemConf>
		randomLocatedObjects = new List<itemConf> (1);

	// Use this for initialization
	protected void Start ()
	{
		if (surfaceDistanceRadius == null) {
			Debug.LogError ("surfaceDistanceRadius needs to be bigger than zero;");
			return;
		}
		base.Start ();
	}

	protected override void init ()
	{
		if (surfaceDistanceRadius == null) {
			Debug.LogError ("surfaceDistanceRadius needs to be bigger than zero;");
			return;
		}
		for (int h = 0; h<randomLocatedObjects.Count; h++) {
			
			itemConf item = randomLocatedObjects [h];
			
			float distance_object_from_center = 
				item.justOnSurface ? surfaceDistanceRadius : Random.Range (minDistance, maxDistance);
			
			for (int i = 0; i<item.randomCounts; i++) {
				rezIndependent (item.itemPrefab, distance_object_from_center);
			}
		}
	}

}
