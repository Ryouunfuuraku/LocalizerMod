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
		public static GameCulture Culture;

		public static string DataBaseUri
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_databaseUri))
				{
					return "https://raw.githubusercontent.com/AxeelAnder/Localizer-Database/zh-Hans/";
				}
				else
				{
					return _databaseUri;
				}
			}
		}
		public static string VersionUri;
		public static string IndexUri;
		public static string CachePath = Path.Combine(Main.SavePath, "LocalizerCache/");

		public List<DownloadItem> Downloadings;

		private static string _databaseUri;
		private bool end = false;
		private Thread _thread;
		private Queue<DownloadItem> _downloadQueue;

		public DownloadMgr()
		{
			_downloadQueue = new Queue<DownloadItem>();
			Downloadings = new List<DownloadItem>();

			if (!Directory.Exists(CachePath))
			{
				Directory.CreateDirectory(CachePath);
			}

			SetCulture(LanguageManager.Instance.ActiveCulture);
			LanguageManager.Instance.OnLanguageChanged += OnLanguageChanged;

			StartDownloadThread();

			CheckUpdate();
		}

		public void Destroy()
		{
			end = true;
			if (_thread != null &&_thread.IsAlive)
			{
				_thread.Abort();
			}
		}

		public void OnLanguageChanged(LanguageManager languageManager)
		{
			SetCulture(languageManager.ActiveCulture);
		}

		public void SetCulture(GameCulture culture)
		{
			Culture = culture;
			_databaseUri = string.Format("https://raw.githubusercontent.com/AxeelAnder/Localizer-Database/{0}/",
				culture.Name);
			VersionUri = _databaseUri + "version.txt";
			IndexUri = _databaseUri + "index.json";

			var path = Path.Combine(CachePath, Culture.Name);
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
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

					Downloadings.Add(item);
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

			Logger.DebugLog(string.Format("local version:{0} remote version:{1}", localVersion, remoteVersion));

			// Compare
			if(remoteVersion > localVersion)
			{
				UpdateDatabase();
			}
		}

		public int FetchVersion()
		{
			var path = Path.Combine(CachePath, Culture.Name, "version.txt");
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
			var path = Path.Combine(CachePath, Culture.Name, "index.json");

			CommonDownloadFileAsync(IndexUri, "Index", path);

			// TODO: Refresh manager
		}

		public void DownloadModText(string mod)
		{
			var path = CreatePathForText(mod);
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			
			DownloadModTextInfo(mod);
			DownloadModItemText(mod);
			DownloadModNPCsText(mod);
			DownloadModBuffsText(mod);
			DownloadModMiscsText(mod);
		}

		public string GetCacheFilePath(string filename)
		{
			return Path.Combine(CachePath, Culture.Name, filename);
		}

		private string CreateUriForText(string mod)
		{
			return DataBaseUri + mod + "/";
		}

		private string CreatePathForText(string mod)
		{
			return CachePath + Culture.Name + "/" + mod + "/";
		}

		public void DownloadModTextInfo(string mod)
		{
			var uri = CreateUriForText(mod) + "Info.json";
			var path = CreatePathForText(mod) + "Info.json";

			CommonDownloadFileAsync(uri, string.Format("{0}'s Info", mod), path);
		}

		public void DownloadModItemText(string mod)
		{
			var uri = CreateUriForText(mod) + "Items.json";
			var path = CreatePathForText(mod) + "Items.json";

			CommonDownloadFileAsync(uri, string.Format("{0}'s Item", mod), path);
		}

		public void DownloadModNPCsText(string mod)
		{
			var uri = CreateUriForText(mod) + "NPCs.json";
			var path = CreatePathForText(mod) + "NPCs.json";

			CommonDownloadFileAsync(uri, string.Format("{0}'s npc", mod), path);
		}

		public void DownloadModBuffsText(string mod)
		{
			var uri = CreateUriForText(mod) + "Buffs.json";
			var path = CreatePathForText(mod) + "Buffs.json";

			CommonDownloadFileAsync(uri, string.Format("{0}'s buff", mod), path);
		}

		public void DownloadModMiscsText(string mod)
		{
			var uri = CreateUriForText(mod) + "Miscs.json";
			var path = CreatePathForText(mod) + "Miscs.json";

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

		public void CommonDownloadFile(string uri, string path)
		{
			var client = new WebClient();
			client.DownloadFile(uri, path);
		}

		internal void DestroyItem(DownloadItem item)
		{
			Downloadings.Remove(item);
		}

		public sealed class DownloadItem
		{
			public string Uri;

			public string Name;

			public string SavePath;

			public float Progress;

			public WebClient Client;

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

				Client = new WebClient();
			}

			internal void Start()
			{
				if(Client == null)
					Client = new WebClient();

				Client.DownloadProgressChanged += (s, e) => SetProgress(e);
				Client.DownloadFileCompleted += (s, e) => OnComplete();
				Client.DownloadFileAsync(new Uri(this.Uri), this.SavePath);
			}

			private void SetProgress(DownloadProgressChangedEventArgs e)
			{
				this.Progress = (float) e.BytesReceived / e.TotalBytesToReceive;
			}

			private void OnComplete()
			{
				Logger.DebugLog(string.Format("{0} downloaded.", SavePath));

				lock (Localizer.downloadMgr.Downloadings)
				{
					Localizer.downloadMgr.DestroyItem(this);
				}

				var textDir = new FileInfo(SavePath).Directory;
				if (ImportTool.CheckDir(textDir.FullName))
				{
					Localizer.ApplyTextFile(textDir);
				}
			}
		}
	}
}
