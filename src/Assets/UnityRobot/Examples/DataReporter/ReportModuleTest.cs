using UnityEngine;
using System.Collections;
using System;
using UnityRobot;


public class ReportModuleTest : MonoBehaviour
{
	public RobotProxy robot;
	public ReportModule reporter;
	public Rect screenRect;
	public float distance = 10f;
	public Material lineMaterial;

	private string _statusMessage = "Ready";
	private bool _connecting = false;
	
	// Use this for initialization
	void Start ()
	{
		robot.OnConnected += OnConnected;
		robot.OnConnectionFailed += OnConnectionFailed;
		robot.OnDisconnected += OnDisconnected;
		robot.OnSearchCompleted += OnSearchCompleted;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnPostRender()
	{
		// Draw graph
		float intervalX = screenRect.width / reporter.maxDataNum;
		float scaleValue = screenRect.height / 2048f;
		int[] data = reporter.DataList;

		lineMaterial.SetPass(0);

		GL.Begin( GL.LINES );
		GL.Color(Color.white);
		GL.Vertex3(screenRect.x, screenRect.y, distance);
		GL.Vertex3(screenRect.x, screenRect.y + screenRect.height, distance);
		GL.Vertex3(screenRect.x, screenRect.y + screenRect.height, distance);
		GL.Vertex3(screenRect.x + screenRect.width, screenRect.y + screenRect.height, distance);
		GL.Vertex3(screenRect.x + screenRect.width, screenRect.y + screenRect.height, distance);
		GL.Vertex3(screenRect.x + screenRect.width, screenRect.y, distance);
		GL.Vertex3(screenRect.x + screenRect.width, screenRect.y, distance);
		GL.Vertex3(screenRect.x, screenRect.y, distance);

		GL.Vertex3(screenRect.x, screenRect.y + screenRect.height * 0.5f, distance);
		GL.Vertex3(screenRect.x + screenRect.width, screenRect.y + screenRect.height * 0.5f, distance);


		GL.Color(Color.yellow);
		for(int i=0; i<data.Length; i++)
		{
			if(i == (data.Length - 1))
			{
				GL.Vertex3(screenRect.x + (data.Length - i) * intervalX, screenRect.y + (screenRect.height * 0.5f) + data[i] * scaleValue, distance);
				GL.Vertex3(screenRect.x, screenRect.y + (screenRect.height * 0.5f), distance);
			}
			else
			{
				GL.Vertex3(screenRect.x + (data.Length - i) * intervalX, screenRect.y + (screenRect.height * 0.5f) + data[i] * scaleValue, distance);
				GL.Vertex3(screenRect.x + (data.Length - (i + 1)) * intervalX, screenRect.y + (screenRect.height * 0.5f) +  data[i + 1] * scaleValue, distance);
			}
		}
		if(data.Length > 0)
			Debug.Log(data[data.Length - 1]);

		GL.End();
	}
	
	void OnGUI()
	{
		Rect guiRect = new Rect(10, 10, 0, 25);
		
		if(robot.Connected == true)
		{
			guiRect.width = 100;
			if(GUI.Button(guiRect, "Disconnect") == true)
			{
				_statusMessage = "Disconnected";
				robot.Disconnect();
			}
			guiRect.y += (guiRect.height + 5);

			guiRect.width = 100;
			if(GUI.Button(guiRect, "Reset") == true)
			{
				reporter.Reset();
			}
			guiRect.y += (guiRect.height + 5);

			guiRect.width = 300;
			GUI.Label(guiRect, string.Format("Reported Number: {0:d}", reporter.ReportedDataNum));
			guiRect.y += (guiRect.height + 5);
		}
		else
		{
			if(_connecting == false)
			{
				guiRect.width = 100;
				if(GUI.Button(guiRect, "Search") == true)
				{
					_statusMessage = "Searching...";
					robot.PortSearch();
				}
				guiRect.y += (guiRect.height + 5);
				
				if(robot.portNames.Count > 0)
				{
					guiRect.width = 100;
					for(int i=0; i<robot.portNames.Count; i++)
					{
						if(GUI.Button(guiRect, robot.portNames[i]) == true)
						{
							_statusMessage = "Connecting...";
							_connecting = true;
							robot.portName = robot.portNames[i];
							robot.Connect();
						}
						guiRect.x += (100 + 5);
					}
					guiRect.x = 10;
					guiRect.y += (guiRect.height + 5);
				}
			}
		}
		
		guiRect.width = 300;
		GUI.Label(guiRect, _statusMessage);
	}
	
	void OnConnected(object sender, EventArgs e)
	{
		_statusMessage = "Success to conncet";
		_connecting = false;
	}
	
	void OnConnectionFailed(object sender, EventArgs e)
	{
		_statusMessage = "Failed to conncet";
		_connecting = false;
	}
	
	void OnDisconnected(object sender, EventArgs e)
	{
		_statusMessage = "Disconnected";
		_connecting = false;
	}
	
	void OnSearchCompleted(object sender, EventArgs e)
	{
		_statusMessage = "Search completed";
	}
}
