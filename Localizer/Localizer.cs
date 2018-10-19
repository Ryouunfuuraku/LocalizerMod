using System;
using System.Collections.Generic;
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

namespace Localizer
{
    public class Localizer : Mod
    {
		public static HarmonyInstance harmony;

		public static UIManager manager;

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
			harmony = HarmonyInstance.Create("Localizer.Main");
			harmony.PatchAll(Assembly.GetExecutingAssembly());
			Patches.DoManualPatches();

			manager = new UIManager();

			AddTranslation();
		}

		public void AddTranslation()
		{
			ModTranslation modTranslation = CreateTranslation("ManagerHeadTitle");
			modTranslation.SetDefault("Localize Manager");
			modTranslation.AddTranslation(GameCulture.Chinese, "汉化管理");
			AddTranslation(modTranslation);

			modTranslation = CreateTranslation("ManagerButton");
			modTranslation.SetDefault("Localize Manager");
			modTranslation.AddTranslation(GameCulture.Chinese, "汉化管理");
			AddTranslation(modTranslation);

			modTranslation = CreateTranslation("BackButton");
			modTranslation.SetDefault("Back");
			modTranslation.AddTranslation(GameCulture.Chinese, "返回");
			AddTranslation(modTranslation);

			modTranslation = CreateTranslation("ExportButton");
			modTranslation.SetDefault("Export");
			modTranslation.AddTranslation(GameCulture.Chinese, "导出文本");
			AddTranslation(modTranslation);

			modTranslation = CreateTranslation("ImportButton");
			modTranslation.SetDefault("Import");
			modTranslation.AddTranslation(GameCulture.Chinese, "导入文本");
			AddTranslation(modTranslation);
		}
		
		public override void Unload()
		{
			ClearTranslations();
			harmony.UnpatchAll();
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
			//DoTests();
#endif
		}

		public static void TurnToManager()
		{
			Main.MenuUI.SetState(manager);
			Main.menuMode = 888;
		}

		public static void DoTests()
		{
			Test.TestAddItemTranslation();
			Test.TestAddNPCTranslation();
			Test.TestAddBuffTranslation();
			Test.TestAddTileTranslation();
			Test.TestAddChatTranslation();
			Test.TestAddChatButtonTranslation();
			Test.TestExportText();
			Test.TestImportText();
		}


	}
}
