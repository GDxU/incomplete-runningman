﻿using UnityEngine;
using System.Collections;

struct CameraPositionSphere
{
	// Position to align camera to, probably somewhere behind the character
	// or position to point camera at, probably somewhere along character's axis
	private Vector3 position;
	// Transform used for any rotation
	private Transform xForm;
	private Quaternion rot;

	public Vector3 Position { get { return position; } set { position = value; } }

	public Quaternion Rot { get { return rot; } set { rot = value; } }

	public Transform XForm { get { return xForm; } set { xForm = value; } }

	public void Init (string camName, Transform parent)
	{
		//string paren_name_go = parent.name;
		foreach (Transform g in parent) {
			if (g.gameObject.name == "body") {
				foreach (Transform t in g.gameObject.transform) {
					if (t.gameObject.name == camName) {
						position = t.transform.localPosition;
						xForm = t.transform;
						xForm.localPosition = position;
					}
				}
			}
		}
	}
}

[RequireComponent (typeof(BarsEffect))]
public class SphereChaseCam : MonoBehaviour
{
	#region Variables (private)
	// Inspector serialized	
	[SerializeField]
	private Transform
		parentRig;
	[SerializeField]
	private float
		distanceAway;
	[SerializeField]
	private float
		distanceAwayMultipler = 1.5f;
	[SerializeField]
	private float
		distanceUp, surroundDisUp = 1f;
	[SerializeField]
	private float
		distanceUpMultiplier = 5f;
	[SerializeField]
	private SphereCamState
		follow; //the actual character with the normalwalkX2
	[SerializeField]
	private Transform
		followXform; //the actual character in transform
	[SerializeField]
	private float
		widescreen = 0.2f;
	[SerializeField]
	private float
		targetingTime = 0.5f;
	[SerializeField]
	private float
		firstPersonLookSpeed = 3.0f;
	[SerializeField]
	private Vector2
		firstPersonXAxisClamp = new Vector2 (-70.0f, 90.0f);
	[SerializeField]
	private float
		fPSRotationDegreePerSecond = 120f;
	[SerializeField]
	private float
		firstPersonThreshold = 0.5f;
	[SerializeField]
	private float
		freeThreshold = -0.1f;
	[SerializeField]
	private Vector2
		camMinDistFromChar = new Vector2 (1f, -0.5f);
	[SerializeField]
	private float
		rightStickThreshold = 0.1f;
	[SerializeField]
	private const float
		freeRotationDegreePerSecond = -5f;
	public bool isTargeted = false;
	public bool exit_fpv = false;
	// Smoothing and damping
	private Vector3 velocityCamSmooth = Vector3.zero;
	[SerializeField]
	private float
		camSmoothDampTime = 0.1f, firstPersonDampTime = 0.1f;
	private Vector3 velocityLookDir = Vector3.zero;
	[SerializeField]
	private float
		lookDirDampTime = 0.1f;
	[SerializeField]
	private Vector3
		firstPersonCameraPos = Vector3.zero;
	
	// Private global only
	private Vector3 lookDir;
	private Vector3 curLookDir;
	private BarsEffect barEffect;
	[SerializeField]
	private CamStates
		camState = CamStates.Behind;
	private float xAxisRot = 0.0f;
	private CameraPositionSphere firstPersonCamPos;
	private float lookWeight;
	private const float TARGETING_THRESHOLD = 0.01f;
	private Vector3 savedRigToGoal;
	private float distanceAwayFree;
	private float distanceUpFree;
	private Vector2 rightStickPrevFrame = Vector2.zero;
	private float surroundingm;
	[SerializeField]
	private float
		surroundSpeed = 0.5f;
	#endregion


	
	#region Properties (public)	
	
	public Transform ParentRig {
		get {
			return this.parentRig;
		}
	}
	
	public Vector3 LookDir {
		get {
			return this.curLookDir;
		}
	}
	
	public CamStates CamState {
		get {
			return this.camState;
		}
		set { this.camState = value;}
	}
	
	public enum CamStates
	{
		Behind,
		FirstPerson,
		Target,
		AutoSurround,
		Free
	}
	
	#endregion


	void Start ()
	{
		parentRig = GetComponent<Transform> ();
		if (parentRig == null) {
			Debug.LogError ("Parent camera to empty GameObject.", this);
		}
		
		//follow = GameObject.FindWithTag ("Player").GetComponent<SphereCamState> ();
		//followXform = GameObject.FindWithTag ("Player").transform;
		if (follow == null || followXform == null) {
			Debug.LogError ("awaiting follow and followXform.", this);
			return;
		}
		lookDir = followXform.forward;
		curLookDir = followXform.forward;
		
		barEffect = GetComponent<BarsEffect> ();
		if (barEffect == null) {
			Debug.LogError ("Attach a widescreen BarsEffect script to the camera.", this);
		}
		
		// Position and parent a GameObject where first person view should be
		firstPersonCamPos = new CameraPositionSphere ();
		firstPersonCamPos.Init (
			"FPCam",
			follow.transform
		);	
	}

	
	/// <summary>
	/// Update is called once per frame.
	/// </summary>
	void Update ()
	{
		
	}
	
