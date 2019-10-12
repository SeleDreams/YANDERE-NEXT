using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YandereNext
{
	class ModLoader : MonoBehaviour
	{
		static private bool hooked = false;
		static private GameObject YN_Object = null;
		static public string CurrentScene { get => _currentScene; }
		private static string _currentScene = "WelcomeScene";
		public static object StartHook(string typeName, string methodName, object thisObj, object[] args, IntPtr[] refArgs, int[] refIdxMatch)
		{
			if (!hooked)
			{
				var YandereNext_Object = new GameObject("YandereNext_Object");
				YandereNext_Object.AddComponent<ModLoader>();
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
			switch (_currentScene)
			{
				case "SponsorScene":
					SceneManager.LoadScene("LoadingScene");
					break;
				case "SchoolScene":
					var resource = (GameObject)Resources.Load("honoka/models legacy/f01_schoolwear_100_h");
					var yandere = FindObjectOfType<YandereScript>();
					var loadedResource = Instantiate(resource);
					loadedResource.transform.position = yandere.transform.position;
					loadedResource.transform.localScale *= 1;
					break;
				default:
					break;
			}
		}
		
	}
}
