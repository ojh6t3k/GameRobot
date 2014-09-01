using UnityEngine;
using System.Collections;
using System;
using UnityRobot;




public class Main : MonoBehaviour 
{
	public RobotProxy _RobotProxy;
	public UIPopupList _UIPortList;

	public AutoTrace _StickL;
	public AutoTrace _StickR;

	public ModuleCenter _moduleCenter;
	public ModuleCamRotation _moduleCamRotation;
	public ModuleSpeed _moduleSpeed;
	
	bool IsConnect_Robot = false;

	public GameObject _goBtnConnect;
	public GameObject _goConnectingDeco;

	public GameObject _goUI_ConnectPnl;

	public GameObject _goUI_Center;
	public GameObject _goUI_CamRotation;
	public GameObject _goUI_Gradient;
	public GameObject _goUI_Radar;
	public GameObject _goUI_Speed;


	// Start ------------------------------------------------------------------------------
	void Start ()
	{
		_RobotProxy.OnConnected += OnConnected;
		_RobotProxy.OnConnectionFailed += OnConnectionFailed;
		_RobotProxy.OnDisconnected += OnDisconnected;
		_RobotProxy.OnSearchCompleted += OnSearchCompleted;

		_RobotProxy.PortSearch();

		ShowConnectUI(true);
		_goConnectingDeco.SetActive(false);
	}




	public void Search_Ports()
	{
		_goBtnConnect.SetActive(false);		// NGUI
		_RobotProxy.PortSearch();
	}
	
	
	public void Connect_Port()
	{
		_RobotProxy.portName = _UIPortList.value;
		_RobotProxy.Connect();
		//_UILblRobotMessage.text = "연결 시도중..."; // NGUI

		_goConnectingDeco.SetActive(true);
		_goBtnConnect.SetActive(false);	// NGUI
	}




	void ShowConnectUI(bool p_Show)
	{
		_goUI_ConnectPnl.SetActive(p_Show);

		_goUI_Center.SetActive(!p_Show);
		_goUI_CamRotation.SetActive(!p_Show);
		_goUI_Gradient.SetActive(!p_Show);
		_goUI_Radar.SetActive(!p_Show);
		_goUI_Speed.SetActive(!p_Show);
	}






	
	// Update -------------------------------------
	void Update () 
	{
		_moduleCenter._fX = _StickR.X;
		_moduleCenter._fY = _StickR.Y;

		_moduleCamRotation._fX = _StickR.X;
		_moduleCamRotation._fY = _StickR.Y;

		_moduleSpeed._fX = _StickL.X;
		_moduleSpeed._fY = _StickL.Y;
	}
















	void OnConnected(object sender, EventArgs e)
	{
		IsConnect_Robot = true;
		Debug.Log("연결됨");
//		_UILblRobotMessage.text = "연결됨"; // NGUI
//		CheckStartButton(); // NGUI
		ShowConnectUI(false);
	}
	
	void OnConnectionFailed(object sender, EventArgs e)
	{
		IsConnect_Robot = false;
		Debug.Log("연결 실패");
//		_UILblRobotMessage.text = "연결 실패"; // NGUI
		_goConnectingDeco.SetActive(false);
		_goBtnConnect.SetActive(true);		// NGUI
//		CheckStartButton(); // NGUI
	}
	
	void OnDisconnected(object sender, EventArgs e)
	{
		IsConnect_Robot = false;
		Debug.Log("연결 끊어짐");
//		_UILblRobotMessage.text = "연결 끊어짐"; // NGUI
		_goBtnConnect.SetActive(true);		// NGUI

		ShowConnectUI(true);

		Invoke("Search_Ports", 0.2f);
	}
	
	void OnSearchCompleted(object sender, EventArgs e)
	{
		_UIPortList.items.Clear();
		
		if(_RobotProxy.portNames.Count > 0)
		{
			for(int i=0; i<_RobotProxy.portNames.Count; i++)
			{
				_UIPortList.items.Add(_RobotProxy.portNames[i]);
			}
			_UIPortList.value = _UIPortList.items[0];
			_goBtnConnect.SetActive(true);		// NGUI
		}
		else if(_RobotProxy.portNames.Count == 0)
		{
			_UIPortList.items.Add("None");
		}
	}




}
