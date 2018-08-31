using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffStatusObject : MonoBehaviour {

    protected virtual void Awake()
    {
        AppendMD5HashToName();
    }

    void AppendMD5HashToName()
    {
        name = name + Utility.CreateMD5(transform.position.ToString() + transform.eulerAngles.ToString());
    }

    //protected void RegisterStatus(bool isOn)
    //{
    //    StatusManager.Instance.RegisterAsOnOffStatusObject(name, isOn);
    //}

    //public virtual void SetOpenableDataStatus(bool isOpen)
    //{
    //    RegisterStatus(isOpen);
    //}
}
