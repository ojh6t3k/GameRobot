using UnityEngine;
using System.Collections;

public class AutoTrace : MonoBehaviour 
{
	public Transform _trTarget;

	public float _fLimitX = 160f;
	public float _fLimitY = 160f;

	public float X = 0f;
	public float Y = 0f;

	Vector3 _v3Stick;

	
	// Update -------------------------------------
	void Update () 
	{
		transform.localPosition = _trTarget.localPosition;

		if (transform.localPosition.x > _fLimitX)
			transform.localPosition = new Vector3(_fLimitX, transform.localPosition.y, 0f);
		else if (transform.localPosition.x < -_fLimitX)
			transform.localPosition = new Vector3(-_fLimitX, transform.localPosition.y, 0f);

		if (transform.localPosition.y > _fLimitY)
			transform.localPosition = new Vector3(transform.localPosition.x, _fLimitY, 0f);
		else if (transform.localPosition.y < -_fLimitY)
			transform.localPosition = new Vector3(transform.localPosition.x, -_fLimitY, 0f);

		X = transform.localPosition.x / _fLimitX;
		Y = transform.localPosition.y / _fLimitY;
	}
}
