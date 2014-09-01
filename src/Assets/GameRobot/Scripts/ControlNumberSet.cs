using UnityEngine;
using System.Collections;

public class ControlNumberSet : MonoBehaviour 
{
	public ControlNumber _Num1;
	public ControlNumber _Num10;
	public ControlNumber _Num100;
	public ControlNumber _NumM;


	// SetValue --------------------------------------------------------
	public void SetValue(float p_Value)
	{
		if (p_Value > 1)
			p_Value = 1;
		else if (p_Value < -1)
			p_Value = -1;

		bool bSign = true;
		if (p_Value < 0)
			bSign = false;

		int nSpeed = (int)Mathf.Floor(Mathf.Abs(p_Value) * 100f);
		string strSpeed = nSpeed.ToString();

		if (strSpeed.Length == 1)
		{
			_Num1.SetNumber(int.Parse(strSpeed.Substring(0,1)));
			if (bSign)
			{
				_Num10.SetNumber(-1);
				_Num100.SetNumber(-1);
				_NumM.SetNumber(-1);
			}
			else
			{
				_Num10.SetNumber(10);
				_Num100.SetNumber(-1);
				_NumM.SetNumber(-1);
			}
		}
		else if (strSpeed.Length == 2)
		{
			_Num10.SetNumber(int.Parse(strSpeed.Substring(0,1)));
			_Num1.SetNumber(int.Parse(strSpeed.Substring(1,1)));
			if (bSign)
			{
				_Num100.SetNumber(-1);
				_NumM.SetNumber(-1);
			}
			else
			{
				_Num100.SetNumber(10);
				_NumM.SetNumber(-1);
			}
		}
		else if (strSpeed.Length == 3)
		{
			_Num100.SetNumber(int.Parse(strSpeed.Substring(0,1)));
			_Num10.SetNumber(int.Parse(strSpeed.Substring(1,1)));
			_Num1.SetNumber(int.Parse(strSpeed.Substring(2,1)));
			if (bSign)
			{
				_NumM.SetNumber(-1);
			}
			else
			{
				_NumM.SetNumber(10);
			}
		}

	}
	

}
