using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARVRSwitchData : MonoBehaviour {
    public GameObject TargetVRScene;
    [HideInInspector]
    public Vector3 TargetPosition { get { return VRAnchor.position; } }
    [HideInInspector]
    public Vector3 TargetRotation { get { return VRAnchor.eulerAngles; } }
    public Transform VRAnchor;
}
