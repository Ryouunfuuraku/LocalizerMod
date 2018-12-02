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
		public static void ImportModTexts(Mod mod, string path)
		{
			Logger.DebugLog(string.Format("Import {0} from {1}", mod.Name, path));
			if (!Directory.Exists(path) || !CheckDir(path))
				return;
			var info = ReadInfo(path);
			var loaded = Localizer.LoadedIndex.Items.Find(i => i.Mod == info.Mod);
			if (loaded != null)
			{
				loaded.Version = info.Version;
			}
			else
			{
				Localizer.LoadedIndex.Items.Add(new Index.Item()
				{
					Mod = info.Mod,
					Version = info.Version,
					Author = info.Translator
				});
			}

			if (Localizer.Config.DeeplyLocalize)
			{
				ImportItemTexts(mod, path, GameCulture.English);
				ImportNPCTexts(mod, path, GameCulture.English);
				ImportBuffTexts(mod, path, GameCulture.English);
				ImportMiscTexts(mod, path, GameCulture.English);
			}
			{
				ImportItemTexts(mod, path, info.Culture);
				ImportNPCTexts(mod, path, info.Culture);
				ImportBuffTexts(mod, path, info.Culture);
				ImportMiscTexts(mod, path, info.Culture);
			}

			ModLoader.RefreshModLanguage(LanguageManager.Instance.ActiveCulture);
		}

		private static string[] _files =
		{
			"Info.json",
			"Items.json",
			"NPCs.json",
			"Buffs.json",
			"Miscs.json"
		};
		public static bool CheckDir(string path)
		{
			Logger.DebugLog(string.Format("Check {0}", path));
			var files = new DirectoryInfo(path).GetFiles();

			foreach (var f in _files)
			{
				if (files.All(file => file.Name != f))
					return false;
			}

			return true;
		}

		public static TextFile.TranslationInfo ReadInfo(string path)
		{
			using (var fs = new FileStream(Path.Combine(path, "Info.json"), FileMode.Open))
			{
				using (var sr = new StreamReader(fs))
				{
					var info = JsonConvert.DeserializeObject<TextFile.TranslationInfo>(sr.ReadToEnd());

					return info;
				}
			}
		}

		public static void ImportItemTexts(Mod mod, string path, GameCulture culture)
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
							TranslateTool.AddItemNameTranslation(moditem, item.Value.NameTranslation, culture);
						if (!string.IsNullOrWhiteSpace(item.Value.TooltipTranslation))
							TranslateTool.AddItemTooltipTranslation(moditem, item.Value.TooltipTranslation, culture);
					}

					foreach (var setbonus in items.SetBonus)
					{
						if (!string.IsNullOrWhiteSpace(setbonus.Value.Translation))
							TranslateTool.AddSetBonusTranslation(mod.GetItem(setbonus.Key), setbonus.Value.Translation, culture);
					}
				}
			}
		}

		public static void ImportNPCTexts(Mod mod, string path, GameCulture culture)
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
							TranslateTool.AddNpcNameTranslation(modnpc, npc.Value.NameTranslation, culture);
					}
				}
			}
		}

		public static void ImportBuffTexts(Mod mod, string path, GameCulture culture)
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
							TranslateTool.AddBuffNameTranslation(modbuff, buff.Value.NameTranslation, culture);
						if (!string.IsNullOrWhiteSpace(buff.Value.TipTranslation))
							TranslateTool.AddBuffTipTranslation(modbuff, buff.Value.TipTranslation, culture);
					}
				}
			}
		}

		public static void ImportMiscTexts(Mod mod, string path, GameCulture culture)
		{
			using (var fs = new FileStream(Path.Combine(path, "Miscs.json"), FileMode.Open))
			{
				using (var sr = new StreamReader(fs))
				{
					var miscs = JsonConvert.DeserializeObject<TextFile.MiscFile>(sr.ReadToEnd());

					foreach (var misc in miscs.Miscs)
					{
						if (!string.IsNullOrWhiteSpace(misc.Value.Translation))
						{
							var translation = mod.CreateTranslation(misc.Key);
							translation.AddTranslation(culture, misc.Value.Translation);
							mod.AddTranslation(translation);
						}
					}
				}
			}
		}
	}
}
