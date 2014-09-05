using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using UnityRobot;

[CustomEditor(typeof(PWMModule))]
public class PWMModuleInspector : Editor
{
	public override void OnInspectorGUI()
	{
		PWMModule pwm = (PWMModule)target;
		
		pwm.id = EditorGUILayout.IntField("ID", pwm.id);
		pwm.Value = (int)EditorGUILayout.Slider("Value", pwm.Value, 0, 255);
	}
}
