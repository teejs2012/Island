using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 rotation = WirelessInputController.DeviceData.GyroData;
		transform.rotation = Quaternion.Euler(rotation);
	}
}
