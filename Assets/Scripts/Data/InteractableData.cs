using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableData : MonoBehaviour {

    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;

    public bool limitUpDownRotation = false;
    public bool limitLeftRightRotation = false;
    public bool limitZoom = false;
}
