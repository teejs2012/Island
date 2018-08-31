using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternTriggerable : OneTimeTrigger
{

    [SerializeField]
    Breakable breakable;
    [SerializeField]
    ParticleSystem fire;
    [SerializeField]
    Dissolvable strawBed;
    [SerializeField]
    GameObject lantern;
    [SerializeField]
    GameObject tunnel;
    [SerializeField]
    float lanternFallTime;
    [SerializeField]
    float fireStartTime;
    [SerializeField]
    float fireEndTime;

    protected override void Awake()
    {
        base.Awake();
        breakable.DoBreak += StartAnimation;
    }

    protected override void Trigger()
    {
        base.Trigger();
        lantern.SetActive(false);
        strawBed.gameObject.SetActive(false);
        tunnel.SetActive(true);
    }

    void StartAnimation()
    {
        RegisterStatus();
        StartCoroutine(DoAnimation());
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
        DoLaternFall();
        yield return new WaitForSeconds(lanternFallTime);
        DoFire();
        lantern.SetActive(false);
        tunnel.SetActive(true);
        yield return new WaitForSeconds(fireStartTime + fireEndTime + 1);
        fire.gameObject.SetActive(false);
    }
}
