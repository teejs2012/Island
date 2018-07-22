using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigitLockSystem : MonoBehaviour {
    [Header("Lock UI")]
    [SerializeField]
    RectTransform UpArrowContainer;
    [SerializeField]
    RectTransform DownArrowContainer;
    [SerializeField]
    RectTransform NumberContainer;
    [SerializeField]
    GameObject UpArrow;
    [SerializeField]
    GameObject DownArrow;
    [SerializeField]
    GameObject NumberItem;

    [Header("Lock 3D")]
    [SerializeField]
    Transform locktop;
    [SerializeField]
    Material lockMaterial;
    [SerializeField]
    float lockedPosY;
    [SerializeField]
    float unlockedPosY;
    [SerializeField]
    int lockedRotY;
    [SerializeField]
    int unlockedRotY;
    [SerializeField]
    GameObject threeButtons;
    [SerializeField]
    GameObject fourButtons;
    [SerializeField]
    GameObject fiveButtons;

    [Header("Others")]
    [SerializeField]
    UIManager UIManager;
    string targetNumber;

    List<Button> UpArrowButtons = new List<Button>();
    List<Button> DownArrowButtons = new List<Button>();
    List<Text> NumberTexts = new List<Text>();

    DigitLockData currentDigitLock;
	
    void OnUpArrowClicked(int i)
    {
        int currentNumber = Convert.ToInt32(NumberTexts[i].text);
        currentNumber++;
        if (currentNumber > 9)
        {
            currentNumber = 0;
        }
        NumberTexts[i].text = currentNumber.ToString();
        CheckNumber();
    }

    void OnDownArrowClicked(int i)
    {
        int currentNumber = Convert.ToInt32(NumberTexts[i].text);
        currentNumber--;
        if (currentNumber < 0)
        {
            currentNumber = 9;
        }
        NumberTexts[i].text = currentNumber.ToString();
        CheckNumber();
    }

    void CheckNumber()
    {
        StringBuilder currentFullNumber = new StringBuilder();
        foreach(var c in NumberTexts)
        {
            currentFullNumber.Append(c.text);
        }
        if (targetNumber.Equals(currentFullNumber.ToString()))
        {
            DoUnlock();
        }
    }

    void HideAllButtons()
    {
        threeButtons.SetActive(false);
        fourButtons.SetActive(false);
        fiveButtons.SetActive(false);
    }

    public void Initialize(DigitLockData digitLock)
    {
        if(currentDigitLock != null && currentDigitLock == digitLock)
        {
            Debug.Log("same lock");
            return;
        }

        currentDigitLock = digitLock;
        this.targetNumber = currentDigitLock.TargetNumber;
        int length = targetNumber.Length;

        HideAllButtons();
        switch (length)
        {
            case 3:
                threeButtons.SetActive(true);
                break;
            case 4:
                fourButtons.SetActive(true);
                break;
            case 5:
                fiveButtons.SetActive(true);
                break;
        }

        UpArrowButtons.Clear();
        DownArrowButtons.Clear();
        NumberTexts.Clear();
        foreach(RectTransform child in UpArrowContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (RectTransform child in DownArrowContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (RectTransform child in NumberContainer)
        {
            Destroy(child.gameObject);
        }

        UpArrow.SetActive(true);
        DownArrow.SetActive(true);
        NumberItem.SetActive(true);
        for (int i = 0; i < length; i++)
        {
            GameObject upArrowGO =  Instantiate(UpArrow, UpArrowContainer);
            var upButton = upArrowGO.GetComponent<Button>();
            if (upButton != null)
            {
                UpArrowButtons.Add(upButton);
                upButton.onClick.AddListener(delegate { OnUpArrowClicked(UpArrowButtons.IndexOf(upButton)); });
            }

            GameObject downArrowGO = Instantiate(DownArrow, DownArrowContainer);
            var downButton = downArrowGO.GetComponent<Button>();
            if (downButton != null)
            {
                DownArrowButtons.Add(downButton);
                downButton.onClick.AddListener(delegate { OnDownArrowClicked(DownArrowButtons.IndexOf(downButton)); });
            }

            GameObject numberItemGO =  Instantiate(NumberItem, NumberContainer);
            var text = numberItemGO.GetComponent<Text>();
            if (text != null)
            {
                NumberTexts.Add(text);
            }
        }

        UpArrow.SetActive(false);
        DownArrow.SetActive(false);
        NumberItem.SetActive(false);

        foreach (var c in NumberTexts)
        {
            c.text = "0";
        }
        lockMaterial.SetFloat("Vector1_37B9DF73", 0);
        locktop.position.Set(locktop.position.x, lockedPosY, locktop.position.z);
        locktop.eulerAngles.Set(locktop.eulerAngles.x, lockedRotY, locktop.eulerAngles.z);
    }

    void DoUnlock()
    {
        LeanTween.moveLocalY(locktop.gameObject, unlockedPosY, 0.3f).setEaseOutElastic().setOnComplete(
                () =>
                {
                    LeanTween.rotateY(locktop.gameObject, unlockedRotY, 0.4f).setEaseInCirc();
                    LeanTween.value(0, 1, 0.6f).setOnUpdate(
                       (float x) =>
                       {
                           lockMaterial.SetFloat("Vector1_37B9DF73", x);
                       }
                   ).setOnComplete(
                        ()=>
                        {
                            UIManager.ClickBackButton();
                            currentDigitLock.TargetLockable.Unlock();
                            currentDigitLock = null;
                        });
                }
            );
    }
}
