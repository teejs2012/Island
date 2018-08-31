using UnityEngine;

public abstract class OneTimeTrigger : MonoBehaviour {
    protected virtual void Trigger()
    {
        isTriggered = true;
    }

    protected void OnDisable()
    {
        if(!isTriggered && StatusManager.Instance.CheckTrigger(name))
        {
            Trigger();
        }
    }

    protected bool isTriggered = false;

    protected virtual void Awake()
    {
        AppendMD5HashToName();
    }

    void AppendMD5HashToName()
    {
        name = name + Utility.CreateMD5(transform.position.ToString() + transform.eulerAngles.ToString());
    }

    protected void RegisterStatus()
    {
        isTriggered = true;
        StatusManager.Instance.RegisterAsTriggeredObject(name);
    }
}
