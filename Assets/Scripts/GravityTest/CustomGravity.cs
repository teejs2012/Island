using UnityEngine;
using System.Collections;

public class CustomGravity : MonoBehaviour {
	
	public float gravityForce = 380;
    bool gyroEnabled = false;
    Gyroscope gyro;
    [SerializeField]
    Transform anchor;
    [SerializeField]
    Transform vuforiaCam;
	// Use this for initialization
	void Start () {
        //CameraDevice.Instance.StopAutoFocus();
        //if (Screen.orientation == ScreenOrientation.LandscapeLeft) {
        //	Debug.Log ("LEFT");

        //} else if (Screen.orientation == ScreenOrientation.LandscapeRight ) {
        //	Debug.Log ("RIGHT");
        //} else if (Screen.orientation == ScreenOrientation.Portrait ) {
        //	Debug.Log ("PORTRAIT");
        //}

        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            gyroEnabled = true;
        }
	}
	
	void FixedUpdate () {
        if (!gyroEnabled)
        {
            return;
        }

        anchor.transform.rotation = gyro.attitude;
        Vector3 localDownVector = anchor.InverseTransformDirection(Vector3.down);
        Vector3 gravityDir = vuforiaCam.TransformDirection(localDownVector);
        //CameraDevice.Instance.StopAutoFocus();
        /*if (Screen.orientation == ScreenOrientation.LandscapeLeft) {
			Physics.gravity = new Vector3 (Input.acceleration.y * grav , -Input.acceleration.x * grav , -Input.acceleration.z * grav);
		} else if (Screen.orientation == ScreenOrientation.LandscapeRight ) {
			Physics.gravity = new Vector3 (-Input.acceleration.y * grav , Input.acceleration.x * grav , -Input.acceleration.z * grav);
		} else if (Screen.orientation == ScreenOrientation.Portrait ) {*/
        Physics.gravity = gravityDir * gravityForce;
        //}



    }
}
