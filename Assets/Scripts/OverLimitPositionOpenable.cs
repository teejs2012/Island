using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverLimitPositionOpenable : PositionOpenable {

    bool pulledOff = false;

    protected override void DoDraggingMovement(Vector3 dir)
    {
        if (pulledOff) return;
        //transform.parent is a container for the rotationOpenable
        Vector3 movementInLocal = transform.InverseTransformVector(dir).normalized;
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
                else
                {
                    CheckIfOverOpenLimit(targetPosition.x, new Vector3(dragValue, 0, 0));
                }
                break;
            case Axis.y:
                targetPosition -= new Vector3(0, dragValue, 0);
                if (Utility.InRange(targetPosition.y, openableData.OpenValue, openableData.CloseValue))
                {
                    transform.localPosition = targetPosition;
                }
                else
                {
                    CheckIfOverOpenLimit(targetPosition.y, new Vector3(0,dragValue,0) );
                }
                break;
            case Axis.z:
                targetPosition -= new Vector3(0, 0, dragValue);
                if (Utility.InRange(targetPosition.z, openableData.OpenValue, openableData.CloseValue))
                {
                    transform.localPosition = targetPosition;
                }
                else
                {
                    CheckIfOverOpenLimit(targetPosition.z, new Vector3(0, 0,dragValue));
                }
                break;
        }
    }

    void CheckIfOverOpenLimit(float value, Vector3 pullDir)
    {
        //Debug.Log(value);
        if (Mathf.Abs(value - openableData.OpenValue) < Mathf.Abs(value - openableData.CloseValue))
        {
            DoShake();
            currentOverLimitAmount += 1;
            if(currentOverLimitAmount > targetOverLimitAmount)
            {
                DoPullOff(-pullDir);
            }
        }
    }


    bool shakeFlag = false;
    void DoShake()
    {
        if (shakeFlag)
        {
            shakeFlag = !shakeFlag;
            transform.Translate(Vector3.up * shakeAmount);
        }
        else
        {
            shakeFlag = !shakeFlag;
            transform.Translate(Vector3.up * -shakeAmount);
        }
    }

    [SerializeField]
    float shakeAmount;
    [SerializeField]
    int targetOverLimitAmount;
    int currentOverLimitAmount = 0;
    [SerializeField]
    float forceAmount;
    [SerializeField]
    Vector3 pulledOffPosition;

    void OnDisable()
    {
        if(!pulledOff && StatusManager.Instance.CheckTrigger(name))
        {
            pulledOff = true;
            Destroy(this.gameObject);
        }
    }

    void DoPullOff(Vector3 pullDir)
    {
        StatusManager.Instance.RegisterAsTriggeredObject(name);
        pulledOff = true;
        StopAllCoroutines();
        LeanTween.cancel(gameObject);
        var rbody = gameObject.AddComponent<Rigidbody>();
        rbody.AddForce(transform.TransformVector(pullDir.normalized) * forceAmount);
        tag = Tags.Untagged;
        Destroy(this);
    }
}
