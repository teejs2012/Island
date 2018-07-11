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
    GameObject bookVolume;
    Book book;
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
        InteractableViewBackButton.onClick.AddListener(gameController.SwitchToNormalView);
        InteractableViewBackButton.onClick.AddListener(HideInteractableViewUI);

        book = bookVolume.GetComponentInChildren<Book>();
        bookVolume.SetActive(false);
    }

    public void ShowBookUI(Sprite[] pages) 
    {
        book.Initialize(pages);
        bookVolume.SetActive(true);

        InteractableViewBackButton.gameObject.SetActive(true);
        InteractableViewBackButton.onClick.AddListener(HideBook);
    }

    void HideBook()
    {
        bookVolume.SetActive(false);
        InteractableViewBackButton.onClick.RemoveListener(HideBook);
    }

    public void ShowInteractableViewUI()
    {
        InteractableViewBackButton.gameObject.SetActive(true);
    }

    void HideInteractableViewUI()
    {
        InteractableViewBackButton.gameObject.SetActive(false);
    }
}
