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

    void Start () {
        gameController.BroadcastState += UpdateCurrentState;
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
    }

    void Update()
    {
        if(currentState != State.InteractableView)
        {
            return;
        }
        if (Input.GetMouseButton(0))
        {
            float xrot = Input.GetAxis("Mouse X") * moveSpeed * Time.deltaTime;
            float yrot = Input.GetAxis("Mouse Y") * moveSpeed * Time.deltaTime;
            Vector3 objectDown = currentGOTransform.InverseTransformDirection(Vector3.down);
            Vector3 objectLeft = currentGOTransform.InverseTransformDirection(Vector3.right);

            currentGOTransform.RotateAround(center, currentGOTransform.forward, objectDown.z * xrot);
            currentGOTransform.RotateAround(center, currentGOTransform.right, objectDown.x * xrot);
            currentGOTransform.RotateAround(center, currentGOTransform.up, objectDown.y * xrot);

            currentGOTransform.RotateAround(center, currentGOTransform.forward, objectLeft.z * yrot);
            currentGOTransform.RotateAround(center, currentGOTransform.right, objectLeft.x * yrot);
            currentGOTransform.RotateAround(center, currentGOTransform.up, objectLeft.y * yrot);
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            currentGOTransform.transform.localScale = currentGOTransform.localScale * (1 + Input.GetAxis("Mouse ScrollWheel")) * zoomSpeed;
        }
    }
}
