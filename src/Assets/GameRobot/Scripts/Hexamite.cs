using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.IO.Ports;



namespace UnityRobot
{
	[AddComponentMenu("UnityRobot/Hexamite")]
	public class Hexamite : MonoBehaviour
	{
		[HideInInspector]
		public List<string> portNames = new List<string>();
		public string portName;

		public EventHandler OnDisconnected;
		public EventHandler OnGetData;

		private int baudrate = 256000;
		private SerialPort _serialPort;

		// Data -------------
		public int nR_Number = 0;
		public int nT_Number = 0;
		public int nDistance = 0;



		void Awake()
		{
			_serialPort = new SerialPort();
			_serialPort.DtrEnable = true; // win32 hack to try to get DataReceived event to fire
			_serialPort.RtsEnable = true;
			_serialPort.DataBits = 8;
			_serialPort.Parity = Parity.None;
			_serialPort.StopBits = StopBits.One;
			_serialPort.ReadTimeout = 1; // since on windows we *cannot* have a separate read thread
			_serialPort.WriteTimeout = 1000;
			_serialPort.NewLine = "\r";
		}

//		// Start ---------------------------------------------------------
//		void Start ()
//		{
//		}
		
		// Update ---------------------------------------------------------
		void Update ()
		{
			if(_serialPort.IsOpen == true)
			{
				// Process RX
				try
				{
					Update_Parsing();
				}
				catch(TimeoutException)
				{
				}
				catch(Exception)
				{
					ErrorDisconnect();
				}
			}
		}



		// Update_Parsing -------------------------------------------------------------
		void Update_Parsing()
		{
			string data = _serialPort.ReadLine();

			if (data.StartsWith("R"))
			{
				nR_Number = int.Parse(data.Substring(1,2));
				nT_Number = 0;
				nDistance = 0;

				if (data.Length > 10)
				{
					nT_Number = int.Parse(data.Substring(5,2));
					nDistance = int.Parse(data.Substring(9,data.Length-9));
					//########## nDistance 변수값 때문에 에러가 발생하는 것 같다 디스컨넥팅 되는 현상-----------
				}

				OnGetData(this, null);
			}
			else
			{
				//Debug.Log(data);
			}
		}







		// StartIPS -------------------------------------
		public void StartIPS()
		{
			try
			{
				_serialPort.Write("v\n");
			}
			catch(TimeoutException)
			{
			}
			catch(Exception)
			{
				ErrorDisconnect();
			}
		}

		// StopIPS -------------------------------------
		public void StopIPS()
		{
			//Write(new byte[] { (byte)CMD.Exit });
		}





		public bool Connected
		{
			get
			{
				return _serialPort.IsOpen;
			}
		}

		public bool Connect()
		{
			try
			{
				_serialPort.PortName = portName;
				_serialPort.BaudRate = baudrate;
				_serialPort.Open();
				return true;
			}
			catch(Exception)
			{
				return false;
			}

		}

		public void Disconnect()
		{
			try
			{
				_serialPort.Close();
			}
			catch(Exception)
			{
			}
		}

		private void ErrorDisconnect()
		{
			try
			{
				_serialPort.Close();
			}
			catch(Exception)
			{
			}

			if(OnDisconnected != null)
				OnDisconnected(this, null);
		}

		public void PortSearch()
		{
			portNames.Clear();
			portNames.AddRange(SerialPort.GetPortNames());
		}
	}
}
