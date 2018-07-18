using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class FanSwitch : Switch {

    [SerializeField]
    GameObject targetFan;
    [SerializeField]
    GameObject targetSwitch;
    [SerializeField]
    float switchRotAmount;
    [SerializeField]
    float buttonSpeed;

    [SerializeField]
    float maxFanSpeed;
    [SerializeField]
    float fanAcceleration;

    float currentFanSpeed = 0;


    protected override void SwitchOn()
    {
        isOn = true;
        LeanTween.rotateAroundLocal(targetSwitch, Vector3.up, switchRotAmount, buttonSpeed);
        LeanTween.value(0, maxFanSpeed, fanAcceleration).setOnUpdate((float x) => { currentFanSpeed = x; }).setOnComplete(()=> { isDoingSwitchAnimation = false; });
        StartCoroutine(SpinFan());
    }

    protected override void SwitchOff()
    {
        
        LeanTween.rotateAroundLocal(targetSwitch, Vector3.up, -switchRotAmount, buttonSpeed);
        LeanTween.value(maxFanSpeed, 0, fanAcceleration).setOnUpdate((float x) => { currentFanSpeed = x; }).setOnComplete(()=> { isOn = false; isDoingSwitchAnimation = false; });
    }

    IEnumerator SpinFan()
    {
        while (isOn)
        {
            targetFan.transform.RotateAroundLocal(Vector3.forward, currentFanSpeed);
            yield return null;
        }
    }
}
