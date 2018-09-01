using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCellEntryInUndergroundController : OneTimeTrigger {

    [SerializeField]
    ARVRSwitchData dataForTunnelEntry;
    [SerializeField]
    GameObject entryToShow;

    [SerializeField]
    VRViewCameraController vrController;

    //bool isTriggered = false;
    protected override void Awake()
    {
        base.Awake();
        vrController.OnSwitchToVRView += TryTrigger;
    }

    protected override void Trigger()
    {
        base.Trigger();
        entryToShow.SetActive(true);
    }

    void TryTrigger(ARVRSwitchData data)
    {
        if (!isTriggered && data == dataForTunnelEntry)
        {
            Trigger();
            RegisterStatus();
        }
    }
}
