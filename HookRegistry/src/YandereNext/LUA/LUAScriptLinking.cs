using System;
using UnityEngine;
using MoonSharp.Interpreter;
using YandereNext.Debugging;
using YandereNext.LUA.Skeletons;
using UnityEngine.SceneManagement;
using System.IO;
using YandereNext.Tools;
using MoonSharp.Interpreter.Interop;

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
				if (!(ex is ArgumentException))
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
		}

		public static void SendToLUA<T>(Func<T> method = null)
		{
			UserData.RegisterType<T>();
			Type t = typeof(T);
			SetGlobals(t.Name, t);
			if (method != null)
			{
				SetGlobals(method.Method.Name, method);
			}
			//Debug.Log(t.Name);
		}

		public static void RegisterTypes()
		{
			//UserData.RegistrationPolicy = InteropRegistrationPolicy.;
			SendToLUA(InstanceCreator.New_GameObject);
			SendToLUA<UnityEngine.Object>();
			SendToLUA<Input>();
			SendToLUA<Debug>();
			SendToLUA<SceneManager>();
			SendToLUA<AssetBundle>();
			SendToLUA(InstanceCreator.New_Vector3);
			SendToLUA(InstanceCreator.New_Vector2);
			SendToLUA<Transform>();
			SendToLUA<Resources>();
			SendToLUA(InstanceCreator.New_Object);
			SendToLUA<MonoBehaviour>();
			SendToLUA<LogConsole>();
			SendToLUA<ModelRetarget>();
			SendToLUA<Camera>();
		}
		// Links static functions to LUA
		public static void LinkFunctions()
		{
			var logAction = (Action<string>)Debug.Log;
			SetGlobals("print", logAction);
			var tFromString = (Func<string, Type>)GeneralFunctions.GetTypeFromString;
			SetGlobals("GetTypeFromString", tFromString);
			var getT = (Func<UserData, Type>)GeneralFunctions.GetType;
			SetGlobals("GetType", getT);
			var inherits = (Func<Type, Type, bool>)GeneralFunctions.Inherits;
			SetGlobals("Inherits", inherits);
			var objectOfType = (Func<string, UnityEngine.Object>)GeneralFunctions.FindObjectOfType;
			SetGlobals("FindObjectOfType", objectOfType);
			var hiddenObjects = (Func<string, GameObject[]>)GeneralFunctions.FindHiddenObjects;
			SetGlobals("FindHiddenObjects", hiddenObjects);
			var setHair = (Action<int>)GeneralFunctions.SetHairstyle;
			SetGlobals("SetHairstyle", setHair);
			var setAccessory = (Action<int>)GeneralFunctions.SetAccessory;
			SetGlobals("SetAccessory", setAccessory);
			var getStudent = (Func<int,GameObject>)GeneralFunctions.GetStudent;
			SetGlobals("GetStudent", getStudent);
			var setPersona = (Action<int>)GeneralFunctions.SetPersona;
			SetGlobals("SetPersona", setPersona);
			var setStudentHair = (Action<int,string>)GeneralFunctions.UpdateStudentHair;
			SetGlobals("SetStudentHairstyle", setStudentHair);
			var fixCam = (Action)GeneralFunctions.FixStreetCamera;
			SetGlobals("FixCamera", fixCam);
			var unlockMech = (Action)GeneralFunctions.UnlockMech;
			SetGlobals("UnlockMech", unlockMech);
			var spawn = (Action)GeneralFunctions.SpawnOsana;
			SetGlobals("SpawnOsana", spawn);
		}

		// Links properties returning informations to LUA
		public static void LinkData()
		{
			SetGlobals("RootDir", YandereNextManager.RootDir);
			SetGlobals("PluginsDir", YandereNextManager.PluginsDir);
			SetGlobals("ModDir", YandereNextManager.ModDir);
			SetGlobals("BundlesDir", YandereNextManager.BundlesDir);
		}
	}
}
