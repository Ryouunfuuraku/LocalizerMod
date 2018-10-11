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
		public override void Load()
		{

		}

		public override void PostSetupContent()
		{
			Test.TestAddItemTranslation();
		}

		public static void AddItemNameTranslation(ModItem item, string itemNameTranslation, GameCulture culture)
		{
			item.DisplayName.AddTranslation(culture, itemNameTranslation);
		}

		public static void AddItemTooltipTranslation(ModItem item, string tooltipTranslation, GameCulture culture)
		{
			item.Tooltip.AddTranslation(culture, tooltipTranslation);
		}
	}
}
