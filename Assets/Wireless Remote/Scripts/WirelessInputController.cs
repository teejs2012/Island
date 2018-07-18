using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;
using System.Text;
using Wireless_Remote;
using UnityEngine.Serialization;

[AddComponentMenu("Event/Wireless Touch Input Module")]

public class WirelessInputController : PointerInputModule
{

	#region Data processing
	private static WirelessInputController Instance;

	public static WirelessWebcamDevice currentWebcamDevice;
	public static bool WebcamServiceReady = false;
	private static WebCamTexture camTexture;

	private static DeviceData data;
	private static float screenWidth, screenHeight;
	private static List<DeviceData> receivedData = new List<DeviceData>();


	private WirelessSettings settings;
	private Texture2D tex;
	private static Texture2D tempWebcamTex;
	private ScreenCapture screenCapture;
	private DesktopData desktopData;

	protected override void Awake()
	{
		base.Awake();

		if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
			this.enabled = false;
		else
		{
			if(Camera.main.GetComponent<ScreenCapture>() == null)
			{
				screenCapture = Camera.main.gameObject.AddComponent<ScreenCapture>();
			}
			else
			{
				screenCapture = Camera.main.GetComponent<ScreenCapture>();
			}
			StartCoroutine(ProcessDataPerFrame());
		}
		Instance = this;
		/*
		if(Instance == null){
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if(Instance != this){
			DestroyImmediate(this.gameObject);
			return;
		}
	*/
		settings = WirelessSettings.GetSettings();
		receivedData.Clear();
		Input.gyro.enabled = true;

		if(ConnectionController.MyConnectionState == ConnectionController.ConnectionState.NotConnected)
		{
			ConnectionController.CreateConnectionOnPlay();
			ConnectionController.onDataRead += ProcessData;
		}

	}

	void OnApplicationQuit()
	{
		ConnectionController.CloseConnection();
		//ConnectionController.StopDiscovering();
	}

	public static DeviceData DeviceData
	{
		get
		{
			if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
			{
				//add touch data
				data = new DeviceData();
				List<WirelessTouch> touches = new List<WirelessTouch>();
				for(int i = 0; i < Input.touchCount; i++)
				{
					touches.Add(WirelessTouch.ConvertUnityTouchToWireless(Input.GetTouch(i)));
				}
			
				data.TouchData = touches.ToArray();

				//add gyro and accelero
				data.AcceleroData = Input.acceleration;
				data.GyroData = Input.gyro.attitude.eulerAngles;

				//webcam
				//set devices fetched from phone
				data.WebcamDevices = WirelessWebcamDevice.ConvertWebcamDevicesToWireless(WebCamTexture.devices);

				if(currentWebcamDevice.name != camTexture.deviceName)
				{
					camTexture = new WebCamTexture();
					camTexture.deviceName = currentWebcamDevice.name;
					camTexture.Play();
				}
				if(tempWebcamTex == null) tempWebcamTex = new Texture2D(1, 1);
				tempWebcamTex.SetPixels(camTexture.GetPixels());
				data.WebcamTexture = tempWebcamTex.EncodeToJPG(100);

				return data;
			}
			else
			{
				if(data == null)
					data = new DeviceData();
				return data;
			}

		} 
		set
		{
			data = value;
		}
	}

