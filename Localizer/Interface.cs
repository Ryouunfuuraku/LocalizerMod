using System;
using System.IO;
using System.Net;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.UI;
using Localizer.UI;
using Terraria.World.Generation;

namespace Localizer
{
	public class Interface
	{
		internal const int MenuID = 50000;
		internal const int BrowserID = 50001;
		internal const int ManagerID = 50002;
		internal const int DownloadID = 50003;
		internal static UIBrowser browser;
		internal static UIManager manager;
		internal static UIDownload download;

		public static void Init()
		{
			browser = new UIBrowser();
			manager = new UIManager();
			download = new UIDownload();
		}
		
		public static void AddButtons(Main main, int selectedMenu, string[] buttonNames, float[] buttonScales, ref int offY, ref int spacing, ref int buttonIndex, ref int numButtons)
		{
			buttonNames[buttonIndex] = Language.GetTextValue("Mods.Localizer.MenuButton");
			if (selectedMenu == buttonIndex)
			{
				Main.PlaySound(10, -1, -1, 1, 1f, 0f);
				Main.menuMode = MenuID;
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
					buttonScales[i] = 1f;
				}

				int buttonIndex = 0;
				buttonNames[buttonIndex] = Language.GetTextValue("Mods.Localizer.MenuBrowserButton");
				if (selectedMenu == buttonIndex)
				{
					Main.PlaySound(SoundID.MenuTick);
					Main.menuMode = BrowserID;
				}

				buttonIndex++;
				buttonNames[buttonIndex] = Language.GetTextValue("Mods.Localizer.MenuManagerButton");
				if (selectedMenu == buttonIndex)
				{
					Main.PlaySound(SoundID.MenuTick);
					Main.menuMode = ManagerID;
				}

				buttonIndex++;
				buttonNames[buttonIndex] = Language.GetTextValue("Mods.Localizer.MenuDownloadButton");
				if (selectedMenu == buttonIndex)
				{
					Main.PlaySound(SoundID.MenuTick);
					Main.menuMode = DownloadID;
				}

				buttonIndex++;
				buttonNames[buttonIndex] = Language.GetTextValue("Mods.Localizer.MenuSettingButton");
				if (selectedMenu == buttonIndex)
				{
					Main.PlaySound(SoundID.MenuTick);
					// Enter settings
				}

				buttonIndex++;
				buttonNames[buttonIndex] = Lang.menu[5].Value;
				if (selectedMenu == buttonIndex || backButtonDown)
				{
					backButtonDown = false;
					Main.menuMode = 0;
					Main.PlaySound(11, -1, -1, 1);
				}
			}

			if (Main.menuMode == BrowserID)
			{
				Main.MenuUI.SetState(browser);
				Main.menuMode = 888;
				browser.LoadList();
			}
			else if(Main.menuMode == ManagerID)
			{
				Main.MenuUI.SetState(manager);
				Main.menuMode = 888;
				manager.LoadModList();
			}
			else if (Main.menuMode == DownloadID)
			{
				Main.MenuUI.SetState(download);
				Main.menuMode = 888;
				download.LoadList();
			}


			return false;
		}
	}
}
