using System;
using UnityEngine;
using UnityEngine.UI;
using DentedPixel;

public class UIManager : MonoBehaviour {

    
    [SerializeField]
    Button InteractableViewBackButton;
    [SerializeField]
    GameObject bookVolume;
    [SerializeField]
    GameObject digitLockUI;
    [SerializeField]
    Image fadeOverlay;
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

    public void ShowLockUI()
    {
        digitLockUI.SetActive(true);
        InteractableViewBackButton.gameObject.SetActive(true);
        InteractableViewBackButton.onClick.AddListener(HideLockUI);
    }

    void HideLockUI()
    {
        digitLockUI.SetActive(false);
        InteractableViewBackButton.onClick.RemoveListener(HideLockUI);
    }

    public void ShowInteractableViewUI()
    {
        InteractableViewBackButton.gameObject.SetActive(true);
    }

    void HideInteractableViewUI()
    {
        InteractableViewBackButton.gameObject.SetActive(false);
    }

    public void FadeScreenTransition(Action action)
    {
        LeanTween.alpha(fadeOverlay.rectTransform, 1, 0.5f).setOnComplete(
            () => {
                if (action != null)
                {
                    action.Invoke();
                }
                LeanTween.alpha(fadeOverlay.rectTransform, 0, 0.5f);
            }
            );
    }

    public void ClickBackButton()
    {
        InteractableViewBackButton.onClick.Invoke();
    }
}
