using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Assertions;
using System;

public class VRViewCameraController : MonoBehaviour {

    [SerializeField]
    float RotationSpeed = 20;
    [SerializeField]
    GameController gameController;
    [SerializeField]
    UIManager uiManager;
    public delegate void ARVRSwitchDataDelegate(ARVRSwitchData data);
    public event ARVRSwitchDataDelegate OnSwitchToVRView;
    public event ARVRSwitchDataDelegate OnExitVRView;

    State currentState;

    float cameraPitch = 0;
    float cameraYaw = 0;
    CinemachineBrain cinemachineBrain;
    CinemachineBlendListCamera currentBlendlistCam;
    List<CinemachineVirtualCamera> camWithTrackedDolly = new List<CinemachineVirtualCamera>();
    CinemachineVirtualCamera currentCamWithTrackedDolly;
    Transform currentCam = null;
    ARVRSwitchData currentData;

    void Start()
    {
        cinemachineBrain = GetComponent<CinemachineBrain>();
        Assert.IsTrue(cinemachineBrain != null);
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
            MoveVRViewCamera();
        }
    }

    public void MoveTrackedDollyPath(float pos)
    {
        if (currentCamWithTrackedDolly == null) return;
        isDoingSwitch = true;
        var trackedDolly = currentCamWithTrackedDolly.GetCinemachineComponent<CinemachineTrackedDolly>();
        LeanTween.value(trackedDolly.m_PathPosition, pos, 2).setOnUpdate((float x) => { trackedDolly.m_PathPosition = x; }).setOnComplete(()=> { isDoingSwitch = false; }) ;
    }

    public void ExitVRView(ARVRSwitchData data)
    {
        if (currentData == null)
            return;

        if (OnExitVRView != null)
        {
            OnExitVRView(data);
        }

        camWithTrackedDolly.Clear();
        var targetVRScene = currentData.TargetVRScene;
        if(targetVRScene != null)
        {
            targetVRScene.SetActive(false);
        }

        cinemachineBrain.enabled = false;
        if (currentBlendlistCam != null)
        {
            Destroy(currentBlendlistCam.gameObject);
            //currentBlendlistCam.enabled = false;
        }
    }

    Gyroscope gyro;

    void PrepareVRScene(GameObject vrScene, List<GameObject> objectsToShow)
    {

#if !UNITY_EDITOR
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
        }
#endif

        vrScene.SetActive(true);
        foreach(Transform child in vrScene.transform)
        {
            child.gameObject.SetActive(false);
        }
        foreach(var item in objectsToShow)
        {
            item.SetActive(true);
        }
    }

    Transform currentARScene;
    Transform currentARSceneParent;

    public void SwitchToVRView(Transform currentARCamTransform, ARVRSwitchData data)
    {
        if (isDoingSwitch)
            return;
        if (data == null)
            return;

        currentData = data;

        var targetVRScene = currentData.TargetVRScene;
        if(targetVRScene != null)
        {
            PrepareVRScene(targetVRScene, data.VRSceneObjectsToShow);
        }

        //temporarily freeze the ar scene for transition to vr view
        currentARScene = currentData.CurrentARScene;
        if(currentARScene != null)
        {
            currentARSceneParent = currentARScene.parent;
            currentARScene.SetParent(null);
            var depthMask = currentData.DepthMask;
            if(depthMask != null)
            {
                depthMask.SetActive(false);
            }
        }

        cinemachineBrain.enabled = true;
        var currentBlendlistCamGO = Instantiate(currentData.blendListCam.gameObject);
        currentBlendlistCam = currentBlendlistCamGO.GetComponent<CinemachineBlendListCamera>();
        SetTransform(currentARCamTransform, currentBlendlistCam.ChildCameras[0].transform);
        currentBlendlistCamGO.SetActive(true);

        foreach (CinemachineVirtualCamera child in currentBlendlistCam.ChildCameras)
        {
            var trackedDolly = child.GetCinemachineComponent<CinemachineTrackedDolly>();
            if (trackedDolly != null)
            {
                trackedDolly.m_PathPosition = 0;
                camWithTrackedDolly.Add(child);
            }
        }
        StartCoroutine(DoTrackedDolly(currentBlendlistCam));
        StartCoroutine(FadeScreen(currentData.waitForTime, currentData.fadeInBlackTime, currentData.stayInBlackTime, currentData.fadeOutBlackTime));

        if (OnSwitchToVRView != null)
        {
            OnSwitchToVRView(currentData);
        }
    }

    IEnumerator FadeScreen(float waitForTime, float fadeInDuration, float blackDuration, float fadeOutDuration)
    {
        yield return new WaitForSeconds(waitForTime);
        uiManager.FadeBlackScreen(1, fadeInDuration);
        yield return new WaitForSeconds(fadeInDuration + blackDuration);
        uiManager.FadeBlackScreen(0, fadeOutDuration);

        //restore the AR scene
        currentARScene.SetParent(currentARSceneParent);
        currentARScene.gameObject.SetActive(false);
        var depthMask = currentData.DepthMask;
        if (depthMask != null)
        {
            depthMask.SetActive(true);
        }
    }

    bool isDoingSwitch = false;
    public bool IsDoingSwitch { get { return isDoingSwitch; } }
    IEnumerator DoTrackedDolly(CinemachineBlendListCamera blendListCam)
    {
        isDoingSwitch = true;
        int i = 0;
        int camWithTrackedDollyCount = camWithTrackedDolly.Count;
        //the last camera is the cam in the tunnel, and needs to be moved by player
        while (i< camWithTrackedDollyCount-1)
        {
            if (blendListCam.IsLiveChild(camWithTrackedDolly[i]))
            {
                var trackedDolly = camWithTrackedDolly[i].GetCinemachineComponent<CinemachineTrackedDolly>();
                LeanTween.value(0, 1, 1).setOnUpdate((float x) => { trackedDolly.m_PathPosition = x; });
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
        currentCamWithTrackedDolly = camWithTrackedDolly[camWithTrackedDollyCount - 1];
        currentCam = blendListCam.ChildCameras[lastCamInd].transform;
        cameraPitch = currentCam.transform.eulerAngles.x;
        cameraYaw = currentCam.transform.eulerAngles.y;
    }

    void SetTransform(Transform from, Transform to)
    {
        to.position = from.position;
        to.eulerAngles = from.eulerAngles;
    }

    void MoveVRViewCamera()
    {
        if (isDoingSwitch)
            return;



#if UNITY_EDITOR
        if (Input.GetMouseButton(1))
        {
            cameraYaw += Input.GetAxis("Mouse X") * RotationSpeed;
            cameraPitch -= Input.GetAxis("Mouse Y") * RotationSpeed;
            currentCam.transform.eulerAngles = new Vector3(cameraPitch, cameraYaw, 0);
        }
#else
        if (gyro == null)
            return;
        currentCam.transform.Rotate(-gyro.rotationRateUnbiased.x*2, -gyro.rotationRateUnbiased.y*2, 0);
#endif
    }
}
