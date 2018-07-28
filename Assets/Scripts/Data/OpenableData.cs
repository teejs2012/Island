using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenableData : MonoBehaviour {

    [SerializeField]
    protected bool isOpen;

    [SerializeField]
    float closeValue;
    [SerializeField]
    float openValue;
    [SerializeField]
    Axis axis;

    [Header("Drag related")]
    [SerializeField]
    Axis dragAxis;
    [SerializeField]
    bool invertDragEffect;
    [SerializeField]
    float dragEffectSpeed = 1;

    public bool IsOpen { get { return isOpen; } set { isOpen = value; } }
    public float CloseValue { get { return closeValue; } }
    public float OpenValue { get { return openValue; } }
    public Axis Axis { get { return axis; } }
    public Axis DragAxis { get { return dragAxis; } }
    public bool InvertDragEffect { get { return invertDragEffect; } }
    public float DragEffectSpeed { get { return dragEffectSpeed; } }
}
