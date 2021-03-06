﻿using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Localizer.DataStructures;
using Newtonsoft.Json;

namespace Localizer.UI
{
	public class UIDownload : UIState
	{
		public UIList downloadingList;
		public UITextPanel<string> backButton;

		private UIElement uIElement;
		private UIPanel uIPanel;

		private UIProgress progress;

		public override void OnInitialize()
		{
			uIElement = new UIElement();
			uIElement.Width.Set(0f, 0.8f);
			uIElement.MaxWidth.Set(600f, 0f);
			uIElement.Top.Set(220f, 0f);
			uIElement.Height.Set(-220f, 1f);
			uIElement.HAlign = 0.5f;

			uIPanel = new UIPanel();
			uIPanel.Width.Set(0f, 1f);
			uIPanel.Height.Set(-110f, 1f);
			uIPanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
			uIPanel.PaddingTop = 0f;
			uIElement.Append(uIPanel);

			UITextPanel<string> uIHeaderTexTPanel = new UITextPanel<string>(Language.GetTextValue("Mods.Localizer.DownloadHeadTitle"), 0.8f, true);
			uIHeaderTexTPanel.HAlign = 0.5f;
			uIHeaderTexTPanel.Top.Set(-35f, 0f);
			uIHeaderTexTPanel.SetPadding(15f);
			uIHeaderTexTPanel.BackgroundColor = new Color(73, 94, 171);
			uIElement.Append(uIHeaderTexTPanel);

			UIScrollbar uIScrollbar = new UIScrollbar();
			uIScrollbar.SetView(100f, 1000f);
			uIScrollbar.Height.Set(-50f, 1f);
			uIScrollbar.Top.Set(50f, 0f);
			uIScrollbar.HAlign = 1f;
			uIPanel.Append(uIScrollbar);

			downloadingList = new UIList();
			downloadingList.Width.Set(-25f, 1f);
			downloadingList.Height.Set(-50f, 1f);
			downloadingList.Top.Set(50f, 0f);
			downloadingList.ListPadding = 5f;
			downloadingList.SetScrollbar(uIScrollbar);
			uIPanel.Append(downloadingList);

			backButton = new UITextPanel<string>(Language.GetTextValue("Mods.Localizer.BackButton"));
			backButton.Width.Set(-10f, 1f / 3f);
			backButton.Height.Set(25f, 0f);
			backButton.VAlign = 1f;
			backButton.Top.Set(-65f, 0f);
			backButton.OnClick += BackClick;
			backButton.OnMouseOver += UICommon.FadedMouseOver;
			backButton.OnMouseOut += UICommon.FadedMouseOut;
			uIElement.Append(backButton);

			Append(uIElement);
		}

		private static void BackClick(UIMouseEvent evt, UIElement listeningElement)
		{
			Main.PlaySound(11, -1, -1, 1, 1f, 0f);
			Main.menuMode = Interface.MenuID;
		}

		internal void Clear()
		{
			downloadingList.Clear();
		}

		internal void LoadList()
		{
			Clear();

			lock (Localizer.downloadMgr.Downloadings)
			{
				foreach (var item in Localizer.downloadMgr.Downloadings)
				{
					downloadingList.Add(new UIDownloadItem(item));
				}
			}
		}
	}
}
