using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class DigitLockData : MonoBehaviour {

    [SerializeField]
    string targetNumber;
    public string TargetNumber { get { return targetNumber; } }

    [SerializeField]
    Lockable targetLockable;
    public Lockable TargetLockable { get { return targetLockable; } }
}
