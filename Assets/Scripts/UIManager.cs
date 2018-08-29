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

    [Header("Camera Frame")]
    [SerializeField]
    GameObject cameraFrame;
    [SerializeField]
    Text targetSceneText;

    void Start()
    {
        BackButton.gameObject.SetActive(false);

        book = bookVolume.GetComponentInChildren<Book>();
        bookVolume.SetActive(false);
    }

    public void HideCameraFrame()
    {
        cameraFrame.SetActive(false);
        targetSceneText.text = string.Empty;
    }

    public void ShowCameraFrame(string targetSceneName)
    {
        cameraFrame.SetActive(true);
        targetSceneText.text = targetSceneName;
    }

    public void ShowBookUI(Sprite[] pages, Sprite bookBack) 
    {
        book.Initialize(pages, bookBack);
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
                LeanTween.alpha(fadeWhiteOverlay.rectTransform, 0, 0.5f);
                if (action != null)
                {
                    action.Invoke();
                }
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
