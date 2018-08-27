using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapersController : MonoBehaviour {

    [SerializeField]
    PaperData[] paperDataList;
    [SerializeField]
    MeshRenderer meshRenderer;
    [SerializeField]
    bool isInvisible = false;

    TextHandler handler;
    PaperData currentData;
    int currentPaperInd;
    bool beingViewed = false;

    void Start()
    {
        if (isInvisible)
        {
            meshRenderer.enabled = false;
        }
    }

    public void Init(TextHandler textHandler)
    {
        //this.gameObject.SetActive(true);
        if (isInvisible)
        {
            meshRenderer.enabled = true;
        }
        beingViewed = true;
        handler = textHandler;
        if (paperDataList.Length > 0)
        {
            currentData = paperDataList[0];
            currentPaperInd = 0;
            ShowPaper();
        }
    }

    public void NextPaper()
    {
        currentPaperInd++;
        if(currentPaperInd >= paperDataList.Length)
        {
            currentPaperInd = 0;
        }
        RefreshPaper();
    }

    //public void PreviousPaper()
    //{
    //    currentPaperInd--;
    //    if (currentPaperInd < 0)
    //    {
    //        currentPaperInd = paperDataList.Length - 1;
    //    }
    //    RefreshPaper();
    //}

    public void ExitPaperView()
    {
        if (isInvisible)
        {
            meshRenderer.enabled = false;
        }
        HidePaper();
        beingViewed = false;
    }

    void ShowPaper()
    {
        if (currentData != null)
        {
            int contentCount = currentData.texts.Count;
            for (int i = 0; i < contentCount; i++)
            {
                currentData.texts[i].text = handler.GetText(currentData.keyWords[i]);
            }
            currentData.content.gameObject.SetActive(true);

            if(currentData.paperMaterial != null)
            {
                meshRenderer.material = currentData.paperMaterial;
            }
        }
    }

    void HidePaper()
    {
        if(currentData != null)
        {
            currentData.content.gameObject.SetActive(false);
        }
    }

    void RefreshPaper()
    {
        HidePaper();
        currentData = paperDataList[currentPaperInd];
        ShowPaper();
    }

    void Update()
    {
        if (!beingViewed) return;
        if (UniformInput.Instance.GetPressDown())
        {
            NextPaper();
        }
    }
}
