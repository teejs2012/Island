using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

public class TextHandler : MonoBehaviour {

    string fileName = "textDoc.csv";
    string path;
    Dictionary<string, string> textDictionary = new Dictionary<string, string>();

    void Start()
    {
#if UNITY_EDITOR
        //path = Application.dataPath + "/StreamingAssets/textDoc.csv";
        path = Path.Combine(Application.streamingAssetsPath, fileName);
#else
        path = Path.Combine(Application.streamingAssetsPath, fileName);
#endif
        StartCoroutine(ParseCSV());
    }

	IEnumerator ParseCSV()
    {
        WWW www = new WWW(path);
        yield return www;
        string content = www.text;
        foreach (var line in content.Split('\n'))
        { 
            string[] segments = line.Split(',');
            if (segments.Length != 2) continue;
            textDictionary[segments[0].Trim()] = segments[1];
        }
    }

    public string GetText(string key)
    {
        if (textDictionary.ContainsKey(key))
        {
            return textDictionary[key];
        }
        else
        {
            return "";
        }
    }
}