	void Update()
	{
		if(Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
		{
			if(Camera.main.GetComponent<ScreenCapture>() == null)
			{
				screenCapture = Camera.main.gameObject.AddComponent<ScreenCapture>();
			}
			else
			{
				screenCapture = Camera.main.GetComponent<ScreenCapture>();
			}
		}

		if(ConnectionController.MyConnectionState == ConnectionController.ConnectionState.NotConnected) return;
		screenWidth = Screen.width;
		screenHeight = Screen.height;

		if(screenCapture != null && screenCapture.capturedTexture != null)
		{
			desktopData = new DesktopData();
			desktopData.showTouchscreenKeyboard = false;
			desktopData.imageData = screenCapture.capturedTexture.EncodeToJPG(settings.ScreenShareQuality);
			desktopData.screenOrientation = settings.DeviceOrientation;
			desktopData.currentWebcamDevice = currentWebcamDevice;
			ConnectionController.SendData(desktopData);
		}
	}

	/// <summary>
	/// Process raw data received from the network
	/// </summary>
	/// <param name="data">Data.</param>
	public static void ProcessData(byte[] rawData)
	{
		//just to be sure, we don't want a stupid bug to end our glorious stream
		if(rawData == null) return;

		//convert data to networkdevice data. This is something old, way before unity had a build in JsonUtility (finally.)
		NetworkDeviceData networkData = JsonUtility.FromJson<NetworkDeviceData>(System.Text.Encoding.ASCII.GetString(rawData));
		//Convert it from a serializable class to a useable class (again, before json we had to serialize everything, and
		//the fun part is that the standard datatypes of unity aren't even serializable (like vector2 or 3)
		DeviceData d = NetworkDeviceData.ConvertNetworkDataToDeviceData(networkData, screenWidth, screenHeight);
		receivedData.Add(d);
	}

	IEnumerator ProcessDataPerFrame()
	{
		while(true)
		{
			yield return new WaitForEndOfFrame();

			if(receivedData.Count > 0)
			{
				DeviceData newData;
				List<WirelessTouch> allTouches = new List<WirelessTouch>();

				//loop through all received data
				for(int i = 0; i < receivedData.Count; i++)
				{
					var dat = receivedData[i];

					for(int t = 0; t < dat.TouchData.Length; t++)
					{
						WirelessTouch currentTouch = dat.TouchData[t];
						int foundTouchIndex = allTouches.FindIndex(T => T.fingerId == currentTouch.fingerId);

						if(foundTouchIndex >= 0)
						{
							if(currentTouch.phase == TouchPhase.Began || currentTouch.phase == TouchPhase.Ended)
							{
								allTouches[foundTouchIndex].phase = currentTouch.phase;
							}
						}
						else
						{
							allTouches.Add(currentTouch);
						}
					}
				}

				newData = receivedData[receivedData.Count - 1];
				newData.TouchData = allTouches.ToArray();

				data = newData;
				receivedData.Clear();
			}
		}
	}

	//Webcam
	public static void StartWebcamService(System.Action<WirelessWebcamDevice[]> OnServiceStarted)
	{
		Instance.StartCoroutine(FetchWebcamDevices((WirelessWebcamDevice[] fetchedDevices) => {
			OnServiceStarted(fetchedDevices);
			WebcamServiceReady = true;
		}));
	}

	private static IEnumerator FetchWebcamDevices(System.Action<WirelessWebcamDevice[]> OnServiceStarted)
	{
		WirelessWebcamDevice[] webcamDevices = null;
		//fetch webcam devices from smartphone, this is not done instantly
		while(webcamDevices == null)
		{
			webcamDevices = WirelessInputController.DeviceData.WebcamDevices;
			yield return null;
		}

		OnServiceStarted(webcamDevices);
	}

	#endregion


	public override void Process()
	{
		if(DeviceData != null)
			ProcessTouchEvents();
	}

	PointerEventData GetTouchPointerEventData(WirelessTouch input, out bool pressed, out bool released)
	{
		PointerEventData pointerData;
		var created = GetPointerData(input.fingerId, out pointerData, true);

		pointerData.Reset();

		pressed = created || (input.phase == TouchPhase.Began);
		released = (input.phase == TouchPhase.Canceled) || (input.phase == TouchPhase.Ended);

		if (created)
			pointerData.position = input.position;

		if (pressed)
			pointerData.delta = Vector2.zero;
		else
			pointerData.delta = input.position - pointerData.position;

		pointerData.position = input.position;

		pointerData.button = PointerEventData.InputButton.Left;

		eventSystem.RaycastAll(pointerData, m_RaycastResultCache);

		var raycast = FindFirstRaycast(m_RaycastResultCache);
		pointerData.pointerCurrentRaycast = raycast;
		m_RaycastResultCache.Clear();
		return pointerData;
	}

	private void ProcessTouchEvents()
	{
		for (int i = 0; i < data.TouchData.Length; ++i)
		{
			WirelessTouch input = data.TouchData[i];

			bool released = input.phase == TouchPhase.Ended;
			bool pressed = true;

			var pointer = this.GetTouchPointerEventData(input, out pressed, out released);
			ProcessTouchPress(pointer, pressed, released);
			if (!released)
			{
				ProcessMove(pointer);
				ProcessDrag(pointer);
			}
			else
				RemovePointerData(pointer);
		}
	}

	private void ProcessTouchPress(PointerEventData pointerEvent, bool pressed, bool released)
	{
		var currentOverGo = pointerEvent.pointerCurrentRaycast.gameObject;

		// PointerDown notification
		if (pressed)
		{
			pointerEvent.eligibleForClick = true;
			pointerEvent.delta = Vector2.zero;
			pointerEvent.dragging = false;
			pointerEvent.useDragThreshold = true;
			pointerEvent.pressPosition = pointerEvent.position;
			pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;

			DeselectIfSelectionChanged(currentOverGo, pointerEvent);

			if (pointerEvent.pointerEnter != currentOverGo)
			{
				// send a pointer enter to the touched element if it isn't the one to select...
				HandlePointerExitAndEnter(pointerEvent, currentOverGo);
				pointerEvent.pointerEnter = currentOverGo;
			}

			// search for the control that will receive the press
			// if we can't find a press handler set the press
			// handler to be what would receive a click.
			var newPressed = ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.pointerDownHandler);

			// didnt find a press handler... search for a click handler
			if (newPressed == null)
				newPressed = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

			// Debug.Log("Pressed: " + newPressed);

			float time = Time.unscaledTime;

			if (newPressed == pointerEvent.lastPress)
			{
				var diffTime = time - pointerEvent.clickTime;
				if (diffTime < 0.3f)
					++pointerEvent.clickCount;
				else
					pointerEvent.clickCount = 1;

				pointerEvent.clickTime = time;
			}
			else
			{
				pointerEvent.clickCount = 1;
			}

			pointerEvent.pointerPress = newPressed;
			pointerEvent.rawPointerPress = currentOverGo;

			pointerEvent.clickTime = time;

			// Save the drag handler as well
			pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(currentOverGo);

			if (pointerEvent.pointerDrag != null)
				ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag);
		}

