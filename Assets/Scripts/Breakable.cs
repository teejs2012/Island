using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class Breakable : MonoBehaviour {

    int count = 0;
    [SerializeField]
    int TotalCountToBreak;

    public event System.Action DoBreak = delegate { };

    bool doingAnimation = false;
    public bool DoingAnimation { get { return doingAnimation; } }
    //public float Force { get { return force; } }
    public void TryBreak()
    {
        if (doingAnimation)
        {
            return;
        }

        count++;
        if(count >= TotalCountToBreak)
        {
            DoBreak();
        }
        else
        {
            DoShake();
        }
    }

    void DoShake()
    {
        doingAnimation = true;
        LeanTween.rotateAround(gameObject, Vector3.up, 5, 0.3f).setEaseShake().setOnComplete(() => { doingAnimation = false; });
    }
}
