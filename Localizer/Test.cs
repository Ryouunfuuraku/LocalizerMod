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
				var type = ModLoader.GetMod("Luiafk").ItemType("FasterMining");
				if (type > 0)
				{
					Localizer.AddItemTranslation(ItemLoader.GetItem(type), "测试一下", GameCulture.Chinese);
				}
			}
			catch (Exception ex)
			{
				ErrorLogger.Log(ex.ToString());
			}
		}
	}
}
