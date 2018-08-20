using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothTriggerable : MonoBehaviour {
    [SerializeField]
    Cloth cloth;
    [SerializeField]
    Transform blocker;
    [SerializeField]
    RotationOpenable openable;
    bool windEnabled = false;
    [SerializeField]
    Vector3 windVector;
    void Start()
    {
        openable.OpenEvent += EnableWind;
        openable.CloseEvent += DisableWind;
    }


    void EnableWind()
    {
        if (windEnabled) return;
        windEnabled = true;
        cloth.externalAcceleration += windVector;
        blocker.localScale = blocker.localScale.SetY(blocker.localScale.y / 2);
    }

    void DisableWind()
    {
        if (!windEnabled) return;
        windEnabled = false;
        cloth.externalAcceleration -= windVector;
        blocker.localScale = blocker.localScale.SetY(blocker.localScale.y * 2);
    }
}
