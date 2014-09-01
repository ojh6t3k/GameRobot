using UnityEngine;
using System.Collections;

public class ModuleCenter : MonoBehaviour 
{
	public float _fX = 0f;
	public float _fY = 0f;
	public float _fRatio = 10f;
	public GameObject _goCenter;
	


	// Update -------------------------------------
	void Update () 
	{
		_goCenter.transform.localPosition = new Vector3(_fX * -_fRatio, _fY * -_fRatio, 0f);
	}
}