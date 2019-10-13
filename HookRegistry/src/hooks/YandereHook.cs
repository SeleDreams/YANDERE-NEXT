using System;
namespace Hooks
{
	[RuntimeHook]
	class Hook
	{

		public Hook()
		{
			HookRegistry.Register(YandereNext.YandereNextEngine.StartHook);
		}

		public static string[] GetExpectedMethods()
		{
			return new string[] { "WelcomeScript::Start" };
		}
		
	}
}
