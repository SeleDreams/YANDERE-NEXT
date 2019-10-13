using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Hooker.src.util
{


	public static class YanderePrepare
	{
		private static bool StartDownload()
		{
			bool result = true;
			var isOnline = false;
			isOnline = YandereDownloader.OnlineCheck("yanderesimulator.com");
			if (isOnline)
			{
				Console.WriteLine("Server ok");
				var downloader = new YandereDownloader();
				bool dlresult = false;
				dlresult = downloader.DownloadURLS() && downloader.DownloadGame() && downloader.ExtractGame();
				if (!dlresult)
				{
					Console.WriteLine("One of more files couldn't be downloaded properly");
					result = false;
					Console.ReadLine();
				}
			}
			else
			{
				Console.WriteLine("Server error. The website is down or you are maybe not connected to the internet.");
				result = false;
			}
			return result;
		}

		public static void AskDownload()
		{
			Console.WriteLine("Yandere Simulator not located in the directory.");
			Console.WriteLine("Do you want to download the latest version of the game ?");
			bool correctEntry = true;
			bool ok = false;
			do
			{
				Console.WriteLine("Input Y to confirm, Input N to quit.");
				string response = Console.ReadLine();

				switch (response)
				{
					case "Y":
					case "y":
						ok = StartDownload();
						break;
					case "N":
					case "n":
						Environment.Exit(1);
						break;
					default:
						correctEntry = false;
						Console.WriteLine("Entry not recognized");
						break;
				}
			} while (!correctEntry || !ok);


		}

		struct UnityObject
		{
			public int ID;
			public int Offset;
			public int Length;
			public uint data1;
			public uint data2;

			public void PrintInfo()
			{
				Console.WriteLine($"ID : {ID} and Offset {Offset} with length of {Length}");
			}
		}
		/*	public static void GetData(BinaryReader binary)
			{
				var tableSize = binary.ReadBytes(4);
				Array.Reverse(tableSize);
				var dataLength = binary.ReadBytes(4);
				Array.Reverse(dataLength);
				var FileGen = binary.ReadBytes(4);
				Array.Reverse(FileGen);
				var dataOffset = binary.ReadBytes(4);
				Array.Reverse(dataOffset);

				var endianness = binary.ReadByte();
				var reserved = binary.ReadBytes(3);
				var unityV = binary.ReadString();
				//binary.ReadBytes(21);
				binary.BaseStream.Position = 0x24;

				int numObjects = binary.ReadInt32();
				UnityObject[] objectArray = new UnityObject[numObjects];
				for (int i = 0; i < numObjects; i++)
				{
					objectArray[i] = new UnityObject
					{
						ID = binary.ReadInt32(),
						Offset = binary.ReadInt32(),
						Length = binary.ReadInt32(),
						data1 = binary.ReadUInt32(),
						data2 = binary.ReadUInt32()
					};
					objectArray[i].PrintInfo();
				}
			}*/
		public static bool GetResourcesNames()
		{
			string filePath = Environment.CurrentDirectory + "\\YandereSimulator\\YandereSimulator_Data\\globalgamemanagers";
			bool fileExists = File.Exists(filePath);
			if (!fileExists)
			{
				Console.WriteLine("File not found");
				return false;
			}
			using (var fs = File.OpenRead(filePath))
			{
				using (var binary = new BinaryReader(fs))
				{
					int scenesCount;
					binary.BaseStream.Position = 0x148e8;
					scenesCount = binary.ReadInt32();
					Console.WriteLine(scenesCount + " resources");
					var scenesArray = new string[scenesCount];
					for (int i = 0; i < scenesCount; i++)
					{
						int size = binary.ReadInt32();
						int rest = size % 4;

						switch (rest)
						{
							case 1:
								rest += 14;
								break;
							case 2:
								rest += 12;
								break;
							case 3:
								rest += 10;
								break;

							default:
								rest += 12;
								break;
						}

						if (size < 256)
						{

							var b = binary.ReadBytes(size);
							scenesArray[i] = Encoding.UTF8.GetString(b);
							if (rest > 0)
							{
								binary.ReadBytes(rest);
							}

							Console.WriteLine(scenesArray[i]);
						}
						else
						{
							throw new Exception("The size " + size + " was too big at position " + binary.BaseStream.Position);
						}

					}
					File.WriteAllLines(Environment.CurrentDirectory + "\\Resources.txt", scenesArray);
					//Console.ReadLine();
				}

				return true;
			}
		}
		public static bool GetSceneNames()
		{
			string filePath = Environment.CurrentDirectory + "\\YandereSimulator\\YandereSimulator_Data\\globalgamemanagers";
			bool fileExists = File.Exists(filePath);
			if (!fileExists)
			{
				Console.WriteLine("File not found");
				return false;
			}
			using (var fs = File.OpenRead(filePath))
			{
				using (var binary = new BinaryReader(fs))
				{
					int scenesCount;
					binary.BaseStream.Position = 0x9af0;
					scenesCount = binary.ReadInt32();
					Console.WriteLine(scenesCount + " scenes");
					var scenesArray = new string[scenesCount];
					for (int i = 0; i < scenesCount; i++)
					{
						int size = binary.ReadInt32();
						int rest = size % 4;
						if (rest == 3)
						{
							rest -= 2;
						}
						else if (rest == 1)
						{
							rest += 2;
						}
						if (size < 256)
						{

							var b = binary.ReadBytes(size);
							scenesArray[i] = Encoding.UTF8.GetString(b);
							if (rest > 0)
							{
								binary.ReadBytes(rest);
							}

							Console.WriteLine(scenesArray[i]);
						}
						else
						{
							throw new Exception("The size " + size + " was too big at position " + binary.BaseStream.Position);
						}

					}
					File.WriteAllLines(Environment.CurrentDirectory + "\\Scenes.txt", scenesArray);
				}

				return true;
			}
		}

		// Prepares the files for the mod engine to work without manual operations.
		public static bool PrepareMod()
		{
			string YanSimDir = Directory.CreateDirectory(Environment.CurrentDirectory + "\\YandereSimulator").FullName;
			string dataDir = YanSimDir + "\\YandereSimulator_Data";
			bool rightDir = Directory.Exists(dataDir) && File.Exists(YanSimDir + "\\YandereSimulator.exe");
			if (!rightDir)
			{
				AskDownload();
			}
			var YanNext_Dir = Directory.CreateDirectory(dataDir + "\\StreamingAssets\\YANDERE_NEXT");
			Directory.CreateDirectory(YanNext_Dir.FullName + "\\" + "Mods");
			Directory.CreateDirectory(YanNext_Dir.FullName + "\\" + "Plugins");
			Directory.CreateDirectory(YanNext_Dir.FullName + "\\" + "Debug");

			if (!File.Exists(YanNext_Dir.FullName + "\\Settings.json"))
			{
				File.Create(YanNext_Dir.FullName + "\\Settings.json");
			}
			string moonsharp_dll = "\\MoonSharp.Interpreter.dll";
			if (File.Exists(Environment.CurrentDirectory + moonsharp_dll))
			{
				File.Copy(Environment.CurrentDirectory + moonsharp_dll, dataDir + "\\Managed\\" + moonsharp_dll,true);
			}
			return true;

		}
	}
}
