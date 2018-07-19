//
//
//COPYRIGHT NICK PEELMAN
//
//
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using System.Threading;
using Wireless_Remote;

[InitializeOnLoad]
public class RemoteWireless : EditorWindow {

	private enum MyState {refreshing, WaitingForConnection, ReadyForConnection, Connecting, Connected, Closing}

	private static RemoteWireless window;

	private WirelessSettings settings;

	private Vector2 scrollPosDeviceList = Vector2.zero, scrollPosSettings = Vector2.zero;


	private static MyState myState = MyState.refreshing;

	private static Dictionary<string, string> broadcastingDevices = new Dictionary<string, string>();
	private static string connectedDeviceName;
	private static string connectedDeviceIP;

	private int selectedToolbar = 0;
	/*
	static RemoteWireless()
	{
		EditorApplication.update += OnUpdate;
	}

	static void OnUpdate()
	{
		
	}*/

	// Add menu named "My Window" to the Window menu
	[MenuItem ("Window/Wireless Remote")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		window = (RemoteWireless)EditorWindow.GetWindow (typeof (RemoteWireless));

		window.titleContent.text = "Wireless Remote";
		window.Show();
		if(!Application.isPlaying)
		{
			DiscoverDevices();
		}
	}

	//on enabeling, we load our previous settings
	void OnEnable()
	{
		LoadState();
		ConnectionController.onDevicesDiscovered += OnDevicesDiscovered;
		settings = WirelessSettings.GetSettings();
	}

	//on disabeling, we need to save out settings.
	void OnDisable()
	{
		SaveState();
		WirelessSettings.SaveSettings(settings);
	}

	//saves the current state of the connection window
	void SaveState()
	{
		EditorPrefs.SetInt("RemoteWindowState", (int)myState);
	}

	//load state of the connection window and current device
	void LoadState()
	{
		connectedDeviceName = ConnectionSettings.GetSettings().DeviceName;
		myState = (MyState)EditorPrefs.GetInt("RemoteWindowState", 0);

	}
		
	//Discover devices
	public static void DiscoverDevices()
	{
		ConnectionController.DiscoverDevices();
	} 

	/// Add Devices to the list which we can connect to.
	public static void OnDevicesDiscovered(System.Net.IPAddress ip, string deviceName)
	{
		if(!broadcastingDevices.ContainsKey(deviceName))
			broadcastingDevices.Add(deviceName, ip.ToString());
		if(myState == MyState.refreshing) myState = MyState.WaitingForConnection;
	}

	//We need to repaint the window every update or else the user won't be updated isntantly.
	public void Update()
	{
		Repaint();
	}

	void OnGUI () {
		GUI.skin.label.richText = true;

		//The buttons on top
		BeginHorCentered();
		selectedToolbar = GUILayout.Toolbar(selectedToolbar, new string[]{"Connection", "Settings"}, GUILayout.Height(20), GUILayout.Width(200));
		EndHorCentered();

		//The window below
		GUILayout.BeginVertical("box");
		if(selectedToolbar == 0)
		{
			switch(myState)
			{
			case MyState.refreshing:
				ShowDeviceList();
				break;
			case MyState.WaitingForConnection:
				ShowDeviceList();
				break;
			case MyState.ReadyForConnection:
				ShowReadyForConnection();
				break;
			case MyState.Connecting:
				ShowConnecting();
				break;
			case MyState.Connected:
				ShowConnected();
				break;
			}
		}
		else if(selectedToolbar == 1)
		{
			ShowSettings();
		}

		GUILayout.EndVertical();
	}

	void ShowSettings()
	{
		scrollPosSettings = GUILayout.BeginScrollView(scrollPosSettings);

		GUILayout.BeginVertical();

		BeginHorCentered();
		GUILayout.Label("<size=20><b>Settings</b></size>");
		EndHorCentered();

		//QUALITY
		GUILayout.BeginVertical("box");
		GUILayout.BeginHorizontal();
		GUILayout.Label("<b>Screenshare quality</b>", GUILayout.Width(400));
		GUILayout.FlexibleSpace();
		settings.ScreenShareQuality = EditorGUILayout.IntSlider(settings.ScreenShareQuality, 1, 100);
		GUILayout.EndHorizontal();

		BeginHorCentered();
		GUILayout.Label("<i>Compression quality of the screenshare, 1 is worst, 100 is best. Lower values have less lagg, but worse quality.</i>");
		EndHorCentered();
		GUILayout.EndVertical();

		//ORIENTATION
		GUILayout.Space(5);
		GUILayout.BeginVertical("box");
		GUILayout.BeginHorizontal();
		GUILayout.Label("<b>Device Orientation</b>", GUILayout.Width(400));
		GUILayout.FlexibleSpace();
		settings.DeviceOrientation = (ScreenOrientation)EditorGUILayout.EnumPopup(settings.DeviceOrientation);
		GUILayout.EndHorizontal();

		BeginHorCentered();
		GUILayout.Label("<i>The orientation the remote controller is set to.</i>");
		EndHorCentered();
		GUILayout.EndVertical();


		//TEST CONNECTION
		/*
		GUILayout.Space(5);
		GUILayout.BeginVertical("box");
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();

		if(GUILayout.Button("Test Connection"))
		{
			
		}

		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();*/

		//INFO
		GUILayout.FlexibleSpace();

		BeginHorCentered();
		GUILayout.Label("<b>If you have any questions/feedback, please contact me at nick.peelman@gmail.com</b>");
		EndHorCentered();

		GUILayout.EndVertical();
		GUILayout.EndScrollView();
	}

