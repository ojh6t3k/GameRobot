using UnityEngine;
using System.Collections;

public class AppGUIKeyBinder : MonoBehaviour
{
	public KeyMap[] keyMaps;

	private bool _ctrl = false;
	private bool _alt = false;
	private bool _shift = false;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.anyKeyDown == true)
		{
			foreach(KeyMap keyMap in keyMaps)
			{
				if(Input.GetKey(keyMap.key) == true
				   && keyMap.ctrl == _ctrl
				   && keyMap.alt == _alt
				   && keyMap.shift == _shift)
				{
					if(keyMap.guiControl.IsEnabled == true
					   && keyMap.guiControl.IsVisible == true)
					{
						keyMap.guiControl.DoClick();
					}
				}	
			}
		}
	}

	void OnGUI()
	{
		if(this.enabled == false)
			return;

		_ctrl = Event.current.control;
		_alt = Event.current.alt;
		_shift = Event.current.shift;
	}
}

[System.Serializable]
public class KeyMap
{
	public string name;
	public dfControl guiControl;
	public KeyCode key;
	public bool ctrl;
	public bool alt;
	public bool shift;
}
