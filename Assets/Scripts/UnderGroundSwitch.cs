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
            //UnderGround.SetActive(true);
        }
        else
        {
            Ground.SetActive(true);
            //UnderGround.SetActive(false);
        }
        isGround = !isGround;
    }
}
