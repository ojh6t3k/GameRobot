using UnityEngine;
using System.Collections;
using System;

namespace UnityRobot
{
	public class AngleModule : ModuleProxy
	{
		protected short _value;
		
		void Awake()
		{
			Reset();
		}
		
		// Use this for initialization
		void Start ()
		{
			
		}
		
		// Update is called once per frame
		void Update ()
		{
			
		}
		
		public override void Reset ()
		{
			_value = 0;
		}
		
		public override void Action ()
		{
		}
		
		public override void OnPop ()
		{
			Pop(ref _value);
		}
		
		public override void OnPush ()
		{
		}
		
		public float Angle
		{
			get
			{
				return (float)_value / 10f;
			}
		}
	}
}
