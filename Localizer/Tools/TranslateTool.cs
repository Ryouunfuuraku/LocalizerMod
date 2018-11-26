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
	public class TranslateTool
	{
		#region Item Translation Methods
		public static void AddItemNameTranslation(ModItem item, string itemNameTranslation, GameCulture culture)
		{
			item.DisplayName.AddTranslation(culture, itemNameTranslation);
		}

		public static void AddItemTooltipTranslation(ModItem item, string tooltipTranslation, GameCulture culture)
		{
			item.Tooltip.AddTranslation(culture, tooltipTranslation);
		}

		public static void AddSetBonusTranslation(ModItem item, string translation, GameCulture culture)
		{
			var type = item.mod.ItemType(item.Name);
			if (!DefaultTranslation.setBonusTranslations.ContainsKey(type))
				DefaultTranslation.setBonusTranslations.Add(type, new LocalizeTranslation(culture, translation));
			else
				DefaultTranslation.setBonusTranslations[type] = new LocalizeTranslation(culture, translation);
		}
		#endregion

		#region NPC Translation Methods
		public static void AddNpcNameTranslation(ModNPC npc, string npcNameTranslation, GameCulture culture)
		{
			npc.DisplayName.AddTranslation(culture, npcNameTranslation);
		}

		public static void AddChatTranslation(string vanilla, string translation)
		{
			if (!GlobalLocalizeNPC.chatTranslations.ContainsKey(vanilla))
				GlobalLocalizeNPC.chatTranslations.Add(vanilla, translation);
			else
				GlobalLocalizeNPC.chatTranslations[vanilla] = translation;
		}

		public static void AddChatButtonTranslation(ModNPC npc, string button1Translation, string button2Translation, GameCulture culture)
		{
			AddChatButtonTranslation(npc.mod.NPCType(npc.Name), button1Translation, button2Translation, culture);
		}
		public static void AddChatButtonTranslation(int type, string button, string buttonTranslation, GameCulture culture)
		{
			if (!DefaultTranslation.chatButtonTranslations.ContainsKey(type))
			{
				var t = new Dictionary<string, LocalizeTranslation>();
				t.Add(button, new LocalizeTranslation(culture, buttonTranslation));
				DefaultTranslation.chatButtonTranslations.Add(type, t);
			}
			else
			{
				DefaultTranslation.chatButtonTranslations[type].Add(button, new LocalizeTranslation(culture, buttonTranslation));
			}
		}
		#endregion

		#region Buff Translation Methods
		public static void AddBuffNameTranslation(ModBuff buff, string buffNameTranslation, GameCulture culture)
		{
			buff.DisplayName.AddTranslation(culture, buffNameTranslation);
		}

		public static void AddBuffTipTranslation(ModBuff buff, string bufftipTranslation, GameCulture culture)
		{
			buff.Description.AddTranslation(culture, bufftipTranslation);
		}
		#endregion

		#region Tile Translation Methods
		// TODO: Change the way so map entry color can be set correctly.
		public static void AddTileNameTranslation(ModTile tile, string tileNameTranslation, GameCulture culture)
		{
			var translation = tile.mod.CreateTranslation(string.Format("MapObject.{0}", tile.Name));
			translation.AddTranslation(culture, tileNameTranslation);

			new LocalizeTile().AddMapEntryTranslation(tile, translation);
		}
		#endregion
	}
}
