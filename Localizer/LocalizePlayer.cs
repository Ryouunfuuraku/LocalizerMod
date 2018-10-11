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
	public class LocalizePlayer : ModPlayer
	{
		internal static Dictionary<string, string> setBounsTranslations = new Dictionary<string, string>();

		public override void PostUpdateEquips()
		{
			if (Main.myPlayer != player.whoAmI)
				return;

			if (setBounsTranslations.ContainsKey(player.setBonus))
			{
				player.setBonus = setBounsTranslations[player.setBonus];
			}
		}
	}
}
