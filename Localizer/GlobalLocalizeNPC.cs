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
	public class GlobalLocalizeNPC : GlobalNPC
	{
		internal static Dictionary<string, string> chatTranslations = new Dictionary<string, string>();

		public override void GetChat(NPC npc, ref string chat)
		{
			if (!npc.CanTalk)
				return;

			if (chatTranslations.ContainsKey(chat))
			{
				chat = chatTranslations[chat];
			}
		}
	}
}
