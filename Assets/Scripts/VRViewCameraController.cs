using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VRViewCameraController : MonoBehaviour {


    [SerializeField]
    float RotationSpeed = 20;
    [SerializeField]
    GameController gameController;
    State currentState;
    CinemachineBrain cinemachineBrain;
    // Use this for initialization
    void Awake()
    {
        cinemachineBrain = GetComponent<CinemachineBrain>();
    }

    void Start()
    {
        gameController.BroadcastState += UpdateCurrentState;
    }

    void UpdateCurrentState(State state)
    {
        currentState = state;
    }
    // Update is called once per frame
    void Update()
    {
        if (currentState == State.VRView && !IsDoingSwitch)
        {
            MoveNormalViewCamera();
        }
    }

    float cameraPitch = 0;
    float cameraYaw = 0;

    List<CinemachineVirtualCamera> camWithTrackedDolly = new List<CinemachineVirtualCamera>();
    Transform currentCam = null;

    public void SwitchToVRView(Transform currentARCamTransform, CinemachineBlendListCamera blendListCam)
    {
        if (isDoingSwitch)
            return;

        SetTransform(currentARCamTransform, blendListCam.ChildCameras[0].transform);
        blendListCam.enabled = true;
        foreach(CinemachineVirtualCamera child in blendListCam.ChildCameras)
        {
            var trackedDolly = child.GetCinemachineComponent<CinemachineTrackedDolly>();
            if (trackedDolly != null)
            {
                camWithTrackedDolly.Add(child);
            }
        }
        StartCoroutine(DoTrackedDolly(blendListCam));
    }

    bool isDoingSwitch = false;
    public bool IsDoingSwitch { get { return isDoingSwitch; } }
    IEnumerator DoTrackedDolly(CinemachineBlendListCamera blendListCam)
    {
        isDoingSwitch = true;
        int i = 0;
        int camWithTrackedDollyCount = camWithTrackedDolly.Count;
        while (i< camWithTrackedDollyCount)
        {
            
            if (blendListCam.IsLiveChild(camWithTrackedDolly[i]))
            {
                var trackedDolly = camWithTrackedDolly[i].GetCinemachineComponent<CinemachineTrackedDolly>();
                LeanTween.value(0, 1, 2).setOnUpdate((float x) => { trackedDolly.m_PathPosition = x; });
                i++;
            }
            else
            {
                yield return null;
            }
        }
        while (blendListCam.IsBlending)
        {
            yield return null;
        }
        isDoingSwitch = false;

        int lastCamInd = blendListCam.ChildCameras.Length - 1;
        currentCam = blendListCam.ChildCameras[lastCamInd].transform;
        cameraPitch = currentCam.transform.eulerAngles.x;
        cameraYaw = currentCam.transform.eulerAngles.y;
    }

    void SetTransform(Transform from, Transform to)
    {
        to.position = from.position;
        to.eulerAngles = from.eulerAngles;
    }

    void MoveNormalViewCamera()
    {
        if (isDoingSwitch)
            return;
        if (Input.GetMouseButton(1))
        {
            cameraYaw += Input.GetAxis("Mouse X") * RotationSpeed;
            cameraPitch -= Input.GetAxis("Mouse Y") * RotationSpeed;
            currentCam.transform.eulerAngles = new Vector3(cameraPitch, cameraYaw, 0);
        }
    }
}
