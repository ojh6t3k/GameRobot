using UnityEngine;
using System.Collections;

public class ControlEngineSet : MonoBehaviour 
{
	public GameObject go_EngineL;
	public GameObject go_EngineR;

	public float _fX = 0f;
	public float _fY = 0f;

	float _fEnginePowL = 0f;
	float _fEnginePowR = 0f;


	// Update ------------------------------------------
	void Update () 
	{
		_fEnginePowL = (_fY + -_fX);
		if (_fEnginePowL > 1f)
			_fEnginePowL = 1f;
		else if (_fEnginePowL < -1f)
			_fEnginePowL = -1f;

		_fEnginePowR = (_fY + _fX);
		if (_fEnginePowR > 1f)
			_fEnginePowR = 1f;
		else if (_fEnginePowR < -1f)
			_fEnginePowR = -1f;

		go_EngineL.transform.Rotate(0f, 0f, Time.deltaTime * 500f * _fEnginePowL);
		go_EngineR.transform.Rotate(0f, 0f, Time.deltaTime * 500f * -_fEnginePowR);
	}
}
