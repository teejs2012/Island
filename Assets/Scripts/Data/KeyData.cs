using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyData : OneTimeTrigger {

    void Start()
    {
        tag = Tags.Key;
    }

    protected override void Trigger()
    {
        base.Trigger();
        Destroy(this.gameObject);
    }

    public void Register()
    {
        RegisterStatus();
    }

    [SerializeField]
    ColorKey keyColor;
    public ColorKey KeyColor { get { return keyColor; } }
}
