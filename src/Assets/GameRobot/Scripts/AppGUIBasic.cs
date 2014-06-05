using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityRobot
{
	public class AppGUIBasic : MonoBehaviour
	{
		public bool autoStart = false;
		public bool ignoreConnection = false;

		public dfPanel guiPanelStart;
		public dfPanel guiPanelSetting;
		public dfPanel guiPanelMain;
		public dfPanel guiPanelTerminal;
		public dfPanel guiPanelMessage;
		public dfDropdown guiPortList;
		public dfDropdown guiBluetoothList;
		public dfLabel guiMessage;
		public dfButton guiMessageOK;
		public RobotProxy robotProxy;

		public event EventHandler OnStartMainGUI;
		public event EventHandler OnExitMainGUI;
		public event EventHandler OnStartSettingGUI;
		public event EventHandler OnExitSettingGUI;
		public event EventHandler OnStartTerminalGUI;
		public event EventHandler OnExitTerminalGUI;

		void Awake()
		{
		}

		// Use this for initialization
		void Start ()
		{
			OnStartProgram();

			if(robotProxy != null)
			{
				robotProxy.OnConnected += OnConnected;
				robotProxy.OnConnectionFailed += OnConnectionFailed;
				robotProxy.OnDisconnected += OnDisconnected;
				robotProxy.OnSearchCompleted += OnSearchCompleted;
			}

			if(autoStart == true)
				OnStartMain();
		}
		
		// Update is called once per frame
		void Update ()
		{
			if(autoStart == true)
			{
				if(robotProxy != null)
				{
					if(robotProxy.Connected == true || ignoreConnection == true)
					{
						if(OnStartMainGUI != null)
							OnStartMainGUI(this, null);

						guiPanelStart.IsVisible = false;
						guiPanelMain.IsVisible = true;
						guiPanelMessage.IsVisible = false;
						autoStart = false;
					}
				}
			}	
		}

		void OnConnected(object sender, EventArgs e)
		{
		}
		
		void OnConnectionFailed(object sender, EventArgs e)
		{
			autoStart = false;
			guiMessage.Text = "Failed to connect Robot!";
			guiMessageOK.IsVisible = true;
		}
		
		void OnDisconnected(object sender, EventArgs e)
		{
			OnExitMain();
			
			guiPanelMessage.IsVisible = true;
			guiMessage.Text = "Disconnected Robot!";
			guiMessageOK.IsVisible = true;
		}
		
		void OnSearchCompleted(object sender, EventArgs e)
		{
#if UNITY_ANDROID
			guiPortList.IsVisible = false;
			guiBluetoothList.IsVisible = true;
			
			guiBluetoothList.Items = robotProxy.portNames.ToArray();
			guiBluetoothList.SelectedValue = robotProxy.portName;
			if(guiBluetoothList.SelectedIndex < 0)
				guiBluetoothList.SelectedIndex = 0;
#else
			guiPortList.IsVisible = true;
			guiBluetoothList.IsVisible = false;
			
			guiPortList.Items = robotProxy.portNames.ToArray();
			guiPortList.SelectedValue = robotProxy.portName;
			if(guiPortList.SelectedIndex < 0)
				guiPortList.SelectedIndex = 0;
#endif
			
			if(OnStartSettingGUI != null)
				OnStartSettingGUI(this, null);
		}

		public void OnStartProgram()
		{
			guiPanelStart.IsVisible = true;
			guiPanelSetting.IsVisible = false;
			guiPanelMain.IsVisible = false;
			guiPanelTerminal.IsVisible = false;
			guiPanelMessage.IsVisible = false;
		}

		public void OnExitProgram()
		{
			Application.Quit();
		}

		public void OnStartSetting()
		{
			guiPanelStart.IsVisible = false;
			guiPanelSetting.IsVisible = true;
			robotProxy.PortSearch();
		}

		public void OnExitSetting()
		{
			if(OnExitSettingGUI != null)
				OnExitSettingGUI(this, null);

			try
			{
	#if UNITY_ANDROID
				robotProxy.portName = guiBluetoothList.SelectedValue;
	#else
				robotProxy.portName = guiPortList.SelectedValue;
	#endif
			}
			catch(Exception)
			{
			}

			guiPanelStart.IsVisible = true;
			guiPanelSetting.IsVisible = false;
		}
		
		public void OnStartMain()
		{
			guiMessage.Text = "Connecting Robot...";
			guiPanelMessage.IsVisible = true;
			guiMessageOK.IsVisible = false;

			autoStart = true;
			if(ignoreConnection == false)
				robotProxy.Connect();
		}
		
		public void OnExitMain()
		{
			if(OnExitMainGUI != null)
				OnExitMainGUI(this, null);

			robotProxy.Disconnect();
			guiPanelStart.IsVisible = true;
			guiPanelMain.IsVisible = false;
		}
		
		public void OnStartTerminal()
		{
			if(OnStartTerminalGUI != null)
				OnStartTerminalGUI(this, null);

			guiPanelMain.IsVisible = false;
			guiPanelTerminal.IsVisible = true;
		}
		
		public void OnExitTerminal()
		{
			if(OnExitTerminalGUI != null)
				OnExitTerminalGUI(this, null);

			guiPanelMain.IsVisible = true;
			guiPanelTerminal.IsVisible = false;
		}
	}
}
