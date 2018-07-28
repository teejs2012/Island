using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampSwitch : Switch {

    [SerializeField]
    Material targetMat;
    [SerializeField]
    GameObject lampLightSource;

    override protected void SwitchOn()
    {
        isOn = true;
        LeanTween.rotateLocal(gameObject, transform.localEulerAngles.SetY(onValue), buttonSpeed);
        targetMat.EnableKeyword("_EMISSION");
        lampLightSource.SetActive(true);
        isDoingSwitchAnimation = false;
    }

    override protected void SwitchOff()
    {
        isOn = false;
        LeanTween.rotateLocal(gameObject, transform.localEulerAngles.SetY(offValue), buttonSpeed);
        targetMat.DisableKeyword("_EMISSION");
        lampLightSource.SetActive(false);
        isDoingSwitchAnimation = false;
    }

    protected override void SetOn()
    {
        transform.localEulerAngles = transform.localEulerAngles.SetY(onValue);
        targetMat.EnableKeyword("_EMISSION");
        lampLightSource.SetActive(true);
    }

    protected override void SetOff()
    {
        transform.localEulerAngles = transform.localEulerAngles.SetY(offValue);
        targetMat.DisableKeyword("_EMISSION");
        lampLightSource.SetActive(false);
    }
}
