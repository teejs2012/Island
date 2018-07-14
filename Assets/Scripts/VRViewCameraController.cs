using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRViewCameraController : MonoBehaviour {


    [SerializeField]
    float RotationSpeed = 20;
    [SerializeField]
    GameController gameController;
    State currentState;
    // Use this for initialization
    void Start()
    {
        gameController.BroadcastState += UpdateCurrentState;
    }

    void UpdateCurrentState(State state)
    {
        currentState = state;
        if(currentState == State.VRView)
        {
            InitVRViewCamera();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (currentState == State.VRView)
        {
            MoveNormalViewCamera();
        }
    }

    float cameraPitch = 0;
    float cameraYaw = 0;

    public void InitVRViewCamera()
    {
        cameraPitch = transform.eulerAngles.x;
        cameraYaw = transform.eulerAngles.y;
    }

    void MoveNormalViewCamera()
    {
        if (Input.GetMouseButton(1))
        {
            cameraYaw += Input.GetAxis("Mouse X") * RotationSpeed;
            cameraPitch -= Input.GetAxis("Mouse Y") * RotationSpeed;
            transform.eulerAngles = new Vector3(cameraPitch, cameraYaw, 0);
        }
    }
}
