using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableViewController : MonoBehaviour {
    [SerializeField]
    GameController gameController;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float zoomSpeed;
    State currentState;
    GameObject currentGO;
    Vector3 center;
    Transform currentGOTransform;

    bool limitUpDownRotation = false;
    bool limitLeftRightRotation = false;
    bool limitZoom = false;
       
    void Start () {
        gameController.BroadcastState += UpdateCurrentState;
#if !UNITY_EDITOR
            moveSpeed = moveSpeed/2;
#endif
    }

    void UpdateCurrentState(State state)
    {
        currentState = state;
        if (currentState == State.InteractableView)
        {
            Init();
        }
    }

    void Init()
    {
        currentGO = gameController.CurrentGO;
        center = currentGO.GetComponent<Collider>().bounds.center;
        currentGOTransform = currentGO.transform;

        InteractableData data = currentGO.GetComponent<InteractableData>();
        limitUpDownRotation = data.limitUpDownRotation;
        limitLeftRightRotation = data.limitLeftRightRotation;
        limitZoom = data.limitZoom;
    }

    void Update()
    {
        if(currentState != State.InteractableView)
        {
            return;
        }

        if(currentOpenable != null && currentOpenable.IsDragging)
        {
            return;
        }

        var zoomAmount = UniformInput.Instance.GetZoomAmount();
        //Debug.Log(zoomAmount);
        if (zoomAmount != 0 && !limitZoom)
        {
            currentGOTransform.transform.localScale = currentGOTransform.localScale * (1 + zoomAmount) * zoomSpeed;
            return;
        }

        if (UniformInput.Instance.GetPress())
        {

            float xrot = UniformInput.Instance.GetMouseX() * moveSpeed * Time.deltaTime;
            float yrot = UniformInput.Instance.GetMouseY() * moveSpeed * Time.deltaTime;
            Vector3 objectDown = currentGOTransform.InverseTransformDirection(Vector3.down);
            Vector3 objectLeft = currentGOTransform.InverseTransformDirection(Vector3.right);
            if (!limitLeftRightRotation)
            {
                currentGOTransform.RotateAround(center, currentGOTransform.forward, objectDown.z * xrot);
                currentGOTransform.RotateAround(center, currentGOTransform.right, objectDown.x * xrot);
                currentGOTransform.RotateAround(center, currentGOTransform.up, objectDown.y * xrot);
            }
            if (!limitUpDownRotation)
            {
                currentGOTransform.RotateAround(center, currentGOTransform.forward, objectLeft.z * yrot);
                currentGOTransform.RotateAround(center, currentGOTransform.right, objectLeft.x * yrot);
                currentGOTransform.RotateAround(center, currentGOTransform.up, objectLeft.y * yrot);
            }
        }
    }

    Openable currentOpenable;

    public void SetOpenable(Openable data)
    {
        currentOpenable = data;
    }
}
