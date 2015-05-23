using UnityEngine;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;

public abstract class resBase : MonoBehaviour
{
	public bool autoInit = false;

	protected abstract void init ();

	public virtual void Start ()
	{
		if (autoInit)
			init ();
	}

	#region making and locating the objects
	protected virtual void createobjectsByConnectionsOnSphere (Vector3 position, Quaternion lookat, int i)
	{
		
	}

	protected virtual void createobjectsByConnectionsOnSphere (Vector3 position, Quaternion lookat)
	{
		
	}

	protected virtual void createobjectsByConnectionsOnSphere (Vector3 position)
	{
		
	}
	
	protected void initByConnectionsOnSphere (Vector3 start, float angleLimit, int count, float sphere_radius_distance)
	{
		Vector3 m = start;
		for (int j=0; j<count; j++) {
			m = fromOnSphereNowToRandomNewPositionInDistance (angleLimit, m, sphere_radius_distance);
			Quaternion lookat = Quaternion.FromToRotation (Vector3.up, getNormal (m)) * Quaternion.identity;
			createobjectsByConnectionsOnSphere (m, lookat, j);
			createobjectsByConnectionsOnSphere (m, lookat);
			createobjectsByConnectionsOnSphere (m);
		}
	}
	#endregion


	protected void rezIndependent (GameObject prefab, float distance)
	{
		Vector3 V = GenerateRandomPos (distance);
		Quaternion rot = Quaternion.Euler (getNormal (V)); 
		GameObject openobject = Instantiate (prefab, V, Quaternion.identity) as GameObject;
		openobject.transform.rotation = Quaternion.FromToRotation (-openobject.transform.up, getNormal (V)) * openobject.transform.rotation;
		openobject.transform.parent = GameObject.FindGameObjectWithTag ("Ground").transform;
	}


	/**
	 * to calculate the position from now to another new point position
	 */
	protected Vector3 fromOnSphereNowToRandomNewPositionInDistance (float angleDetla, Vector3 nowPos, float distance_radius)
	{
		return GetPoint2 (genPositionfromQuaternion (nowPos), angleDetla) * distance_radius;
	}

	/**
	 * get the normal angle by position
	 */
	public Vector3 getNormal (Vector3 vpos)
	{
		Vector3 t = GetComponent<Transform> ().position - vpos;
		float distance = t.magnitude;
		return t / distance;
	}

	public Quaternion genPositionfromQuaternion (Vector3 normalized)
	{
		return  Quaternion.LookRotation (normalized);
	}

	public Vector3 GenerateRandomPos (float onSurfaceDistance)
	{
		//float onSurfaceDistance = onSurface ? surfaceDistanceRadius : Random.Range (minDistance, maxDistance);
		Vector3 newPos = Random.onUnitSphere * onSurfaceDistance;
		return newPos;
	}

	/**
	 * http://answers.unity3d.com/questions/58692/randomonunitsphere-but-within-a-defined-range.html?sort=oldest
	 * 
	 * I'm trying to instiate a prefabs randomly onUnitSphere, but Instead of instantiating all over the sphere I want them to only instantiate within a small radias on the sphere.
	 */
	public static Vector3 GetPointOnUnitSphereCap (Quaternion targetDirection, float angle)
	{
		float angleInRad = Random.Range (0.0F, angle) * Mathf.Rad2Deg;
		Vector3 pointoncircle = (Random.insideUnitCircle.normalized) * Mathf.Sin (angleInRad);
		float distance = Mathf.Tan (angleInRad);
		Vector2 PointOnCircle = Random.insideUnitCircle;
		//Mathf.Cos(angleInRad)
		Vector3 V = new Vector3 (PointOnCircle.x, PointOnCircle.y, distance);
		return targetDirection * V;
	}

	public static Vector3 GetPoint2 (Quaternion targetDirection, float angle)
	{
		double angleInRad = Mathf.Clamp (90.0f - angle, Mathf.Epsilon, 90.0f - Mathf.Epsilon) * Mathf.Deg2Rad;
		float distance = Mathf.Tan ((float) angleInRad);
		Vector3 PointOnCircle = Random.insideUnitCircle;
		Vector3 V = new Vector3 (PointOnCircle.x, PointOnCircle.y, distance);

		return targetDirection * V;

	}
}
