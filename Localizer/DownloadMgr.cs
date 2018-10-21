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

		public static void CheckUpdate()
		{

		}

		public static void FetchVersion()
		{

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
