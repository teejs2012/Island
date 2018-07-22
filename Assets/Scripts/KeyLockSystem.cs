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

    void Awake()
    {
        allKeyImages.Add(RedKeyImage);
        allKeyImages.Add(BlueKeyImage);
        Assert.IsTrue(allKeyImages.Count == (int)ColorKey.LastKey);
    }

    public void ActivateKey(ColorKey key, GameObject keyGO, Camera cam)
    {
        if (activatedKeys.Contains(key))
        {
            return;
        }
        allKeyImages[(int)key].gameObject.SetActive(true);
        Vector3 imageScreenPos = allKeyImages[(int)key].rectTransform.position;

        LeanTween.move(keyGO, cam.ScreenToWorldPoint(new Vector3(imageScreenPos.x, imageScreenPos.y, 1)), keyObjectMoveAnimationTime);
        LeanTween.scale(keyGO, new Vector3(0.01f, 0.01f, 0.01f), keyObjectMoveAnimationTime).setEaseOutCirc().setOnComplete(
            ()=> {
                keyGO.SetActive(false);
                LeanTween.alpha(allKeyImages[(int)key].rectTransform, 0.75f, keyUIAnimationTime).setEaseOutBounce();
            });

        //Debug.Log("anchored : "+ allKeyImages[(int)key].rectTransform.anchoredPosition +" position: " + allKeyImages[(int)key].rectTransform.position);
        activatedKeys.Add(key);
    }

    public void TryUnlock(KeyLockData data)
    {
        if (activatedKeys.Contains(data.KeyColor))
        {
            data.TargetLockable.Unlock();
            activatedKeys.Remove(data.KeyColor);
            LeanTween.alpha(allKeyImages[(int)data.KeyColor].rectTransform, 0, keyUIAnimationTime).setEaseOutBounce().setOnComplete(
                () => { allKeyImages[(int)data.KeyColor].gameObject.SetActive(false); }
                );
        }
        else
        {
            data.TargetLockable.ShakeTargetLock();
        }
    }
}
