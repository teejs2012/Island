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

    //[SerializeField]
    //KeyLockSystem KeyLockSystem;

    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        DontDestroyOnLoad(gameObject);
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
        Save();
    }

    public bool CheckTrigger(string name)
    {
        if (data.TriggeredObjects.Contains(name))
        {
            return true;
        }
        return false;
    }

    public void RegisterAsActiveKeys(int keyColor)
    {
        data.ActiveKeys.Add(keyColor);
        Save();
    }

    public void RegisterAsUsedKeys(int keyColor)
    {
        data.UsedKeys.Add(keyColor);
        Save();
    }

    public List<int> GetActiveKeys()
    {
        return data.ActiveKeys;
    }

    public List<int> GetUsedKeys()
    {
        return data.UsedKeys;
    }

    //public void RegisterAsOnOffStatusObject(string name, bool isOpen)
    //{
    //    //Debug.Log("Registering as openable");
    //    if (data.OnOffStatusObjects.ContainsKey(name))
    //    {
    //        data.OnOffStatusObjects[name] = isOpen;
    //    }
    //    else
    //    {
    //        data.OnOffStatusObjects.Add(name, isOpen);
    //    }
    //    Save();
    //}

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
        data =  JsonConvert.DeserializeObject<GameData>(dataString);

        //foreach (var triggerableName in data.TriggeredObjects)
        //{
        //    var triggerableGO = GameObject.Find(triggerableName);
        //    //Debug.Log("Find triggerable with name : " + triggerableName + "; found? : " + (triggerableGO != null).ToString());
        //    if (triggerableGO != null)
        //    {
        //        var triggerable = triggerableGO.GetComponent<OneTimeTrigger>();
        //        if (triggerable != null)
        //        {
        //            triggerable.Trigger();
        //        }

        //        //bad practise.. 
        //        //overlimitpositionopenable is also a traggerable but not inheret from onetimetrigger
        //        var overlimit = triggerableGO.GetComponent<OverLimitPositionOpenable>();
        //        if(overlimit != null)
        //        {
        //            overlimit.Trigger();
        //        }
        //    }
        //}

        //foreach(var pair in data.OnOffStatusObjects)
        //{
        //    var onOffStatusObjectGO = GameObject.Find(pair.Key);
        //    if(onOffStatusObjectGO != null)
        //    {
        //        var onOffStatusObject = onOffStatusObjectGO.GetComponent<OnOffStatusObject>();
        //        if(onOffStatusObject != null)
        //        {
        //            onOffStatusObject.SetOpenableDataStatus(pair.Value);
        //        }
        //    }
        //}

        //if (KeyLockSystem != null)
        //{
        //    foreach (int keyColor in data.ActiveKeys)
        //    {
        //        KeyLockSystem.SetActivatedKey((ColorKey)keyColor);
        //    }

        //    foreach (int keyColor in data.UsedKeys)
        //    {
        //        KeyLockSystem.SetUsedKey((ColorKey)keyColor);
        //    }
        //}
    }
}
