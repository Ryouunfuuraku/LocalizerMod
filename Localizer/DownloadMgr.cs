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

		WebClient client;

		public DownloadMgr()
		{
			if (!Directory.Exists(CachePath))
			{
				Directory.CreateDirectory(CachePath);
			}

			client = new WebClient();

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
			CommonDownloadFile(VersionUri, path);
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

			CommonDownloadFile(IndexUri, path);

			// TODO: Refresh manager
		}

		public void DownloadModText(string culture, string mod)
		{
			var uri = DataBaseUri + culture + "/" + mod + "/";
			var path = CachePath + culture + "/" + mod + "/";
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			DownloadInfo(uri, path);
			DownloadItems(uri, path);
			DownloadNPCs(uri, path);
			DownloadBuffs(uri, path);
			DownloadMiscs(uri, path);
		}

		void DownloadInfo(string uri, string path)
		{
			var fileName = "Info.json";
			CommonDownloadFile(uri + fileName, path + fileName);
		}
		
		void DownloadItems(string uri, string path)
		{
			var fileName = "Items.json";
			CommonDownloadFile(uri + fileName, path + fileName);
		}

		void DownloadNPCs(string uri, string path)
		{
			var fileName = "NPCs.json";
			CommonDownloadFile(uri + fileName, path + fileName);
		}

		void DownloadBuffs(string uri, string path)
		{
			var fileName = "Buffs.json";
			CommonDownloadFile(uri + fileName, path + fileName);
		}

		void DownloadMiscs(string uri, string path)
		{
			var fileName = "Miscs.json";
			CommonDownloadFile(uri + fileName, path + fileName);
		}

		void CommonDownloadFile(string url, string path)
		{
			// TODO: Show download progress UI
			client.DownloadFile(url, path);
		}
	}
}
