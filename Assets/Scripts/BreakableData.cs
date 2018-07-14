using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableData : MonoBehaviour {

    int count = 0;
    [SerializeField]
    int TotalCountToBreak;
    [SerializeField]
    float force;
    [SerializeField]
    //GameObject fire;
    ParticleSystem fire;
    public float Force { get { return force; } }
    public void IncreaseCount()
    {
        count++;
        CheckCount();
    }

    void CheckCount()
    {
        if(count >= TotalCountToBreak)
        {
            Destroy(GetComponent<SpringJoint>());
            var rigidbody = GetComponent<Rigidbody>();
            rigidbody.drag = 0;
            rigidbody.velocity = Vector3.zero;
            StartCoroutine(WaitAndStartFire());
        }
    }

    IEnumerator WaitAndStartFire()
    {
        yield return new WaitForSeconds(1);
        fire.gameObject.SetActive(true);
        fire.enableEmission = true;
    }

}
