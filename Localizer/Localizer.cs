using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Localizer
{
    public class Localizer : Mod
    {
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

		}

		public override void PostSetupContent()
		{
			Test.TestAddItemTranslation();
			Test.TestAddNPCTranslation();
		}

		#region Item Translation Methods
		public static void AddItemNameTranslation(ModItem item, string itemNameTranslation, GameCulture culture)
		{
			item.DisplayName.AddTranslation(culture, itemNameTranslation);
		}

		public static void AddItemTooltipTranslation(ModItem item, string tooltipTranslation, GameCulture culture)
		{
			item.Tooltip.AddTranslation(culture, tooltipTranslation);
		}
		
		public static void AddSetBounsTranslation(string vanilla, string translation)
		{
			LocalizePlayer.setBounsTranslations.Add(vanilla, translation);
		}
		#endregion

		#region NPC Translation Methods
		public static void AddNpcNameTranslation(ModNPC npc, string npcNameTranslation, GameCulture culture)
		{
			npc.DisplayName.AddTranslation(culture, npcNameTranslation);
		}
		#endregion
	}
}
