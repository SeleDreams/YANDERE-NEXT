using System;
using UnityEngine;
using MoonSharp.Interpreter;
using YandereNext.Debugging;
using YandereNext.LUA.Skeletons;

namespace YandereNext.LUA
{
	public static class LUAScriptLinking
	{
		public static void SetGlobals<T>(string key, T table)
		{
			LUAScriptManager.ScriptInstance.Globals[key] = table;
		}

		public static object GetGlobals(string key)
		{
			return LUAScriptManager.ScriptInstance.Globals[key];
		}

		public static void CallFunction(string name, params object[] args)
		{
			try
			{
				LUAScriptManager.ScriptInstance.Call(GetGlobals(name), args);
			}
			catch (Exception ex)
			{
				if (ex is SyntaxErrorException || ex is ScriptRuntimeException)
				{
					Debug.Log(ex.GetType().ToString() + " : " + ex.Message);
				}
			}
		}

		public static void StartLinking()
		{
			Debug.Log("Registering types");
			RegisterTypes();
			Debug.Log("Linking Functions");
			LinkFunctions();
			Debug.Log("Linking Data");
			LinkData();
			Debug.Log("Linking Static Classes");
			LinkStaticClasses();
			Debug.Log("Linking Classes");
			LinkClasses();
		}
	
		public static void RegisterTypes()
		{
			//UserData.RegistrationPolicy = InteropRegistrationPolicy.Automatic;
			UserData.RegisterType<GameObjectSkeleton>();
			UserData.RegisterType<InputSkeleton>();
		}

		// Links static functions to LUA
		public static void LinkFunctions()
		{
			var logAction = (Action<string>)Debug.Log;
			SetGlobals("print", logAction);

			var displayLog = (Action<bool>)LogConsoleCommands.DisplayLog;
			SetGlobals("DisplayLog", displayLog);

			var toggleLog = (Action)LogConsoleCommands.ToggleLog;
			SetGlobals("ToggleLog", toggleLog);
		}

		// Links classes that can be instantiated in LUA
		public static void LinkClasses()
		{
			var goSkeleton = (Func<GameObjectSkeleton>)GameObjectSkeleton.CreateInstance;
			SetGlobals("GameObject", goSkeleton);
		}

		//Links static classes to LUA
		public static void LinkStaticClasses()
		{
			SetGlobals("Input", InputSkeleton.CreateInstance());
		}

		// Links properties returning informations to LUA
		public static void LinkData()
		{
			SetGlobals("YN_CurrentMod", YandereNextEngine.CurrentMod);
		}
	}
}
