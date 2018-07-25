using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothTriggerable : MonoBehaviour {
    [SerializeField]
    Cloth cloth;
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
    }

    void DisableWind()
    {
        if (!windEnabled) return;
        windEnabled = false;
        cloth.externalAcceleration -= windVector;
    }
}
