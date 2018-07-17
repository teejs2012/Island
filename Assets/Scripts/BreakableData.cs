using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class BreakableData : Triggerable {

    int count = 0;
    [SerializeField]
    int TotalCountToBreak;

    [Header("Animation")]
    [SerializeField]
    ParticleSystem fire;
    [SerializeField]
    Dissolvable strawBed;
    [SerializeField]
    GameObject lantern;
    [SerializeField]
    GameObject tunnel;
    [SerializeField]
    Vector3 fallPosition;
    [SerializeField]
    float lanternFallTime;
    [SerializeField]
    float fireStartTime;
    [SerializeField]
    float fireEndTime;

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
        LeanTween.rotateAround(lantern, Vector3.up, 5, 0.3f).setEaseShake().setOnComplete(() => { doingAnimation = false; });
    }

    void DoBreak()
    {
        isTriggered = true;
        tag = Tags.Untagged;
        StartCoroutine(DoAnimation());
    }

    public override void Trigger()
    {
        isTriggered = true;
        lantern.SetActive(false);
        strawBed.gameObject.SetActive(false);
        tunnel.SetActive(true);
    }

    void DoLaternFall()
    {
        lantern.AddComponent<Rigidbody>();
    }

    void DoFire()
    {
        fire.gameObject.SetActive(true);
        fire.enableEmission = true;
        LeanTween.value(0, 16, fireStartTime).setOnUpdate(
            (float x) =>
            {
                fire.emissionRate = x;
            }
            ).setOnComplete(() => {
                strawBed.Dissolve(fireEndTime);
                LeanTween.value(16, 0, fireEndTime).setOnUpdate(
                    (float x) =>
                    {
                        fire.emissionRate = x;
                    });
            });
    }

    IEnumerator DoAnimation()
    {
        doingAnimation = true;
        DoLaternFall();
        yield return new WaitForSeconds(lanternFallTime);
        DoFire();
        lantern.SetActive(false);
        tunnel.SetActive(true);
        yield return new WaitForSeconds(fireStartTime +  fireEndTime + 1);
        fire.gameObject.SetActive(false);
        doingAnimation = false;
    }

}
