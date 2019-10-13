using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

namespace YandereNext
{
	public class ScriptLinking
	{
		TypeConverter _converter;
		InputSkeleton _input;
		public ScriptLinking()
		{
			_converter = new TypeConverter();
			_input = new InputSkeleton();
		}

		public void SetGlobals<T>(string key, T table)
		{
			ScriptManager.ScriptInstance.Globals[key] = table;
		}

		public object GetGlobals(string key)
		{
			return ScriptManager.ScriptInstance.Globals[key];
		}
		GameObjectSkeleton CreateGameObjectSkeleton()
		{
			return new GameObjectSkeleton();
		}

		void ToggleLog()
		{
			DisplayLog(!YandereNextEngine.DisplayLog);
		}
		void DisplayLog(bool log)
		{
			YandereNextEngine.DisplayLog = log;
		}
		public void RegisterTypes()
		{
			UserData.RegistrationPolicy = InteropRegistrationPolicy.Automatic;
		}
		public void LinkFunctions()
		{
			var logAction = (Action<string>)Debug.Log;
			SetGlobals("print", logAction);

			var goSkeleton = (Func<GameObjectSkeleton>)CreateGameObjectSkeleton;
			SetGlobals("GameObject", goSkeleton);

			var displayLog = (Action<bool>)DisplayLog;
			SetGlobals("DisplayLog", displayLog);

			var toggleLog = (Action)ToggleLog;
			SetGlobals("ToggleLog", toggleLog);
		}

		public void LinkVariables()
		{
			SetGlobals("Input", _input);
			SetGlobals("YN_CurrentScene", YandereNextEngine.CurrentScene);
		}
	}
}
