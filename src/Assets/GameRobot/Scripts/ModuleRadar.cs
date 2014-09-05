using UnityEngine;
using System.Collections;

public class ModuleRadar : MonoBehaviour 
{
	public float _fRadar1 = 1f;
	public float _fRadar2 = 1f;
	public float _fRadar3 = 1f;
	public float _fRadar4 = 1f;

	public float _fAngle = 0f;

	public GameObject _goAngleSet;

	public GameObject	_goRadarTrail1;
	public GameObject	_goRadarTrail2;
	public GameObject	_goRadarTrail3;
	public GameObject	_goRadarTrail4;


	
	// Update --------------------------------------------------
	void Update () 
	{
		_goRadarTrail1.transform.localPosition = new Vector3(0f, (_fRadar1 * 140f) + 60f, 0f);
		_goRadarTrail2.transform.localPosition = new Vector3(0f, (_fRadar2 * 140f) + 60f, 0f);
		_goRadarTrail3.transform.localPosition = new Vector3(0f, (_fRadar3 * 140f) + 60f, 0f);
		_goRadarTrail4.transform.localPosition = new Vector3(0f, (_fRadar4 * 140f) + 60f, 0f);

		_goAngleSet.transform.localEulerAngles = new Vector3(0f, 0f, _fAngle);
	}
}
