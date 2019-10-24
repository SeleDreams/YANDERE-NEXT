using UnityEngine;

namespace YandereNext.Debugging
{
	public class LogConsoleSkeleton
	{
		public static LogConsoleSkeleton CreateInstance()
		{
			return new LogConsoleSkeleton();
		}
		public void ToggleLog()
		{
			DisplayLog(!LogConsole.log);
		}

		public void DisplayLog(bool command)
		{
			LogConsole.log = command;
		}

		public void Clear()
		{
			LogManager.Clear();
		}
		

	}
}
