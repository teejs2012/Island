using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CustomGravity : MonoBehaviour {
	
	public float gravityForce = 380;
    bool gyroEnabled = false;
    Gyroscope gyro;
    //[SerializeField]
    //Transform anchor;
    [SerializeField]
    Transform imageTarget;
    [SerializeField]
    Transform vuforiaCam;

    [Header("UI")]
    public Text arCamX;
    public Text arCamY;
    public Text arCamZ;

    public Text imageTargetX;
    public Text imageTargetY;
    public Text imageTargetZ;

    //public Text deviceX;
    //public Text deviceY;
    //public Text deviceZ;
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

        //if (SystemInfo.supportsGyroscope)
        //{
        //    gyro = Input.gyro;
        //    gyro.enabled = true;
        //    gyroEnabled = true;
        //}
	}
	
	void Update () {
        //if (!gyroEnabled)
        //{
        //    return;
        //}

        
        //anchor.transform.rotation = gyro.attitude;
        //deviceX.text = anchor.transform.eulerAngles.x.ToString();
        //deviceY.text = anchor.transform.eulerAngles.y.ToString();
        //deviceZ.text = anchor.transform.eulerAngles.z.ToString();

        arCamX.text = vuforiaCam.eulerAngles.x.ToString();
        arCamY.text = vuforiaCam.eulerAngles.y.ToString();
        arCamZ.text = vuforiaCam.eulerAngles.z.ToString();

        imageTargetX.text = imageTarget.eulerAngles.x.ToString();
        imageTargetY.text = imageTarget.eulerAngles.y.ToString();
        imageTargetZ.text = imageTarget.eulerAngles.z.ToString();


        //      Vector3 localDownVector = anchor.InverseTransformDirection(Vector3.down);
        //      Vector3 gravityDir = vuforiaCam.TransformDirection(localDownVector);
        //      //CameraDevice.Instance.StopAutoFocus();
        //      /*if (Screen.orientation == ScreenOrientation.LandscapeLeft) {
        //	Physics.gravity = new Vector3 (Input.acceleration.y * grav , -Input.acceleration.x * grav , -Input.acceleration.z * grav);
        //} else if (Screen.orientation == ScreenOrientation.LandscapeRight ) {
        //	Physics.gravity = new Vector3 (-Input.acceleration.y * grav , Input.acceleration.x * grav , -Input.acceleration.z * grav);
        //} else if (Screen.orientation == ScreenOrientation.Portrait ) {*/
        //      Physics.gravity = gravityDir * gravityForce;
        //      //}
    }
}
