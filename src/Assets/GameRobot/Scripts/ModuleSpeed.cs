using UnityEngine;
using System.Collections;

public class ModuleSpeed : MonoBehaviour 
{
	public ControlEngineSet _controlEngineSet;

	public ControlNumberSet _controlNumberSet;
	public ControlSpeedGauge _controlSpeedGaugeP;
	public ControlSpeedGauge _controlSpeedGaugeM;

	public float _fX = 0f;
	public float _fY = 0f;




	// Update ------------------------------------------
	void Update () 
	{
		_controlEngineSet._fX = _fX;
		_controlEngineSet._fY = _fY;

		_controlNumberSet.SetValue(_fY);
		_controlSpeedGaugeP.SetGauge(_fY);
		_controlSpeedGaugeM.SetGauge(_fY);
	}
}
