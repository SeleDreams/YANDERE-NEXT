using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace Hooker.src.util
{
	class YandereDownloader
	{
		// Public members
		public bool isDownloading { get => _downloading; }
		public int Progress { get => _progress; }
		public const string BaseURL = "https://yanderesimulator.com";
		private string GameURL = "http://dl.yanderesimulator.com/latest.zip";

		public YandereDownloader()
		{
		}

		public bool DownloadURLS()
		{
			return Download(BaseURL + "/urls.txt");
		}

		public bool DownloadGame()
		{
			return Download(GameURL);
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
			Console.WriteLine("Starting game extraction");
			try
			{
				using (var archive = new ZipArchive(File.OpenRead(Path.GetFileName(GameURL))))
				{
					int i = 0;
					foreach (ZipArchiveEntry file in archive.Entries)
					{
						++i;
						using (Stream stream = file.Open())
						{
							string filename = file.FullName;
							Console.WriteLine("Extracting " + filename);
							string output = Environment.CurrentDirectory + "\\YandereSimulator\\" + filename;
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
				Console.WriteLine("An error occurred while opening the game archive, it's probably corrupted");
				Console.WriteLine("Try to redownload the game archive by deleting the YandereSimulator directory and latest.zip");
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
