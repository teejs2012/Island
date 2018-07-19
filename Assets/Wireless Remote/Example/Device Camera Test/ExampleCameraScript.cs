using UnityEngine;
using System.Collections;
using Wireless_Remote;

public class ExampleCameraScript : MonoBehaviour {

	public enum CurrentCamera {backCamera, frontCamera};
	public CurrentCamera currentCamera = CurrentCamera.backCamera;

	public Material cubeMaterial;
	private Texture2D tex;

	void Start () {
		//start the service, this will not be done instantly because of the wireless connection.
		WirelessInputController.StartWebcamService( (WirelessWebcamDevice[] fetchedDevices) => {
			//code here will be executed when we have received out webcam devices, so, let's assign one
			WirelessInputController.currentWebcamDevice = fetchedDevices[(int)currentCamera];
			//note, assigning is enough. It'll automatically fire up the webcam and data will be send to you

			//and create the texture of course
			tex = new Texture2D(1, 1);
		});
	}

	void Update () 
	{
		//if the service is ready, we can receive data!
		if(WirelessInputController.WebcamServiceReady)
		{
			//simply load the byte array containing image data in to the texture and we're done ;)
			tex.LoadImage(WirelessInputController.DeviceData.WebcamTexture);
			cubeMaterial.mainTexture = tex;
		}
	}
}
