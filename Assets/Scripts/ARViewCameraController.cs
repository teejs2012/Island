using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARViewCameraController : MonoBehaviour {

    [SerializeField]
    float RotationSpeed = 20;
    [SerializeField]
    float MoveSpeed = 1;
    [SerializeField]
    GameController gameController;
    State currentState;
	// Use this for initialization
	void Start () {
        InitNormalViewCamera();
        gameController.BroadcastState += UpdateCurrentState;
    }
	
    void UpdateCurrentState(State state)
    {
        currentState = state;
    }
	// Update is called once per frame
	void Update () {
		if(currentState == State.ARView)
        {
            MoveNormalViewCamera();
        }
	}

    float cameraPitch = 0;
    float cameraYaw = 0;

    void InitNormalViewCamera()
    {
        cameraPitch = transform.eulerAngles.x;
        cameraYaw = transform.eulerAngles.y;
    }

    void MoveNormalViewCamera()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(moveX, 0, moveY)* MoveSpeed);

        //if (Input.GetMouseButtonDown(1))
        //{
        //    InitNormalViewCamera();
        //}

        if (Input.GetMouseButton(1))
        {
            cameraYaw += Input.GetAxis("Mouse X") * RotationSpeed;
            cameraPitch -= Input.GetAxis("Mouse Y") * RotationSpeed;
            transform.eulerAngles = new Vector3(cameraPitch, cameraYaw, 0);
        }
    }
}
