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
using Terraria.Localization;
using System.Reflection;

namespace Localizer.UI
{
	public class UIModListItem : UIPanel
	{
		private readonly Mod mod;
		private readonly Texture2D dividerTexture;
		private readonly Texture2D innerPanelTexture;
		private readonly UIText modName;
		private readonly UITextPanel<string> button2;

		public UIModListItem(Mod mod)
		{
			this.mod = mod;
			this.BorderColor = new Color(89, 116, 213) * 0.7f;
			this.dividerTexture = TextureManager.Load("Images/UI/Divider");
			this.innerPanelTexture = TextureManager.Load("Images/UI/InnerPanelBackground");
			this.Height.Set(90f, 0f);
			this.Width.Set(0f, 1f);
			base.SetPadding(6f);

			string text = mod.DisplayName;
			this.modName = new UIText(text, 1f, false);
			this.modName.Left.Set(10f, 0f);
			this.modName.Top.Set(5f, 0f);
			base.Append(this.modName);
			UITextPanel<string> button = new UITextPanel<string>("Export", 1f, false);
			button.Width.Set(100f, 0f);
			button.Height.Set(30f, 0f);
			button.Left.Set(430f, 0f);
			button.Top.Set(40f, 0f);
			button.PaddingTop -= 2f;
			button.PaddingBottom -= 2f;
			button.OnMouseOver += UICommon.FadedMouseOver;
			button.OnMouseOut += UICommon.FadedMouseOut;
			button.OnClick += ExportModText;
			base.Append(button);
			//button2 = new UITextPanel<string>("...", 1f, false);
			//button2.Width.Set(100f, 0f);
			//button2.Height.Set(30f, 0f);
			//button2.Left.Set(button.Left.Pixels - button2.Width.Pixels - 5f, 0f);
			//button2.Top.Set(40f, 0f);
			//button2.PaddingTop -= 2f;
			//button2.PaddingBottom -= 2f;
			//button2.OnMouseOver += UICommon.FadedMouseOver;
			//button2.OnMouseOut += UICommon.FadedMouseOut;
			//button2.OnClick += this.ToggleEnabled;
			//base.Append(button2);

			//if (loadedMod != null)
			//{
			//	loaded = true;
			//	int[] values = { loadedMod.items.Count, loadedMod.npcs.Count, loadedMod.tiles.Count, loadedMod.walls.Count, loadedMod.buffs.Count, loadedMod.mountDatas.Count };
			//	string[] localizationKeys = { "ModsXItems", "ModsXNPCs", "ModsXTiles", "ModsXWalls", "ModsXBuffs", "ModsXMounts" };
			//	int xOffset = -40;
			//	for (int i = 0; i < values.Length; i++)
			//	{
			//		if (values[i] > 0)
			//		{
			//			Texture2D iconTexture = Main.instance.infoIconTexture[i];
			//			keyImage = new UIHoverImage(iconTexture, Language.GetTextValue($"tModLoader.{localizationKeys[i]}", values[i]));
			//			keyImage.Left.Set(xOffset, 1f);
			//			base.Append(keyImage);
			//			xOffset -= 18;
			//		}
			//	}
			//}
		}

		public void ExportModText(UIMouseEvent evt, UIElement listeningElement)
		{
			var path = Path.Combine(Main.SavePath, "ExportedText/", mod.Name);
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			ExportTool.ExportItemTexts(mod, path);
			ExportTool.ExportNPCTexts(mod, path);
			ExportTool.ExportBuffTexts(mod, path);
			ExportTool.ExportMiscTexts(mod, path);
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
