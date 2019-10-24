namespace Hooks
{
	[RuntimeHook]
	class Hook
	{

		public Hook()
		{
			HookRegistry.Register(YandereNext.YandereNextManager.StartHook);
		}

		public static string[] GetExpectedMethods()
		{
			return new string[] { "WelcomeScript::Start" };
		}
		
	}
}
