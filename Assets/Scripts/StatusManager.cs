using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Newtonsoft.Json;

public class StatusManager : MonoBehaviour{
    const string fileName = "gamedata.json";

    static StatusManager _instance;
    public static StatusManager Instance { get { return _instance; } }
    GameData data = new GameData();

    [SerializeField]
    KeyLockSystem KeyLockSystem;

    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }

    void Start()
    {
        Load();
    }

    public void Reset()
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public void RegisterAsTriggeredObject(string name)
    {
        data.TriggeredObjects.Add(name);
    }

    public void RegisterAsActiveKeys(int keyColor)
    {
        data.ActiveKeys.Add(keyColor);
    }

    public void RegisterAsUsedKeys(int keyColor)
    {
        data.UsedKeys.Add(keyColor);
    }

    public void RegisterAsOpenable(string name, bool isOpen)
    {
        Debug.Log("Registering as openable");
        if (data.Openables.ContainsKey(name))
        {
            data.Openables[name] = isOpen;
        }
        else
        {
            data.Openables.Add(name, isOpen);
        }
    }

    public void Save()
    {
        string dataString = JsonConvert.SerializeObject(data);
        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(path, dataString);
    }

    public void Load()
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(path)) return;

        string dataString = File.ReadAllText(path);
        GameData data =  JsonConvert.DeserializeObject<GameData>(dataString);

        foreach (var triggerableName in data.TriggeredObjects)
        {
            var triggerableGO = GameObject.Find(triggerableName);
            //Debug.Log("Find triggerable with name : " + triggerableName + "; found? : " + (triggerableGO != null).ToString());
            if (triggerableGO != null)
            {
                var triggerable = triggerableGO.GetComponent<OneTimeTrigger>();
                if (triggerable != null)
                {
                    triggerable.Trigger();
                }
            }
        }

        foreach(var pair in data.Openables)
        {
            var openableGO = GameObject.Find(pair.Key);
            if(openableGO != null)
            {
                var openable = openableGO.GetComponent<Openable>();
                if(openable != null)
                {
                    openable.SetOpenableDataStatus(pair.Value);
                }
            }
        }

        if (KeyLockSystem != null)
        {
            foreach (int keyColor in data.ActiveKeys)
            {
                KeyLockSystem.SetActivatedKey((ColorKey)keyColor);
            }

            foreach (int keyColor in data.UsedKeys)
            {
                KeyLockSystem.SetUsedKey((ColorKey)keyColor);
            }

            foreach (var key in FindObjectsOfType<KeyData>())
            {
                if (data.ActiveKeys.Contains((int)key.KeyColor) || data.UsedKeys.Contains((int)key.KeyColor))
                {
                    Destroy(key.gameObject);
                }
            }
        }
    }

//#if UNITY_EDITOR
//    [MenuItem("TJS/GameStatus/Save")]
//    public static void EditorSave()
//    {
//        GameData data = new GameData();

//        foreach(var triggerable in GameObject.FindObjectsOfType<Triggerable>())
//        {
//            if (triggerable.IsTriggered)
//            {
//                data.TriggeredObjects.Add(triggerable.gameObject.name);
//            }
//        }

//        var keyLockSystem = GameObject.FindObjectOfType<KeyLockSystem>();
//        //Debug.Log("keylocksystem is null : " + keyLockSystem == null);
//        if(keyLockSystem != null)
//        {
//            foreach(int keyColor in keyLockSystem.ActivatedKeys)
//            {
//                data.ActiveKeys.Add(keyColor);
//            }
//            foreach(int keyColor in keyLockSystem.UsedKeys)
//            {
//                data.UsedKeys.Add(keyColor);
//            }
//        }

//        Debug.Log(data.ToString());

//        string dataString = JsonUtility.ToJson(data);

//        string path = Path.Combine(Application.persistentDataPath, fileName);
//        File.WriteAllText(path, dataString);
//    }

//    [MenuItem("TJS/GameStatus/Load")]
//    public static void Load()
//    {
//        string path = Path.Combine(Application.persistentDataPath, fileName);
//        if (File.Exists(path))
//        {
//            string dataString = File.ReadAllText(path);
//            GameData data = JsonUtility.FromJson<GameData>(dataString);
//            foreach(var triggerableName in data.TriggeredObjects)
//            {
//                var triggerableGO = GameObject.Find(triggerableName);
//                if(triggerableGO != null)
//                {
//                    var triggerable = triggerableGO.GetComponent<Triggerable>();
//                    if(triggerable != null)
//                    {
//                        triggerable.Trigger();
//                    }
//                }
//            }

//            var keyLockSystem = GameObject.FindObjectOfType<KeyLockSystem>();
//            if (keyLockSystem != null)
//            {
//                foreach(int keyColor in data.ActiveKeys)
//                {
//                    keyLockSystem.SetActivatedKey((ColorKey)keyColor);
//                }

//                foreach(int keyColor in data.UsedKeys)
//                {
//                    keyLockSystem.SetUsedKey((ColorKey)keyColor);
//                }

//                foreach(var key in GameObject.FindObjectsOfType<KeyData>())
//                {
//                    if(data.ActiveKeys.Contains((int)key.KeyColor) || data.UsedKeys.Contains((int)key.KeyColor))
//                    {
//                        GameObject.Destroy(key.gameObject);
//                    }
//                }
//            }
//        }
//    }

//    [MenuItem("TJS/GameStatus/Reset Save")]
//    public static void Reset()
//    {
//        string path = Path.Combine(Application.persistentDataPath, fileName);
//        if (File.Exists(path))
//        {
//            File.Delete(path);
//        }
//    }
//#endif
}
