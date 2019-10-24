using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace YandereNext
{
	class YandereNextEngine : MonoBehaviour
	{
		void Awake()
		{
			DontDestroyOnLoad(gameObject);
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
		private static string _currentScene = "WelcomeScene";
	}
}
