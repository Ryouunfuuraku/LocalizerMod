using System;
using System.IO;
using System.Net;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.UI;
using Localizer.UI;

namespace Localizer
{
	public class Interface
	{
		internal const int BrowserID = 40000;
		internal const int ManagerID = 40001;
		internal static UIBrowser browser = new UIBrowser();
		internal static UILocalManager manager = new UILocalManager();
		
		public static void AddButtons(Main main, int selectedMenu, string[] buttonNames, float[] buttonScales, ref int offY, ref int spacing, ref int buttonIndex, ref int numButtons)
		{
			buttonNames[buttonIndex] = Language.GetTextValue("Mods.Localizer.ManagerButton");
			if (selectedMenu == buttonIndex)
			{
				Main.PlaySound(10, -1, -1, 1, 1f, 0f);
				Main.menuMode = 100000;
			}
			buttonIndex++;
			numButtons++;
		}
		
		public static bool LocalizerMenus(Main main, int selectedMenu, string[] buttonNames, float[] buttonScales, int[] buttonVerticalSpacing, ref int offY, ref int spacing, ref int numButtons, ref bool backButtonDown)
		{
			if (Main.menuMode == 100000)
			{
				Localizer.TurnToManager();
				return true;
			}

			return false;
		}
	}
}
