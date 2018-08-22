using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ARVRSwitchData : MonoBehaviour {
    public GameObject TargetVRScene;
    public Transform CurrentARScene;
    public GameObject DepthMask;
    public List<GameObject> VRSceneObjectsToShow = new List<GameObject>();
    public CinemachineBlendListCamera blendListCam;
    public float waitForTime;
    public float fadeInBlackTime;
    public float stayInBlackTime;
    public float fadeOutBlackTime;
}
