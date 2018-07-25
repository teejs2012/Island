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
    GameObject UpArrowPrototype;
    [SerializeField]
    GameObject DownArrowPrototype;
    [SerializeField]
    GameObject NumberItemPrototype;

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

    DigitLock currentDigitLock;
	
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

    void Prepare3D(int number)
    {
        threeButtons.SetActive(false);
        fourButtons.SetActive(false);
        fiveButtons.SetActive(false);
        switch (number)
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
    }
    void PrepareUI(int number)
    {
        UpArrowButtons.Clear();
        DownArrowButtons.Clear();
        NumberTexts.Clear();
        foreach (RectTransform child in UpArrowContainer)
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

        UpArrowPrototype.SetActive(true);
        DownArrowPrototype.SetActive(true);
        NumberItemPrototype.SetActive(true);
        for (int i = 0; i < number; i++)
        {
            GameObject upArrowGO = Instantiate(UpArrowPrototype, UpArrowContainer);
            var upButton = upArrowGO.GetComponent<Button>();
            if (upButton != null)
            {
                UpArrowButtons.Add(upButton);
                upButton.onClick.AddListener(delegate { OnUpArrowClicked(UpArrowButtons.IndexOf(upButton)); });
            }

            GameObject downArrowGO = Instantiate(DownArrowPrototype, DownArrowContainer);
            var downButton = downArrowGO.GetComponent<Button>();
            if (downButton != null)
            {
                DownArrowButtons.Add(downButton);
                downButton.onClick.AddListener(delegate { OnDownArrowClicked(DownArrowButtons.IndexOf(downButton)); });
            }

            GameObject numberItemGO = Instantiate(NumberItemPrototype, NumberContainer);
            var text = numberItemGO.GetComponent<Text>();
            if (text != null)
            {
                NumberTexts.Add(text);
            }
        }

        UpArrowPrototype.SetActive(false);
        DownArrowPrototype.SetActive(false);
        NumberItemPrototype.SetActive(false);

        foreach (var c in NumberTexts)
        {
            c.text = "0";
        }
    }
    void ResetLockAnimation()
    {
        lockMaterial.SetFloat("Vector1_37B9DF73", 0);
        locktop.position.Set(locktop.position.x, lockedPosY, locktop.position.z);
        locktop.eulerAngles.Set(locktop.eulerAngles.x, lockedRotY, locktop.eulerAngles.z);
    }

    public void Initialize(DigitLock digitLock)
    {
        if(currentDigitLock != null && currentDigitLock == digitLock)
        {
            Debug.Log("same lock");
            return;
        }

        currentDigitLock = digitLock;
        this.targetNumber = currentDigitLock.TargetNumber;
        int length = targetNumber.Length;

        Prepare3D(length);
        PrepareUI(length);
        ResetLockAnimation();

        UIManager.ShowDigitLockUI();
    }


    void DoUnlock()
    {
        UIManager.HideDigitLockUI();
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
                            currentDigitLock.Unlock();
                            currentDigitLock = null;
                        });
                }
            );
    }
}
