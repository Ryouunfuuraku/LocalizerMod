using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Harmony;
using Newtonsoft.Json;
using Localizer.DataStructures;
using Localizer.UI;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using System.Net;

namespace Localizer
{
    public class Localizer : Mod
	{
		public static HarmonyInstance harmony;
		
		public static DownloadMgr downloadMgr;

		public static Index LoadedIndex;

		public Localizer()
		{
			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true,
			};
		}

		public override void Load()
		{
			ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

			harmony = HarmonyInstance.Create("Localizer.Main");
			harmony.PatchAll(Assembly.GetExecutingAssembly());
			Patches.DoManualPatches();

			LoadedIndex = new Index();
			
			downloadMgr = new DownloadMgr();
			Interface.Init();
			
			AddTranslation();

		}

		public void AddTranslation()
		{
			ModTranslation modTranslation = CreateTranslation("ManagerHeadTitle");
			modTranslation.SetDefault("Localize Manager");
			modTranslation.AddTranslation(GameCulture.Chinese, "汉化管理器");
			AddTranslation(modTranslation);

			modTranslation = CreateTranslation("BrowserHeadTitle");
			modTranslation.SetDefault("Browser");
			modTranslation.AddTranslation(GameCulture.Chinese, "汉化浏览器");
			AddTranslation(modTranslation);
			
			modTranslation = CreateTranslation("DownloadHeadTitle");
			modTranslation.SetDefault("Download");
			modTranslation.AddTranslation(GameCulture.Chinese, "下载管理");
			AddTranslation(modTranslation);

			modTranslation = CreateTranslation("MenuButton");
			modTranslation.SetDefault("Localizer");
			modTranslation.AddTranslation(GameCulture.Chinese, "汉化");
			AddTranslation(modTranslation);
			
			modTranslation = CreateTranslation("MenuBrowserButton");
			modTranslation.SetDefault("Browser");
			modTranslation.AddTranslation(GameCulture.Chinese, "汉化浏览器");
			AddTranslation(modTranslation);
			
			modTranslation = CreateTranslation("MenuManagerButton");
			modTranslation.SetDefault("Manager");
			modTranslation.AddTranslation(GameCulture.Chinese, "汉化管理器");
			AddTranslation(modTranslation);

			modTranslation = CreateTranslation("MenuDownloadButton");
			modTranslation.SetDefault("Download");
			modTranslation.AddTranslation(GameCulture.Chinese, "下载管理");
			AddTranslation(modTranslation);

			modTranslation = CreateTranslation("MenuSettingButton");
			modTranslation.SetDefault("Settings");
			modTranslation.AddTranslation(GameCulture.Chinese, "汉化设置");
			AddTranslation(modTranslation);

			modTranslation = CreateTranslation("BackButton");
			modTranslation.SetDefault("Back");
			modTranslation.AddTranslation(GameCulture.Chinese, "返回");
			AddTranslation(modTranslation);

			modTranslation = CreateTranslation("DownloadButton");
			modTranslation.SetDefault("Download");
			modTranslation.AddTranslation(GameCulture.Chinese, "下载文本");
			AddTranslation(modTranslation);

			modTranslation = CreateTranslation("UpdateTextButton");
			modTranslation.SetDefault("Update");
			modTranslation.AddTranslation(GameCulture.Chinese, "更新文本");
			AddTranslation(modTranslation);

			modTranslation = CreateTranslation("CheckUpdateButton");
			modTranslation.SetDefault("Check Update");
			modTranslation.AddTranslation(GameCulture.Chinese, "检查更新");
			AddTranslation(modTranslation);

			modTranslation = CreateTranslation("ExportButton");
			modTranslation.SetDefault("Export");
			modTranslation.AddTranslation(GameCulture.Chinese, "导出文本");
			AddTranslation(modTranslation);

			modTranslation = CreateTranslation("ImportButton");
			modTranslation.SetDefault("Import");
			modTranslation.AddTranslation(GameCulture.Chinese, "导入文本");
			AddTranslation(modTranslation);
			
			modTranslation = CreateTranslation("MachineTranslateButton");
			modTranslation.SetDefault("MachineTrans");
			modTranslation.AddTranslation(GameCulture.Chinese, "机翻");
			AddTranslation(modTranslation);

			modTranslation = CreateTranslation("Downloading");
			modTranslation.SetDefault("Downloading: ");
			modTranslation.AddTranslation(GameCulture.Chinese, "下载中: ");
			AddTranslation(modTranslation);

			modTranslation = CreateTranslation("Author");
			modTranslation.SetDefault("Author: ");
			modTranslation.AddTranslation(GameCulture.Chinese, "翻译者: ");
			AddTranslation(modTranslation);

			modTranslation = CreateTranslation("ReloadText");
			modTranslation.SetDefault("Reload texts");
			modTranslation.AddTranslation(GameCulture.Chinese, "重新加载文本");
			AddTranslation(modTranslation);
		}
		
		public override void Unload()
		{
			ClearTranslations();
			harmony.UnpatchAll();
		}

		public static void ApplyTextFile(DirectoryInfo textDir)
		{
			try
			{
				Logger.DebugLog(string.Format("Apply {0}", textDir.Name));
				var mod = ModLoader.GetMod(textDir.Name);
				if (mod != null)
				{
					ImportTool.ImportModTexts(mod, textDir.FullName);
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex);
			}
		}

		public static void LoadTextFiles()
		{
			var cacheDir = new DirectoryInfo(DownloadMgr.CachePath);
			foreach (var langDir in cacheDir.EnumerateDirectories())
			{
				if (langDir.Name == LanguageManager.Instance.ActiveCulture.Name)
				{
					foreach (var textDir in langDir.EnumerateDirectories())
					{
						ApplyTextFile(textDir);
					}
				}
			}
		}

		static void ClearTranslations()
		{
			DefaultTranslation.chatButtonTranslations.Clear();
			DefaultTranslation.setBonusTranslations.Clear();
			GlobalLocalizeNPC.chatTranslations.Clear();
		}

		public override void PostSetupContent()
		{
#if DEBUG
			DoTests();
#endif
			LoadTextFiles();
		}

		public static void DoTests()
		{
			//Test.TestAddItemTranslation();
			//Test.TestAddNPCTranslation();
			//Test.TestAddBuffTranslation();
			//Test.TestAddTileTranslation();
			//Test.TestAddChatTranslation();
			//Test.TestAddChatButtonTranslation();
			//Test.TestPullRemoteTexts();
		}


	}
}
