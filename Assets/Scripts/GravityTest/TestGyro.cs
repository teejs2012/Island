using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestGyro : MonoBehaviour {

    Quaternion rot = new Quaternion(0, 0, 1, 0);
    public GameObject camera;
    void Update()
    {
        camera.transform.localRotation = Quaternion.Euler(WirelessInputController.DeviceData.GyroData) * rot;
    }
}
