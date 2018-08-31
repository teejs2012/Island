using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeTunnelController : OneTimeTrigger
{
    [SerializeField]
    ARVRSwitchData dataForTreeExit;
    [SerializeField]
    GameObject entryToShow;

    [SerializeField]
    VRViewCameraController vrController;

    protected override void Awake()
    {
        base.Awake();
        vrController.OnExitVRView += TryTrigger;
    }

    protected override void Trigger()
    {
        base.Trigger();
        entryToShow.SetActive(true);
    }

    void TryTrigger(ARVRSwitchData data)
    {
        if (!isTriggered && data.Equals(dataForTreeExit))
        {
            Trigger();
            RegisterStatus();
        }
    }
}
