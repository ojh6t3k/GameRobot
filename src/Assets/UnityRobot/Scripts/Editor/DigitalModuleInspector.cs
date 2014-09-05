using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using UnityRobot;

[CustomEditor(typeof(DigitalModule))]
public class DigitalModuleInspector : Editor
{
	public override void OnInspectorGUI()
	{
		DigitalModule digital = (DigitalModule)target;

		digital.id = EditorGUILayout.IntField("ID", digital.id);
		digital.mode = (DigitalModule.Mode)EditorGUILayout.EnumPopup("Mode", digital.mode);

		int index = (int)digital.Value;
		if(index > 0)
			index = 1;
		int newIndex = GUILayout.SelectionGrid(index, new string[] {"LOW", "HIGH"}, 2);
		if(digital.mode == DigitalModule.Mode.OUTPUT)
			digital.Value = (byte)newIndex;
		else
			EditorUtility.SetDirty(target);
	}
}
