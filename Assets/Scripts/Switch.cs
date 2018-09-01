using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public abstract class Switch : OnOffStatusObject {
    [SerializeField]
    protected bool isOn;
    [SerializeField]
    protected float buttonSpeed;
    [SerializeField]
    protected float onValue;
    [SerializeField]
    protected float offValue;

    protected bool isDoingSwitchAnimation = false;

    protected override void Awake()
    {
        base.Awake();
    }

    void OnEnable()
    {
        if (isOn)
        {
            SetOn();
        }
        else
        {
            SetOff();
        }
    }

    void OnDisable()
    {
        StopAllCoroutines();
        LeanTween.cancel(gameObject);
        isDoingSwitchAnimation = false;
    }

    protected abstract void SetOn();
    protected abstract void SetOff();

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
        //RegisterStatus(isOn);
    }

    //public override void SetOpenableDataStatus(bool isOn)
    //{
    //    base.SetOpenableDataStatus(isOn);
    //    this.isOn = isOn;
    //}

    virtual protected void SwitchOn() { }
    virtual protected void SwitchOff() { }
}