	public void ShowConnected()
	{
		ShowFirstScrollView();
		BeginHorCentered();
		GUILayout.Label("<size=20><b>Connected to " + connectedDeviceName + "</b></size>");
		EndHorCentered();

		EndFirstScrollView();

		BeginHorCentered();

		if(GUILayout.Button("Disconnect", GUILayout.Width(100)))
		{
			ConnectionSettings.RemoteSettings();
			ConnectionController.CloseConnection();
			myState = MyState.refreshing;
			DiscoverDevices();
		}
		EndHorCentered();


		GUILayout.EndVertical();
	}

	public void ShowConnecting()
	{
		ShowFirstScrollView();
		BeginHorCentered();
		GUILayout.Label("<size=20><b>Connecting to " + connectedDeviceName + "</b></size>");
		EndHorCentered();

		EndFirstScrollView();

		BeginHorCentered();

		if(GUILayout.Button("Cancel", GUILayout.Width(70)))
		{
			ConnectionSettings.RemoteSettings();
			ConnectionController.CloseConnection();
			myState = MyState.refreshing;
			DiscoverDevices();
		}
		EndHorCentered();


		GUILayout.EndVertical();
	}


	public void ShowReadyForConnection()
	{
		ShowFirstScrollView();
		BeginHorCentered();
		GUILayout.Label("<size=20><b>Ready for connection with " + connectedDeviceName + "</b></size>");
		EndHorCentered();

		BeginHorCentered();
		GUILayout.Label("<size=12>When you press play, a connection with the device will be established.</size>");
		EndHorCentered();

		EndFirstScrollView();

		BeginHorCentered();

		if(GUILayout.Button("Choose other device", GUILayout.Width(150)))
		{
			ConnectionSettings.RemoteSettings();
			ConnectionController.CloseConnection();
			myState = MyState.refreshing;
			DiscoverDevices();
		}
		EndHorCentered();


		GUILayout.EndVertical();
	}

	public void ShowDeviceList()
	{
		ShowFirstScrollView();
		BeginHorCentered();
		GUILayout.Label("<size=20><b>Discovered devices: " + broadcastingDevices.Count.ToString() + "</b></size>");
		EndHorCentered();

		foreach(var item in broadcastingDevices)
		{
			BeginHorCentered();
			if(GUILayout.Button(item.Key, GUILayout.Width(150)))
			{
				connectedDeviceName = item.Key;
				connectedDeviceIP = item.Value;
				myState = MyState.ReadyForConnection;
				ConnectionSettings.SaveSettings(connectedDeviceName, connectedDeviceIP);
				if(Application.isPlaying)
				{
					ConnectionController.CreateConnectionOnPlay();
				}
			}
			EndHorCentered();
		}

		EndFirstScrollView();

		BeginHorCentered();

		if(broadcastingDevices.Count != 0)
		{
			if(GUILayout.Button("Refresh", GUILayout.Width(70)))
			{
				broadcastingDevices.Clear();
				DiscoverDevices();
			}
		}
		else
		{
			GUILayout.Label("<i>Searching for devices...</i>");
		}
		EndHorCentered();


		GUILayout.EndVertical();
	}
		

	private void ShowFirstScrollView()
	{
		GUILayout.BeginVertical();
		scrollPosDeviceList = GUILayout.BeginScrollView(scrollPosDeviceList);
	}

	private void EndFirstScrollView()
	{
		GUILayout.EndScrollView();
	}

	#region Horizontal

	private void BeginHorCentered()
	{
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
	}

	private void EndHorCentered()
	{
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}

	#endregion

	#region Vertical

	private void BeginVertCentered(GUIStyle style)
	{
		GUILayout.BeginVertical(style);
		GUILayout.FlexibleSpace();
	}

	private void BeginVertCentered()
	{
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
	}

	private void EndVertCentered()
	{
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
	}

	#endregion

	#region Field

	public void DisplayString(ref string text)
	{
		text = GUILayout.TextArea(text);
	}

	public void DisplayString(string text)
	{
		GUILayout.Label(text);
	}

	#endregion
}