	/// <summary>
	/// Debugging information should be put here.
	/// </summary>
	void OnDrawGizmos ()
	{	
		
	}

	private Vector3 cameraWatchPosition ()
	{
		Vector3 mypos = this.transform.position;
		Vector3 currentPostion = followXform.position;

		Vector3 dir = currentPostion - mypos;
		Vector2 Look = dir.normalized;

		Ray r2 = new Ray (currentPostion, follow.getNormalCharacterFromGround ());
		Ray r3 = new Ray (r2.GetPoint (0), Vector3.forward);
		return	r3.GetPoint (distanceAway);
	}

	private Vector3 cameraFollowPosition ()
	{
		Ray r2 = new Ray (offsetCameraFromCharacter (), -followXform.forward);
		return	r2.GetPoint (distanceAway);
	}

	private Vector3 offsetCameraFromCharacter ()
	{

		//Vector3 DistanceUp = new Vector3 (0f, distanceUp, 0f);
		Vector3 currentPostion = followXform.position;
		Ray ray = new Ray (currentPostion, followXform.up); 
		return	ray.GetPoint (distanceUp);
		//Quaternion.FromToRotation (DistanceUp, Vector3.Normalize (followXform.rotation)) * followXform.rotation;   
	}

	private void barEffectFilter (float rightY)
	{
		if (isTargeted) {
			barEffect.coverage = Mathf.SmoothStep (barEffect.coverage, widescreen, targetingTime);
			camState = CamStates.Target;
		} else {			
			barEffect.coverage = Mathf.SmoothStep (barEffect.coverage, 0f, targetingTime);
			
			// * First Person *
			if (rightY > firstPersonThreshold && camState != CamStates.Free && camState != CamStates.Free && !follow.IsInLocomotion ()) {
				// Reset look before entering the first person mode
				xAxisRot = 0;
				lookWeight = 0f;
				camState = CamStates.FirstPerson;
			}
			if (rightY < freeThreshold && System.Math.Round (follow.Speed, 2) == 0) {
				camState = CamStates.Free;
				savedRigToGoal = Vector3.zero;
			}
			// * Behind the back *
			if ((camState == CamStates.FirstPerson && exit_fpv) || (camState == CamStates.Target && !isTargeted)) {
				camState = CamStates.Behind;	
			}


		}

	}

	private bool isjump;

