using UnityEngine;
using System.Collections;

public class ModuleCamRotation : MonoBehaviour 
{
	public float _fX = 0f;
	public float _fY = 0f;
	public float _fAngle = 45f;
	public GameObject _goCamDirection;
	public GameObject _goGaugeH;
	public GameObject _goGaugeV;



	
	// Update ---------------------------------
	void Update () 
	{
		_goCamDirection.transform.localEulerAngles = new Vector3(0f, 0f, _fAngle * _fX);
		_goGaugeH.transform.localPosition = new Vector3(245f * _fX, 20f, 0f);
		_goGaugeV.transform.localPosition = new Vector3(245f * _fY, 20f, 0f);
	}
}
