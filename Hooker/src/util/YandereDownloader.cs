using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Hooker.src.util
{
	class YandereDownloader
	{
		// Public members
		public bool isDownloading { get => _downloading; }
		public int Progress { get => _progress; }
		public const string BaseURL = "https://yanderesimulator.com";
		public const string GameURL = "http://dl.yanderesimulator.com/latest.zip";
		public readonly string ModAPI;

		public YandereDownloader()
		{
			ModAPI = File.ReadLines(Environment.CurrentDirectory + "\\mods.txt").First();
		}

		public bool DownloadURLS()
		{
			return Download(BaseURL + "/urls.txt");
		}

		public bool DownloadGame()
		{
			return Download(GameURL);
		}

		public bool DownloadAPI()
		{
			return Download(ModAPI);
		}

		public bool ExtractFile(Stream filestream, string output)
		{
			string directoryStucture = Path.GetDirectoryName(output);
			Directory.CreateDirectory(directoryStucture);
			if (output.EndsWith("/"))
			{
				return true;
			}
			try
			{
				using (FileStream outputStream = File.Create(output))
				{
					int data;
					while ((data = filestream.ReadByte()) > -1)
					{
						outputStream.WriteByte((byte)data);
					}
				};
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine("An error occurred while extracting this file from the game archive : " + output);
				Console.WriteLine("Try to redownload the game archive by deleting the YandereSimulator directory and latest.zip");
				Console.WriteLine(ex.GetType().ToString() + " : " + ex.Message);
				return false;
			}
		}

		public bool ExtractGame()
		{
			string outPath =  Environment.CurrentDirectory + "\\YandereSimulator\\";
			string gamePath = Environment.CurrentDirectory + "\\" + Path.GetFileName(GameURL);
			if (!Directory.Exists(outPath) || !File.Exists(outPath + "\\YandereSimulator.exe"))
				return Extract(gamePath, outPath);
			else
				Console.WriteLine("Game already extracted");
				return true;
		}

		public bool ExtractAPI()
		{
			Console.WriteLine("EXTRACTING API");
			string outputPath = Environment.CurrentDirectory + "\\YandereSimulator\\YandereSimulator_Data\\StreamingAssets\\Yandere_Next";
			if (!Directory.Exists(outputPath))
			{
				DirectoryInfo dir = Directory.CreateDirectory(Environment.CurrentDirectory + "\\temp");

				try
				{
					var temp = Extract(Environment.CurrentDirectory + "\\" + Path.GetFileName(ModAPI), dir.FullName);
					var directories = dir.GetDirectories();
					foreach (var d in directories)
					{
						if (d.Name.Contains("YANDERE-NEXT"))
						{
							Directory.Move(d.FullName, outputPath);
						}
					}
					dir.Delete();
					File.Delete(Environment.CurrentDirectory + "\\" + Path.GetFileName(ModAPI));

					return true;
				}
				catch
				{
					return false;
				}
			}
			else
			{
				Console.WriteLine("Mod api already extracted");
				return true;
			}
		}

		public bool Extract(string filePath, string outputDir)
		{
			Console.WriteLine("Starting extraction");
			try
			{
				using (var archive = new ZipArchive(File.OpenRead(Path.GetFileName(filePath))))
				{
					int i = 0;
					foreach (ZipArchiveEntry file in archive.Entries)
					{
						++i;
						using (Stream stream = file.Open())
						{
							string filename = file.FullName;
							Console.WriteLine("Extracting " + filename);
							string output = outputDir + "\\" + filename;
							bool result = ExtractFile(stream, output);
							if (!result)
							{
								throw new Exception("File extraction failure : " + filename);
							}
						}

					}
				};

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine("An error occurred while opening the archive, it's probably corrupted");
				Console.WriteLine("Try to redownload the archive by deleting it");
				Console.WriteLine(ex.GetType().ToString() + " : " + ex.Message);
				return false;
			}

		}
		public bool Download(string url)
		{
			var result = true;
			var urls = new Uri(url);
			string filename = Path.GetFileName(url);
			if (!File.Exists(Environment.CurrentDirectory + "\\" + filename))
			{
				using (var _client = new WebClient())
				{

					_client.DownloadProgressChanged += (s, e) =>
					{

						Console.WriteLine($"{(float)e.BytesReceived / 1024 / 1024} Mb downloaded.");
					};
					_client.DownloadFileCompleted += (s, e) =>
					{
						if (e.Error == null)
						{
							result = false;
						}
					};


					_client.DownloadFileAsync(urls, Environment.CurrentDirectory + $"\\{filename}");

					while (_client.IsBusy)
					{

					}
				}
			}
			else
			{
				Console.WriteLine("Skipping... File already exists");
			}
			return result;
		}

		public static bool OnlineCheck(string url)
		{
			bool retVal = false;
			try
			{
				Ping pingSender = new Ping();
				PingOptions options = new PingOptions();
				// Use the default Ttl value which is 128,
				// but change the fragmentation behavior.
				options.DontFragment = true;
				// Create a buffer of 32 bytes of data to be transmitted.
				string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
				byte[] buffer = Encoding.ASCII.GetBytes(data);
				int timeout = 120;

				PingReply reply = pingSender.Send(url, timeout, buffer, options);
				if (reply.Status == IPStatus.Success)
				{
					retVal = true;
				}
			}
			catch (Exception ex)
			{
				retVal = false;
				Console.WriteLine(ex.Message);
			}
			return retVal;
		}

		// Private members
		private bool _downloading = false;
		private int _progress = 0;
	}
}
