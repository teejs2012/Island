using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class FanSwitch : Switch {

    [SerializeField]
    GameObject targetFan;
    [SerializeField]
    float maxFanSpeed;
    [SerializeField]
    float fanAcceleration;

    float currentFanSpeed = 0;


    protected override void SwitchOn()
    {
        isOn = true;
        LeanTween.rotateLocal(gameObject,transform.localEulerAngles.SetY(onValue), buttonSpeed);
        LeanTween.value(0, maxFanSpeed, fanAcceleration).setOnUpdate((float x) => { currentFanSpeed = x; }).setOnComplete(()=> { isDoingSwitchAnimation = false; });
        StartCoroutine(SpinFan());
    }

    protected override void SwitchOff()
    {
        isOn = false;
        LeanTween.rotateLocal(gameObject,transform.localEulerAngles.SetY(offValue), buttonSpeed);
        LeanTween.value(maxFanSpeed, 0, fanAcceleration).setOnUpdate((float x) => { currentFanSpeed = x; }).setOnComplete(()=> {isDoingSwitchAnimation = false; StopAllCoroutines(); });
    }

    IEnumerator SpinFan()
    {
        while (true)
        {
            targetFan.transform.Rotate(Vector3.forward, currentFanSpeed);
            yield return null;
        }
    }

    protected override void SetOn()
    {
        transform.localEulerAngles = transform.localEulerAngles.SetY(onValue);
        currentFanSpeed = maxFanSpeed;
        StartCoroutine(SpinFan());
    }

    protected override void SetOff()
    {
        transform.localEulerAngles = transform.localEulerAngles.SetY(offValue);
        currentFanSpeed = 0;
    }
}
