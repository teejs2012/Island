using UnityEngine;

public abstract class OneTimeTrigger : MonoBehaviour {
    public abstract void Trigger();

    protected virtual void Awake()
    {
        AppendMD5HashToName();
    }

    void AppendMD5HashToName()
    {
        name = name + Utility.CreateMD5(transform.position.ToString());
    }

    protected void RegisterStatus()
    {
        StatusManager.Instance.RegisterAsTriggeredObject(name);
    }
}
