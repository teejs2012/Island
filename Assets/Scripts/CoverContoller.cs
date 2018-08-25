using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CoverContoller : MonoBehaviour {

    [SerializeField]
    GameObject loadingBarParent;
    [SerializeField]
    Image loadingBar;  
    [SerializeField]
    Button startButton;

    void Awake()
    {
        Assert.IsTrue(startButton != null);
        Assert.IsTrue(loadingBar != null);
        startButton.onClick.AddListener(()=> { LoadScene(); });
    }

    void LoadScene()
    {
        startButton.interactable = false;
        var loadOperation = SceneManager.LoadSceneAsync("Main");
        loadingBarParent.SetActive(true);
        StartCoroutine(UpdateLoadingBar(loadOperation));
    }

    IEnumerator UpdateLoadingBar(AsyncOperation operation)
    {
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.fillAmount = progress;
            yield return null;
        }
    }
}
