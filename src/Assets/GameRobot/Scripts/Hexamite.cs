using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.IO.Ports;
using System.Text;



namespace UnityRobot
{
	public class SimpleKalman
	{
		private float Q;
		private float R;
		private float P;
		private float X;
		private float K;

		public SimpleKalman()
		{
			Reset();
		}

		public void Reset()
		{
			Q = 0.001f; // 0.00001f
			R = 0.01f;
			P = 1f;
			X = 0f;
		}
				
		public float Process(float value)
		{
			K = (P + Q) / (P + Q + R);
			P = R * (P + Q) / (R + P + Q);

			float result = X + (value - X) * K;
			X = result;			
			return result;
		}
	}


	[Serializable]
	public class HexT
	{
		public int id;
		public float distance_X0Y0;
		public float distance_X1Y0;
		public float distance_X0Y1;
		public float distance_X1Y1;
		public Vector2 planeSize;
		public Vector3 position;

		private SimpleKalman _X0Y0_Kalman = new SimpleKalman();
		private SimpleKalman _X1Y0_Kalman = new SimpleKalman();
		private SimpleKalman _X0Y1_Kalman = new SimpleKalman();
		private SimpleKalman _X1Y1_Kalman = new SimpleKalman();

		public float DISTANCE_X0Y0
		{
			get
			{
				return distance_X0Y0;
			}
			set
			{
				distance_X0Y0 = _X0Y0_Kalman.Process(value);
			}
		}

		public float DISTANCE_X1Y0
		{
			get
			{
				return distance_X1Y0;
			}
			set
			{
				distance_X1Y0 = _X1Y0_Kalman.Process(value);
			}
		}

		public float DISTANCE_X0Y1
		{
			get
			{
				return distance_X0Y1;
			}
			set
			{
				distance_X0Y1 = _X0Y1_Kalman.Process(value);
			}
		}

		public float DISTANCE_X1Y1
		{
			get
			{
				return distance_X1Y1;
			}
			set
			{
				distance_X1Y1 = _X1Y1_Kalman.Process(value);
			}
		}

		public void Reset()
		{
			distance_X0Y0 = 0f;
			distance_X1Y0 = 0f;
			distance_X0Y1 = 0f;
			distance_X1Y1 = 0f;
			position = Vector3.zero;

			_X0Y0_Kalman.Reset();
			_X1Y0_Kalman.Reset();
			_X0Y1_Kalman.Reset();
			_X1Y1_Kalman.Reset();
		}

		public void ComputePosition()
		{
			float[] distance = new float[]{ distance_X0Y0, distance_X1Y0, distance_X0Y1, distance_X1Y1 };
			int[] index = new int[] { 0, 1, 2, 3 };
			for(int i=0; i<distance.Length; i++)
			{
				for(int j=0; j<(distance.Length - 1); j++)
				{
					if(distance[j] > distance[j+1])
					{
						float fTemp = distance[j];
						distance[j] = distance[j+1];
						distance[j+1] = fTemp;

						int iTemp = index[j];
						index[j] = index[j+1];
						index[j+1] = iTemp;
					}
				}
			}
			if(distance[0] == 0)
				return;

			float d0 = 0f;
			float d1 = 0f;
			float x = planeSize.x;
			float y = planeSize.y;
			float x2 = x * x;
			float y2 = y * y;
			Vector3 r = Vector3.zero;
			if(index[0] == 0)
			{
				d0 = distance_X0Y0 * distance_X0Y0;
				if(distance_X1Y0 > 0)
				{
					d1 = distance_X1Y0 * distance_X1Y0;
					r.x = (d0 - d1 + x2) / (2f * x);
					position.x = r.x;
				}
				if(distance_X0Y1 > 0)
				{
					d1 = distance_X0Y1 * distance_X0Y1;
					r.y = (d0 - d1 + y2) / (2f * y);
					position.y = r.y;
				}
				if(r.x != 0 && r.y != 0)
				{
					r.z = d0 - (r.x * r.x + r.y * r.y);
					if(r.z > 0)
						position.z = Mathf.Sqrt(r.z);
				}
			}
			else if(index[0] == 1)
			{
				d0 = distance_X1Y0 * distance_X1Y0;
				if(distance_X0Y0 > 0)
				{
					d1 = distance_X0Y0 * distance_X0Y0;
					r.x = (d0 - d1 + x2) / (2f * x);
					position.x = x - r.x;
				}
				if(distance_X1Y1 > 0)
				{
					d1 = distance_X1Y1 * distance_X1Y1;
					r.y = (d0 - d1 + y2) / (2f * y);
					position.y = r.y;
				}
				if(r.x != 0 && r.y != 0)
				{
					r.z = d0 - (r.x * r.x + r.y * r.y);
					if(r.z > 0)
						position.z = Mathf.Sqrt(r.z);
				}
			}
			else if(index[0] == 2)
			{
				d0 = distance_X0Y1 * distance_X0Y1;
				if(distance_X1Y1 > 0)
				{
					d1 = distance_X1Y1 * distance_X1Y1;
					r.x = (d0 - d1 + x2) / (2f * x);
					position.x = r.x;
				}
				if(distance_X0Y0 > 0)
				{
					d1 = distance_X0Y0 * distance_X0Y0;
					r.y = (d0 - d1 + y2) / (2f * y);
					position.y = y - r.y;
				}
				if(r.x != 0 && r.y != 0)
				{
					r.z = d0 - (r.x * r.x + r.y * r.y);
					if(r.z > 0)
						position.z = Mathf.Sqrt(r.z);
				}
			}
			else if(index[0] == 3)
			{
				d0 = distance_X1Y1 * distance_X1Y1;
				if(distance_X0Y1 > 0)
				{
					d1 = distance_X0Y1 * distance_X0Y1;
					r.x = (d0 - d1 + x2) / (2f * x);
					position.x = x - r.x;
				}
				if(distance_X1Y0 > 0)
				{
					d1 = distance_X1Y0 * distance_X1Y0;
					r.y = (d0 - d1 + y2) / (2f * y);
					position.y = y - r.y;
				}
				if(r.x != 0 && r.y != 0)
				{
					r.z = d0 - (r.x * r.x + r.y * r.y);
					if(r.z > 0)
						position.z = Mathf.Sqrt(r.z);
				}
			}
		}
	}

