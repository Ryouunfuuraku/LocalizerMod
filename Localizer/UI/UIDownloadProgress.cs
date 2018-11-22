using System;
using System.IO;
using System.Collections.Generic;
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
	public class UIDownloadProgress : UIState
	{
		private UIProgress progress;
		private string name;
		private Action cancelAction;

		public override void OnInitialize()
		{
			progress = new UIProgress();
			progress.Width.Set(0f, 0.8f);
			progress.MaxWidth.Set(600f, 0f);
			progress.Height.Set(150f, 0f);
			progress.HAlign = 0.5f;
			progress.VAlign = 0.5f;
			progress.Top.Set(10f, 0f);
			base.Append(progress);

			var cancel = new UITextPanel<string>(Language.GetTextValue("UI.Cancel"), 0.75f, true);
			cancel.VAlign = 0.5f;
			cancel.HAlign = 0.5f;
			cancel.Top.Set(170f, 0f);
			cancel.OnMouseOver += UICommon.FadedMouseOver;
			cancel.OnMouseOut += UICommon.FadedMouseOut;
			cancel.OnClick += CancelClick;
			base.Append(cancel);
		}

		public override void OnActivate()
		{
			progress.SetText(Language.GetTextValue("Mods.Localizer.Downloading", name));
			progress.SetProgress(0f);
		}

		public void SetDownloading(string name)
		{
			this.name = name;
		}

		public void SetCancel(Action cancelAction)
		{
			this.cancelAction = cancelAction;
		}

		internal void SetProgress(DownloadProgressChangedEventArgs e)
		{
			SetProgress(e.BytesReceived, e.TotalBytesToReceive);
		}

		internal void SetProgress(long count, long len)
		{
			//progress?.SetText("Downloading: " + name + " -- " + count+"/" + len);
			progress.SetProgress((float)count / len);
		}

		private void CancelClick(UIMouseEvent evt, UIElement listeningElement)
		{
			Main.PlaySound(10);
			cancelAction();
		}
	}
}
