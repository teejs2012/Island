using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Openable : OnOffStatusObject
{
    LockableData lockableData;
    protected OpenableData openableData;

    public event System.Action CloseEvent = delegate { };
    public event System.Action OpenEvent = delegate { };

    protected override void Awake()
    {
        base.Awake();
        openableData = GetComponent<OpenableData>();
        lockableData = GetComponent<LockableData>();
    }

    //public override void SetOpenableDataStatus(bool isOpen)
    //{
    //    base.SetOpenableDataStatus(isOpen);
    //    openableData.IsOpen = isOpen;
    //}

    protected virtual void OnEnable()
    {
        //if(lockableData != null && !lockableData.IsLocked)
        //{
        //    RevealContent();
        //}
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

    void OnDisable()
    {
        LeanTween.cancelAll();
        resetIsChangeStatus();
    }

    protected  abstract void SetOpen();
    protected abstract void SetClose();



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
        if (lockableData == null || lockableData.TargetLock == null) return;
        if (LeanTween.isTweening(lockableData.TargetLock)) return;
        LeanTween.rotateAround(lockableData.TargetLock, Vector3.up, 10, 0.5f).setEaseShake();
    }


    #region Dragging Code
    public bool IsDragging { get { return isDragging; } }
    protected bool isDragging = false;
    Camera currentCam;
    public void StartDrag(Camera cam)
    {
        if (lockableData != null && lockableData.IsLocked)
        {
            ShakeTargetLock();
            return;
        }

        if (isChangingStatus)
        {
            return;
        }

        isDragging = true;
        currentCam = cam;
        currentMouseX = UniformInput.Instance.GetPressPosition().x;
        currentMouseY = UniformInput.Instance.GetPressPosition().y;
    }

    float currentMouseX = 0;
    float currentMouseY = 0;

    void Update()
    {
        DoDraggingControl();
    }

    void DoDraggingControl()
    {
        if (isDragging)
        {
            if (UniformInput.Instance.GetPressUp())
            {
                EndDrag();
                return;
            }

            float deltaX = UniformInput.Instance.GetPressPosition().x - currentMouseX;
            float deltaY = UniformInput.Instance.GetPressPosition().y - currentMouseY;

            currentMouseX = UniformInput.Instance.GetPressPosition().x;
            currentMouseY = UniformInput.Instance.GetPressPosition().y;

            Vector3 movementInWorld = currentCam.transform.TransformVector(new Vector3(deltaX, deltaY, 0));
            DoDraggingMovement(movementInWorld);
        }
    }

    void EndDrag()
    {
        isDragging = false;
        ChangeStatus();
    }

    protected abstract void DoDraggingMovement(Vector3 dir);

    protected float GetDragValue(Vector3 v)
    {
        float result = 0;
        switch (openableData.DragAxis)
        {
            case Axis.x:
                result = v.x;
                break;
            case Axis.y:
                result = v.y;
                break;
            case Axis.z:
                result = v.z;
                break;
        }

        result *= (openableData.InvertDragEffect ? -1 : 1) * openableData.DragEffectSpeed;
        return result;
    }
#endregion

#region Animation Code
    bool isChangingStatus = false;
    void ChangeStatus()
    {
        isChangingStatus = true;
        if (TendingToOpen())
        {
            Open();
        }
        else
        {
            Close();
        }
        //RegisterStatus(openableData.IsOpen);
    }

    protected abstract bool TendingToOpen();

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

#endregion
}
