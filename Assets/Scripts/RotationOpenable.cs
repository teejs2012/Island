using System;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(OpenableData))]
public class RotationOpenable : Openable {

    protected override void Awake()
    {
        base.Awake();
        Assert.IsNotNull(openableData);   
    }
    protected override void SetOpen()
    {
        SetRotation(openableData.OpenValue);
    }
    protected override void SetClose()
    {
        SetRotation(openableData.CloseValue);
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
                LeanTween.rotateLocal(gameObject, transform.localEulerAngles.SetX(rotValue), 0.2f).setEaseOutBack().setOnComplete(callback);
                break;
            case Axis.y:
                LeanTween.rotateLocal(gameObject, transform.localEulerAngles.SetY(rotValue), 0.2f).setEaseOutBack().setOnComplete(callback);
                break;
            case Axis.z:
                LeanTween.rotateLocal(gameObject, transform.localEulerAngles.SetZ(rotValue), 0.2f).setEaseOutBack().setOnComplete(callback);
                break;
        }
    }

    protected override void DoCloseAnimation(Action callback)
    {
        DoAnimationRotation(openableData.CloseValue, callback);
    }

    protected override void DoOpenAnimation(Action callback)
    {
        DoAnimationRotation(openableData.OpenValue, callback);
    }

    protected override void DoDraggingMovement(Vector3 dir)
    {
        //transform.parent is a container for the rotationOpenable
        Vector3 movementInLocal = transform.parent.InverseTransformVector(dir).normalized;
        float dragValue = GetDragValue(movementInLocal);
        //Debug.Log("drag value is : " + dragValue);
        Vector3 targetEulerAngles = transform.localEulerAngles;
        switch (openableData.Axis)
        {
            case Axis.x:
                targetEulerAngles -= new Vector3(dragValue, 0, 0);
                if (Utility.InRange(ConverAngleRange(targetEulerAngles.x), openableData.OpenValue, openableData.CloseValue))
                {
                    transform.localEulerAngles = targetEulerAngles;
                }
                break;
            case Axis.y:
                targetEulerAngles -= new Vector3(0, dragValue, 0);
                if (Utility.InRange(ConverAngleRange(targetEulerAngles.y), openableData.OpenValue, openableData.CloseValue))
                {
                    transform.localEulerAngles = targetEulerAngles;
                }
                break;
            case Axis.z:
                targetEulerAngles -= new Vector3(0, 0, dragValue);
                if (Utility.InRange(ConverAngleRange(targetEulerAngles.z), openableData.OpenValue, openableData.CloseValue))
                {
                    transform.localEulerAngles = targetEulerAngles;
                }
                break;
        }
    }

    float ConverAngleRange(float angle)
    {
        if (angle > 180) return angle - 360;
        else return angle;
    }

    protected override bool TendingToOpen()
    {
        float target = 0;
        switch (openableData.Axis)
        {
            case Axis.x:
                target = transform.localEulerAngles.x;
                break;
            case Axis.y:
                target = transform.localEulerAngles.y;
                break;
            case Axis.z:
                target = transform.localEulerAngles.z;
                break;
        }
        target = ConverAngleRange(target);
        if (Mathf.Abs(target - openableData.OpenValue) < Mathf.Abs(target - openableData.CloseValue))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
