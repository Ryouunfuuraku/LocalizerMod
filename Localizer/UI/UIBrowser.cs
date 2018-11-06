using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
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
	public class UIBrowser : UIState
	{
		public UIList textList;
		public UITextPanel<string> backButton;

		private UIElement uIElement;
		private UIPanel uIPanel;

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

			textList = new UIList();
			textList.Width.Set(-25f, 1f);
			textList.Height.Set(-50f, 1f);
			textList.Top.Set(50f, 0f);
			textList.ListPadding = 5f;
			uIPanel.Append(textList);

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

			LoadList();

		}

		private static void BackClick(UIMouseEvent evt, UIElement listeningElement)
		{
			Main.PlaySound(11, -1, -1, 1, 1f, 0f);
			Main.menuMode = 0;
		}

		private static void LoadList()
		{
			using (var fs = new FileStream(Path.Combine(DownloadMgr.CachePath, "index.json"), FileMode.Open))
			{
				using (var sr = new StreamReader(fs))
				{
					var index = JsonConvert.DeserializeObject<Index>(sr.ReadToEnd());

					// TODO: Load content
				}
			}
		}
	}
}
