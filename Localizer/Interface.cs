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
		internal const int MenuID = 50000;
		internal const int BrowserID = 50001;
		internal const int ManagerID = 50002;
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
			if (Main.menuMode == MenuID)
			{
				offY = 210;
				spacing = 42;
				numButtons = 9;
				buttonVerticalSpacing[numButtons - 1] = 18;
				for (int i = 0; i < numButtons; i++)
				{
					buttonScales[i] = 0.75f;
				}

				int buttonIndex = 0;
				buttonNames[buttonIndex] = "";
				if (selectedMenu == buttonIndex)
				{
					Main.PlaySound(SoundID.MenuTick);
				}

				buttonIndex++;
				buttonNames[buttonIndex] = Lang.menu[5].Value;
				if (selectedMenu == buttonIndex || backButtonDown)
				{
					backButtonDown = false;
					Main.menuMode = 11;
					Main.PlaySound(11, -1, -1, 1);
				}
			}
			if (Main.menuMode == ManagerID)
			{
				Localizer.TurnToManager();
				return true;
			}

			return false;
		}
	}
}
