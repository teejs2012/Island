//
//
//COPYRIGHT NICK PEELMAN
//

using UnityEngine;
using System.Collections;
using UnityEditor;
using Wireless_Remote;

[CustomEditor(typeof(WirelessInputController))]
public class WirelessInputInspector : Editor {


	DeviceData deviceData = null;

	public override void OnInspectorGUI ()
	{
		EditorUtility.SetDirty( target );

		GUI.skin.label.richText = true;


		if(Event.current.type != EventType.Repaint)
		{
			deviceData = WirelessInputController.DeviceData;
		}

		if(deviceData == null) return;


		GUILayout.BeginVertical("box");
		GUILayout.BeginHorizontal();
		//GUILayout.FlexibleSpace();
		GUILayout.Label("<b><size=12>Enable module</size></b>");
		//GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		(target as WirelessInputController).enabled = EditorGUILayout.Toggle((target as WirelessInputController).enabled);
		GUILayout.EndVertical();

		GUILayout.BeginVertical("box");
		GUILayout.BeginHorizontal();
		//GUILayout.FlexibleSpace();
		GUILayout.Label("<b><size=12>Device Resolution</size></b>");
		//GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		GUILayout.Label(deviceData.screenRes.ToString());
		GUILayout.EndVertical();

		GUILayout.BeginVertical("box");
		GUILayout.BeginHorizontal();
		//GUILayout.FlexibleSpace();
		GUILayout.Label("<b><size=12>Gyro data</size></b>");
		//GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		GUILayout.Label(deviceData.GyroData.ToString());
		GUILayout.EndVertical();

		GUILayout.BeginVertical("box");
		GUILayout.BeginHorizontal();
		//GUILayout.FlexibleSpace();
		GUILayout.Label("<b><size=12>Accelero data</size></b>");
		//GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		GUILayout.Label(deviceData.AcceleroData.ToString());
		GUILayout.EndVertical();

		GUILayout.BeginVertical("box");
		GUILayout.BeginHorizontal();
		//GUILayout.FlexibleSpace();
		GUILayout.Label("<b><size=12>Touch data</size></b>");
		//GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		for(int i = 0; i < deviceData.TouchData.Length; i++)
		{
			GUILayout.BeginHorizontal();
			//GUILayout.FlexibleSpace();
			GUILayout.Label("<size= 10>Touch " + (i + 1).ToString() + "</size>");
			//GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			GUILayout.Label("Phase: " + deviceData.TouchData[i].phase.ToString());
			GUILayout.Label("Position: " + deviceData.TouchData[i].position.ToString());
		}
		GUILayout.EndVertical();
	}
}
