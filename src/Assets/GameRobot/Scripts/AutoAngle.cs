using UnityEngine;
using System.Collections;

public class AutoAngle : MonoBehaviour 
{
	public float	_Interval = 0.5f;
	public float	_Angle = 15f;



	void Start () 
	{
		AddAngle();
	}
	

	void AddAngle()
	{
		transform.Rotate(0f, 0f, _Angle);
		Invoke("AddAngle", _Interval);
	}


}
