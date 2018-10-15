using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Terraria.Localization;
using Terraria.ModLoader;
using Localizer.DataStructures;

namespace Localizer
{
	public class ImportTool
	{
		public static void ImportItemTexts(Mod mod, string url)
		{
			using (var fs = new FileStream(Path.Combine(url, "Items.json"), FileMode.Open))
			{
				using (var sr = new StreamReader(fs))
				{
					var items = JsonConvert.DeserializeObject<TextFile.ItemFile>(sr.ReadToEnd());

					foreach (var item in items.Items)
					{
						var moditem = mod.GetItem(item.Key);

						if (moditem == null)
							continue;

						if (!string.IsNullOrWhiteSpace(item.Value.NameTranslation))
							TranslateTool.AddItemNameTranslation(moditem, item.Value.NameTranslation, GameCulture.Chinese);
						if (!string.IsNullOrWhiteSpace(item.Value.TooltipTranslation))
							TranslateTool.AddItemTooltipTranslation(moditem, item.Value.TooltipTranslation, GameCulture.Chinese);
					}
				}
			}
		}
	}
}
