using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampSwitch : Switch {

    [SerializeField]
    Material targetMat;
    [SerializeField]
    GameObject switchButton;
    [SerializeField]
    float switchRotAmount;
    //[SerializeField]
    //float offRot;

    override protected void SwitchOn()
    {
        targetMat.EnableKeyword("_EMISSION");
        LeanTween.rotateAroundLocal(switchButton, Vector3.up, switchRotAmount, 0.2f);
        isOn = true;
        isDoingSwitchAnimation = false;
    }

    override protected void SwitchOff()
    {
        targetMat.DisableKeyword("_EMISSION");
        LeanTween.rotateAroundLocal(switchButton, Vector3.up, -switchRotAmount, 0.2f);
        isOn = false;
        isDoingSwitchAnimation = false;
    }
}
