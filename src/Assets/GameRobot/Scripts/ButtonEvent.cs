using UnityEngine;
using System.Collections;

public class ButtonEvent : MonoBehaviour 
{

	void OnPress(bool isDown)
	{
		if (!isDown)
			transform.localPosition = Vector3.zero;
	}
}
