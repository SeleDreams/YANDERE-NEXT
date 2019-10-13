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
			DontDestroyOnLoad(gameObject);
		}
	
		void Awake()
		{			
			LUAScriptLinking.CallFunction("_Awake");
		}

		void Start()
		{
			LUAScriptLinking.CallFunction("_Start");
		}

		void Update()
		{
			LUAScriptLinking.CallFunction("_Update");
		}

		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			LUAScriptLinking.CallFunction("_OnSceneLoaded", scene.name, mode);
		}

	}
}
