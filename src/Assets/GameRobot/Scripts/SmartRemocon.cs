using UnityEngine;
using System;
using System.Collections;
using UnityRobot;

public class SmartRemocon : MonoBehaviour
{
	public AppGUIBasic appGUIBasic;
	public RobotProxy robotProxy;

	public dfLabel guiDRCMessage;
	public dfLabel guiTargetDistance;
	public dfButton guiShoot;
	public dfTouchJoystick guiLeftJoystic;
	public dfTouchJoystick guiRightJoystic;
	public dfTextureSprite guiCameraImage;

	public DRCwifi drcWiFi;
	public WheelController wheelController;
	public PanTiltController panTilt;
	public DistanceSensor distanceSensor;

	public WeaponSystem weapon;
	public GameObject laserObject;
	public GameObject wallObject;


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
		float distance = distanceSensor.Distance;

		wheelController.ControlCircle(guiLeftJoystic.Position);
		panTilt.ControlPoint(100f, guiRightJoystic.Position * 200);

		Texture image = drcWiFi.image;
		if(image != null)
			guiCameraImage.Texture = image;

		guiDRCMessage.IsVisible = !drcWiFi.Connected;
	//	guiDRCMessage.Text = drcWiFi.text;

		weapon.equipment = distanceSensor.isEnter;
		guiShoot.IsVisible = weapon.equipment;
		if(guiShoot.IsVisible == true)
		{
			if(distance > 0)
			{
				guiTargetDistance.IsVisible = true;
				guiTargetDistance.Text = string.Format("To Target: {0:f1}cm", distance / 10f);
				wallObject.transform.position = (laserObject.transform.forward * (distance / 60f)) + laserObject.transform.position;
			}
			else
				guiTargetDistance.IsVisible = false;

			if(guiShoot.State == dfButton.ButtonState.Pressed)
				weapon.shooting = true;
			else
				weapon.shooting = false;
		}
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
	}

	void OnExitMain(object sender, EventArgs e)
	{
		weapon.equipment = false;
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
}
