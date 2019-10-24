using GameKnowledgeBase;
using System;
using System.IO;
using Hooker.util;
using Hooker.src.util;

namespace Hooker
{
	class Program
	{
		// Operation verbs
		public const string OPERATION_HOOK = "hook";
		public const string OPERATION_RESTORE = "restore";

		public const string EXCEPTION_MSG =
			"An exception occurred and assemblies might be in inconsistent state. " +
			"Please use the restore function to bring back the assemblies to original state!";

		// The logger to use for communicating messages
		public static Logger Log;

		// Prepare the general options
		public static void Prepare(GeneralOptions options)
		{
			// Initialise new logger
			Log = new Logger(options);

			// Check the game path
			string gamePath = Path.GetFullPath(options.GamePath);
			options.GamePath = gamePath;
			if (!Directory.Exists(gamePath))
			{
				throw new DirectoryNotFoundException("Exe option `gamedir` is invalid!");
			}
		}



		static string[] CustomArgs(string[] args)
		{
			foreach (string s in args)
			{
				Console.Write(s + " ");
			}
			string[] hook = new string[] {"hook",
				"-d",Environment.CurrentDirectory + "\\YandereSimulator",
				"-h", Environment.CurrentDirectory + "\\YandereHook",
				"-l",Environment.CurrentDirectory + "\\NextLib.dll"
			};
			string[] unhook = new string[] {"restore",
				"-d",Environment.CurrentDirectory + "\\YandereSimulator"
			};
			if (args.Length <= 0)
			{
				Console.WriteLine("NO ARGS");
				
				if (!File.Exists(Environment.CurrentDirectory + "\\hooked"))
				{
					try
					{
						File.Create(Environment.CurrentDirectory + "\\hooked");
					}
					catch (Exception ex)
					{
						Console.WriteLine("An exception occurred when creating the hook file");
						Console.WriteLine(ex.GetType().ToString());
						Console.ReadLine();
						Environment.Exit(1);
					}
					return hook;
				}

			}
			else if (args.Length >= 1 && args[0] == "unhook")
			{
				Console.WriteLine("UNHOOK");
				try
				{

					File.Delete(Environment.CurrentDirectory + "\\hooked");
				}
				catch (Exception ex)
				{
					Console.WriteLine("An exception occurred when deleting the hook file");
					Console.WriteLine(ex.GetType().ToString());
					Console.ReadLine();
					Environment.Exit(1);
				}

				return unhook;
			}
			return args;
		}

		static void LaunchGame()
		{
			var process = new System.Diagnostics.Process();
			var startInfo = new System.Diagnostics.ProcessStartInfo
			{
				FileName = Environment.CurrentDirectory + "\\YandereSimulator\\YandereSimulator.exe"
			};
			process.StartInfo = startInfo;
			process.Start();
		}
		static int Main(string[] args)
		{
				YanderePrepare.PrepareMod();
		//	if (!File.Exists("Scenes.txt"))
			//	YanderePrepare.GetSceneNames();
			//if (!File.Exists("Resources.txt"))
			//	YanderePrepare.GetResourcesNames();
				if (File.Exists(Environment.CurrentDirectory + "\\hooked"))
				{
					LaunchGame();
					return 0;
				}
				
			string[] actions = CustomArgs(args);
			//Console.ReadLine();
			// Operation
			string invokedOperation = "";
			// General options.
			GeneralOptions generalOptions = null;
			// Options specific to the action to perform.
			object invokedOperationOptions = null;

			var opts = new Options();
			// Must check for null, because the parser won't..
			if (actions == null || actions.Length == 0)
			{
				Console.WriteLine(opts.GetUsage("help"));
				goto ERROR;
			}

			CommandLine.Parser.Default.ParseArgumentsStrict(actions, opts, (verb, subOptions) =>
			{
				// Action to store correct information for further instructing the processor.
				invokedOperation = verb;
				invokedOperationOptions = subOptions;
			}, () =>
			{
				// Failed attempt at parsing the provided arguments.
				Environment.Exit(-2);
			});

			try
			{
				// Process general options
				generalOptions = (GeneralOptions)invokedOperationOptions;
				Prepare(generalOptions);
			}
			catch (Exception e)
			{
				Log.Exception(e.Message, e);
				goto ERROR;
			}

			// Use knowledge about the game HearthStone. Game knowledge is defined in the shared code
			// project KnowledgeBase. See `GameKnowledgeBase.HSKB` for more information.
			// Change the following line if you want to hook another game.
			var gameKnowledge = new GameKB(generalOptions.GamePath, new YandereSimulatorKB());

			try
			{
				switch (invokedOperation)
				{

					case OPERATION_HOOK:
						Log.Info("Hooking operation started!\n");
						var hookHelper = new HookHelper((HookSubOptions)invokedOperationOptions);
						hookHelper.TryHook(gameKnowledge);
						Log.Info("Hooked the game libraries!");

						break;
					case OPERATION_RESTORE:
						Log.Info("Restore operation started!\n");
						var restore = new Restore((RestoreSubOptions)invokedOperationOptions);
						restore.TryRestore(gameKnowledge);
						Log.Info("Restored the original game libraries!");
						break;
					default:
						throw new ArgumentException("Invalid verb processed");
				}
			}
			catch (Exception e)
			{
				Log.Exception(EXCEPTION_MSG, e);
				goto ERROR;
			}

			//Console.ReadLine();
			// All OK
			if (args.Length == 0)
			{
				LaunchGame();
			}
			return 0;

			ERROR:
			var tprocess = new System.Diagnostics.Process();
			var tstartInfo = new System.Diagnostics.ProcessStartInfo
			{
				FileName = Environment.CurrentDirectory + "\\unhook.bat"
			};
			tprocess.StartInfo = tstartInfo;
			tprocess.Start();
			Console.ReadLine();
			return 1;
		}
		
	}
}

