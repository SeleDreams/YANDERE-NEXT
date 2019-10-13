using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YandereNext
{
	class YandereNextEngine : MonoBehaviour
	{
		static private bool hooked = false;
		static private GameObject YN_Object = null;
		static public bool DisplayLog = true;
		static public string  CurrentMod { get => _currentMod; }
		static public readonly string RootPath = Environment.CurrentDirectory + "\\YandereSimulator\\YandereSimulator_Data\\StreamingAssets\\Yandere_Next\\";
		static public string CurrentScene { get => _currentScene; }

		private static string _currentScene = "WelcomeScene";
		private static string _currentMod = "default";

		public static object StartHook(string typeName, string methodName, object thisObj, object[] args, IntPtr[] refArgs, int[] refIdxMatch)
		{
			if (!hooked)
			{
				var YandereNext_Object = new GameObject("YandereNext_Object");
				YandereNext_Object.AddComponent<YandereNextEngine>();
				YandereNext_Object.AddComponent<Debugger>();
				YandereNext_Object.AddComponent<ScriptManager>();
				DontDestroyOnLoad(YandereNext_Object);
				YN_Object = YandereNext_Object;
				hooked = true;
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
		
	}
}
