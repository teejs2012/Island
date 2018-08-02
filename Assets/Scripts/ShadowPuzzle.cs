using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPuzzle : MonoBehaviour {

    bool isDragging = false;
    Camera currentCam;
    float currentMouseX = 0;
    float currentMouseY = 0;
    [SerializeField]
    bool invertX;
    //[SerializeField]
    //bool invertZ;
    [SerializeField]
    float dragEffect;

    public void StartDrag(Camera cam)
    {

        isDragging = true;
        currentCam = cam;
        currentMouseX = Input.mousePosition.x;
        currentMouseY = Input.mousePosition.y;
    }

    void Update()
    {
        if (isDragging)
        {
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                return;
            }

            float deltaX = Input.mousePosition.x - currentMouseX;
            //float deltaY = Input.mousePosition.y - currentMouseY;
            currentMouseX = Input.mousePosition.x;
            //currentMouseY = Input.mousePosition.y;

            

            //Vector3 movementInWorld = currentCam.transform.TransformVector(new Vector3(deltaX, deltaY, 0));
            //Vector3 movementInLocal = transform.parent.InverseTransformVector(movementInWorld);
            float dragValue = 0;
            dragValue = deltaX * (invertX ? -1 : 1) * dragEffect;

            //if (Mathf.Abs(movementInWorld.x) > Mathf.Abs(movementInWorld.z))
            //{
            //    float invert = invertX ? -1 : 1;
            //    dragValue = movementInWorld.x * invert;
            //}
            //else
            //{
            //    float invert = invertZ ? -1 : 1;
            //    dragValue = movementInWorld.z * invert;
            //}

            float targetAngle = transform.localEulerAngles.y + dragValue * dragEffect;
            if (targetAngle > 360) targetAngle -= 360;
            else if (targetAngle < 0) targetAngle += 360;
            transform.localEulerAngles = transform.localEulerAngles.SetY(targetAngle);
        }   
    }
}
