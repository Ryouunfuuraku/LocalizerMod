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
using Localizer.DataStructures;

namespace Localizer.UI
{
	public class UIManagerItem : UIPanel
	{
		private readonly Mod mod;
		private readonly Texture2D dividerTexture;
		private readonly Texture2D innerPanelTexture;
		private readonly UIText modName;
		private readonly UITextPanel<string> button;
		private readonly UITextPanel<string> button2;
		private readonly UITextPanel<string> button3;

		public UIManagerItem(Mod mod)
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

			button = new UITextPanel<string>(Language.GetTextValue("Mods.Localizer.ExportButton"), 1f, false);
			button.Width.Set(100f, 0f);
			button.Height.Set(30f, 0f);
			button.Left.Set(430f, 0f);
			button.Top.Set(40f, 0f);
			button.PaddingTop -= 2f;
			button.PaddingBottom -= 2f;
			button.OnMouseOver += UICommon.FadedMouseOver;
			button.OnMouseOut += UICommon.FadedMouseOut;
			button.OnClick += UpdateModText;
			button.OnDoubleClick += ExportModText;
			base.Append(button);

			button2 = new UITextPanel<string>(Language.GetTextValue("Mods.Localizer.ImportButton"), 1f, false);
			button2.Width.Set(100f, 0f);
			button2.Height.Set(30f, 0f);
			button2.Left.Set(button.Left.Pixels - button2.Width.Pixels - 5f, 0f);
			button2.Top.Set(40f, 0f);
			button2.PaddingTop -= 2f;
			button2.PaddingBottom -= 2f;
			button2.OnMouseOver += UICommon.FadedMouseOver;
			button2.OnMouseOut += UICommon.FadedMouseOut;
			button2.OnClick += ImportModText;
			base.Append(button2);

			button3 = new UITextPanel<string>(Language.GetTextValue("Mods.Localizer.MachineTranslateButton"), 1f, false);
			button3.Width.Set(100f, 0f);
			button3.Height.Set(30f, 0f);
			button3.Left.Set(button2.Left.Pixels - button3.Width.Pixels - 5f, 0f);
			button3.Top.Set(40f, 0f);
			button3.PaddingTop -= 2f;
			button3.PaddingBottom -= 2f;
			button3.OnMouseOver += UICommon.FadedMouseOver;
			button3.OnMouseOut += UICommon.FadedMouseOut;
			button3.OnClick += MachineTranslateModText;
			base.Append(button3);
		}

		public void ExportModText(UIMouseEvent evt, UIElement listeningElement)
		{
			var path = Path.Combine(Main.SavePath, "Texts/", mod.Name);
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			ExportTool.ExportInfo(mod, path);
			ExportTool.ExportItemTexts(mod, path);
			ExportTool.ExportNPCTexts(mod, path);
			ExportTool.ExportBuffTexts(mod, path);
			ExportTool.ExportMiscTexts(mod, path);
		}

		public void UpdateModText(UIMouseEvent evt, UIElement listeningElement)
		{
			var path = Path.Combine(Main.SavePath, "Texts/", mod.Name);
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			if (!ImportTool.CheckDir(path))
			{
				ExportTool.ExportInfo(mod, path);
				ExportTool.ExportItemTexts(mod, path);
				ExportTool.ExportNPCTexts(mod, path);
				ExportTool.ExportBuffTexts(mod, path);
				ExportTool.ExportMiscTexts(mod, path);
			}
			else
			{
				var items = CommonTools.LoadJson<TextFile.ItemFile>(Path.Combine(path, "Items.json"));
				UpdateTool.UpdateItemsText(items, ExportTool.GetItemTexts(mod));
				CommonTools.DumpJson(Path.Combine(path, "Items.json"), items);

				var npcs = CommonTools.LoadJson<TextFile.NPCFile>(Path.Combine(path, "NPCs.json"));
				UpdateTool.UpdateNPCsText(npcs, ExportTool.GetNPCTexts(mod));
				CommonTools.DumpJson(Path.Combine(path, "NPCs.json"), npcs);

				var buffs = CommonTools.LoadJson<TextFile.BuffFile>(Path.Combine(path, "Buffs.json"));
				UpdateTool.UpdateBuffsText(buffs, ExportTool.GetBuffTexts(mod));
				CommonTools.DumpJson(Path.Combine(path, "Buffs.json"), buffs);

				var miscs = CommonTools.LoadJson<TextFile.MiscFile>(Path.Combine(path, "Miscs.json"));
				UpdateTool.UpdateMiscsText(miscs, ExportTool.GetMiscTexts(mod));
				CommonTools.DumpJson(Path.Combine(path, "Miscs.json"), miscs);
			}
		}
		
		public void ImportModText(UIMouseEvent evt, UIElement listeningElement)
		{
			var path = Path.Combine(Main.SavePath, "Texts/", mod.Name);
			var info = ImportTool.ReadInfo(path);
			ImportTool.ImportItemTexts(mod, path, info.Culture);
			ImportTool.ImportNPCTexts(mod, path, info.Culture);
			ImportTool.ImportBuffTexts(mod, path, info.Culture);
			ImportTool.ImportMiscTexts(mod, path, info.Culture); 

			ModLoader.RefreshModLanguage(LanguageManager.Instance.ActiveCulture);
		}

		public void MachineTranslateModText(UIMouseEvent evt, UIElement listeningElement)
		{
			var path = Path.Combine(Main.SavePath, "Texts/", mod.Name);
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			ExportModText(evt, listeningElement);
			var translator = new MachineTranslator();
			translator.TranslateFromPath(path);
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
