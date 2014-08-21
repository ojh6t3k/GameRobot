using UnityEngine;
using System.Collections;
using UnityRobot;

namespace UnityRobot
{
	public class ReportModule : ModuleProxy
	{
		public int maxDataNum = 1000;
		public int intervalMicros = 1000;
		
		protected ArrayList _dataList = new ArrayList();

		private int _intervalMicros;
		private ArrayList _tempList = new ArrayList();
		private byte _reportedDataNum = 0;
		
		void Awake()
		{
			Reset();
		}
		
		// Use this for initialization
		void Start ()
		{
			_intervalMicros = intervalMicros;
		}
		
		// Update is called once per frame
		void Update ()
		{
			if(_intervalMicros != intervalMicros)
			{
				_intervalMicros = intervalMicros;
				canUpdate = true;
			}
		}
		
		public override void Reset ()
		{
			_reportedDataNum = 0;
			_dataList.Clear();
			canUpdate = true;
		}
		
		public override void Action ()
		{
			int numRemove = (_dataList.Count + _tempList.Count) - maxDataNum;
			if(numRemove > 0)
				_dataList.RemoveRange(0, numRemove);

			_dataList.AddRange(_tempList);
		}
		
		public override void OnPop ()
		{
			_tempList.Clear();
			Pop(ref _reportedDataNum);
			for(int i=0; i<_reportedDataNum; i++)
			{
				short data = 500;
				Pop(ref data);
				_tempList.Add((int)data);
			}
		}
		
		public override void OnPush ()
		{
			Push((ushort)_intervalMicros);
		}
		
		public int[] DataList
		{
			get
			{
				return (int[])_dataList.ToArray(typeof(int));
			}
		}

		public int ReportedDataNum
		{
			get
			{
				return (int)_reportedDataNum;
			}
		}
	}
}