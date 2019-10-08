using System;
namespace Hooks
{
	[RuntimeHook]
	class Hook
	{

		public Hook()
		{
			HookRegistry.Register(YandereNext.ModLoader.Start);
			
		}

		private void InitDynamicTypes() { }

		public static string[] GetExpectedMethods()
		{
			return new string[] { "WelcomeScript::Start" };
		}

		
	}
}
