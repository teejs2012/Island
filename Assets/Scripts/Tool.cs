using UnityEngine;
using UnityEditor;

public class InteractableWindow : EditorWindow {

	[MenuItem("Tools/Set Interactable Params")]
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
