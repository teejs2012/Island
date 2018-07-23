using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

public class TextHandler : MonoBehaviour {

    string path;
    Dictionary<string, string> textDictionary = new Dictionary<string, string>();

    void Start()
    {
        path = Application.dataPath + "/StreamingAssets/textDoc.csv";
        ParseCSV();
    }

	void ParseCSV()
    {
        if (!File.Exists(path)) return;
        foreach(var line in File.ReadAllLines(path))
        {
            string[] segments = line.Split(',');
            Assert.IsTrue(segments.Length == 2);
            textDictionary[segments[0]] = segments[1];
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
