using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

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

    private void OnGUI()
    {
        if (GUILayout.Button("Customize"))
        {
            if(Selection.gameObjects.Length == 0)
            {
                return;
            }
            currentGO = Selection.gameObjects[0];
            originalGOPosition = currentGO.transform.position;
            originalGORotation = currentGO.transform.rotation;
            originalGOScale = currentGO.transform.localScale;
            var data = currentGO.GetComponent<InteractableData>();
            if(data == null)
            {
                currentGO.transform.position = new Vector3(100, 100, 100);
            }
            else
            {
                currentGO.transform.position = data.Position;
                currentGO.transform.rotation = data.Rotation;
                currentGO.transform.localScale = data.Scale;

            }
            Camera.main.transform.position = new Vector3(100, 100, 99);
            Camera.main.transform.rotation = Quaternion.identity;
            Camera.main.transform.localScale = Vector3.one;
            SceneView.lastActiveSceneView.FrameSelected();
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
            currentGO.transform.position = originalGOPosition;
            currentGO.transform.rotation = originalGORotation;
            currentGO.transform.localScale = originalGOScale;
            SceneView.lastActiveSceneView.FrameSelected();
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
            imageTargetTransforms.Add(child.name, child);
            child.gameObject.SetActive(true);
            child.SetParent(null);
        }
        foreach(Transform child in editorViewParent.transform)
        {
            child.SetParent(imageTargetTransforms[child.name]);
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

#endif
