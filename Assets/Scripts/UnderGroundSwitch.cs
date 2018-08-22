using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderGroundSwitch : MonoBehaviour {

    [SerializeField]
    GameObject Ground;
    //[SerializeField]
    //GameObject UnderGround;

    bool isGround = true;

    public void Switch()
    {
        if (isGround)
        {
            Ground.SetActive(false);
        }
        else
        {
            Ground.SetActive(true);
        }
        isGround = !isGround;
    }

    void OnEnable()
    {
        if (!isGround)
        {
            Ground.SetActive(false);
        }
    }
}
