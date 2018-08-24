using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
public class InteractableWindow : EditorWindow {

    [MenuItem("TJS/Set Interactable Params")]
    public static void SetInteractableParams()
    {
        GetWindow<InteractableWindow>("Set Interactable");
    }

    GameObject currentGO;
    Vector3 originalGOPosition;
    Quaternion originalGORotation;
    Vector3 originalGOScale;
    Transform originalParent;
    bool isDoingCustomization = false;
    private void OnGUI()
    {
        if (GUILayout.Button("Customize"))
        {
            if(Selection.gameObjects.Length == 0)
            {
                return;
            }
            if (isDoingCustomization)
                return;
            currentGO = Selection.gameObjects[0];
            originalGOPosition = currentGO.transform.position;
            originalGORotation = currentGO.transform.rotation;
            originalGOScale = currentGO.transform.localScale;
            originalParent = currentGO.transform.parent;
            var data = currentGO.GetComponent<InteractableData>();
            if(data == null)
            {
                currentGO.transform.position = new Vector3(100, 100, 100);
            }
            else
            {
                currentGO.transform.SetParent(null);
                currentGO.transform.position = data.Position;
                currentGO.transform.rotation = data.Rotation;
                currentGO.transform.localScale = data.Scale;

            }
            Camera.main.transform.position = new Vector3(100, 100, 99);
            Camera.main.transform.rotation = Quaternion.identity;
            Camera.main.transform.localScale = Vector3.one;
            SceneView.lastActiveSceneView.FrameSelected();
            isDoingCustomization = true;
        }

        if(GUILayout.Button("Save"))
        {
            currentGO.tag = "Interactable";
            var data = currentGO.GetComponent<InteractableData>();
            if(data == null)
            {
                data = currentGO.AddComponent<InteractableData>();
            }

            data.Position = currentGO.transform.position;
            data.Rotation = currentGO.transform.rotation;
            data.Scale = currentGO.transform.localScale;
        }

        if (GUILayout.Button("Back"))
        {
            if (!isDoingCustomization)
                return;
            currentGO.transform.SetParent(originalParent);
            currentGO.transform.position = originalGOPosition;
            currentGO.transform.rotation = originalGORotation;
            currentGO.transform.localScale = originalGOScale;
            SceneView.lastActiveSceneView.FrameSelected();
            isDoingCustomization = false;
        }
    }
}

public class DevelopmentViewMode
{
    const string ARCameraContainer = "ARCameraContainer";
    const string AREditorCamera = "AREditorCamera";
    const string ARVuforiaCamera = "ARVuforiaCamera";
    const string EditorViewParent = "EditorViewParent";
    const string ImageTargetParent = "ImageTargetParent";


    [MenuItem("TJS/Development View/Vuforia View")]
    public static void SetDevelopmentARView()
    {
        GameObject cameraContainer = GameObject.Find(ARCameraContainer);
        foreach(Transform child in cameraContainer.transform)
        {
            if (child.name.Equals(AREditorCamera))
            {
                child.gameObject.SetActive(false);
                child.tag = Tags.Untagged;
            }
            if (child.name.Equals(ARVuforiaCamera))
            {
                child.gameObject.SetActive(true);
                child.tag = Tags.MainCameraTag;
            }
        }

        GameObject editorViewParent = GameObject.Find(EditorViewParent);
        GameObject imageTargetParent = GameObject.Find(ImageTargetParent);
        Dictionary<string, Transform> imageTargetTransforms = new Dictionary<string, Transform>(); 
        foreach (Transform child in imageTargetParent.transform)
        {
            //Debug.Log(child.name);
            imageTargetTransforms.Add(child.name, child);
        }

        Dictionary<Transform, Transform> childParentTransformsPair = new Dictionary<Transform, Transform>();
        foreach(Transform child in editorViewParent.transform)
        {
            if (imageTargetTransforms.ContainsKey(child.name))
            {
                childParentTransformsPair.Add(child, imageTargetTransforms[child.name]);
            } 
        }

        foreach(var pair in childParentTransformsPair)
        {
            pair.Key.SetParent(pair.Value);
            pair.Value.SetParent(null);
            pair.Value.gameObject.SetActive(true);
        }
    }

