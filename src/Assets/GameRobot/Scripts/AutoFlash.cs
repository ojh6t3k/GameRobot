using UnityEngine;
using System.Collections;

public class AutoFlash : MonoBehaviour 
{
	UISprite _UISprite;


	// Start ---------------------------------
	void Start () 
	{
		_UISprite = gameObject.GetComponent<UISprite>();
	}



	public void StartFlash()
	{
		_UISprite.alpha = 1f;
	}




	// Update --------------------------------
	void Update () 
	{
		if (_UISprite == null)
			return;

		if (_UISprite.alpha <= 0f)
			return;

		_UISprite.alpha = _UISprite.alpha - Time.deltaTime * 10f;
	}
}