	void LateUpdate ()
	{		
		if (follow == null || followXform == null) {
			return;
		}
		
		// Pull values from controller/keyboard
		float rightX = 0f;
		//Input.GetAxis("RightStickX");
		float rightY = 0f;//Input.GetAxis("RightStickY");
		float leftX = 0f;//Input.GetAxis("Horizontal");
		float leftY = 0f;//Input.GetAxis("Vertical");	

		Vector3 characterOffset = offsetCameraFromCharacter ();
		Vector3 lookAt = characterOffset;
		Vector3 targetPosition = Vector3.zero;
		// Determine camera state
		// * Targeting *
		//isTargeted
		//if (Input.GetAxis ("Target") > TARGETING_THRESHOLD) {
		// Set the Look At Weight - amount to use look at IK vs using the head's animation
		//follow.Animator.SetLookAtWeight (lookWeight);
		barEffectFilter (rightY);
		// Execute camera state
		switch (camState) {
		case CamStates.Behind:
			//ResetCamera ();
			// Only update camera look direction if moving

			lookDir = Vector3.Lerp (followXform.right * (leftX < 0 ? 1f : -1f), followXform.forward * (leftY < 0 ? -1f : 1f), Mathf.Abs (Vector3.Dot (this.transform.forward, followXform.forward)));
			Debug.DrawRay (this.transform.position, lookDir, Color.white);
				
			// Calculate direction from camera to player, kill Y, and normalize to give a valid direction with unit magnitude
			curLookDir = Vector3.Normalize (characterOffset - this.transform.position);
			curLookDir.y = 0;
			Debug.DrawRay (this.transform.position, curLookDir, Color.green);
			// Damping makes it so we don't update targetPosition while pivoting; camera shouldn't rotate around player
			curLookDir = Vector3.SmoothDamp (curLookDir, lookDir, ref velocityLookDir, lookDirDampTime);
			//targetPosition = characterOffset + followXform.up * distanceUp - Vector3.Normalize (curLookDir) * distanceAway;
			targetPosition = follow.IsInJump () ? cameraWatchPosition () : cameraFollowPosition ();
				//characterOffset - Vector3.Normalize (curLookDir) * distanceAway;
			//+ followXform.up * distanceUp ;
			isjump = follow.IsInJump ();
			Debug.DrawLine (followXform.position, targetPosition, Color.gray);
			//this.transform.localEulerAngles.z = followXform.localEulerAngles.z; 
		
				//new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y, followXform.eulerAngles.z);
			//Quaternion.Lerp (this.transform.rotation, followXform.rotation, Time.deltaTime);
			break;
		case CamStates.Target:
			ResetCamera ();
			lookDir = followXform.forward;
			curLookDir = followXform.forward;
			targetPosition = characterOffset + followXform.up * distanceUp - lookDir * distanceAway;
			
			break;
		case CamStates.AutoSurround:
			ResetCamera ();

			//surroundingm += surroundSpeed;
		//	targetPosition = followXform.position + surroundDisUp * (1 + 1 / distanceUpMultiplier) * Vector3.up + Quaternion.Euler (0f, surroundingm, 0f) * followXform.forward * distanceAway;

			// Damping makes it so we don't update targetPosition while pivoting; camera shouldn't rotate around player
			//curLookDir = Vector3.SmoothDamp (curLookDir, lookDir, ref velocityLookDir, lookDirDampTime);
			// Calculate direction from camera to player, kill Y, and normalize to give a valid direction with unit magnitude
			curLookDir = Vector3.Normalize (characterOffset - this.transform.position);
			//curLookDir.y = 0;

		
			Ray r2 = new Ray (offsetCameraFromCharacter (), -followXform.forward);
			r2.GetPoint (distanceAway);
			parentRig.RotateAround (offsetCameraFromCharacter (), followXform.up, surroundSpeed * Time.deltaTime);

			float currentMoonDistance = Vector3.Distance (followXform.position, parentRig.position);
			Vector3 fizxx = (surroundDisUp - currentMoonDistance) * curLookDir;
			targetPosition = parentRig.position - fizxx;


			//transform.RotateAround (followXform.position, followXform.up, surroundingm);
			Debug.DrawLine (followXform.position, targetPosition, Color.magenta);

		
			//targetPosition = followXform.position +
			Debug.DrawRay (followXform.position, followXform.up, Color.red);

			lookAt = followXform.position + distanceUp * Vector3.up;
			//lookAt = rotation;
			Debug.DrawLine (followXform.position, targetPosition, Color.white);

			Debug.DrawRay (targetPosition, lookAt, Color.red);
			break;	
		case CamStates.FirstPerson:	
			// Looking up and down
			// Calculate the amount of rotation and apply to the firstPersonCamPos GameObject
			xAxisRot += (leftY * 0.5f * firstPersonLookSpeed);			
			xAxisRot = Mathf.Clamp (xAxisRot, firstPersonXAxisClamp.x, firstPersonXAxisClamp.y); 
			firstPersonCamPos.XForm.localRotation = Quaternion.Euler (xAxisRot, 0, 0);
			
			// Superimpose firstPersonCamPos GameObject's rotation on camera
			Quaternion rotationShift = Quaternion.FromToRotation (this.transform.forward, firstPersonCamPos.XForm.forward);		
			this.transform.rotation = rotationShift * this.transform.rotation;		
			
			// Move character model's head
			//follow.Animator.SetLookAtPosition (firstPersonCamPos.XForm.position + firstPersonCamPos.XForm.forward);
			//lookWeight = Mathf.Lerp (lookWeight, 1.0f, Time.deltaTime * firstPersonLookSpeed);
			
			
			// Looking left and right
			// Similarly to how character is rotated while in locomotion, use Quaternion * to add rotation to character
			//Vector3 rotationAmount = Vector3.Lerp (Vector3.zero, new Vector3 (0f, fPSRotationDegreePerSecond * (leftX < 0f ? -1f : 1f), 0f), Mathf.Abs (leftX));
			//Quaternion deltaRotation = Quaternion.Euler (rotationAmount * Time.deltaTime);
			//follow.transform.rotation = (follow.transform.rotation * deltaRotation);
			
			// Move camera to firstPersonCamPos
			targetPosition = firstPersonCamPos.XForm.position;
			
			// Smoothly transition look direction towards firstPersonCamPos when entering first person mode
			lookAt = Vector3.Lerp (targetPosition + followXform.forward, this.transform.position + this.transform.forward, camSmoothDampTime * Time.deltaTime);
			Debug.DrawRay (Vector3.zero, lookAt, Color.black);
			Debug.DrawRay (Vector3.zero, targetPosition + followXform.forward, Color.white);	
			Debug.DrawRay (Vector3.zero, firstPersonCamPos.XForm.position + firstPersonCamPos.XForm.forward, Color.cyan);
			
			// Choose lookAt target based on distance
			lookAt = (Vector3.Lerp (this.transform.position + this.transform.forward, lookAt, Vector3.Distance (this.transform.position, firstPersonCamPos.XForm.position)));
			break;
		case CamStates.Free:
			lookWeight = Mathf.Lerp (lookWeight, 0.0f, Time.deltaTime * firstPersonLookSpeed);
			
			// Move height and distance from character in separate parentRig transform since RotateAround has control of both position and rotation
			Vector3 rigToGoalDirection = Vector3.Normalize (characterOffset - this.transform.position);
			// Can't calculate distanceAway from a vector with Y axis rotation in it; zero it out
			rigToGoalDirection.y = 0f;
			
			Vector3 rigToGoal = characterOffset - parentRig.position;
			rigToGoal.y = 0;
			Debug.DrawRay (parentRig.transform.position, rigToGoal, Color.red);
			
			// Panning in and out
			// If statement works for positive values; don't tween if stick not increasing in either direction; also don't tween if user is rotating
			// Checked against rightStickThreshold because very small values for rightY mess up the Lerp function
			if (rightY < -1f * rightStickThreshold && rightY <= rightStickPrevFrame.y && Mathf.Abs (rightX) < rightStickThreshold) {
				distanceUpFree = Mathf.Lerp (distanceUp, distanceUp * distanceUpMultiplier, Mathf.Abs (rightY));
				distanceAwayFree = Mathf.Lerp (distanceAway, distanceAway * distanceAwayMultipler, Mathf.Abs (rightY));
				targetPosition = characterOffset + followXform.up * distanceUpFree - rigToGoalDirection * distanceAwayFree;
			} else if (rightY > rightStickThreshold && rightY >= rightStickPrevFrame.y && Mathf.Abs (rightX) < rightStickThreshold) {
				// Subtract height of camera from height of player to find Y distance
				distanceUpFree = Mathf.Lerp (Mathf.Abs (transform.position.y - characterOffset.y), camMinDistFromChar.y, rightY);
				// Use magnitude function to find X distance	
				distanceAwayFree = Mathf.Lerp (rigToGoal.magnitude, camMinDistFromChar.x, rightY);
				
				targetPosition = characterOffset + followXform.up * distanceUpFree - rigToGoalDirection * distanceAwayFree;
			}
			
			// Store direction only if right stick inactive
			if (rightX != 0 || rightY != 0) {
				savedRigToGoal = rigToGoalDirection;
			}
			
			
			// Rotating around character
			parentRig.RotateAround (characterOffset, followXform.up, freeRotationDegreePerSecond * (Mathf.Abs (rightX) > rightStickThreshold ? rightX : 0f));
			
			// Still need to track camera behind player even if they aren't using the right stick; achieve this by saving distanceAwayFree every frame
			if (targetPosition == Vector3.zero) {
				targetPosition = characterOffset + followXform.up * distanceUpFree - savedRigToGoal * distanceAwayFree;
			}
			
			//				SmoothPosition(transform.position, targetPosition);
			//				transform.LookAt(lookAt);	
			break;
		}
		
		
		//if (camState != CamStates.Free) {
		CompensateForWalls (characterOffset, ref targetPosition);
		SmoothPosition (parentRig.position, targetPosition);
		//transform.LookAt (lookAt);	eulerAngles
		transform.LookAt (lookAt);
		this.transform.localEulerAngles = new Vector3 (this.transform.localEulerAngles.x, this.transform.localEulerAngles.y, followXform.localEulerAngles.z);
		rightStickPrevFrame = new Vector2 (rightX, rightY);
	}

