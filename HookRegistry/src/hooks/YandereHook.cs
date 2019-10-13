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

		// private void InitDynamicTypes() { }

		public static string[] GetExpectedMethods()
		{
			return new string[] { "WelcomeScript::Start" };
		}

		
	}
}
