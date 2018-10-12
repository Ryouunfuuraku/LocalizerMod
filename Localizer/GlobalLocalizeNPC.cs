using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Localizer.DataStructures;

namespace Localizer
{
	public class GlobalLocalizeNPC : GlobalNPC
	{
		internal static Dictionary<string, string> chatTranslations = new Dictionary<string, string>();
		internal static Dictionary<int, ChatButtonTranslation> chatButtonTranslations = new Dictionary<int, ChatButtonTranslation>();

		public override void GetChat(NPC npc, ref string chat)
		{
			if (!npc.CanTalk)
				return;

			if (chatTranslations.ContainsKey(chat))
			{
				chat = chatTranslations[chat];
			}
		}

		public static bool TranslateChatButton(ModNPC npc, ref string button1, ref string button2)
		{
			bool translated = false;
			if (chatButtonTranslations.ContainsKey(npc.npc.netID))
			{
				var t = chatButtonTranslations[npc.npc.netID];
				if(t.button1 != null)
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
	}
}
