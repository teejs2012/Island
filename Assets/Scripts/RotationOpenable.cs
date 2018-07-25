using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(OpenableData))]
public class RotationOpenable : Openable {

    protected override void Awake()
    {
        base.Awake();
        Assert.IsNotNull(openableData);   
    }

    void SetRotation(float rotValue)
    {
        switch (openableData.Axis)
        {
            case Axis.x:
                transform.localEulerAngles = transform.localEulerAngles.SetX(rotValue);
                break;
            case Axis.y:
                transform.localEulerAngles = transform.localEulerAngles.SetY(rotValue);
                break;
            case Axis.z:
                transform.localEulerAngles = transform.localEulerAngles.SetZ(rotValue);
                break;
        }
    }
    
    void DoAnimationRotation(float rotValue, System.Action callback)
    {
        switch (openableData.Axis)
        {
            case Axis.x:
                LeanTween.rotateLocal(gameObject, transform.localEulerAngles.SetX(rotValue), 1).setOnComplete(callback);
                break;
            case Axis.y:
                LeanTween.rotateLocal(gameObject, transform.localEulerAngles.SetY(rotValue), 1).setOnComplete(callback);
                break;
            case Axis.z:
                LeanTween.rotateLocal(gameObject, transform.localEulerAngles.SetZ(rotValue), 1).setOnComplete(callback);
                break;
        }
    }

    protected override void SetOpen()
    {
        SetRotation(openableData.OpenRot);
    }

    protected override void SetClose()
    {
        SetRotation(openableData.CloseRot);
    }

    protected override void DoCloseAnimation(Action callback)
    {
        DoAnimationRotation(openableData.CloseRot, callback);
    }

    protected override void DoOpenAnimation(Action callback)
    {
        DoAnimationRotation(openableData.OpenRot, callback);
    }
}
