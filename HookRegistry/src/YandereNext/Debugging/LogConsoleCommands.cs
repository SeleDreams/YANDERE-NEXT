using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YandereNext.Debugging
{
	public static class LogConsoleCommands
	{
		public static void ToggleLog()
		{
			DisplayLog(!LogConsole.log);
		}
		public static void DisplayLog(bool command)
		{
			LogConsole.log = command;
		}
	}
}
