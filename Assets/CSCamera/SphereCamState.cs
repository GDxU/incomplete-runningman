using UnityEngine;
using System.Collections;

public class SphereCamState : MonoBehaviour
{
	#region Variables (private)
	[SerializeField]
	private SphereChaseCam
		gamecam;
	[SerializeField]
	private float
		rotationDegreePerSecond = 120f;
	[SerializeField]
	private float
		directionSpeed = 1.5f;
	[SerializeField]
	private float
		directionDampTime = 0.25f;
	[SerializeField]
	private float
		speedDampTime = 0.05f;
	[SerializeField]
	private float
		fovDampTime = 3f;
	[SerializeField]
	private float
		jumpMultiplier = 1f;
	[SerializeField]
	private CapsuleCollider
		capCollider;
	[SerializeField]
	private float
		jumpDist = 1f;
	[SerializeField]
	private bool
		sprint = true;
	private AnimatorStateInfo stateInfo;
	private AnimatorTransitionInfo transInfo;
	private float 
		leftX = 0f, 
		leftY = 0f,
		speed = 0f, 
		direction = 0f, 
		charAngle = 0f,
		capsuleHeight;
	private const float 
		SPRINT_SPEED = 2.0f, 
		SPRINT_FOV = 75.0f, 
		NORMAL_FOV = 60.0f;

	private normalwalk2 walkernormal;
	#endregion
	#region Properties (public)
	public float Speed { get { return this.speed; } }
	public Vector3 getNormalCharacterFromGround(){ return walkernormal.getNormal();}
	public SphereChaseCam SphereCam { set { this.gamecam = value; } }

	public bool IsInJump ()
	{
		return walkernormal.IsJump();
	}

	public bool IsInIdleJump ()
	{
		return false;
	}

	public bool IsInLocomotionJump ()
	{
		return false;
	}

	public bool IsInLocomotion ()
	{
		return false;
	}

	public bool IsInPivot ()
	{
		return false;
	}

	public void StickToWorldspace (Transform root, Transform camera, ref float directionOut, ref float speedOut, ref float angleOut, bool isPivoting)
	{
		Vector3 rootDirection = root.forward;
		
		Vector3 stickDirection = new Vector3 (leftX, 0, leftY);
		
		speedOut = stickDirection.sqrMagnitude;		
		
		// Get camera rotation
		Vector3 CameraDirection = camera.forward;
		CameraDirection.y = 0.0f; // kill Y
		Quaternion referentialShift = Quaternion.FromToRotation (Vector3.forward, Vector3.Normalize (CameraDirection));
		
		// Convert joystick input in Worldspace coordinates
		Vector3 moveDirection = referentialShift * stickDirection;
		Vector3 axisSign = Vector3.Cross (moveDirection, rootDirection);
		
		Debug.DrawRay (new Vector3 (root.position.x, root.position.y + 2f, root.position.z), moveDirection, Color.green);
		//camera target position
		Debug.DrawRay (new Vector3 (camera.position.x, camera.position.y, camera.position.z), rootDirection, Color.magenta);
		Debug.DrawRay (new Vector3 (root.position.x, root.position.y + 2f, root.position.z), stickDirection, Color.blue);
		Debug.DrawRay (new Vector3 (root.position.x, root.position.y + 2.5f, root.position.z), axisSign, Color.red);
		
		float angleRootToMove = Vector3.Angle (rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);
		if (!isPivoting) {
			angleOut = angleRootToMove;
		}
		angleRootToMove /= 180f;
		
		directionOut = angleRootToMove * directionSpeed;
	}
	#endregion
	// Use this for initialization
	void Start ()
	{
		walkernormal = GetComponent<normalwalk2>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (gamecam == null)
			return;



		if (gamecam.CamState != SphereChaseCam.CamStates.FirstPerson) {
			charAngle = 0f;
			direction = 0f;	
			float charSpeed = 0f;

			// Translate controls stick coordinates into world/cam/character space
			StickToWorldspace (this.transform, gamecam.transform, ref direction, ref charSpeed, ref charAngle, IsInPivot ());		
			


			//if (Input.GetButton ("Sprint")) {
			if (sprint) {
				speed = Mathf.Lerp (speed, SPRINT_SPEED, Time.deltaTime);
				gamecam.GetComponent<Camera> ().fieldOfView = Mathf.Lerp (gamecam.GetComponent<Camera> ().fieldOfView, SPRINT_FOV, fovDampTime * Time.deltaTime);
			} else {
				speed = charSpeed;
				gamecam.GetComponent<Camera> ().fieldOfView = Mathf.Lerp (gamecam.GetComponent<Camera> ().fieldOfView, NORMAL_FOV, fovDampTime * Time.deltaTime);		
			}
		}
	}
}
