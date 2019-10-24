using UnityEngine;

namespace YandereNext.Debugging
{
	public class LogConsole : MonoBehaviour
	{
		static public bool log = true;

		void Clear()
		{
			LogManager.Clear();
		}
		void Start()
		{
			pos = new Vector2(2, 2);
			size = new Vector2(200, 400);
			logStyle = new GUIStyle();
			logStyle.normal.textColor = Color.blue;
		}

		void OnGUI()
		{
			if (log)
			{
				var textRect = new Rect(pos, size);
				GUI.Label(textRect, LogManager.Log, logStyle);
			}
		}

		// Private members
		private Vector2 pos, size;
		private GUIStyle logStyle;
	}
}
