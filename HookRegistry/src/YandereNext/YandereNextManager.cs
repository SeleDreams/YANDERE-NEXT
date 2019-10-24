using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using YandereNext.Debugging;
using YandereNext.LUA;
using System.IO;

namespace YandereNext
{
	class YandereNextManager
	{
		// Public members
		
		static public readonly string RootDir = Application.streamingAssetsPath + "/Yandere_Next/";
		static public readonly string PluginsDir = RootDir + "/Plugins/";
		static public string ModDir { get => RootDir + "/Mods/" + _currentMod + "/"; }
		static public string BundlesDir { get => ModDir + "/bundles/"; }
		static public GameObject Instance { get => YN_Object; }

		public static string[] ModulePaths
		{
			get => new string[] {
						RootDir + "/Plugins/scripts/?_plugin.lua",
						ModDir + "/scripts/?.lua"
					};
		}

		
		public static object StartHook(string typeName, string methodName, object thisObj, object[] args, IntPtr[] refArgs, int[] refIdxMatch)
		{
			if (!hooked)
			{
				var YandereNext_Object = new GameObject("YandereNext_Object");
				YandereNext_Object.AddComponent<YandereNextEngine>();
				LogManager.SetupLog();
				YandereNext_Object.AddComponent<LogConsole>();
				YN_Object = YandereNext_Object;
				hooked = true;
				Debug.Log("Starting scripting system");
				LUAScriptManager.LoadScript(File.ReadAllText(ModDir + "/scripts/main.lua"));
			}
			return null;
		}
		
		// Private members
		private static string _currentMod = "default";
		static private bool hooked = false;
		static private GameObject YN_Object = null;

	}
}
