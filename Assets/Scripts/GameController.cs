using System.Collections;
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

    VRViewCameraController vrController;
    InteractableViewController interController;

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
    Transform originalParent;

    public delegate void ReturnState(State state);
    public event ReturnState BroadcastState;

    void Start()
    {
        vrController = VRViewCamera.GetComponent<VRViewCameraController>();
        interController = InteractableViewCamera.GetComponent<InteractableViewController>();
        currentCamera = ARViewCamera;
        CurrentState = State.ARView;
    }

    void Update () {
        if (UniformInput.Instance.GetPressDown())
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
        Ray ray = cam.ScreenPointToRay(UniformInput.Instance.GetPressPosition()); 
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
                    case Tags.Openable:
                        TryChangeStatus(hit.collider);
                        break;
                    case Tags.TunnelNavigation:
                        TryNavigateTunnel(hit.collider);
                        break;
                    case Tags.UnderGroundSwitch:
                        TrySwitchGround(hit.collider);
                        break;
                }
            }
        }
    }

    void TrySwitchGround(Collider col)
    {
        var groundSwitch = col.GetComponent<UnderGroundSwitch>();
        if(groundSwitch != null)
        {
            groundSwitch.Switch();
        }
    }

    void TryNavigateTunnel(Collider col)
    {
        var tunnelNavigation = col.GetComponent<TunnelNavigationData>();
        if(tunnelNavigation != null)
        {
            var vrConroller = VRViewCamera.GetComponent<VRViewCameraController>();
            if (vrConroller.IsDoingSwitch)
                return;
            vrConroller.MoveTrackedDollyPath(tunnelNavigation.PathValue);
            tunnelNavigation.ChangeNavigationDirection();
        }
    }

    void TryUnlockKeyLock(Collider col)
    {
        var keyLock = col.GetComponent<KeyLock>();
        if (keyLock != null)
        {
            KeyLockSystem.TryUnlock(keyLock);
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
        var data = col.GetComponent<Openable>();
        if(data != null)
        {
            //data.ChangeStatus();
            data.StartDrag(currentCamera);
            if(CurrentState == State.InteractableView)
            {
                interController.SetOpenable(data);
            }
        }
    }

    void TryBreak(Collider col, Vector3 direction)
    {
        var data = col.GetComponentInParent<Breakable>();
        if (data != null)
        {
            data.TryBreak();
        }
    }

    [HideInInspector]
    public DefaultTrackableEventHandler targetHandler;
    [HideInInspector]
    public bool isLookingForSpecificMarker;

    public void OnTargetHandlerFound()
    {
        isLookingForSpecificMarker = false;
        targetHandler = null;
        UIManager.HideCameraFrame();
    }

    void SwitchARVRView(Collider col)
    {
        if (vrController.IsDoingSwitch)
            return;
        var data = col.GetComponent<ARVRSwitchData>();
        if (data == null) return;

        if (CurrentState == State.ARView)
        {
            vrController.SwitchToVRView(ARViewCamera.transform, data);
            CurrentState = State.VRView;
            SwitchToCamera(VRViewCamera);
        }
        else
        {
            UIManager.FadeWhiteScreen(() =>
                                        {
                                            CurrentState = State.ARView;
                                            SwitchToCamera(ARViewCamera);
                                            vrController.ExitVRView(data);
                                            UIManager.ShowCameraFrame(data.targetARSceneName);
                                            targetHandler = data.targetARSceneHandler;
                                            isLookingForSpecificMarker = true;
                                        }
                                    );
        }
    }

    void SwitchDigitLockView(Collider col)
    {
        var digitLock = col.gameObject.GetComponent<DigitLock>();
        if(digitLock != null)
        {
            DigitLockSystem.Initialize(digitLock);
            UIManager.ShowDigitLockUI();
            UIManager.ShowBackButton(GetSwitchBackFunction(), UIManager.HideDigitLockUI);
            CurrentState = State.DigitLockView;
            SwitchToCamera(DigitLockViewCamera);
        }
    }

    void SwitchToBookView(Collider col)
    {
        var data = col.gameObject.GetComponent<BookData>();
        if (data != null)
        {
            UIManager.ShowBookUI(data.Pages, data.bookBack);
            UIManager.ShowBackButton(GetSwitchBackFunction(), UIManager.HideBookUI);
            CurrentState = State.BookView;
            SwitchToCamera(InteractableViewCamera);
        }
    }

    void SwitchToPaperView(Collider col)
    {
        PrepareGOForInteractableView(col.gameObject);

        PapersController paperController = col.gameObject.GetComponent<PapersController>();
        if(paperController != null)
        {
            paperController.Init(TextHandler);

            UIManager.ShowBackButton(RestoreGOFromInteractableView, GetSwitchBackFunction(), paperController.ExitPaperView);

            CurrentState = State.PaperView;
            SwitchToCamera(InteractableViewCamera);
        }
    }

    void SwitchToInteractableView(Collider col)
    {
        PrepareGOForInteractableView(col.gameObject);

        UIManager.ShowBackButton(RestoreGOFromInteractableView, GetSwitchBackFunction(), ()=> { interController.SetOpenable(null); });

        CurrentState = State.InteractableView;
        SwitchToCamera(InteractableViewCamera);
    }

    UnityEngine.Events.UnityAction GetSwitchBackFunction()
    {
        if(CurrentState == State.ARView)
        {
            return SwitchToARView;
        }
        else
        {
            return SwitchToVRView;
        }
    }

    void SwitchToARView()
    {
        CurrentState = State.ARView;
        SwitchToCamera(ARViewCamera);
    }

    void SwitchToVRView()
    {
        CurrentState = State.VRView;
        SwitchToCamera(VRViewCamera);
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

    void PrepareGOForInteractableView(GameObject go)
    {
        currentGO = go;
        SaveGOTransform(currentGO.transform);
        InteractableData data = currentGO.GetComponent<InteractableData>();
        if (data != null)
        {
            currentGO.transform.SetParent(null);
            currentGO.transform.position = data.Position;
            currentGO.transform.rotation = data.Rotation;
            currentGO.transform.localScale = data.Scale;
            currentGO.tag = Tags.Untagged;
        }
    }

    void RestoreGOFromInteractableView()
    {
        currentGO.transform.SetParent(originalParent);
        currentGO.transform.position = originalGOPosition;
        currentGO.transform.rotation = originalGORotation;
        currentGO.transform.localScale = originalGOScale;
        currentGO.tag = originalTag;
        currentGO = null;
    }


    void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            StatusManager.Instance.Save();
            CurrentState = State.InteractableView;
            SwitchToCamera(InteractableViewCamera);
        }
        else
        {
            CurrentState = State.ARView;
            SwitchToCamera(ARViewCamera);
        }
    }

    void OnApplicationQuit()
    {
        StatusManager.Instance.Save();
    }


    #region helper functions
    void SaveGOTransform(Transform t)
    {
        originalGOPosition = t.position;
        originalGORotation = t.rotation;
        originalGOScale = t.localScale;
        originalTag = t.tag;
        originalParent = t.parent;
    }
    #endregion
}
