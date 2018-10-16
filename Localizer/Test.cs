using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Localizer.DataStructures;

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
					TranslateTool.AddItemNameTranslation(ItemLoader.GetItem(type), "测试item名字", GameCulture.Chinese);

					TranslateTool.AddItemTooltipTranslation(ItemLoader.GetItem(type), "测试tooltip", GameCulture.Chinese);

					TranslateTool.AddSetBonusTranslation(ItemLoader.GetItem(type), "测试setbonus", GameCulture.Chinese);
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
					TranslateTool.AddNpcNameTranslation(NPCLoader.GetNPC(type), "测试npc名字", GameCulture.Chinese);
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
					TranslateTool.AddBuffNameTranslation(BuffLoader.GetBuff(type), "测试buff名字", GameCulture.Chinese);
					TranslateTool.AddBuffTipTranslation(BuffLoader.GetBuff(type), "测试buff说明", GameCulture.Chinese);
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
					TranslateTool.AddTileNameTranslation(TileLoader.GetTile(type), "测试tile名字", GameCulture.Chinese);
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
					TranslateTool.AddChatTranslation("Sometimes I feel like I'm different from everyone else here.", "测试npc对话1");
					TranslateTool.AddChatTranslation("What's your favorite color? My favorite colors are white and black.", "测试npc对话2");
					TranslateTool.AddChatTranslation("What? I don't have any arms or legs? Oh, don't be ridiculous!", "测试npc对话3");
				}
			}
			catch (Exception ex)
			{
				ErrorLogger.Log(ex.ToString());
			}
		}

		public static void TestAddChatButtonTranslation()
		{
			try
			{
				var type = ModLoader.GetMod("Bluemagic").NPCType("Hardmode Guide");
				if (type > 0)
				{
					TranslateTool.AddChatButtonTranslation(type, "测试npc对话按扭1", "测试npc对话按扭2", GameCulture.Chinese);
				}
			}
			catch (Exception ex)
			{
				ErrorLogger.Log(ex.ToString());
			}
		}

		public static void TestExportText()
		{
			var path = Path.Combine(Main.SavePath, "TestText/");

			var mod = ModLoader.GetMod("Bluemagic");

			ExportTool.ExportItemTexts(mod, path);
			ExportTool.ExportNPCTexts(mod, path);
			ExportTool.ExportBuffTexts(mod, path);
			ExportTool.ExportMiscTexts(mod, path);
		}

		public static void TestImportText()
		{
			var path = Path.Combine(Main.SavePath, "TestImportText/");

			var mod = ModLoader.GetMod("Bluemagic");

			ImportTool.ImportItemTexts(mod, path);
			ImportTool.ImportNPCTexts(mod, path);
			ImportTool.ImportBuffTexts(mod, path);
			ImportTool.ImportMiscTexts(mod, path);
		}
	}
}
