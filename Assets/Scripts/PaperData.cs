using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PaperData : MonoBehaviour {

    public Canvas content;
    public List<string> keyWords = new List<string>();
    public List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
    public Material paperMaterial;
}
