using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Harmony;
using Newtonsoft.Json;
using Localizer.DataStructures;
using System.Reflection;

namespace Localizer
{
    public class Localizer : Mod
    {
		public static HarmonyInstance harmony;

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
		}
		
		public override void Unload()
		{
			ClearTranslations();
			harmony.UnpatchAll();
		}

		static void ClearTranslations()
		{
			GlobalLocalizeNPC.chatButtonTranslations.Clear();
			GlobalLocalizeNPC.chatTranslations.Clear();
		}

		public override void PostSetupContent()
		{
#if DEBUG
			DoTests();
#endif
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
