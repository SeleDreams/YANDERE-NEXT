using UnityEngine.SceneManagement;

namespace YandereNext.LUA.Skeletons
{
	public class SceneManagerSkeleton
	{
		public void LoadScene(string name)
		{
			SceneManager.LoadScene(name);
		}

		public static SceneManagerSkeleton CreateInstance()
		{
			return new SceneManagerSkeleton();
		}
	}
}
