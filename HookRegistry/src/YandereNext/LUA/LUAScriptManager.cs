using UnityEngine;
using MoonSharp.Interpreter;

namespace YandereNext.LUA
{
	public class LUAScriptManager
	{


        public static Script ScriptInstance { get; private set; }

        static GameObject _managerinstance;

		public static void LoadScript(string script)
		{
			if (!_loaded)
			{
				InitScript();
				ScriptInstance.DoString(script);
				_managerinstance = new GameObject("LUAScriptManager");
				_managerinstance.AddComponent<LUAScriptFunctionsCaller>();
			}
		}

		static void InitScript()
		{
			Script.DefaultOptions.ScriptLoader = new LUAScriptLoader()
			{
				ModulePaths = YandereNextManager.ModulePaths,
				IgnoreLuaPathGlobal = true

			};
			ScriptInstance = new Script();
			LUAScriptLinking.StartLinking();
			_loaded = true;
		}
		
		static bool _loaded = false;
	}
}
