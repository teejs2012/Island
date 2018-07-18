using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

	public Rigidbody myRig;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 inputAccelero = WirelessInputController.DeviceData.AcceleroData;
		Vector3 force = Vector3.zero;
		force.x = inputAccelero.x;
		force.z = inputAccelero.y;

		myRig.AddForce(force, ForceMode.VelocityChange);
	}
}
