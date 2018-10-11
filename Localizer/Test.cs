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
					Localizer.AddItemNameTranslation(ItemLoader.GetItem(type), "测试item名字", GameCulture.Chinese);

					Localizer.AddItemTooltipTranslation(ItemLoader.GetItem(type), "测试tooltip", GameCulture.Chinese);

					Localizer.AddSetBounsTranslation("Increases purity shield capacity by 1200", "测试setbonus");
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
					Localizer.AddNpcNameTranslation(NPCLoader.GetNPC(type), "测试npc名字", GameCulture.Chinese);
				}
			}
			catch (Exception ex)
			{
				ErrorLogger.Log(ex.ToString());
			}
		}

		public static void TestAddBuffTranslation()
		{
			try
			{
				var type = ModLoader.GetMod("Bluemagic").BuffType("Sunlight");
				if (type > 0)
				{
					Localizer.AddBuffNameTranslation(BuffLoader.GetBuff(type), "测试buff名字", GameCulture.Chinese);
					Localizer.AddBuffTipTranslation(BuffLoader.GetBuff(type), "测试buff说明", GameCulture.Chinese);
				}
			}
			catch (Exception ex)
			{
				ErrorLogger.Log(ex.ToString());
			}
		}
		
		public static void TestAddTileTranslation()
		{
			try
			{
				var type = ModLoader.GetMod("Bluemagic").TileType("ElementalBar");
				if (type > 0)
				{
					Localizer.AddTileNameTranslation(TileLoader.GetTile(type), "测试tile名字", GameCulture.Chinese);
				}
			}
			catch (Exception ex)
			{
				ErrorLogger.Log(ex.ToString());
			}
		}

		public static void TestAddChatTranslation()
		{
			try
			{
				var type = ModLoader.GetMod("Bluemagic").NPCType("Hardmode Guide");
				if (type > 0)
				{
					Localizer.AddChatTranslation("Sometimes I feel like I'm different from everyone else here.", "测试npc对话1");
					Localizer.AddChatTranslation("What's your favorite color? My favorite colors are white and black.", "测试npc对话2");
					Localizer.AddChatTranslation("What? I don't have any arms or legs? Oh, don't be ridiculous!", "测试npc对话3");
				}
			}
			catch (Exception ex)
			{
				ErrorLogger.Log(ex.ToString());
			}
		}
	}
}
