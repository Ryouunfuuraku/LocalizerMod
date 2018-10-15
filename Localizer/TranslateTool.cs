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

		public static void AddChatTranslation(string vanilla, string translation)
		{
			GlobalLocalizeNPC.chatTranslations.Add(vanilla, translation);
		}

		public static void AddChatButtonTranslation(ModNPC npc, string button1Translation, string button2Translation, GameCulture culture)
		{
			GlobalLocalizeNPC.chatButtonTranslations.Add(npc.npc.netID, new ChatButtonTranslation(culture, button1Translation, button2Translation));
		}
		public static void AddChatButtonTranslation(int type, string button1Translation, string button2Translation, GameCulture culture)
		{
			GlobalLocalizeNPC.chatButtonTranslations.Add(type, new ChatButtonTranslation(culture, button1Translation, button2Translation));
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
