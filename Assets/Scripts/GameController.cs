using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    enum State
    {
        NormalView, InteractableView
    }

    State currentState = State.NormalView;

    public Camera NormalViewCamera;
    public Camera InteractableViewCamera;

    [SerializeField]
    UIManager UIManager;

    //For Interactable
    GameObject currentGO;
    Vector3 originalGOPosition;
    Quaternion originalGORotation;
    Vector3 originalGOScale;
    Vector3 center;
    Transform currentGOTransform;

    [Header("Params")]
    public float movespeed;

    // Update is called once per frame
    void Update () {
        switch (currentState)
        {
            case State.NormalView:
                NormalViewUpdate();
                break;
            case State.InteractableView:
                InteractableViewUpdate();
                break;
        }		
	}

    void NormalViewUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null && hit.collider.CompareTag("Interactable"))
                {
                    SwitchToInteractableView(hit.collider);
                }
            }
        }
    }

    void SwitchToInteractableView(Collider col)
    {
        currentState = State.InteractableView;
        InteractableViewCamera.depth = 0;
        currentGO = col.gameObject;
        SaveGOTransform(currentGO.transform);
        InteractableData data = currentGO.GetComponent<InteractableData>();
        if (data != null)
        {
            currentGO.transform.position = data.Position;
            currentGO.transform.rotation = data.Rotation;
            currentGO.transform.localScale = data.Scale;
        }
        UIManager.ShowInteractableViewUI();
        center = currentGO.GetComponent<Collider>().bounds.center;
        currentGOTransform = currentGO.transform;
    }

    public void SwitchToNormalView()
    {
        currentState = State.NormalView;
        InteractableViewCamera.depth = -1;
        NormalViewCamera.depth = 0;
        //move back the interactable object
        currentGO.transform.position = originalGOPosition;
        currentGO.transform.rotation = originalGORotation;
        currentGO.transform.localScale = originalGOScale;
        currentGO = null;
        UIManager.HideInteractableViewUI();
    }

    void SaveGOTransform(Transform t)
    {
        originalGOPosition = t.position;
        originalGORotation = t.rotation;
        originalGOScale = t.localScale;
    }

    void InteractableViewUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            float xrot = Input.GetAxis("Mouse X") * movespeed * Time.deltaTime;
            float yrot = Input.GetAxis("Mouse Y") * movespeed * Time.deltaTime;
            Vector3 objectDown = currentGOTransform.InverseTransformDirection(Vector3.down);
            Vector3 objectLeft = currentGOTransform.InverseTransformDirection(Vector3.right);

            currentGOTransform.RotateAround(center, currentGOTransform.forward, objectDown.z * xrot);
            currentGOTransform.RotateAround(center, currentGOTransform.right, objectDown.x * xrot);
            currentGOTransform.RotateAround(center, currentGOTransform.up, objectDown.y * xrot);

            currentGOTransform.RotateAround(center, currentGOTransform.forward, objectLeft.z * yrot);
            currentGOTransform.RotateAround(center, currentGOTransform.right, objectLeft.x * yrot);
            currentGOTransform.RotateAround(center, currentGOTransform.up, objectLeft.y * yrot);


            // float xrot = Input.GetAxis ("Mouse X") * movespeed * Time.deltaTime;
            // float yrot = Input.GetAxis ("Mouse Y") * movespeed * Time.deltaTime;
            // Vector3 objectUp = currentTarget.InverseTransformDirection (Vector3.up);
            // Vector3 objectLeft = currentTarget.InverseTransformDirection (Vector3.left);
            // currentTarget.Rotate (-objectUp * xrot);
            // currentTarget.Rotate (-objectLeft * yrot);
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            currentGOTransform.transform.localScale = currentGOTransform.localScale * (1 + Input.GetAxis("Mouse ScrollWheel"));
        }
    }

}
