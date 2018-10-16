using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Localizer.DataStructures;

namespace Localizer
{
	public class DefaultTranslation
	{
		internal static Dictionary<int, ChatButtonTranslation> chatButtonTranslations = new Dictionary<int, ChatButtonTranslation>();
		internal static Dictionary<int, LocalizeTranslation> setBonusTranslations = new Dictionary<int, LocalizeTranslation>();

		public static bool TranslateChatButton(ModNPC npc, ref string button1, ref string button2)
		{
			bool translated = false;
			if (chatButtonTranslations.ContainsKey(npc.npc.netID))
			{
				var t = chatButtonTranslations[npc.npc.netID];
				if (t.button1 != null)
				{
					button1 = t.button1.GetTranslation();
					translated = true;
				}
				if (t.button2 != null)
				{
					button2 = t.button2.GetTranslation();
					translated = true;
				}
			}

			return translated;
		}

		public static bool TranslateSetBonus(Item item)
		{
			bool translated = false;

			if (setBonusTranslations.ContainsKey(item.netID))
			{
				Main.player[Main.myPlayer].setBonus = setBonusTranslations[item.netID].GetTranslation();
				translated = true;
			}

			return translated;
		}
	}
}
