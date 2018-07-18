using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class Switch : MonoBehaviour {


    [SerializeField]
    protected bool isOn;

    protected bool isDoingSwitchAnimation = false;

    public void ChangeStatus()
    {
        if (isDoingSwitchAnimation)
        {
            return;
        }
        isDoingSwitchAnimation = true;
        if (isOn)
        {
            SwitchOff();
        }
        else
        {
            SwitchOn();
        }
    }

    virtual protected void SwitchOn() { }
    virtual protected void SwitchOff() { }



}
