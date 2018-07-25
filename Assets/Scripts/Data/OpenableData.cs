using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenableData : MonoBehaviour {

    [SerializeField]
    protected bool isOpen;
    [SerializeField]
    float closeRot;
    [SerializeField]
    float openRot;
    [SerializeField]
    Axis axis;

    public bool IsOpen { get { return isOpen; } set { isOpen = value; } }
    public float CloseRot { get { return closeRot; } }
    public float OpenRot { get { return openRot; } }
    public Axis Axis { get { return axis; } }
}
