using UnityEngine;
using System.Collections;

public class DeviceRotation : MonoBehaviour
{
	public void Start ()
	{
		//check if the gyro is enabled
		#if UNITY_IPHONE
		if(Input.gyro.enabled){
			enabledGyro = true;
			//debug
			//ToDebug("Gyro Enabled");
		} else {
			//TODO: show a warning message
		}
		#endif
		
		#if UNITY_ANDROID
		//TODO : Android Gyro Version
		#endif
		
		#if UNITY_WINDOWS_PHONE
		//TODO : Windows Phone Gyro Version
		#endif
		
		#if UNITY_EDITOR 
		Debug.Log("No gyro in the Unity Editor");
		#endif
	}

	private static bool gyroInitialized = false;
	
	public static bool HasGyroscope {
		get {
			return SystemInfo.supportsGyroscope;
		}
	}
	
	public static Quaternion Get ()
	{
		if (!gyroInitialized) {
			InitGyro ();
		}
		
		return HasGyroscope
			? ReadGyroscopeRotation ()
				: Quaternion.identity;
	}
	
	private static void InitGyro ()
	{
		if (HasGyroscope) {
			Input.gyro.enabled = true;                // enable the gyroscope
			Input.gyro.updateInterval = 0.0167f;    // set the update interval to it's highest value (60 Hz)
		}
		gyroInitialized = true;
	}
	
	private static Quaternion ReadGyroscopeRotation ()
	{
		return new Quaternion (0.5f, 0.5f, -0.5f, 0.5f) * Input.gyro.attitude * new Quaternion (0, 0, 1, 0);
	}

	private float x, y;
	private float turnSpeed = 0.4f;
	public void Update ()
	{
		//get values from the gyroscope
		x = Input.gyro.rotationRate.x;
		y = Input.gyro.rotationRate.y;
	}
	private float gyroSensitivity=0.4f;
	//rotate the camera rigt and left (y rotation)
	public void RotateRightLeft (float axis)
	{
		transform.RotateAround (transform.position, Vector3.up, -axis * Time.deltaTime * gyroSensitivity);
	}


	public void Roate2(){
		transform.Rotate (0, gyroSensitivity * turnSpeed * Time.deltaTime, 0);
	}
}
