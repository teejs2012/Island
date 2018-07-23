using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class StatusManager{


    static string fileName = "gamedata.json";

#if UNITY_EDITOR
    [MenuItem("TJS/GameStatus/Save")]
    public static void Save()
    {
        GameData data = new GameData();

        foreach(var triggerable in GameObject.FindObjectsOfType<Triggerable>())
        {
            if (triggerable.IsTriggered)
            {
                data.TriggeredObjects.Add(triggerable.gameObject.name);
            }
        }

        var keyLockSystem = GameObject.FindObjectOfType<KeyLockSystem>();
        //Debug.Log("keylocksystem is null : " + keyLockSystem == null);
        if(keyLockSystem != null)
        {
            foreach(int keyColor in keyLockSystem.ActivatedKeys)
            {
                data.ActiveKeys.Add(keyColor);
            }
            foreach(int keyColor in keyLockSystem.UsedKeys)
            {
                data.UsedKeys.Add(keyColor);
            }
        }

        Debug.Log(data.ToString());

        string dataString = JsonUtility.ToJson(data);

        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(path, dataString);
    }

    [MenuItem("TJS/GameStatus/Load")]
    public static void Load()
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(path))
        {
            string dataString = File.ReadAllText(path);
            GameData data = JsonUtility.FromJson<GameData>(dataString);
            foreach(var triggerableName in data.TriggeredObjects)
            {
                var triggerableGO = GameObject.Find(triggerableName);
                if(triggerableGO != null)
                {
                    var triggerable = triggerableGO.GetComponent<Triggerable>();
                    if(triggerable != null)
                    {
                        triggerable.Trigger();
                    }
                }
            }

            var keyLockSystem = GameObject.FindObjectOfType<KeyLockSystem>();
            if (keyLockSystem != null)
            {
                foreach(int keyColor in data.ActiveKeys)
                {
                    keyLockSystem.SetActivatedKey((ColorKey)keyColor);
                }

                foreach(int keyColor in data.UsedKeys)
                {
                    keyLockSystem.SetUsedKey((ColorKey)keyColor);
                }

                foreach(var key in GameObject.FindObjectsOfType<KeyData>())
                {
                    if(data.ActiveKeys.Contains((int)key.KeyColor) || data.UsedKeys.Contains((int)key.KeyColor))
                    {
                        GameObject.Destroy(key.gameObject);
                    }
                }
            }
        }
    }

    [MenuItem("TJS/GameStatus/Reset Save")]
    public static void Reset()
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
#endif
}
