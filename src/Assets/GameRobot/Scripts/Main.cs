using UnityEngine;
using System.Collections;
using System;
using UnityRobot;




public class Main : MonoBehaviour 
{
	public RobotProxy _RobotProxy;
	public PanTiltController _panTilt;
	public WheelController _wheel4WD;
	public DRCwifi _wifiCamera;
	public UITexture _camImage;
	public DistanceSensor _distForward;
	public DistanceSensor _distRight;
	public DistanceSensor _distLeft;
	public DistanceSensor _distBackward;
	public AngleModule _radarAngle;
	public IMUModule _imuAngle;

	public UIPopupList _UIPortList;

	public AutoTrace _StickL;
	public AutoTrace _StickR;

	public ModuleCenter _moduleCenter;
	public ModuleCamRotation _moduleCamRotation;
	public ModuleSpeed _moduleSpeed;
	public ModuleRadar _moduleRadar;
	public ModuleGradient _moduleGradient;
	
	bool IsConnect_Robot = false;

	public GameObject _goBtnConnect;
	public GameObject _goConnectingDeco;

	public GameObject _goUI_ConnectPnl;
	public GameObject _goUI_DisconnectPnl;
	public GameObject _goBtnExitPnl;

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
		_goConnectingDeco.SetActive(false);		// NGUI
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
		_goConnectingDeco.SetActive(true);
		_goBtnConnect.SetActive(false);	// NGUI
	}



	public void Disconnect_Port()
	{
		_RobotProxy.Disconnect();
		IsConnect_Robot = false;
		_goBtnConnect.SetActive(true);		// NGUI
		ShowConnectUI(true);
		Invoke("Search_Ports", 0.2f);
	}





	void ShowConnectUI(bool p_Show)
	{
		_goUI_ConnectPnl.SetActive(p_Show);
		_goUI_DisconnectPnl.SetActive(!p_Show);
		_goBtnExitPnl.SetActive(p_Show);

		_goUI_Center.SetActive(!p_Show);
		_goUI_CamRotation.SetActive(!p_Show);
		_goUI_Gradient.SetActive(!p_Show);
		_goUI_Radar.SetActive(!p_Show);
		_goUI_Speed.SetActive(!p_Show);
	}



	public void AppExit()
	{
		Application.Quit();
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

		if(_RobotProxy.Connected == true)
		{
			_wheel4WD.ControlRect(new Vector2(_StickL.X, _StickL.Y));
			_panTilt.ControlPoint(1f, new Vector2(_StickR.X, _StickR.Y));

			Texture2D image = _wifiCamera.image;
			if(image != null)
				_camImage.mainTexture = image;

			_moduleGradient._fAngX = _imuAngle.Angle.y;
			_moduleGradient._fAngZ = _imuAngle.Angle.x;

			_moduleRadar._fRadar1 = _distRight.Distance / 800f;
			_moduleRadar._fRadar2 = _distLeft.Distance / 800f;
			_moduleRadar._fRadar3 = _distBackward.Distance / 800f;
			_moduleRadar._fRadar4 = _distForward.Distance / 800f;
			_moduleRadar._fAngle = _radarAngle.Angle;
		}
	}
















	void OnConnected(object sender, EventArgs e)
	{
		IsConnect_Robot = true;
		_goConnectingDeco.SetActive(false);		// NGUI
		ShowConnectUI(false);
	}
	
	void OnConnectionFailed(object sender, EventArgs e)
	{
		IsConnect_Robot = false;
		_goConnectingDeco.SetActive(false);		// NGUI
		_goBtnConnect.SetActive(true);		// NGUI
	}
	
	void OnDisconnected(object sender, EventArgs e)
	{
		IsConnect_Robot = false;
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