	[AddComponentMenu("UnityRobot/Hexamite")]
	public class Hexamite : MonoBehaviour
	{
		[HideInInspector]
		public List<string> portNames = new List<string>();
		public string portName;

		public EventHandler OnDisconnected;
		public EventHandler OnGetData;

		public Vector2 planeSize = new Vector2(1000, 1000);
		public int id_HexR_X0Y0;
		public int id_HexR_X1Y0;
		public int id_HexR_X0Y1;
		public int id_HexR_X1Y1;
		public HexT[] hexT_list;

		private int baudrate = 256000;
		private SerialPort _serialPort;

		private int _lastID = 0;
		private bool _runIPS = false;


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

			int[] idList = new int[] { id_HexR_X0Y0, id_HexR_X1Y0, id_HexR_X0Y1, id_HexR_X1Y1 };
			for(int i=0; i<idList.Length; i++)
			{
				for(int j=0; j<(idList.Length - 1); j++)
				{
					if(idList[j] > idList[j+1])
					{
						int temp = idList[j];
						idList[j] = idList[j+1];
						idList[j+1] = temp;
					}
				}
			}
			_lastID = idList[0];
		}

//		// Start ---------------------------------------------------------
//		void Start ()
//		{
//		}
		
		// Update ---------------------------------------------------------
		void Update ()
		{
			if(_serialPort.IsOpen == true && _runIPS == true)
			{
				while(true)
				{
					// Process RX
					string data = "";

					try
					{
						data = _serialPort.ReadLine();

						if (data.StartsWith("X"))
						{
						}
						else if (data.StartsWith("R"))
						{
							string[] tokens = data.Split(new char[] { ' ', '_' });
							if(tokens.Length > 2)
							{
								int rxID = int.Parse(tokens[0].Substring(1));
								int txID = int.Parse(tokens[1].Substring(1));
								if(tokens[2].StartsWith("A"))
								{
									int distance = int.Parse(tokens[2].Substring(1));
									foreach(HexT hexT in hexT_list)
									{
										if(hexT.id == txID)
										{
											if(rxID == id_HexR_X0Y0)
												hexT.distance_X0Y0 = distance;
											else if(rxID == id_HexR_X1Y0)
												hexT.distance_X1Y0 = distance;
											else if(rxID == id_HexR_X0Y1)
												hexT.distance_X0Y1 = distance;
											else if(rxID == id_HexR_X1Y1)
												hexT.distance_X1Y1 = distance;

											if(rxID == _lastID)
												hexT.ComputePosition();

											break;
										}
									}
								}
							}
						}
					}
					catch(TimeoutException)
					{
						break;
					}
					catch(IOException)
					{
						ErrorDisconnect();
						break;
					}
					catch(Exception e)
					{
						//Debug.Log(data);
						//Debug.Log(e);
					}
				}
			}
		}

		// StartIPS -------------------------------------
		public void StartIPS()
		{
			foreach(HexT hexT in hexT_list)
			{
				hexT.planeSize = planeSize;
				hexT.Reset();
			}

			SendCommand("M&$"); // start sync mode
			_runIPS = true;
		}

		// StopIPS -------------------------------------
		public void StopIPS()
		{
			SendCommand("M&%");
			_runIPS = false;
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
				_serialPort.PortName = "//./" + portName;
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

		private void SendCommand(string cmd)
		{
			try
			{
				byte[] asciiCMD = Encoding.ASCII.GetBytes(cmd);
				byte checksum = 0;
				for(int i=0; i<asciiCMD.Length; i++)
					checksum += asciiCMD[i];
				_serialPort.Write(cmd + "/");
				_serialPort.Write(new byte[] { checksum, 13 }, 0, 2 );
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
}
