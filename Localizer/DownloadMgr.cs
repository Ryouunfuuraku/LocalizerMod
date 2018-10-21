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
			DownloadInfo(Path.Combine(DataBaseUri, culture, mod));
			DownloadItems(Path.Combine(DataBaseUri, culture, mod));
			DownloadNPCs(Path.Combine(DataBaseUri, culture, mod));
			DownloadBuffs(Path.Combine(DataBaseUri, culture, mod));
			DownloadMiscs(Path.Combine(DataBaseUri, culture, mod));
		}

		void DownloadInfo(string uri)
		{
			var fileName = "Info.json";
			CommonDownloadFile(uri + fileName, CachePath + fileName);
		}

		void DownloadItems(string uri)
		{
			var fileName = "Items.json";
			CommonDownloadFile(uri + fileName, CachePath + fileName);
		}

		void DownloadNPCs(string uri)
		{
			var fileName = "NPCs.json";
			CommonDownloadFile(uri + fileName, CachePath + fileName);
		}

		void DownloadBuffs(string uri)
		{
			var fileName = "Buffs.json";
			CommonDownloadFile(uri + fileName, CachePath + fileName);
		}

		void DownloadMiscs(string uri)
		{
			var fileName = "Miscs.json";
			CommonDownloadFile(uri + fileName, CachePath + fileName);
		}

		void CommonDownloadFile(string url, string path)
		{
			// TODO: Show download progress UI
			client.DownloadFile(url, path);
		}
	}
}
