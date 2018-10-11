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
	public class Test
	{
		public static void TestAddItemTranslation()
		{
			try
			{
				var type = ModLoader.GetMod("Bluemagic").ItemType("PuriumBreastplate");
				if (type > 0)
				{
					Localizer.AddItemNameTranslation(ItemLoader.GetItem(type), "测试一下", GameCulture.Chinese);

					Localizer.AddItemTooltipTranslation(ItemLoader.GetItem(type), "测试两下", GameCulture.Chinese);

					Localizer.AddSetBounsTranslation("Increases purity shield capacity by 1200", "测试三下");
				}
			}
			catch (Exception ex)
			{
				ErrorLogger.Log(ex.ToString());
			}
		}

		public static void TestAddNPCTranslation()
		{
			try
			{
				var type = ModLoader.GetMod("Bluemagic").NPCType("Phantom");
				if (type > 0)
				{
					Localizer.AddNpcNameTranslation(NPCLoader.GetNPC(type), "测试四下", GameCulture.Chinese);
				}
			}
			catch (Exception ex)
			{
				ErrorLogger.Log(ex.ToString());
			}
		}
	}
}
