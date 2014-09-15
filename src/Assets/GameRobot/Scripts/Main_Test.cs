using UnityEngine;
using System.Collections;
using System;
using UnityRobot;




public class Main_Test : MonoBehaviour 
{
	public Hexamite _Hexamite;


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








	// Start ------------------------------------------------------------------------------
	void Start ()
	{
		_Hexamite.OnDisconnected += OnDisconnected;
		_Hexamite.OnGetData += OnGetData;
		
		_Hexamite.PortSearch();
		SearchCompleted();
		
		ShowConnectUI(true);
		_goConnectingDeco.SetActive(false);		// NGUI
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
		float x0 = ((_fR0 * _fR0) - (_fR1 * _fR1) + 1000f) / 2000f;
		float z0 = ((_fR0 * _fR0) - (_fR2 * _fR2) + 1000f) / 2000f;

		_goTestBox.transform.position = new Vector3( x0, 0f, -z0);
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
	}








}
