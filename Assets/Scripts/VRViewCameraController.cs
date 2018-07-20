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
        if(currentState == State.VRView)
        {
            InitVRViewCamera();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (currentState == State.VRView)
        {
            MoveNormalViewCamera();
        }
    }

    float cameraPitch = 0;
    float cameraYaw = 0;

    public void InitVRViewCamera()
    {
        cameraPitch = transform.eulerAngles.x;
        cameraYaw = transform.eulerAngles.y;
    }

    public void SwitchToVRView(Transform currentARCamTransform, CinemachineBlendListCamera blendListCam)
    {
        SetTransform(currentARCamTransform, blendListCam.ChildCameras[0].transform);
        blendListCam.enabled = true;
        foreach(CinemachineVirtualCamera child in blendListCam.ChildCameras)
        {
            var trackedDolly = child.GetCinemachineComponent<CinemachineTrackedDolly>();
            if (trackedDolly != null)
            {
                LeanTween.value(0, 1, 2).setOnUpdate((float x) => { trackedDolly.m_PathPosition = x; });
            }
        }
    }

    void SetTransform(Transform from, Transform to)
    {
        to.position = from.position;
        to.eulerAngles = from.eulerAngles;
    }

    void MoveNormalViewCamera()
    {
        if (Input.GetMouseButton(1))
        {
            cameraYaw += Input.GetAxis("Mouse X") * RotationSpeed;
            cameraPitch -= Input.GetAxis("Mouse Y") * RotationSpeed;
            transform.eulerAngles = new Vector3(cameraPitch, cameraYaw, 0);
        }
    }
}
