using UnityEngine;
using System;
using System.Collections;
using UnityRobot;

public class ExplorerRobot : MonoBehaviour
{
	public AppGUIBasic appGUIBasic;
	public RobotProxy robotProxy;
	
	public dfLabel guiDRCMessage;
	public dfTouchJoystick guiLeftJoystic;
	public dfTouchJoystick guiRightJoystic;
	public dfTextureSprite guiCameraImage;
	
	public DRCwifi drcWiFi;
	public WheelController wheelController;
	public PanTiltController panTilt;
	public DigitalModule led;
	public DigitalModule sound;
	public ServoModule servo_R;
	public ServoModule servo_L;

	public float R_openAngle = 0;
	public float R_closeAngle = 0;
	public float L_openAngle = 0;
	public float L_closeAngle = 0;

	
	void Awake()
	{
	}
	
	// Use this for initialization
	void Start ()
	{
		appGUIBasic.OnStartMainGUI += OnStartMain;
		appGUIBasic.OnExitMainGUI += OnExitMain;
		appGUIBasic.OnStartSettingGUI += OnStartSetting;
		appGUIBasic.OnExitSettingGUI += OnExitSetting;
		appGUIBasic.OnStartTerminalGUI += OnStartTerminal;
		appGUIBasic.OnExitTerminalGUI += OnExitTerminal;
		
		this.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		wheelController.ControlCircle(guiLeftJoystic.Position);
		panTilt.ControlPoint(100f, guiRightJoystic.Position * 200);
		
		Texture image = drcWiFi.image;
		if(image != null)
			guiCameraImage.Texture = image;
		
		guiDRCMessage.IsVisible = !drcWiFi.Connected;
		//	guiDRCMessage.Text = drcWiFi.text;
	}
	
	void OnStartSetting(object sender, EventArgs e)
	{
	}
	
	void OnExitSetting(object sender, EventArgs e)
	{
	}
	
	void OnStartMain(object sender, EventArgs e)
	{
		drcWiFi.Reset();
		this.enabled = true;
		servo_R.Angle = R_openAngle;
		servo_L.Angle = L_openAngle;
	}
	
	void OnExitMain(object sender, EventArgs e)
	{
		this.enabled = false;
	}
	
	void OnStartTerminal(object sender, EventArgs e)
	{
	}
	
	void OnExitTerminal(object sender, EventArgs e)
	{
	}
	
	void OnTxCompleted(object sender, EventArgs e)
	{
	}
	
	void OnUpdated(object sender, EventArgs e)
	{
	}

	public void OnButtonLED()
	{
		if(led.Value == 0)
			led.Value = 1;
		else
			led.Value = 0;
	}

	public void OnButtonSound()
	{
		if(sound.Value == 0)
			sound.Value = 1;
		else
			sound.Value = 0;
	}

	public void OnButtonGripper()
	{
		if(servo_R.Angle == R_closeAngle)
			servo_R.Angle = R_openAngle;
		else
			servo_R.Angle = R_closeAngle;

		if(servo_L.Angle == L_closeAngle)
			servo_L.Angle = L_openAngle;
		else
			servo_L.Angle = L_closeAngle;
	}
}
