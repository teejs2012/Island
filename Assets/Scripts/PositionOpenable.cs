using System;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(OpenableData))]
public class PositionOpenable : Openable {


    protected override void Awake()
    {
        base.Awake();
        Assert.IsNotNull(openableData);
    }

    void SetPosition(float rotValue)
    {
        switch (openableData.Axis)
        {
            case Axis.x:
                transform.localPosition = transform.localPosition.SetX(rotValue);
                break;
            case Axis.y:
                transform.localPosition = transform.localPosition.SetY(rotValue);
                break;
            case Axis.z:
                transform.localPosition = transform.localPosition.SetZ(rotValue);
                break;
        }
    }

    void DoAnimationPosition(float posValue, System.Action callback)
    {
        switch (openableData.Axis)
        {
            case Axis.x:
                LeanTween.moveLocal(gameObject, transform.localPosition.SetX(posValue), 0.2f).setEaseOutBack().setOnComplete(callback);
                break;
            case Axis.y:
                LeanTween.moveLocal(gameObject, transform.localPosition.SetY(posValue), 0.2f).setEaseOutBack().setOnComplete(callback);
                break;
            case Axis.z:
                LeanTween.moveLocal(gameObject, transform.localPosition.SetZ(posValue), 0.2f).setEaseOutBack().setOnComplete(callback);
                break;
        }
    }

    protected override void SetOpen()
    {
        SetPosition(openableData.OpenValue);
    }

    protected override void SetClose()
    {
        SetPosition(openableData.CloseValue);
    }

    protected override void DoCloseAnimation(Action callback)
    {
        DoAnimationPosition(openableData.CloseValue, callback);
    }

    protected override void DoOpenAnimation(Action callback)
    {
        DoAnimationPosition(openableData.OpenValue, callback);
    }

    protected override void DoDraggingMovement(Vector3 dir)
    {
        //transform.parent is a container for the rotationOpenable
        Vector3 movementInLocal = transform.parent.InverseTransformVector(dir).normalized;
        float dragValue = GetDragValue(movementInLocal);
        //Debug.Log("drag value is : " + dragValue);
        Vector3 targetPosition = transform.localPosition;
        switch (openableData.Axis)
        {
            case Axis.x:
                targetPosition -= new Vector3(dragValue, 0, 0);
                if (Utility.InRange(targetPosition.x, openableData.OpenValue, openableData.CloseValue))
                {
                    transform.localPosition = targetPosition;
                }
                break;
            case Axis.y:
                targetPosition -= new Vector3(0, dragValue, 0);
                if (Utility.InRange(targetPosition.y, openableData.OpenValue, openableData.CloseValue))
                {
                    transform.localPosition = targetPosition;
                }
                break;
            case Axis.z:
                targetPosition -= new Vector3(0, 0, dragValue);
                if (Utility.InRange(targetPosition.z, openableData.OpenValue, openableData.CloseValue))
                {
                    transform.localPosition = targetPosition;
                }
                break;
        }
    }

    protected override bool TendingToOpen()
    {
        float target = 0;
        switch (openableData.Axis)
        {
            case Axis.x:
                target = transform.localPosition.x;
                break;
            case Axis.y:
                target = transform.localPosition.y;
                break;
            case Axis.z:
                target = transform.localPosition.z;
                break;
        }
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
