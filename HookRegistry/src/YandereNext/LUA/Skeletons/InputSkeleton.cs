using UnityEngine;
using MoonSharp.Interpreter;

namespace YandereNext.LUA.Skeletons
{
	public class InputSkeleton
	{
		public static InputSkeleton CreateInstance()
		{
			return new InputSkeleton();
		}
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
