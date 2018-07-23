using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


public class KeyLockSystem : MonoBehaviour {



    [Header("UI")]
    [SerializeField]
    Image RedKeyImage;
    [SerializeField]
    Image BlueKeyImage;

    float keyObjectMoveAnimationTime = 1;
    float keyUIAnimationTime = 1;

    List<Image> allKeyImages = new List<Image>();
    HashSet<ColorKey> activatedKeys = new HashSet<ColorKey>();
    public HashSet<ColorKey> ActivatedKeys { get { return activatedKeys; } }
    HashSet<ColorKey> usedKeys = new HashSet<ColorKey>();
    public HashSet<ColorKey> UsedKeys { get { return usedKeys; } }

    void Awake()
    {
        allKeyImages.Add(RedKeyImage);
        allKeyImages.Add(BlueKeyImage);
        Assert.IsTrue(allKeyImages.Count == (int)ColorKey.LastKey);
    }

    public void SetActivatedKey(ColorKey key)
    {
        if (activatedKeys.Contains(key))
        {
            return;
        }

        activatedKeys.Add(key);

        Image keyImage = allKeyImages[(int)key];
        keyImage.rectTransform.SetAsFirstSibling();
        keyImage.gameObject.SetActive(true);
        keyImage.fillAmount = 1;
    }

    public void SetUsedKey(ColorKey key)
    {
        if (usedKeys.Contains(key))
        {
            return;
        }

        usedKeys.Add(key);
    }

    public void ActivateKey(ColorKey key, GameObject keyGO, Camera cam)
    {
        if (activatedKeys.Contains(key))
        {
            return;
        }

        var col = keyGO.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Image keyImage = allKeyImages[(int)key];
        keyImage.rectTransform.SetAsFirstSibling();
        keyImage.gameObject.SetActive(true);
        Vector3 imageScreenPos = keyImage.rectTransform.position;

        LeanTween.move(keyGO, cam.ScreenToWorldPoint(new Vector3(imageScreenPos.x, imageScreenPos.y, 1)), keyObjectMoveAnimationTime);
        LeanTween.scale(keyGO, new Vector3(0.01f, 0.01f, 0.01f), keyObjectMoveAnimationTime).setEaseOutCirc().setOnComplete(
            ()=> {
                keyGO.SetActive(false);
                LeanTween.value(0,1,keyUIAnimationTime).setOnUpdate((float x)=> { keyImage.fillAmount = x; }).setEaseOutCubic();
            });

        //Debug.Log("anchored : "+ allKeyImages[(int)key].rectTransform.anchoredPosition +" position: " + allKeyImages[(int)key].rectTransform.position);
        activatedKeys.Add(key);
    }

    public void TryUnlock(KeyLockData data)
    {
        if (activatedKeys.Contains(data.KeyColor))
        {
            data.TargetLockable.Unlock();
            int keyInd = (int)data.KeyColor;
            activatedKeys.Remove(data.KeyColor);
            usedKeys.Add(data.KeyColor);
            LeanTween.value(1, 0, keyUIAnimationTime).setOnUpdate((float x) => { allKeyImages[keyInd].fillAmount = x; }).setEaseOutCubic().setOnComplete(
                () => { allKeyImages[keyInd].gameObject.SetActive(false); }
                );
        }
        else
        {
            data.TargetLockable.ShakeTargetLock();
        }
    }
}
