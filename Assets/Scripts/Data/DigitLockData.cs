using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

[RequireComponent(typeof(DigitLock))]
public class DigitLockData : MonoBehaviour {

    [SerializeField]
    string targetNumber;
    public string TargetNumber { get { return targetNumber; } }

    [SerializeField]
    Openable targetOpenable;
    public Openable TargetOpenable { get { return targetOpenable; } }
}