	private void SmoothPosition (Vector3 fromPos, Vector3 toPos)
	{		
		
		float damp = camState == CamStates.FirstPerson ? firstPersonDampTime : camSmoothDampTime;
		// Making a smooth transition between camera's current position and the position it wants to be in
		parentRig.position = Vector3.SmoothDamp (fromPos, toPos, ref velocityCamSmooth, damp);
	}
	
	private void CompensateForWalls (Vector3 fromObject, ref Vector3 toTarget)
	{
		Debug.DrawLine (fromObject, toTarget, Color.cyan);
		// Compensate for walls between camera
		RaycastHit wallHit = new RaycastHit ();		
		if (Physics.Linecast (fromObject, toTarget, out wallHit)) {
			Debug.DrawRay (wallHit.point, wallHit.normal, Color.red);
			toTarget = new Vector3 (wallHit.point.x, toTarget.y, wallHit.point.z);
		}
	}
	
	/// <summary>
	/// Reset local position of camera inside of parentRig and resets character's look IK.
	/// </summary>
	private void ResetCamera ()
	{
		lookWeight = Mathf.Lerp (lookWeight, 0.0f, Time.deltaTime * firstPersonLookSpeed);
		transform.localRotation = Quaternion.Lerp (transform.localRotation, Quaternion.identity, Time.deltaTime);
	}


}
