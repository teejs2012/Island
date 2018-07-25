using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockableData : MonoBehaviour {

    [SerializeField]
    GameObject targetLock;
    [SerializeField]
    List<GameObject> contentToActivate;
    [SerializeField]
    bool isLocked = true;

    public GameObject TargetLock {get {return targetLock; } }
    public List<GameObject> ContentToActivate { get { return contentToActivate; } }
    public bool IsLocked { get { return isLocked; } set { isLocked = value; } }
}
