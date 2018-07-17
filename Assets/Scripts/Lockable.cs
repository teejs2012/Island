using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class Lockable : Triggerable {

    [SerializeField]
    DigitLockData targetLock;
    [SerializeField]
    bool isLocked = true;

    [Header("Animation")]
    [SerializeField]
    GameObject Lid;
    [SerializeField]
    float openPos;
    [SerializeField]
    float closePos;
    [SerializeField]
    bool isOpen = false;

    bool isShaking = false;
    bool isChangingStatus = false;

    public bool IsLocked
    {
        get { return isLocked; }
    }

	public void Unlock()
    {
        isTriggered = true;
        isLocked = false;
        Destroy(targetLock.gameObject);
        Open();
    }

    public override void Trigger()
    {
        isTriggered = true;
        isLocked = false;
        Destroy(targetLock.gameObject);
    }

    public void ChangeStatus()
    {
        if (isChangingStatus)
        {
            return;
        }
        isChangingStatus = true;
        if (isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    public void ShakeTargetLock()
    {
        if (!isShaking)
        {
            isShaking = true;
            LeanTween.rotateAround(targetLock.gameObject, Vector3.up, 10, 0.5f).setEaseShake().setOnComplete(()=> { isShaking = false; });
        }
    }

    void Open()
    {
        LeanTween.rotateX(Lid, openPos, 1).setEaseInElastic().setEaseOutBounce().setOnComplete(()=> { isChangingStatus = false; });
        isOpen = true;
    }

    void Close()
    {
        LeanTween.rotateX(Lid, closePos, 1).setEaseInElastic().setEaseOutBounce().setOnComplete(() => { isChangingStatus = false; });
        isOpen = false;
    }
}
