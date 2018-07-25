using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyExtensions {

	public static Vector3 SetX(this Vector3 v, float value)
    {
        return new Vector3(value, v.y, v.z);
    }

    public static Vector3 SetY(this Vector3 v, float value)
    {
        return new Vector3(v.x, value, v.z);
    }

    public static Vector3 SetZ(this Vector3 v, float value)
    {
        return new Vector3(v.x, v.y, value);
    }
}