		// PointerUp notification
		if (released)
		{
			// Debug.Log("Executing pressup on: " + pointer.pointerPress);
			ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);

			// Debug.Log("KeyCode: " + pointer.eventData.keyCode);

			// see if we mouse up on the same element that we clicked on...
			var pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

			// PointerClick and Drop events
			if (pointerEvent.pointerPress == pointerUpHandler && pointerEvent.eligibleForClick)
			{
				ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
				/*if(pointerUpHandler.GetComponent<InputField>() != null)
				{
					desktopData.showTouchscreenKeyboard = true;
				}*/
			}
			else if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
			{
				ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.dropHandler);
			}

			pointerEvent.eligibleForClick = false;
			pointerEvent.pointerPress = null;
			pointerEvent.rawPointerPress = null;

			if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
				ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);

			pointerEvent.dragging = false;
			pointerEvent.pointerDrag = null;

			if (pointerEvent.pointerDrag != null)
				ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);

			pointerEvent.pointerDrag = null;

			// send exit events as we need to simulate this on touch up on touch device
			ExecuteEvents.ExecuteHierarchy(pointerEvent.pointerEnter, pointerEvent, ExecuteEvents.pointerExitHandler);
			pointerEvent.pointerEnter = null;
		}
	}

	public override void DeactivateModule()
	{
		base.DeactivateModule();
		ClearSelection();
	}

	public override string ToString()
	{
		var sb = new StringBuilder();
		foreach (var pointerEventData in m_PointerData)
			sb.AppendLine(pointerEventData.ToString());
		return sb.ToString();
	}
}
