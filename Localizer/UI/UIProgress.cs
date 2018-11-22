using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria;
using Terraria.UI;

namespace Localizer.UI
{
	internal class UIProgress : UIPanel
	{
		private UIText text;
		private float progress;

		public UIProgress()
		{
			text = new UIText("", 0.75f, true);
			text.Top.Set(20f, 0f);
			text.HAlign = 0.5f;
			base.Append(text);
			progress = 0f;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			CalculatedStyle space = GetInnerDimensions();
			spriteBatch.Draw(Main.magicPixel, new Rectangle((int)space.X + 10, (int)space.Y + (int)space.Height / 2 + 20, (int)space.Width - 20, 10), new Rectangle(0, 0, 1, 1), new Color(0, 0, 70));
			spriteBatch.Draw(Main.magicPixel, new Rectangle((int)space.X + 10, (int)space.Y + (int)space.Height / 2 + 20, (int)((space.Width - 20) * progress), 10), new Rectangle(0, 0, 1, 1), new Color(200, 200, 70));
		}

		public void SetText(string text)
		{
			this.text.SetText(text, 0.75f, true);
		}

		public void SetProgress(float progress)
		{
			this.progress = progress;
		}
	}
}
