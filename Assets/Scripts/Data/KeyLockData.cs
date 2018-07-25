using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KeyLock))]
public class KeyLockData : MonoBehaviour {

    void Start()
    {
        tag = Tags.KeyLock;
    }

    [SerializeField]
    ColorKey keyColor;
    public ColorKey KeyColor { get { return keyColor; } }

    [SerializeField]
    Openable targetOpenable;
    public Openable TargetOpenable { get { return targetOpenable; } }
}
