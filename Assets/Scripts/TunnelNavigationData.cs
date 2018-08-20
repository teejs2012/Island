using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelNavigationData : MonoBehaviour {
    [SerializeField]
    float pathValue;
    [SerializeField]
    GameObject nextNavigation;
    [SerializeField]
    GameObject previousNavigation;
    public float PathValue { get { return pathValue; } }

    bool forward = true;

    public void ChangeNavigationDirection()
    {
        if (forward)
        {
            pathValue -= 1;
            nextNavigation.SetActive(true);
            previousNavigation.SetActive(false);
        }
        else
        {
            nextNavigation.SetActive(false);
            previousNavigation.SetActive(true);
            pathValue += 1;
        }
        forward = !forward;
    }
}
