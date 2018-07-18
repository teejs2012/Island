using UnityEngine;
using System.Collections;
using Wireless_Remote;

public class ExampleInputScript : MonoBehaviour {

	private GameObject selectedGameObject;

	void Update () {
		
		//store all touches
		WirelessTouch[] touches = WirelessInputController.DeviceData.TouchData;

		//loop through all the touches
		for(int i = 0; i < touches.Length; i++)
		{
			//All of the normal touch functionality works like the unity's one
			//except for the positions, which is similar, but not alike the vector2 from unity because those cant' be send over a network ;-;
			if(touches[i].phase == TouchPhase.Began)
			{
				//Debug.Log("Touch Started");
				//select an object from the touch position
				selectedGameObject = CastRay(touches[i].position);
			}
			else if(touches[i].phase == TouchPhase.Moved)
			{
				if(selectedGameObject != null)
				{
					selectedGameObject.transform.position = GetWorldPosFromScreenSpace(
						new Vector3(
							touches[i].position.x, 
							touches[i].position.y, 
							Mathf.Abs(Camera.main.transform.position.z - selectedGameObject.transform.position.z)));
				}
			}
			else if(touches[i].phase == TouchPhase.Ended)
			{
				//Debug.Log("Touch ended");
				selectedGameObject = null;
			}
		}
	}

	Vector3 GetWorldPosFromScreenSpace(Vector3 screenSpacePos)
	{
		return Camera.main.ScreenToWorldPoint(screenSpacePos);
	}

	GameObject CastRay(Vector2 screenPosition)
	{
		RaycastHit hit;

		if(Physics.Raycast(Camera.main.ScreenPointToRay(screenPosition), out hit))
		{
			return hit.transform.gameObject;
		}
		else
		{
			return null;
		}
	}
}
