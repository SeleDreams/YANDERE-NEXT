using MoonSharp.Interpreter;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YandereNext.LUA
{
	public class LUAScriptFunctionsCaller : MonoBehaviour
	{

		LUAScriptFunctionsCaller()
		{
			SceneManager.sceneLoaded += OnSceneLoaded;
			SceneManager.sceneUnloaded += OnSceneUnloaded;
			DontDestroyOnLoad(gameObject);
		}
	
		void Awake()
		{			
			LUAScriptLinking.CallFunction("Awake");
		}

		void Start()
		{
			LUAScriptLinking.CallFunction("Start");
		}

		void Update()
		{
			LUAScriptLinking.CallFunction("Update");
		}

		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			LUAScriptLinking.CallFunction("OnSceneLoaded", scene.name, mode);
		}

		void OnSceneUnloaded(Scene scene)
		{
			LUAScriptLinking.CallFunction("OnSceneUnloaded", scene.name);
		}

	}
}
