using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ARVRSwitchData : MonoBehaviour {
    public GameObject TargetVRScene;
    [Header("AR switch to VR")]
    public Transform CurrentARScene;
    public GameObject DepthMask;
    public List<GameObject> VRSceneObjectsToShow = new List<GameObject>();
    public CinemachineBlendListCamera blendListCam;
    public float waitForTime;
    public float fadeInBlackTime;
    public float stayInBlackTime;
    public float fadeOutBlackTime;
    [Header("VR switch to AR")]
    public string targetARSceneName;
    public DefaultTrackableEventHandler targetARSceneHandler;
}
