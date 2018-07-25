using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DigitLockData))]
public class DigitLock : OneTimeTrigger {

    DigitLockData data;
    public string TargetNumber { get { return data.TargetNumber; } }

	protected override void Awake()
    {
        data = GetComponent<DigitLockData>();
        base.Awake();
    }

    public void Unlock()
    {
        data.TargetOpenable.Unlock();
        RegisterStatus();

        var dissolvable = GetComponent<Dissolvable>();
        if (dissolvable != null)
        {
            dissolvable.Dissolve(1, () => { Destroy(gameObject); });
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void Trigger()
    {
        Unlock();
    }
}
