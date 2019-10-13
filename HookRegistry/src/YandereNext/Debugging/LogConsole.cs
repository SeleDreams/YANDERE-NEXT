using System;
using System.Collections.Generic;
using UnityEngine;

namespace YandereNext.Debugging
{
	public class LogConsole : MonoBehaviour
	{
		static public bool log = true;

		void Start()
		{
			pos = new Vector2(2, 2);
			size = new Vector2(200, 400);
			logStyle = new GUIStyle();
			logStyle.normal.textColor = Color.blue;
		}

		void OnEnable()
		{
			Application.logMessageReceived += new Application.LogCallback(Log);
		}

		void OnDisable()
		{
			Application.logMessageReceived -= new Application.LogCallback(Log);
		}

		void Log(string message, string stackTrace, LogType logtype)
		{
			if (logMessages.Count == _maxLines)
			{
				logMessages.RemoveAt(0);
			}
			logMessages.Add(message);
			var sArray = logMessages.ToArray();
			logContent = String.Join("\n", sArray);
		}


		void OnGUI()
		{
			if (log)
			{
				var textRect = new Rect(pos, size);
				GUI.Label(textRect, logContent, logStyle);
			}
		}

		// Private members
		private List<string> logMessages = new List<string>();
		private int _maxLines = 20;
		private string logContent;
		private Vector2 pos, size;
		private GUIStyle logStyle;
	}
}
