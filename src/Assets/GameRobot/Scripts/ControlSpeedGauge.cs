using UnityEngine;
using System.Collections;

public class ControlSpeedGauge : MonoBehaviour 
{
	public UISprite[] _sprGauge = new UISprite[20];

	public bool _Sign = true;
	
	int _nCurSpeed = 0;

	bool _bIsAllOff = false;


	// SetGauge -------------------------------------------------
	public void SetGauge(float p_Speed)
	{
		if (p_Speed > 1)
			p_Speed = 1;
		else if (p_Speed < -1)
			p_Speed = -1;

		bool bSign = true;
		if (p_Speed < 0)
			bSign = false;

		int nSpeed = (int)Mathf.Floor(Mathf.Abs(p_Speed) * 20f);



		if ( ((_Sign != bSign) && (!_bIsAllOff)) || (p_Speed == 0))
		{
			_bIsAllOff = true;
			foreach(UISprite spr in _sprGauge)
			{
				spr.alpha = 0.21f;
				_nCurSpeed = 0;
			}
			return;
		}

		if ( (_Sign != bSign) && (_bIsAllOff) )
			return;

		if (_nCurSpeed == nSpeed)
			return;



		_bIsAllOff = false;

		if (_nCurSpeed < nSpeed)
		{
			_sprGauge[_nCurSpeed].alpha = 1f;
			_nCurSpeed ++;
		}
		else if (_nCurSpeed > nSpeed)
		{
			_sprGauge[_nCurSpeed-1].alpha = 0.21f;
			_nCurSpeed --;
		}


	}
}