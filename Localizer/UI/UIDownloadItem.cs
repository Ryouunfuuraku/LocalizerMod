using System;
using System.Collections.Generic;
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
		private readonly DownloadMgr.DownloadItem item;
		private readonly Texture2D dividerTexture;
		private readonly Texture2D innerPanelTexture;
		private readonly UIText name;
		private readonly UIProgress progress;

		public UIDownloadItem(DownloadMgr.DownloadItem item)
		{
			this.item = item;
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
			progress.Left.Set(10f, 0f);
			progress.Top.Set(10f, 0f);
			base.Append(progress);

			UITextPanel<string> cancelButton = new UITextPanel<string>(Language.GetTextValue("Mods.Localizer.DownloadButton"), 1f, false);
			cancelButton.Width.Set(100f, 0f);
			cancelButton.Height.Set(30f, 0f);
			cancelButton.Left.Set(430f, 0f);
			cancelButton.Top.Set(40f, 0f);
			cancelButton.PaddingTop -= 2f;
			cancelButton.PaddingBottom -= 2f;
			cancelButton.OnMouseOver += UICommon.FadedMouseOver;
			cancelButton.OnMouseOut += UICommon.FadedMouseOut;
			//button.OnClick += CancelDownload;
			base.Append(cancelButton);
		}

		public void DrawPanel(SpriteBatch spriteBatch, Vector2 position, float width)
		{
			progress.SetProgress(item.Progress);
			spriteBatch.Draw(this.innerPanelTexture, position, new Rectangle?(new Rectangle(0, 0, 8, this.innerPanelTexture.Height)), Color.White);
			spriteBatch.Draw(this.innerPanelTexture, new Vector2(position.X + 8f, position.Y), new Rectangle?(new Rectangle(8, 0, 8, this.innerPanelTexture.Height)), Color.White, 0f, Vector2.Zero, new Vector2((width - 16f) / 8f, 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(this.innerPanelTexture, new Vector2(position.X + width - 8f, position.Y), new Rectangle?(new Rectangle(16, 0, 8, this.innerPanelTexture.Height)), Color.White);
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
