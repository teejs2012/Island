using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelUndergroundController : OneTimeTrigger {

    [SerializeField]
    GameObject Light;
    [SerializeField]
    Transform undergroundTransform;
    [SerializeField]
    Transform[] undergroundCovers;

    [SerializeField]
    Vector3 PosForMine;
    [SerializeField]
    Vector3 PosForCell;

    [SerializeField]
    ARVRSwitchData dataForMine;
    [SerializeField]
    ARVRSwitchData dataForUndergroundToMine;
    [SerializeField]
    ARVRSwitchData dataForCell;
    [SerializeField]
    ARVRSwitchData dataForUndergroundToCell;
    [SerializeField]
    ARVRSwitchData dataForUnderground;

    [SerializeField]
    VRViewCameraController vrController;


    bool isTriggered = false;
    protected override void Awake()
    {
        base.Awake();
        vrController.OnSwitchToVRView += SetUnderGroundPos;
        vrController.OnExitVRView += TryTrigger;
    }
    public override void Trigger()
    {
        Debug.Log("Doing trigger");
        isTriggered = true;
        Light.SetActive(true);
        OpenUndergroundCover();
        RegisterStatus();
    }

    void SetUnderGroundPos(ARVRSwitchData data)
    {
        if (data.Equals(dataForMine) || data.Equals(dataForUndergroundToMine))
        {
            undergroundTransform.localPosition = PosForMine;
        }
        else if (data.Equals(dataForCell) || data.Equals(dataForUndergroundToCell))
        {
            undergroundTransform.localPosition = PosForCell;
        }
    }

    void TryTrigger(ARVRSwitchData data)
    {
        Debug.Log("Doing try trigger");
        if(!isTriggered && data.Equals(dataForUnderground))
        {
            Trigger();
        }
    }

    void OpenUndergroundCover()
    {
        foreach(var cover in undergroundCovers)
        {
            cover.localEulerAngles = cover.localEulerAngles.SetZ(120);
        }
    }
}
