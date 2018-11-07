using System;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using Harmony.ILCopying;
using Newtonsoft.Json;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Localizer.DataStructures;

namespace Localizer
{
	public class DownloadMgr
	{
		public static readonly string DataBaseUri = "https://raw.githubusercontent.com/AxeelAnder/Localizer-Database/master/";
		public static readonly string VersionUri = "https://raw.githubusercontent.com/AxeelAnder/Localizer-Database/master/version.txt";
		public static readonly string IndexUri = "https://raw.githubusercontent.com/AxeelAnder/Localizer-Database/master/index.json";
		public static readonly string CachePath = Path.Combine(Main.SavePath, "LocalizerCache/");

		public DownloadMgr()
		{
			if (!Directory.Exists(CachePath))
			{
				Directory.CreateDirectory(CachePath);
			}

			CheckUpdate();
		}

		public void Update()
		{
			DownloadIndex();
		}

		public void CheckUpdate()
		{
			var path = Path.Combine(CachePath, "version.txt");
			int localVersion = 0;
			int remoteVersion = 0;

			// Read local version
			if (File.Exists(path))
			{
				var content = File.ReadAllLines(path);
				if (content != null && content.Length > 0)
				{
					localVersion = int.Parse(content[0]);
				}
			}

			remoteVersion = FetchVersion();

			// Compare
			if(remoteVersion > localVersion)
			{
				Update();
			}
		}

		public int FetchVersion()
		{
			var path = Path.Combine(CachePath, "version.txt");
			CommonDownloadFile(VersionUri, path, new WebClient());
			if (File.Exists(path))
			{
				var content = File.ReadAllLines(path);
				if (content != null && content.Length > 0)
				{
					return int.Parse(content[0]);
				}
			}

			return 0;
		}

		public void DownloadIndex()
		{
			var path = Path.Combine(CachePath, "index.json");

			CommonDownloadFileAsync(IndexUri, path, new WebClient());

			// TODO: Refresh manager
		}

		public void DownloadModText(string culture, string mod, WebClient client)
		{
			var uri = DataBaseUri + culture + "/" + mod + "/";
			var path = CachePath + culture + "/" + mod + "/";
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			DownloadInfo(uri, path, client);
			DownloadItems(uri, path, client);
			DownloadNPCs(uri, path, client);
			DownloadBuffs(uri, path, client);
			DownloadMiscs(uri, path, client);
		}

		void DownloadInfo(string uri, string path, WebClient client)
		{
			var fileName = "Info.json";
			CommonDownloadFileAsync(uri + fileName, path + fileName, client);
		}
		
		void DownloadItems(string uri, string path, WebClient client)
		{
			var fileName = "Items.json";
			CommonDownloadFileAsync(uri + fileName, path + fileName, client);
		}

		void DownloadNPCs(string uri, string path, WebClient client)
		{
			var fileName = "NPCs.json";
			CommonDownloadFileAsync(uri + fileName, path + fileName, client);
		}

		void DownloadBuffs(string uri, string path, WebClient client)
		{
			var fileName = "Buffs.json";
			CommonDownloadFileAsync(uri + fileName, path + fileName, client);
		}

		void DownloadMiscs(string uri, string path, WebClient client)
		{
			var fileName = "Miscs.json";
			CommonDownloadFileAsync(uri + fileName, path + fileName, client);
		}

		void CommonDownloadFileAsync(string url, string path, WebClient client)
		{
			client.DownloadFileAsync(new Uri(url), path);
		}
		
		void CommonDownloadFile(string url, string path, WebClient client)
		{
			client.DownloadFile(url, path);
		}
	}
}
