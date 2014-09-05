using UnityEngine;
using System.Collections;
using System;

namespace UnityRobot
{
	public class IMUModule : ModuleProxy
	{
		protected short _roll;
		protected short _pitch;
		
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
			_roll = 0;
			_pitch = 0;
		}
		
		public override void Action ()
		{
		}
		
		public override void OnPop ()
		{
			Pop(ref _roll);
			Pop(ref _pitch);
		}
		
		public override void OnPush ()
		{
		}
		
		public Vector2 Angle
		{
			get
			{
				return new Vector2((float)_roll / 10f, (float)_pitch / 10f);
			}
		}
	}
}
