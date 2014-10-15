using UnityEngine;
using System.Collections;
using System;
using UnityRobot;




public class Main_Test : MonoBehaviour 
{
	public Hexamite _Hexamite;
	public GameObject target;
	public GameObject plane;


	public UIPopupList _UIPortList;

	bool IsConnect_Robot = false;
	
	public GameObject _goBtnConnect;
	public GameObject _goConnectingDeco;
	
	public GameObject _goUI_ConnectPnl;
	public GameObject _goUI_DisconnectPnl;

	public GameObject _goAncTopLeft;
	public GameObject _goAncTopRight;
	public GameObject _goAncBottomLeft;
	public GameObject _goAncBottomRight;


	public UILabel _UILblR40T20;
	public UILabel _UILblR40T21;

	public UILabel _UILblR41T20;
	public UILabel _UILblR41T21;

	public UILabel _UILblR42T20;
	public UILabel _UILblR42T21;

	public UILabel _UILblR43T20;
	public UILabel _UILblR43T21;


	public AutoFlash _scrR40;
	public AutoFlash _scrR40T20;
	public AutoFlash _scrR40T21;

	public AutoFlash _scrR41;
	public AutoFlash _scrR41T20;
	public AutoFlash _scrR41T21;

	public AutoFlash _scrR42;
	public AutoFlash _scrR42T20;
	public AutoFlash _scrR42T21;

	public AutoFlash _scrR43;
	public AutoFlash _scrR43T20;
	public AutoFlash _scrR43T21;




	float _fR0 = 0f;
	float _fR1 = 0f;
	float _fR2 = 0f;
	float _fR3 = 0f;

	public GameObject _goTestBox;



	private Quaternion _qCurrent;
	private Quaternion _qGoal;
	private float _qT = 1f;

	private Vector3 _vCurrent;
	private Vector3 _vGoal;
	private float _vT = 1f;


	// Start ------------------------------------------------------------------------------
	void Start ()
	{
		_Hexamite.OnDisconnected += OnDisconnected;
		_Hexamite.OnGetData += OnGetData;
		
		_Hexamite.PortSearch();
		SearchCompleted();
		
		ShowConnectUI(true);
		_goConnectingDeco.SetActive(false);		// NGUI

		if(target != null)
		{
			_qGoal = target.transform.localRotation;
			_vGoal = target.transform.localPosition;
		}
	}
	
	
	
	
	public void Search_Ports()
	{
		_goBtnConnect.SetActive(false);		// NGUI
		_Hexamite.PortSearch();
		SearchCompleted();
	}
	
	
	public void Connect_Port()
	{
		_Hexamite.portName = _UIPortList.value;
		if(_Hexamite.Connect() == true)
		{
			IsConnect_Robot = true;
			_goConnectingDeco.SetActive(false);		// NGUI
			ShowConnectUI(false);
		}
		else
		{
			IsConnect_Robot = false;
			_goConnectingDeco.SetActive(false);		// NGUI
			_goBtnConnect.SetActive(true);		// NGUI
		}
	}
	



	void SearchCompleted()
	{
		_UIPortList.items.Clear();
		
		if(_Hexamite.portNames.Count > 0)
		{
			for(int i=0; i<_Hexamite.portNames.Count; i++)
			{
				_UIPortList.items.Add(_Hexamite.portNames[i]);
			}
			_UIPortList.value = _UIPortList.items[0];
			_goBtnConnect.SetActive(true);		// NGUI
		}
		else if(_Hexamite.portNames.Count == 0)
		{
			_UIPortList.items.Add("None");
		}
	}




	
	public void Disconnect_Port()
	{
		_Hexamite.Disconnect();
		IsConnect_Robot = false;
		_goBtnConnect.SetActive(true);		// NGUI
		ShowConnectUI(true);
	//	Invoke("Search_Ports", 0.2f);
	}
	
	
	
	
	
	void ShowConnectUI(bool p_Show)
	{
		_goUI_ConnectPnl.SetActive(p_Show);
		_goUI_DisconnectPnl.SetActive(!p_Show);

		_goAncTopLeft.SetActive(!p_Show);
		_goAncTopRight.SetActive(!p_Show);
		_goAncBottomLeft.SetActive(!p_Show);
		_goAncBottomRight.SetActive(!p_Show);
	}











