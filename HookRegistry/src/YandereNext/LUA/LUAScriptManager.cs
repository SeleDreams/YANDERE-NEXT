using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MoonSharp.Interpreter;
using System.IO;
using UnityEngine.SceneManagement;

namespace YandereNext.LUA
{
	public class LUAScriptManager
	{


		public static Script ScriptInstance { get => _currentScript; }
		private static Script _currentScript;
		static GameObject _managerinstance;

		public static void LoadScript(string script)
		{
			if (!_loaded)
			{
				InitScript();
				_currentScript.DoString(script);
				_managerinstance = new GameObject("LUAScriptManager");
				_managerinstance.AddComponent<LUAScriptFunctionsCaller>();



			}
		}

		static void InitScript()
		{
			Script.DefaultOptions.ScriptLoader = new LUAScriptLoader()
			{
				ModulePaths = YandereNextEngine.ModulePaths,
				IgnoreLuaPathGlobal = true

			};
			_currentScript = new Script();
			LUAScriptLinking.StartLinking();
			_loaded = true;
		}

		

		static bool _loaded = false;
	}
}
