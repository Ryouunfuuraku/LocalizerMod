using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Harmony;
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

		private bool end = false;
		private Thread _thread;
		private Queue<DownloadItem> _downloadQueue;
		private List<DownloadItem> _downloadings;

		public DownloadMgr()
		{
			_downloadQueue = new Queue<DownloadItem>();
			_downloadings = new List<DownloadItem>();

			if (!Directory.Exists(CachePath))
			{
				Directory.CreateDirectory(CachePath);
			}

			StartDownloadThread();

			CheckUpdate();
		}

		private void StartDownloadThread()
		{ 
			if(_thread == null)
				_thread = new Thread(DownloadUpdate);

			_thread.Start();
		}

		private void DownloadUpdate()
		{
			while (!end)
			{
				while (_downloadQueue.Count > 0)
				{
					var item = _downloadQueue.Dequeue();

					item.Start();

					_downloadings.Add(item);
				}

				Thread.Sleep(10);
			}
		}

		public void UpdateDatabase()
		{
			DownloadIndex();
		}

		public void UpdateMod()
		{

		}

		public void CheckUpdate()
		{
			CheckModUpdate();
			CheckDatabaseUpdate();
		}

		public void CheckModUpdate()
		{

		}

		public void CheckDatabaseUpdate()
		{
			var path = Path.Combine(CachePath, "version.txt");
			int localVersion = 0;
			int remoteVersion = 0;

			// Read local version
			if (File.Exists(path))
			{
				var content = File.ReadAllLines(path);
				if (content.Length != 0 && content.Length > 0)
				{
					localVersion = int.Parse(content[0]);
				}
			}

			remoteVersion = FetchVersion();

			// Compare
			if(remoteVersion > localVersion)
			{
				UpdateDatabase();
			}
		}

		public int FetchVersion()
		{
			var path = Path.Combine(CachePath, "version.txt");
			CommonDownloadFileAsync(VersionUri, "Version", path);
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

			CommonDownloadFileAsync(IndexUri, "Index", path);

			// TODO: Refresh manager
		}

		public void DownloadModText(string culture, string mod, WebClient client)
		{
			var path = CachePath + culture + "/" + mod + "/";
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			
			DownloadModItemText(culture, mod);
			DownloadModNPCsText(culture, mod);
			DownloadModBuffsText(culture, mod);
			DownloadModMiscsText(culture, mod);
		}

		private string CreateUriForText(string culture, string mod)
		{
			return DataBaseUri + culture + "/" + mod + "/";
		}

		private string CreatePathForText(string culture, string mod)
		{
			return CachePath + culture + "/" + mod + "/";
		}

		public void DownloadModItemText(string culture, string mod)
		{
			var uri = CreateUriForText(culture, mod) + "Items.json";
			var path = CreatePathForText(culture, mod) + "Items.json";

			CommonDownloadFileAsync(uri, string.Format("{0}'s item", mod), path);
		}

		public void DownloadModNPCsText(string culture, string mod)
		{
			var uri = CreateUriForText(culture, mod) + "NPCs.json";
			var path = CreatePathForText(culture, mod) + "NPCs.json";

			CommonDownloadFileAsync(uri, string.Format("{0}'s npc", mod), path);
		}

		public void DownloadModBuffsText(string culture, string mod)
		{
			var uri = CreateUriForText(culture, mod) + "Buffs.json";
			var path = CreatePathForText(culture, mod) + "Buffs.json";

			CommonDownloadFileAsync(uri, string.Format("{0}'s buff", mod), path);
		}

		public void DownloadModMiscsText(string culture, string mod)
		{
			var uri = CreateUriForText(culture, mod) + "Miscs.json";
			var path = CreatePathForText(culture, mod) + "Miscs.json";

			CommonDownloadFileAsync(uri, string.Format("{0}'s misc", mod), path);
		}

		public void CommonDownloadFileAsync(string uri, string name, string path)
		{
			CommonDownloadFileAsync(new DownloadItem(uri, name, path));
		}

		public void CommonDownloadFileAsync(DownloadItem item)
		{
			_downloadQueue.Enqueue(item);
		}

		internal void DestroyItem(DownloadItem item)
		{
			_downloadings.Remove(item);
		}

		public sealed class DownloadItem
		{
			public string Uri;

			public string Name;

			public string SavePath;

			public float Progress;

			internal WebClient _client;

			public DownloadItem(string uri, string name, string savePath = "")
			{
				this.Uri = uri;
				this.Name = name;
				this.Progress = 0f;

				if (string.IsNullOrEmpty(savePath))
				{
					this.SavePath = Path.Combine(CachePath, Name);
				}
				this.SavePath = savePath;

				var dir = Path.GetDirectoryName(SavePath);
				if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
				{
					Directory.CreateDirectory(dir);
				}

				_client = new WebClient();
			}

			internal void Start()
			{
				if(_client == null)
					_client = new WebClient();

				_client.DownloadProgressChanged += (s, e) => SetProgress(e);
				_client.DownloadFileCompleted += (s, e) => OnComplete();
				_client.DownloadFileAsync(new Uri(this.Uri), this.SavePath);
			}

			private void SetProgress(DownloadProgressChangedEventArgs e)
			{
				this.Progress = (float) e.BytesReceived / e.TotalBytesToReceive;
			}

			private void OnComplete()
			{
				Localizer.downloadMgr.DestroyItem(this);
			}
		}
	}
}