	// Update -------------------------------------
	void Update () 
	{
		if(target != null)
		{
			Vector3 pos1 = ConvertPosition(_Hexamite.hexT_list[0].position);
			Vector3 pos2 = ConvertPosition(_Hexamite.hexT_list[1].position);
			Vector3 diff = pos1 - pos2;
			Vector3 center = (pos1 + pos2) * 0.5f;

			if(diff != Vector3.zero)
			{
				Quaternion q = Quaternion.LookRotation(diff);
				if(q != _qGoal)
				{
					_qT = 0f;
					_qCurrent = target.transform.localRotation;
					_qGoal = q;
				}
			}

			if(center != _vGoal)
			{
				_vT = 0f;
				_vCurrent = target.transform.localPosition;
				_vGoal = center;
			}

			if(_qT < 1f)
			{
				target.transform.localRotation = Quaternion.Lerp(_qCurrent, _qGoal, _qT);
				_qT += (Time.deltaTime * 4f);
			}
			else
				target.transform.localRotation = _qGoal;

			if(_vT < 1f)
			{
				target.transform.localPosition = Vector3.Lerp(_vCurrent, _vGoal, _vT);
				_vT += (Time.deltaTime * 4f);
			}
			else
				target.transform.localPosition = _vGoal;
		}
	}

	Vector3 ConvertPosition(Vector3 pos)
	{
		float temp = pos.z;
		pos.z = pos.y;
		pos.y = temp;
		Vector3 scale = plane.transform.localScale * 10f;
		pos.x *= (scale.x / _Hexamite.planeSize.x);
		pos.y *= (scale.x / _Hexamite.planeSize.x);
		pos.z *= (scale.z / _Hexamite.planeSize.y);
		pos -= new Vector3(scale.x * 0.5f, 0f, scale.z * 0.5f);

		return pos;
	}



	void OnDisconnected(object sender, EventArgs e)
	{
		IsConnect_Robot = false;
		_goBtnConnect.SetActive(true);		// NGUI
		ShowConnectUI(true);
		Invoke("Search_Ports", 0.2f);
	}



	void OnGetData(object sender, EventArgs e)
	{
		/*
		switch(_Hexamite.nR_Number)
		{
		case 40:
			_scrR40.StartFlash();
			if (_Hexamite.nT_Number == 20)
			{
				_scrR40T20.StartFlash();
				_UILblR40T20.text = "T20\n" + _Hexamite.nDistance.ToString();
				_fR0 = (float)_Hexamite.nDistance;
			}
			else if (_Hexamite.nT_Number == 21)
			{
				_scrR40T21.StartFlash();
				_UILblR40T21.text = "T21\n" + _Hexamite.nDistance.ToString();
			}
			break;
		case 41:
			_scrR41.StartFlash();
			if (_Hexamite.nT_Number == 20)
			{
				_scrR41T20.StartFlash();
				_UILblR41T20.text = "T20\n" + _Hexamite.nDistance.ToString();
				_fR1 = (float)_Hexamite.nDistance;
			}
			else if (_Hexamite.nT_Number == 21)
			{
				_scrR41T21.StartFlash();
				_UILblR41T21.text = "T21\n" + _Hexamite.nDistance.ToString();
			}
			break;
		case 42:
			_scrR42.StartFlash();
			if (_Hexamite.nT_Number == 20)
			{
				_scrR42T20.StartFlash();
				_UILblR42T20.text = "T20\n" + _Hexamite.nDistance.ToString();
				_fR2 = (float)_Hexamite.nDistance;
			}
			else if (_Hexamite.nT_Number == 21)
			{
				_scrR42T21.StartFlash();
				_UILblR42T21.text = "T21\n" + _Hexamite.nDistance.ToString();
			}
			break;
		case 43:
			_scrR43.StartFlash();
			if (_Hexamite.nT_Number == 20)
			{
				_scrR43T20.StartFlash();
				_UILblR43T20.text = "T20\n" + _Hexamite.nDistance.ToString();
			}
			else if (_Hexamite.nT_Number == 21)
			{
				_scrR43T21.StartFlash();
				_UILblR43T21.text = "T21\n" + _Hexamite.nDistance.ToString();
			}
			break;
		default:
			break;

		}
		*/
	}








}
