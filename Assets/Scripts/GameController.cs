﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class GameController : MonoBehaviour {
    State currentState;
    State CurrentState
    {
        get { return currentState; }
        set {
            currentState = value;
            if(BroadcastState != null)
            {
                BroadcastState(currentState);
            }
        }
    }

    public Camera ARViewCamera;
    public Camera VRViewCamera;
    public Camera InteractableViewCamera;
    public Camera DigitLockViewCamera;

    Camera currentCamera;

    [SerializeField]
    UIManager UIManager;
    [SerializeField]
    DigitLockSystem DigitLockSystem;
    [SerializeField]
    KeyLockSystem KeyLockSystem;
    [SerializeField]
    TextHandler TextHandler;

    //For Interactable
    GameObject currentGO;
    public GameObject CurrentGO
    {
        get { return currentGO; }
    }
    Vector3 originalGOPosition;
    Quaternion originalGORotation;
    Vector3 originalGOScale;
    string originalTag;

    public delegate void ReturnState(State state);
    public event ReturnState BroadcastState;


    void Start()
    {
        currentCamera = ARViewCamera;
        CurrentState = State.ARView;
    }

    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            if(CurrentState == State.ARView)
            {
                CheckHit(ARViewCamera);
            }
            else if(CurrentState == State.VRView)
            {
                CheckHit(VRViewCamera);
            }
            else if(CurrentState == State.InteractableView)
            {
                CheckHit(InteractableViewCamera);
            }
        }	
	}

    void CheckHit(Camera cam)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                switch (hit.collider.tag)
                {
                    case Tags.InteractableTag:
                        SwitchToInteractableView(hit.collider);
                        break;
                    case Tags.BookTag:
                        SwitchToBookView(hit.collider);
                        break;
                    case Tags.ARVRSwitch:
                        SwitchARVRView(hit.collider);
                        break;
                    case Tags.Breakable:
                        TryBreak(hit.collider, ray.direction);
                        break;
                    case Tags.DigitLock:
                        SwitchDigitLockView(hit.collider);
                        break;
                    case Tags.Lockable:
                        TryChangeStatus(hit.collider);
                        break;
                    case Tags.Switch:
                        TrySwitch(hit.collider);
                        break;
                    case Tags.Key:
                        TryActivateKey(hit.collider);
                        break;
                    case Tags.KeyLock:
                        TryUnlockKeyLock(hit.collider);
                        break;
                    case Tags.Paper:
                        SwitchToPaperView(hit.collider);
                        break;
                }
            }
        }
    }

    void TryUnlockKeyLock(Collider col)
    {
        var data = col.GetComponent<KeyLockData>();
        if (data != null)
        {
            KeyLockSystem.TryUnlock(data);
        }
    }

    void TryActivateKey(Collider col)
    {
        var data = col.GetComponent<KeyData>();
        if(data != null)
        {
            KeyLockSystem.ActivateKey(data.KeyColor,col.gameObject, currentCamera);
        }
    }

    void TrySwitch(Collider col)
    {
        var data = col.GetComponent<Switch>();
        if(data != null)
        {
            data.ChangeStatus();
        }
    }

    void TryChangeStatus(Collider col)
    {
        var data = col.GetComponent<Lockable>();
        if(data != null)
        {
            if (data.IsLocked)
            {
                data.ShakeTargetLock();
            }
            else
            {
                data.ChangeStatus();
            }
        }
    }

    void TryBreak(Collider col, Vector3 direction)
    {
        var data = col.GetComponentInParent<BreakableData>();
        if (data != null)
        {
            data.TryBreak();
        }
    }

    GameObject currentVRScene;

    void SwitchARVRView(Collider col)
    {
        var vrConroller = VRViewCamera.GetComponent<VRViewCameraController>();
        if (vrConroller.IsDoingSwitch)
            return;
        if (CurrentState == State.ARView)
        {
            CurrentState = State.VRView;
            SwitchToCamera(VRViewCamera);
            var data = col.GetComponent<ARVRSwitchData>();
            if (data != null)
            {
                vrConroller.SwitchToVRView(ARViewCamera.transform, data.blendListCam);
            }
        }
        else
        {
            UIManager.FadeScreenTransition(
                    () =>
                    {
                        CurrentState = State.ARView;
                        SwitchToCamera(ARViewCamera);
                        if (currentVRScene != null)
                        {
                            currentVRScene.SetActive(false);
                        }
                    }
                );
        }
    }

    void SwitchDigitLockView(Collider col)
    {
        var data = col.gameObject.GetComponent<DigitLockData>();
        if(data != null)
        {
            DigitLockSystem.Initialize(data);
            UIManager.ShowDigitLockUI();
            UIManager.ShowBackButton(SwitchToARView, UIManager.HideDigitLockUI);
            CurrentState = State.DigitLockView;
            SwitchToCamera(DigitLockViewCamera);
        }
    }

    void SwitchToBookView(Collider col)
    {
        var data = col.gameObject.GetComponent<BookData>();
        if (data != null)
        {
            UIManager.ShowBookUI(data.Pages);
            UIManager.ShowBackButton(SwitchToARView, UIManager.HideBookUI);
            CurrentState = State.BookView;
            SwitchToCamera(InteractableViewCamera);
        }
    }

    void SwitchToPaperView(Collider col)
    {
        PrepareGOForInteractableView(col.gameObject);

        PaperData paperData = col.gameObject.GetComponent<PaperData>();
        if(paperData != null)
        {
            int contentCount = paperData.texts.Count;
            for(int i = 0; i< contentCount; i++)
            {
                paperData.texts[i].text = TextHandler.GetText(paperData.keyWords[i]);
            }
            paperData.content.gameObject.SetActive(true);
        }

        UIManager.ShowBackButton(RestoreGOFromInteractableView, SwitchToARView, () => { paperData.content.gameObject.SetActive(false); });

        CurrentState = State.PaperView;
        SwitchToCamera(InteractableViewCamera);
    }

    void SwitchToInteractableView(Collider col)
    {
        PrepareGOForInteractableView(col.gameObject);

        UIManager.ShowBackButton(RestoreGOFromInteractableView, SwitchToARView);

        CurrentState = State.InteractableView;
        SwitchToCamera(InteractableViewCamera);
    }

    void PrepareGOForInteractableView(GameObject go)
    {
        currentGO = go;
        SaveGOTransform(currentGO.transform);
        InteractableData data = currentGO.GetComponent<InteractableData>();
        if (data != null)
        {
            currentGO.transform.position = data.Position;
            currentGO.transform.rotation = data.Rotation;
            currentGO.transform.localScale = data.Scale;
            currentGO.tag = Tags.Untagged;
        }
    }

    #region Camera
    void DisableAllCameras()
    {
        ARViewCamera.gameObject.SetActive(false);
        VRViewCamera.enabled = false;
        InteractableViewCamera.enabled = false;
        DigitLockViewCamera.enabled = false;
    }

    void SwitchToCamera(Camera cam)
    {
        DisableAllCameras();
        if(cam == ARViewCamera)
        {
            cam.gameObject.SetActive(true);
        }
        else
        {
            cam.enabled = true;
        }
        currentCamera = cam;
    }
    #endregion

    void RestoreGOFromInteractableView()
    {
        currentGO.transform.position = originalGOPosition;
        currentGO.transform.rotation = originalGORotation;
        currentGO.transform.localScale = originalGOScale;
        currentGO.tag = originalTag;
        currentGO = null;
    }

    void SwitchToARView()
    {
        CurrentState = State.ARView;
        SwitchToCamera(ARViewCamera);
    }

    #region helper functions
    void SaveGOTransform(Transform t)
    {
        originalGOPosition = t.position;
        originalGORotation = t.rotation;
        originalGOScale = t.localScale;
        originalTag = t.tag;
    }
    #endregion
}
