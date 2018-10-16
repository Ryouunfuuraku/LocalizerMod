using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Localizer
{
	public class Hooks
	{
		public delegate bool PreSetChatButtonHandler(ModNPC npc, ref string button1, ref string button2);
		public static PreSetChatButtonHandler PreSetChatButton;

		public delegate void PostSetChatButtonHandler(ModNPC npc, ref string button1, ref string button2);
		public static PostSetChatButtonHandler PostSetChatButton;

		public static void SetChatButton(ModNPC npc, ref string button1, ref string button2)
		{
			if(PreSetChatButton == null || !PreSetChatButton.Invoke(npc, ref button1, ref button2))
			{
				if(!DefaultTranslation.TranslateChatButton(npc, ref button1, ref button2))
					npc.SetChatButtons(ref button1, ref button2);
			}

			if(PostSetChatButton != null)
			{
				PostSetChatButton.Invoke(npc, ref button1, ref button2);
			}
		}

		public delegate bool PreUpdateArmorSetHandler(Player player, Item item);
		public static PreUpdateArmorSetHandler PreUpdateArmorSet;

		public delegate void PostUpdateArmorSetHandler(Player player, Item item);
		public static PostUpdateArmorSetHandler PostUpdateArmorSet;

		public static void UpdateArmorSet(Item item, Player player)
		{
			if (PreUpdateArmorSet == null || !PreUpdateArmorSet.Invoke(player, item))
			{
				if(player.whoAmI != Main.myPlayer || !DefaultTranslation.TranslateSetBonus(item))
				{
					item.modItem.UpdateArmorSet(player);
				}
			}

			if (PostUpdateArmorSet != null)
			{
				PostUpdateArmorSet.Invoke(player, item);
			}
		}
	}
}
