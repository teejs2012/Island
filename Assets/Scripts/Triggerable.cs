using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Triggerable : MonoBehaviour {

    protected bool isTriggered;
    public bool IsTriggered { get { return isTriggered; } }
    public abstract void Trigger();
}
