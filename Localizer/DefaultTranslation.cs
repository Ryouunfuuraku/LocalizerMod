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
		internal static Dictionary<int, Dictionary<string, LocalizeTranslation>> chatButtonTranslations = new Dictionary<int, Dictionary<string, LocalizeTranslation>>();
		internal static Dictionary<int, LocalizeTranslation> setBonusTranslations = new Dictionary<int, LocalizeTranslation>();

		public static bool TranslateChatButton(ModNPC npc, ref string button1, ref string button2)
		{
			bool translated = false;

			if (chatButtonTranslations.ContainsKey(npc.npc.netID))
			{
				var t = chatButtonTranslations[npc.npc.netID];
				if (t.ContainsKey(button1))
				{
					button1 = t[button1].GetTranslation();
					translated = true;
				}
				if (t.ContainsKey(button2))
				{
					button2 = t[button2].GetTranslation();
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
