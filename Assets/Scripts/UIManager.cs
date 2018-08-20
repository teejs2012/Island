using System;
using UnityEngine;
using UnityEngine.UI;
using DentedPixel;

public class UIManager : MonoBehaviour {

    
    [SerializeField]
    Button BackButton;
    [SerializeField]
    GameObject bookVolume;
    [SerializeField]
    GameObject digitLockUI;
    [SerializeField]
    Image fadeWhiteOverlay;
    [SerializeField]
    Image fadeBlackOverlay;
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
        BackButton.gameObject.SetActive(false);

        book = bookVolume.GetComponentInChildren<Book>();
        bookVolume.SetActive(false);
    }

    public void ShowBookUI(Sprite[] pages) 
    {
        book.Initialize(pages);
        bookVolume.SetActive(true);
    }

    public void HideBookUI()
    {
        bookVolume.SetActive(false);
    }

    public void ShowDigitLockUI()
    {
        digitLockUI.SetActive(true);
    }

    public void HideDigitLockUI()
    {
        digitLockUI.SetActive(false);
    }

    public void ShowBackButton(params UnityEngine.Events.UnityAction[] actions)
    {
        BackButton.gameObject.SetActive(true);
        BackButton.onClick.RemoveAllListeners();
        foreach(var action in actions)
        {
            BackButton.onClick.AddListener(action);
        }

        BackButton.onClick.AddListener(HideBackButton);
    }

    void HideBackButton()
    {
        BackButton.gameObject.SetActive(false);
    }

    public void FadeWhiteScreen(Action action)
    {
        LeanTween.alpha(fadeWhiteOverlay.rectTransform, 1, 0.5f).setOnComplete(
            () => {
                if (action != null)
                {
                    action.Invoke();
                }
                LeanTween.alpha(fadeWhiteOverlay.rectTransform, 0, 0.5f);
            }
            );
    }

    public void FadeBlackScreen(float toValue, float time)
    {
        LeanTween.alpha(fadeBlackOverlay.rectTransform, toValue, time);
    }

    public void ClickBackButton()
    {
        BackButton.onClick.Invoke();
    }
}
