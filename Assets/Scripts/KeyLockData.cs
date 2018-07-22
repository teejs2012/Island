using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyLockData : MonoBehaviour {

    [SerializeField]
    ColorKey keyColor;
    public ColorKey KeyColor { get { return keyColor; } }

    [SerializeField]
    Lockable targetLockable;
    public Lockable TargetLockable { get { return targetLockable; } }
}
