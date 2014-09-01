using UnityEngine;
using System.Collections;

public class ModuleGradient : MonoBehaviour 
{
	public float _fAngX = 0f;
	public float _fAngZ = 0f;
	public GameObject _goGradientX;
	public GameObject _goGradientZ;


	// Update ---------------------------------------
	void Update () 
	{
		if (_fAngX > 90f)
			_fAngX = 90f;
		else if (_fAngX < -90f)
			_fAngX = -90f;

		_goGradientX.transform.localScale = new Vector3(1f, _fAngX / 90f, 0f);

		_goGradientZ.transform.localEulerAngles = new Vector3(0f, 0f, _fAngZ);
	}
}