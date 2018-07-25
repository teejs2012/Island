using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Openable : MonoBehaviour {
    LockableData lockableData;
    protected OpenableData openableData;
    bool isShaking = false;

    public event System.Action CloseEvent = delegate { };
    public event System.Action OpenEvent = delegate { };

    protected virtual void Awake()
    {
        openableData = GetComponent<OpenableData>();
        lockableData = GetComponent<LockableData>();
        name += Utility.CreateMD5(transform.position.ToString());
    }

    protected virtual void OnEnable()
    {
        if(lockableData != null && !lockableData.IsLocked)
        {
            RevealContent();
        }
        if (openableData.IsOpen)
        {
            SetOpen();
            OpenEvent();
        }
        else
        {
            SetClose();
            CloseEvent();
        }
    }

    protected  abstract void SetOpen();
    protected abstract void SetClose();

    void RegisterStatus()
    {
        StatusManager.Instance.RegisterAsOpenable(name, openableData.IsOpen);
    }

    public void SetOpenableDataStatus(bool isOpen)
    {
        openableData.IsOpen = isOpen;
        RegisterStatus();
    }

    void RevealContent()
    {
        foreach (var content in lockableData.ContentToActivate)
        {
            content.SetActive(true);
        }
    }

    public void Unlock()
    {
        if (lockableData != null)
        {
            lockableData.IsLocked = false;
            RevealContent();
        }
    }

    protected bool IsLocked()
    {
        if(lockableData != null && lockableData.IsLocked)
        {
            return true;
        }
        return false;
    }

    void ShakeTargetLock()
    {
        if (isShaking || lockableData == null || lockableData.TargetLock == null) return;
        isShaking = true;
        LeanTween.rotateAround(lockableData.TargetLock, Vector3.up, 10, 0.5f).setEaseShake().setOnComplete(() => { isShaking = false; });
    }

    bool isChangingStatus = false;
    public void ChangeStatus()
    {
        if(lockableData != null && lockableData.IsLocked)
        {
            ShakeTargetLock();
            return;
        }

        if (isChangingStatus)
        {
            return;
        }

        isChangingStatus = true;
        if (openableData.IsOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
        RegisterStatus();
    }

    void Close()
    {
        openableData.IsOpen = false;
        DoCloseAnimation(resetIsChangeStatus);
        CloseEvent();
    }

    void Open()
    {
        openableData.IsOpen = true;
        DoOpenAnimation(resetIsChangeStatus);
        OpenEvent();
    }
    
    void resetIsChangeStatus()
    {
        isChangingStatus = false;
    }

    protected abstract void DoCloseAnimation(System.Action callback);
    protected abstract void DoOpenAnimation(System.Action callback);
}
