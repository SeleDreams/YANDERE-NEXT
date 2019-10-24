using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;
using YandereNext.Tools;
using MoonSharp.Interpreter;

namespace YandereNext.LUA.Skeletons
{
	public static class InstanceCreator
	{
		public static GameObject New_GameObject()
		{
			return new GameObject();

		}

		public static Vector3 New_Vector3()
		{
			return new Vector3();
		}

		public static Vector2 New_Vector2()
		{
			return new Vector2();
		}

		public static UnityEngine.Object New_Object()
		{
			return new UnityEngine.Object();
		}
	}
}
