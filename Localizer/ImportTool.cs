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
		public static void ImportItemTexts(Mod mod, string path)
		{
			using (var fs = new FileStream(Path.Combine(path, "Items.json"), FileMode.Open))
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

					foreach (var setbonus in items.SetBonus)
					{
						if (!string.IsNullOrWhiteSpace(setbonus.Value.Translation))
							TranslateTool.AddSetBonusTranslation(mod.GetItem(setbonus.Key), setbonus.Value.Translation, GameCulture.Chinese);
					}
				}
			}
		}

		public static void ImportNPCTexts(Mod mod, string path)
		{
			using (var fs = new FileStream(Path.Combine(path, "NPCs.json"), FileMode.Open))
			{
				using (var sr = new StreamReader(fs))
				{
					var npcs = JsonConvert.DeserializeObject<TextFile.NPCFile>(sr.ReadToEnd());

					foreach (var npc in npcs.NPCs)
					{
						var modnpc = mod.GetNPC(npc.Key);

						if (modnpc == null)
							continue;

						if (!string.IsNullOrWhiteSpace(npc.Value.NameTranslation))
							TranslateTool.AddNpcNameTranslation(modnpc, npc.Value.NameTranslation, GameCulture.Chinese);
					}
				}
			}
		}

		public static void ImportBuffTexts(Mod mod, string path)
		{
			using (var fs = new FileStream(Path.Combine(path, "Buffs.json"), FileMode.Open))
			{
				using (var sr = new StreamReader(fs))
				{
					var buffs = JsonConvert.DeserializeObject<TextFile.BuffFile>(sr.ReadToEnd());

					foreach (var buff in buffs.Buffs)
					{
						var modbuff = mod.GetBuff(buff.Key);

						if (modbuff == null)
							continue;

						if (!string.IsNullOrWhiteSpace(buff.Value.NameTranslation))
							TranslateTool.AddBuffNameTranslation(modbuff, buff.Value.NameTranslation, GameCulture.Chinese);
						if (!string.IsNullOrWhiteSpace(buff.Value.TipTranslation))
							TranslateTool.AddBuffTipTranslation(modbuff, buff.Value.TipTranslation, GameCulture.Chinese);
					}
				}
			}
		}
	}
}
