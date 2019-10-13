using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using YandereNext.Debugging;
using YandereNext.LUA;
using System.IO;

namespace YandereNext
{
	class YandereNextEngine : MonoBehaviour
	{
		// Public members
		static public string CurrentMod { get => RootPath + "\\Mods\\" + _currentMod + "\\"; }
		static public readonly string RootPath = Environment.CurrentDirectory + "\\YandereSimulator\\YandereSimulator_Data\\StreamingAssets\\Yandere_Next\\";
		static public string CurrentScene { get => _currentScene; }
		public static string[] ModulePaths
		{
			get => new string[] {
						RootPath + "\\Plugins\\scripts\\?_plugin.lua",
						CurrentMod + "\\scripts\\?.lua"
					};
		}

		
		public static object StartHook(string typeName, string methodName, object thisObj, object[] args, IntPtr[] refArgs, int[] refIdxMatch)
		{
			if (!hooked)
			{
				var YandereNext_Object = new GameObject("YandereNext_Object");
				YandereNext_Object.AddComponent<YandereNextEngine>();
				YandereNext_Object.AddComponent<LogConsole>();
				DontDestroyOnLoad(YandereNext_Object);
				YN_Object = YandereNext_Object;
				hooked = true;
				Debug.Log("Starting scripting system");
				LUAScriptManager.LoadScript(File.ReadAllText(CurrentMod + "\\scripts\\main.lua"));
			}
			return null;
		}


		void Awake()
		{
			SceneManager.sceneLoaded += OnSceneLoaded;
		}


		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			_currentScene = scene.name;

			//	Debug.Log(scene.name + " loaded");
			switch (_currentScene)
			{
				case "SponsorScene":
					//SceneManager.LoadScene("LoadingScene");
					break;
				/*case "SchoolScene":
					var resource = (GameObject)Resources.Load("honoka/models legacy/f01_schoolwear_100_h");
					var yandere = FindObjectOfType<YandereScript>();
					var loadedResource = Instantiate(resource);
					loadedResource.transform.position = yandere.transform.position;
					loadedResource.transform.localScale *= 1;
					break;*/
				default:
					break;
			}
		}

		// Private members
		private static string _currentScene = "WelcomeScene";
		private static string _currentMod = "default";
		static private bool hooked = false;
		static private GameObject YN_Object = null;

	}
}
