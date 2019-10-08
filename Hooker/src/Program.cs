using GameKnowledgeBase;
using System;
using System.IO;
using Hooker.util;

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

		static bool Download()
		{
			return false;
		}
		// Prepares the files for the mod engine to work without manual operations.
		static bool PrepareMod()
		{
			string dataDir = Environment.CurrentDirectory + "\\YandereSimulator_Data";
			bool rightDir = Directory.Exists(dataDir);
			if (!rightDir)
			{
				Console.WriteLine("Yandere Simulator not located in the directory.");
				Console.WriteLine("Do you want to download the latest version of the game ?");
				bool ok = false;
				do
				{
					Console.WriteLine("Input Y to confirm, Input N to quit.");
					string response = Console.ReadLine();

					switch (response)
					{
						case "Y":
						case "y":
							bool result = Download();
							if (result)
							{
								ok = true;
							}
							Console.WriteLine("failed to download");
							break;
						case "N":
						case "n":
							Environment.Exit(1);
							break;
						default:
							Console.WriteLine("Entry not recognized");
							break;
					}
				} while (!ok);

				Environment.Exit(1);
			}
			var YanNext_Dir = Directory.CreateDirectory(dataDir + "\\StreamingAssets\\YANDERE_NEXT");
			Directory.CreateDirectory(YanNext_Dir.FullName + "\\" + "Mods");
			Directory.CreateDirectory(YanNext_Dir.FullName + "\\" + "Plugins");
			Directory.CreateDirectory(YanNext_Dir.FullName + "\\" + "Debug");

			if (!File.Exists(YanNext_Dir.FullName + "\\Settings.json"))
			{
				File.Create(YanNext_Dir.FullName + "\\Settings.json");
			}
			return true;

		}
		static string[] CustomArgs(string[] args)
		{
			if (args.Length <= 0)
			{
				string[] hook = new string[] {"hook",
				"-d",Environment.CurrentDirectory,
				"-h", Environment.CurrentDirectory + "\\YandereHook",
				"-l",Environment.CurrentDirectory + "\\NextLib.dll"
			};
				string[] unhook = new string[] {"restore",
				"-d",Environment.CurrentDirectory
			};
				if (!File.Exists(Environment.CurrentDirectory + "\\hooked"))
				{
					try
					{
						using (File.Create(Environment.CurrentDirectory + "\\hooked"))
						{

						}
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
				else
				{
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
			}
			return args;
		}
		static int Main(string[] args)
		{
			PrepareMod();
			string[] actions = CustomArgs(args);
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

			// All OK
			return 0;

			ERROR:
			return 1;
		}
	}
}

