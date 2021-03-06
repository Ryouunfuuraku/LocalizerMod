﻿using System;
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
	public class UIBrowserItem : UIPanel
	{
		public readonly Index.Item item;
		private readonly Texture2D dividerTexture;
		private readonly Texture2D innerPanelTexture;
		private readonly UIText modName;
		private readonly UIText authorName;
		private readonly UITextPanel<string> button;

		public UIBrowserItem(Index.Item item)
		{
			button = new UITextPanel<string>(Language.GetTextValue("Mods.Localizer.DownloadButton"), 1f, false);

			var loaded = Localizer.LoadedIndex.Items.Find(i => i.Mod == item.Mod);
			if (loaded != null)
			{
				Logger.DebugLog(string.Format("Mod:{0} local version:{1} remote version:{2}", item.Mod, loaded.Version, item.Version));
				if (item.Version > loaded.Version)
				{
					button = new UITextPanel<string>(Language.GetTextValue("Mods.Localizer.UpdateTextButton"), 1f, false);
				}
				else
				{
					return;
				}
			}

			this.item = item;
			this.BorderColor = new Color(89, 116, 213) * 0.7f;
			this.dividerTexture = TextureManager.Load("Images/UI/Divider");
			this.innerPanelTexture = TextureManager.Load("Images/UI/InnerPanelBackground");
			this.Height.Set(90f, 0f);
			this.Width.Set(0f, 1f);
			base.SetPadding(6f);
			
			this.modName = new UIText(item.Mod, 1f, false);
			this.modName.Left.Set(10f, 0f);
			this.modName.Top.Set(5f, 0f);
			base.Append(this.modName);

			this.authorName = new UIText(Language.GetTextValue("Mods.Localizer.Author") + item.Author, 1f, false);
			this.authorName.Left.Set(10f, 0f);
			this.authorName.Top.Set(50f, 0f);
			base.Append(this.authorName);

			button.Width.Set(100f, 0f);
			button.Height.Set(30f, 0f);
			button.Left.Set(430f, 0f);
			button.Top.Set(40f, 0f);
			button.PaddingTop -= 2f;
			button.PaddingBottom -= 2f;
			button.OnMouseOver += UICommon.FadedMouseOver;
			button.OnMouseOut += UICommon.FadedMouseOut;
			button.OnClick += DownloadText;
			base.Append(button);
		}

		public void DownloadText(UIMouseEvent evt, UIElement listeningElement)
		{
			Localizer.downloadMgr.DownloadModText(item.Mod);
			base.RemoveChild(this);
		}

		public void DrawPanel(SpriteBatch spriteBatch, Vector2 position, float width)
		{
			spriteBatch.Draw(this.innerPanelTexture, position, new Rectangle?(new Rectangle(0, 0, 8, this.innerPanelTexture.Height)), Color.White);
			spriteBatch.Draw(this.innerPanelTexture, new Vector2(position.X + 8f, position.Y), new Rectangle?(new Rectangle(8, 0, 8, this.innerPanelTexture.Height)), Color.White, 0f, Vector2.Zero, new Vector2((width - 16f) / 8f, 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(this.innerPanelTexture, new Vector2(position.X + width - 8f, position.Y), new Rectangle?(new Rectangle(16, 0, 8, this.innerPanelTexture.Height)), Color.White);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			Vector2 drawPos = new Vector2(innerDimensions.X + 5f, innerDimensions.Y + 30f);
			spriteBatch.Draw(this.dividerTexture, drawPos, null, Color.White, 0f, Vector2.Zero, new Vector2((innerDimensions.Width - 10f) / 8f, 1f), SpriteEffects.None, 0f);
			drawPos = new Vector2(innerDimensions.X + 10f, innerDimensions.Y + 45f);
			//this.DrawPanel(spriteBatch, drawPos, 85f);
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
