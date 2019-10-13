using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MoonSharp.Interpreter;
using System.IO;
using UnityEngine.SceneManagement;

namespace YandereNext
{
	public class ScriptManager : MonoBehaviour
	{
		
		
		public static Script ScriptInstance { get => _currentScript; }
		static Script _currentScript;
		static ScriptLinking _link;

		static void LoadScript()
		{
			if (!_loaded)
			{
				InitScript();
				_currentScript.DoString(File.ReadAllText(YandereNextEngine.RootPath + "\\Plugins\\scripts\\yn_plugin.lua"));
			}
		}

		static void InitScript()
		{
			_loaded = true;
			_currentScript = new Script();
			_currentScript.Options.ScriptLoader = new YandereScriptLoader()
			{
				ModulePaths = new string[] {
						YandereNextEngine.RootPath + "\\Plugins\\scripts\\?_plugin.lua",
						YandereNextEngine.RootPath + YandereNextEngine.CurrentMod + "\\scripts\\?.lua"
					}

			};
			
			_link = new ScriptLinking();
			_link.RegisterTypes();
			_link.LinkFunctions();
			_link.LinkVariables();
		}

		void Awake()
		{
			LoadScript();
			SceneManager.sceneLoaded += OnSceneLoaded;
			_currentScript.Call(_link.GetGlobals("_Awake"));
		}

		void Start()
		{
			if (_loaded)
				_currentScript.Call(_link.GetGlobals("_Start"));
		}

		void Update()
		{
			if (_loaded)
				_currentScript.Call(_link.GetGlobals("_Update"));
		}

		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (_loaded)
				_currentScript.Call(_link.GetGlobals("_OnSceneLoaded"), scene.name, mode);
		}
		static bool _loaded;
	}
}
