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

    void DoAnimationPosition(float rotValue, System.Action callback)
    {
        switch (openableData.Axis)
        {
            case Axis.x:
                LeanTween.moveLocal(gameObject, transform.localPosition.SetX(rotValue), 1).setOnComplete(callback);
                break;
            case Axis.y:
                LeanTween.moveLocal(gameObject, transform.localPosition.SetY(rotValue), 1).setOnComplete(callback);
                break;
            case Axis.z:
                LeanTween.moveLocal(gameObject, transform.localPosition.SetZ(rotValue), 1).setOnComplete(callback);
                break;
        }
    }

    protected override void SetOpen()
    {
        SetPosition(openableData.OpenRot);
    }

    protected override void SetClose()
    {
        SetPosition(openableData.CloseRot);
    }

    protected override void DoCloseAnimation(Action callback)
    {
        DoAnimationPosition(openableData.CloseRot, callback);
    }

    protected override void DoOpenAnimation(Action callback)
    {
        DoAnimationPosition(openableData.OpenRot, callback);
    }

}
