using UnityEngine;
using System.Collections;

public class ControlNumber : MonoBehaviour 
{
	public UISprite _SprNumber;

	int _nCurNumber = 0;



	// Start ------------------------------------
	void Start () 
	{
		//_SprNumber.spriteName = "LCD_Num_Null";
	}



	// SetNumber --------------------------------------------
	public void SetNumber(int p_Number)
	{
		if (_nCurNumber == p_Number)
			return;

		switch(p_Number)
		{
		case -1:
			_SprNumber.spriteName = "LCD_Num_Null";
			break;
		case 0:
			_SprNumber.spriteName = "LCD_Num_0";
			break;
		case 1:
			_SprNumber.spriteName = "LCD_Num_1";
			break;
		case 2:
			_SprNumber.spriteName = "LCD_Num_2";
			break;
		case 3:
			_SprNumber.spriteName = "LCD_Num_3";
			break;
		case 4:
			_SprNumber.spriteName = "LCD_Num_4";
			break;
		case 5:
			_SprNumber.spriteName = "LCD_Num_5";
			break;
		case 6:
			_SprNumber.spriteName = "LCD_Num_6";
			break;
		case 7:
			_SprNumber.spriteName = "LCD_Num_7";
			break;
		case 8:
			_SprNumber.spriteName = "LCD_Num_8";
			break;
		case 9:
			_SprNumber.spriteName = "LCD_Num_9";
			break;
		case 10:
			_SprNumber.spriteName = "LCD_Num_m";
			break;
		}

		_nCurNumber = p_Number;
	}
}
