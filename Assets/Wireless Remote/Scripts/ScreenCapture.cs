using UnityEngine;
using System.Collections;

public class ScreenCapture : MonoBehaviour {

	public Texture2D capturedTexture;

	public Rect captureRect;
	public RenderTexture renderTex;
	private Camera myCam;


	int screenWidth;
	int screenHeight;

	void Start()
	{
		//Get the camera
		myCam = GetComponent<Camera>();
		//save screen dimensions
		screenWidth = Mathf.RoundToInt(Screen.width);
		screenHeight = Mathf.RoundToInt(Screen.height);
		//create new rendertexture
		renderTex = new RenderTexture(screenWidth, screenHeight, 24);
		capturedTexture = new Texture2D(screenWidth, screenHeight, TextureFormat.RGB24, false);
	}

	void OnPostRender()
	{
		myCam = Camera.current;
		/*var canvasses = GameObject.FindObjectsOfType<Canvas>();

		for(int i = 0; i < canvasses.Length; i++)
		{
			canvasses[i].renderMode = RenderMode.ScreenSpaceCamera;
			canvasses[i].worldCamera = myCam;
		}*/

		screenWidth = Mathf.RoundToInt(Screen.width);
		screenHeight = Mathf.RoundToInt(Screen.height);

		myCam.targetTexture = renderTex;

		//myCam.Render();

		RenderTexture.active = renderTex;
		capturedTexture.ReadPixels(new Rect(0, 0, screenWidth, screenHeight), 0, 0);
		capturedTexture.Apply();

		myCam.targetTexture = null;
		RenderTexture.active = null;
   		Destroy(renderTex);

	}
}
