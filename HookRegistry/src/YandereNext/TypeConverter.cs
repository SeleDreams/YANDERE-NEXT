using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoonSharp.Interpreter;
using UnityEngine;
namespace YandereNext
{
	public class TypeConverter
	{

		public static DynValue CastObject(object obj)
		{
			return DynValue.FromObject(ScriptManager.ScriptInstance, obj);
		}
		public static T TryCast<T>(DynValue value)
		{
			return value.ToObject<T>();
		}
		public static DynValue CastString(string s)
		{
			return DynValue.NewString(s);
		}

		public static DynValue CastInt(int n)
		{
			return DynValue.NewNumber(n);
		}
	}
}
