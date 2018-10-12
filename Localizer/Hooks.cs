using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
				if(!GlobalLocalizeNPC.TranslateChatButton(npc, ref button1, ref button2))
					npc.SetChatButtons(ref button1, ref button2);
			}

			if(PreSetChatButton != null)
			{
				PostSetChatButton.Invoke(npc, ref button1, ref button2);
			}
		}
	}
}
