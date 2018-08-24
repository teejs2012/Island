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

    bool isTriggered = false;
    protected override void Awake()
    {
        base.Awake();
        vrController.OnSwitchToVRView += TryTrigger;
    }

    public override void Trigger()
    {
        isTriggered = true;
        entryToShow.SetActive(true);
        RegisterStatus();
    }

    void TryTrigger(ARVRSwitchData data)
    {
        if (!isTriggered && data.Equals(dataForTunnelEntry))
        {
            Trigger();
        }
    }
}
