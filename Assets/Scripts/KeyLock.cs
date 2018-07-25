﻿using UnityEngine;

[RequireComponent(typeof(KeyLockData))]
public class KeyLock : OneTimeTrigger {

    KeyLockData data;

    public ColorKey KeyColor
    {
        get { return data.KeyColor; }
    }
	
	protected override void Awake () {
        data = GetComponent<KeyLockData>();
        base.Awake();
	}
	
	public void Unlock()
    {
        data.TargetOpenable.Unlock();
        RegisterStatus();

        var dissolvable = GetComponent<Dissolvable>();
        if(dissolvable != null)
        {
            dissolvable.Dissolve(1, () => { Destroy(gameObject); });
        }
        else
        {
            Destroy(gameObject);
        }
    }

    bool isShaking = false;
    public void Shake()
    {
        if (isShaking) return;
        isShaking = true;
        LeanTween.rotateAround(gameObject, Vector3.up, 10, 0.5f).setEaseShake().setOnComplete(() => { isShaking = false; });
    }

    public override void Trigger()
    {
        Unlock();
    }
}
