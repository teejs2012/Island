using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    //static UIManager instance;
    //public static UIManager Instance
    //{
    //    get { return instance; }
    //}
    [SerializeField]
    Button InteractableViewBackButton;
    [SerializeField]
    GameController gameController;
    //void Awake()
    //{
    //    if(instance == null)
    //    {
    //        instance = this;
    //    }
    //}

    void Start()
    {
        InteractableViewBackButton.gameObject.SetActive(false);
        InteractableViewBackButton.onClick.AddListener(() => { gameController.SwitchToNormalView(); });
    }

    public void ShowInteractableViewUI()
    {
        InteractableViewBackButton.gameObject.SetActive(true);
    }

    public void HideInteractableViewUI()
    {
        InteractableViewBackButton.gameObject.SetActive(false);
    }
}
