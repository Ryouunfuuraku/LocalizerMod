using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria;
using Terraria.Graphics;
using Terraria.UI;
using Terraria.ModLoader;
using System.Linq;
using System.Net;
using Terraria.Localization;
using System.Reflection;
using Localizer.DataStructures;

namespace Localizer.UI
{
	public class UIDownloadItem : UIPanel
	{
		public DownloadMgr.DownloadItem Item;

		private readonly Texture2D dividerTexture;
		private readonly Texture2D innerPanelTexture;
		private readonly UIText name;
		private readonly UIProgress progress;

		public UIDownloadItem(DownloadMgr.DownloadItem item)
		{
			this.Item = item;
			this.BorderColor = new Color(89, 116, 213) * 0.7f;
			this.dividerTexture = TextureManager.Load("Images/UI/Divider");
			this.innerPanelTexture = TextureManager.Load("Images/UI/InnerPanelBackground");
			this.Height.Set(90f, 0f);
			this.Width.Set(0f, 1f);
			base.SetPadding(6f);

			string text = item.Name;
			this.name = new UIText(text, 1f, false);
			this.name.Left.Set(10f, 0f);
			this.name.Top.Set(5f, 0f);
			base.Append(this.name);
			
			progress = new UIProgress();
			progress.Width.Set(0f, 0.8f);
			progress.MaxWidth.Set(600f, 0f);
			progress.Height.Set(25f, 0f);
			progress.HAlign = 0.5f;
			progress.VAlign = 0.5f;
			progress.Top.Set(14f, 0f);
			base.Append(progress);

			//UITextPanel<string> cancelButton = new UITextPanel<string>(Language.GetTextValue("Mods.Localizer.DownloadButton"), 1f, false);
			//cancelButton.Width.Set(100f, 0f);
			//cancelButton.Height.Set(30f, 0f);
			//cancelButton.Left.Set(430f, 0f);
			//cancelButton.Top.Set(40f, 0f);
			//cancelButton.PaddingTop -= 2f;
			//cancelButton.PaddingBottom -= 2f;
			//cancelButton.OnMouseOver += UICommon.FadedMouseOver;
			//cancelButton.OnMouseOut += UICommon.FadedMouseOut;
			//button.OnClick += CancelDownload;
			//base.Append(cancelButton);

			item.Client.DownloadProgressChanged += OnProgressChange;
			item.Client.DownloadFileCompleted += OnComplete;
		}

		public void OnProgressChange(object sender, DownloadProgressChangedEventArgs e)
		{
			name.SetText(name.Text + " " + ((float)e.BytesReceived / e.TotalBytesToReceive));
			progress.SetProgress((float)e.BytesReceived / e.TotalBytesToReceive);
		}

		public void OnComplete(object sender, AsyncCompletedEventArgs e)
		{
			lock (Interface.download)
			{
				Interface.download.LoadList();
			}

			lock (Item)
			{
				var textDir = new FileInfo(Item.SavePath).Directory;
				if (ImportTool.CheckDir(textDir.FullName))
				{
					Localizer.ApplyTextFile(textDir);
				}
			}
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			this.BackgroundColor = new Color(73, 94, 171);
			this.BorderColor = new Color(89, 116, 213);
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			this.BackgroundColor = new Color(63, 82, 151) * 0.7f;
			this.BorderColor = new Color(89, 116, 213) * 0.7f;
		}
	}
}
