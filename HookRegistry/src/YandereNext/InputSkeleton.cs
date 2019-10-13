using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace YandereNext
{
	public class InputSkeleton
	{
		public bool GetKey(string key)
		{
			return Input.GetKey(key);
		}

		public bool GetKeyDown(string key)
		{
			return Input.GetKeyDown(key);
		}

		public bool GetKeyUp(string key)
		{
			return Input.GetKeyUp(key);
		}

		public float GetAxis(string axis)
		{
			return Input.GetAxis(axis);
		}

	}
}
