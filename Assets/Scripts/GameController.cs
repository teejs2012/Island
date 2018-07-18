using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class GameController : MonoBehaviour {
    State currentState = State.ARView;
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

    [SerializeField]
    UIManager UIManager;
    [SerializeField]
    LockSystem LockSystem;
    //For Interactable
    GameObject currentGO;
    Vector3 originalGOPosition;
    Quaternion originalGORotation;
    Vector3 originalGOScale;
    Vector3 center;
    Transform currentGOTransform;

    [Header("Params")]
    public float movespeed;

    public delegate void ReturnState(State state);
    public event ReturnState BroadcastState;

    // Update is called once per frame
    void Update () {
        switch (CurrentState)
        {
            case State.ARView:
                ARViewUpdate();
                break;
            case State.VRView:
                VRViewUpdate();
                break;
            case State.InteractableView:
                InteractableViewUpdate();
                break;
            case State.BookView:
            case State.DigitLockView:
                break;
            
        }		
	}

    void CheckHit(Ray ray)
    {
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
                }
            }
        }
    }

    void VRViewUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckHit(VRViewCamera.ScreenPointToRay(Input.mousePosition));
        }
    }

    void ARViewUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckHit(ARViewCamera.ScreenPointToRay(Input.mousePosition));
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
        //var rigidbody = col.GetComponent<Rigidbody>();
        if (data != null)
        {
            data.TryBreak();
        }
    }

    GameObject currentVRScene;

    void SwitchARVRView(Collider col)
    {
        if(CurrentState == State.ARView)
        {
            CurrentState = State.VRView;
            VRViewCamera.transform.position = ARViewCamera.transform.position;
            VRViewCamera.transform.rotation = ARViewCamera.transform.rotation;
            ActivateVRViewCamera();
            var data = col.GetComponent<ARVRSwitchData>();
            if(data != null)
            {
                currentVRScene = data.TargetVRScene;
                currentVRScene.SetActive(true);

                LeanTween.move(VRViewCamera.gameObject, data.TargetPosition, 1);
                LeanTween.rotate(VRViewCamera.gameObject, data.TargetRotation, 1);
            }
        }
        else
        {
            UIManager.FadeScreenTransition(
                    () =>
                    {
                        CurrentState = State.ARView;
                        ActivateARViewCamera();
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
            LockSystem.Initialize(data);
            UIManager.ShowLockUI();
            CurrentState = State.DigitLockView;
            ActivateLockViewCamera();
        }
    }

    void SwitchToBookView(Collider col)
    {
        var data = col.gameObject.GetComponent<BookData>();
        if (data != null)
        {
            UIManager.ShowBookUI(data.Pages);
            CurrentState = State.BookView;
            ActivateInteractableViewCamera();
        }
    }

    void SwitchToInteractableView(Collider col)
    {

        currentGO = col.gameObject;
        SaveGOTransform(currentGO.transform);
        InteractableData data = currentGO.GetComponent<InteractableData>();
        if (data != null)
        {
            currentGO.transform.position = data.Position;
            currentGO.transform.rotation = data.Rotation;
            currentGO.transform.localScale = data.Scale;
        }
        center = currentGO.GetComponent<Collider>().bounds.center;
        currentGOTransform = currentGO.transform;
        UIManager.ShowInteractableViewUI();

        CurrentState = State.InteractableView;
        ActivateInteractableViewCamera();
    }

    #region Camera
    void DisableAllCameras()
    {
        ARViewCamera.enabled = false;
        VRViewCamera.enabled = false;
        InteractableViewCamera.enabled = false;
        DigitLockViewCamera.enabled = false;
    }

    void ActivateARViewCamera()
    {
        DisableAllCameras();
        ARViewCamera.enabled = true;
    }

    void ActivateVRViewCamera()
    {
        DisableAllCameras();
        VRViewCamera.enabled = true;
    }

    void ActivateInteractableViewCamera()
    {
        DisableAllCameras();
        InteractableViewCamera.enabled = true;
    }

    void ActivateLockViewCamera()
    {
        DisableAllCameras();
        DigitLockViewCamera.enabled = true;
    }
    #endregion

    public void SwitchToNormalView()
    {
        if (CurrentState == State.InteractableView)
        {

            //move back the interactable object
            currentGO.transform.position = originalGOPosition;
            currentGO.transform.rotation = originalGORotation;
            currentGO.transform.localScale = originalGOScale;
            currentGO = null;
        }
        CurrentState = State.ARView;
        ActivateARViewCamera();
    }

    void SaveGOTransform(Transform t)
    {
        originalGOPosition = t.position;
        originalGORotation = t.rotation;
        originalGOScale = t.localScale;
    }

    void InteractableViewUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            float xrot = Input.GetAxis("Mouse X") * movespeed * Time.deltaTime;
            float yrot = Input.GetAxis("Mouse Y") * movespeed * Time.deltaTime;
            Vector3 objectDown = currentGOTransform.InverseTransformDirection(Vector3.down);
            Vector3 objectLeft = currentGOTransform.InverseTransformDirection(Vector3.right);

            currentGOTransform.RotateAround(center, currentGOTransform.forward, objectDown.z * xrot);
            currentGOTransform.RotateAround(center, currentGOTransform.right, objectDown.x * xrot);
            currentGOTransform.RotateAround(center, currentGOTransform.up, objectDown.y * xrot);

            currentGOTransform.RotateAround(center, currentGOTransform.forward, objectLeft.z * yrot);
            currentGOTransform.RotateAround(center, currentGOTransform.right, objectLeft.x * yrot);
            currentGOTransform.RotateAround(center, currentGOTransform.up, objectLeft.y * yrot);
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            currentGOTransform.transform.localScale = currentGOTransform.localScale * (1 + Input.GetAxis("Mouse ScrollWheel"));
        }
    }

}
