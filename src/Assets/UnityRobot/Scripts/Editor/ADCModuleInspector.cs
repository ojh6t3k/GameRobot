using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using UnityRobot;

[CustomEditor(typeof(ADCModule))]
public class ADCModuleInspector : Editor
{
	public override void OnInspectorGUI()
	{
		ADCModule analog = (ADCModule)target;
		
		analog.id = EditorGUILayout.IntField("ID", analog.id);
		analog.threshold = EditorGUILayout.IntField("Threshold", analog.threshold);
		EditorGUILayout.LabelField("Value", analog.Value.ToString());

		if(Application.isPlaying == true)
			EditorUtility.SetDirty(target);
	}
}
