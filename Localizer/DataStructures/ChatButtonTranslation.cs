using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Localizer.DataStructures
{
	public class ChatButtonTranslation
	{
		public LocalizeTranslation button1;
		public LocalizeTranslation button2;

		public ChatButtonTranslation(GameCulture culture, string button1Translation = null, string button2Translation = null)
		{
			if(button1Translation != null)
			{
				button1 = new LocalizeTranslation(culture, button1Translation);
			}

			if(button2Translation != null)
			{
				button2 = new LocalizeTranslation(culture, button2Translation);
			}
		}
	}
}
