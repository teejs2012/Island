using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyData : MonoBehaviour {

    void Start()
    {
        tag = Tags.Key;
    }

    [SerializeField]
    ColorKey keyColor;
    public ColorKey KeyColor { get { return keyColor; } }
}
