using System;
using System.Collections.Generic;
using UnityEngine;

namespace YandereNext.Debugging
{
	class LogManager
	{
		public static string Log { get => logContent; }

		public static void Clear()
		{
			logMessages.Clear();
			logContent = "";
		}
		public static void SetupLog()
		{
			Application.logMessageReceived += new Application.LogCallback(Register);
		}

		static void Register(string message, string stackTrace, LogType logtype)
		{
			if (logMessages.Count == _maxLines)
			{
				logMessages.RemoveAt(0);
			}
			logMessages.Add(message);
			var sArray = logMessages.ToArray();
			logContent = String.Join("\n", sArray);
		}

		static private List<string> logMessages = new List<string>();
		static private int _maxLines = 20;
		static private string logContent = "";
	}
}