    [MenuItem("TJS/Development View/Editor View")]
    public static void SetDevelopmentEditorView()
    {
        GameObject cameraContainer = GameObject.Find(ARCameraContainer);
        foreach (Transform child in cameraContainer.transform)
        {
            if (child.name.Equals(ARVuforiaCamera))
            {
                child.gameObject.SetActive(false);
                child.tag = Tags.Untagged;
            }
            if (child.name.Equals(AREditorCamera))
            {
                child.gameObject.SetActive(true);
                child.tag = Tags.MainCameraTag;
            }
        }

        GameObject editorViewParent = GameObject.Find(EditorViewParent);
        GameObject imageTargetParent = GameObject.Find(ImageTargetParent);
        foreach (var trackable in GameObject.FindObjectsOfType<DefaultTrackableEventHandler>())
        {
            trackable.transform.GetChild(0).SetParent(editorViewParent.transform);
            trackable.transform.SetParent(imageTargetParent.transform);
            trackable.gameObject.SetActive(false);
        }
    }
}

public class FileManagement
{
    const string directoryOfPrefabToOrganize = "Prefabs";
    const string directoryNewPrefabSubFolder = "New";
    const string targetMaterialParent = "TJS";
    [MenuItem("TJS/Organize Materials")]
    public static void OrganizeMaterials()
    {
        string pathToDirectoryOfPrefabToOrganize = Path.Combine(Application.dataPath, directoryOfPrefabToOrganize);
        if (!Directory.Exists(pathToDirectoryOfPrefabToOrganize)) return;

        DirectoryInfo dInfoOfPrefabToOrganize = new DirectoryInfo(pathToDirectoryOfPrefabToOrganize);
        foreach(var prefabFileToOrganize in dInfoOfPrefabToOrganize.GetFiles())
        {
            if (prefabFileToOrganize.Name.Contains(".meta")) continue; //skip meta files
            string fileNameWithoutExtension = prefabFileToOrganize.Name.Split('.')[0];
            string targetMaterialPath = Application.dataPath + "/" + targetMaterialParent + "/" + fileNameWithoutExtension;
            
            if (!Directory.Exists(targetMaterialPath))
            {
                Directory.CreateDirectory(targetMaterialPath);
            }
            DirectoryInfo dinfoOfTargetMaterialPath = new DirectoryInfo(targetMaterialPath);


            HashSet<string> nonRepeatedDependency = new HashSet<string>();
            Dictionary<string, string> GUIDPair = new Dictionary<string, string>();
            
            foreach (string dependencyFile in AssetDatabase.GetDependencies("Assets" +"/"+ dInfoOfPrefabToOrganize.Name+"/"+prefabFileToOrganize.Name, true))
            {
                if (!nonRepeatedDependency.Contains(dependencyFile))
                {
                    nonRepeatedDependency.Add(dependencyFile);
                }
                else
                {
                    continue;
                }

                if (!File.Exists(dependencyFile)) return;
                FileInfo finfoOfDependencyFile = new FileInfo(dependencyFile);
                if (finfoOfDependencyFile.Extension.Equals(".mat"))
                {
                    //skip if it is already in the target material folder
                    if (finfoOfDependencyFile.Directory.Equals(dinfoOfTargetMaterialPath)) continue;

                    string targetFileName = targetMaterialPath + "/" + finfoOfDependencyFile.Name;
                    while (File.Exists(targetFileName))
                    {
                        targetFileName = targetMaterialPath + "/" + finfoOfDependencyFile.Name.Split('.')[0] + "0" + finfoOfDependencyFile.Extension;
                    }
                    string relativeTargetFilePath = targetFileName.Substring(targetFileName.IndexOf("Assets"));
                    AssetDatabase.CopyAsset(dependencyFile, relativeTargetFilePath);
                    GUIDPair.Add(AssetDatabase.AssetPathToGUID(dependencyFile), AssetDatabase.AssetPathToGUID(relativeTargetFilePath));
                }
            }

            string prefabContent = File.ReadAllText(prefabFileToOrganize.FullName);
            foreach(var pair in GUIDPair)
            {
                Debug.Log(string.Format("key is: {0} ; value is : {1};", pair.Key, pair.Value));
                if (prefabContent.Contains(pair.Key))
                {
                    Debug.Log("prefabcontent contains key :" + pair.Key);
                    prefabContent = prefabContent.Replace(pair.Key, pair.Value);
                }
            }
            File.WriteAllText(pathToDirectoryOfPrefabToOrganize+"/"+directoryNewPrefabSubFolder +"/" + prefabFileToOrganize.Name, prefabContent);
        }

    }
}

#endif
