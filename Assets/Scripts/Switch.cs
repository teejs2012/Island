using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class Switch : MonoBehaviour {

    [SerializeField]
    Material targetMat;
    [SerializeField]
    GameObject switchButton;
    [SerializeField]
    float onRot;
    [SerializeField]
    float offRot;
    [SerializeField]
    bool isOn;


    public void ChangeStatus()
    {
        if (isOn)
        {
            SwitchOff();
        }
        else
        {
            SwitchOn();
        }
    }

    void SwitchOn()
    {
        targetMat.EnableKeyword("_EMISSION");
        LeanTween.rotateY(switchButton, onRot, 0.2f);
        isOn = true;
    }
    void SwitchOff()
    {
        targetMat.DisableKeyword("_EMISSION");
        LeanTween.rotateY(switchButton, offRot, 0.2f);
        isOn = false;
    }
}
