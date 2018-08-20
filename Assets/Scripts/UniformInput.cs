using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniformInput : MonoBehaviour {

    static UniformInput _instance;
    public static UniformInput Instance { get { return _instance; } }
    [SerializeField]
    VRViewCameraController vrController;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }

    #region zoom
    public float GetZoomAmount()
    {
#if UNITY_EDITOR
        return GetZoomAmountPC();
#else
        return GetZoomAmountMobile();
#endif
    }

    float GetZoomAmountPC()
    {
        return Input.GetAxis("Mouse ScrollWheel");
    }

    float GetZoomAmountMobile()
    {
        if (Input.touchCount > 1)
        {
            var currentPosition0 = Input.touches[0].position;
            var currentPosition1 = Input.touches[1].position;
            var previousPosition0 = currentPosition0 - Input.touches[0].deltaPosition;
            var previousPosition1 = currentPosition1 - Input.touches[1].deltaPosition;
            var currentDistance = Vector2.Distance(currentPosition0, currentPosition1);
            var previousDistance = Vector2.Distance(previousPosition0, previousPosition1);
            return currentDistance / previousDistance;
        }

        return 0;
    }
    #endregion

    #region mouseX
    public float GetMouseX()
    {
#if UNITY_EDITOR
        return GetMouseXPC();
#else
        return GetMouseXMobile();
#endif
    }

    float GetMouseXPC()
    {
        return Input.GetAxis("Mouse X");
    }

    float GetMouseXMobile()
    {
        if (Input.touchCount > 0)
        {
            return Input.touches[0].deltaPosition.x;
        }
        return 0;
    }
    #endregion

    #region mouseY
    public float GetMouseY()
    {
#if UNITY_EDITOR
        return GetMouseYPC();
#else
        return GetMouseYMobile();
#endif
    }

    float GetMouseYPC()
    {
        return Input.GetAxis("Mouse Y");
    }

    float GetMouseYMobile()
    {
        if(Input.touchCount > 0)
        {
            return Input.touches[0].deltaPosition.y;
        }
        return 0;
    }
    #endregion

    public bool GetPressDown()
    {
        if (vrController.IsDoingSwitch) return false;
#if UNITY_EDITOR
        return GetPressDownPC();
#else
        return GetPressDownMobile();
#endif
    }
    bool GetPressDownPC()
    {
        return Input.GetMouseButtonDown(0);
    }
    bool GetPressDownMobile()
    {
        return (Input.touchCount > 0 && (Input.touches[0].phase == TouchPhase.Began));
    }

    public bool GetPressUp()
    {
        if (vrController.IsDoingSwitch) return false;
#if UNITY_EDITOR
        return GetPressUpPC();
#else
        return GetPressUpMobile();
#endif
    }
    bool GetPressUpPC()
    {
        return Input.GetMouseButtonUp(0);
    }
    bool GetPressUpMobile()
    {
        return (Input.touchCount > 0 && (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled));
    }

    public bool GetPress()
    {
        if (vrController.IsDoingSwitch) return false;
#if UNITY_EDITOR
        return GetPressPC();
#else
        return GetPressMobile();
#endif
    }
    bool GetPressPC()
    {
        return Input.GetMouseButton(0);
    }
    bool GetPressMobile()
    {
        return Input.touchCount > 0;
    }

    public Vector2 GetPressPosition()
    {
#if UNITY_EDITOR
        return GetPressPositionPC();
#else
        return GetPressPositionMobile();
#endif
    }
    Vector2 GetPressPositionPC()
    {
        var position = Input.mousePosition;
        return new Vector2(position.x, position.y);
    }
    Vector2 GetPressPositionMobile()
    {
        return Input.touches[0].position;
    }
}
