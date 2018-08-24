using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelNavigationData : MonoBehaviour {
    [SerializeField]
    float pathValue;
    [SerializeField]
    GameObject nextNavigation;
    public float PathValue { get { return pathValue; } }

    public void ChangeNavigationDirection()
    {
        nextNavigation.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
