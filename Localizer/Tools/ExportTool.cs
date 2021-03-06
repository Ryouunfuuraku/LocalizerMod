﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using Harmony.ILCopying;
using Newtonsoft.Json;
using Terraria.Localization;
using Terraria.ModLoader;
using Localizer.DataStructures;

namespace Localizer
{
	public class ExportTool
	{
		public static void ExportInfo(Mod mod, string path)
		{
			if (mod != null)
			{
				var infoFile = new TextFile.TranslationInfo(mod, LanguageManager.Instance.ActiveCulture);

				using (var fs = new FileStream(Path.Combine(path, "Info.json"), FileMode.Create))
				{
					using (var sw = new StreamWriter(fs))
					{
						sw.Write(JsonConvert.SerializeObject(infoFile, Formatting.Indented));
					}
				}
			}
		}
		
		public static void ExportItemTexts(Mod mod, string path)
		{
			if (mod != null)
			{
				var itemFile = GetItemTexts(mod);

				using (var fs = new FileStream(Path.Combine(path, "Items.json"), FileMode.Create))
				{
					using (var sw = new StreamWriter(fs))
					{
						sw.Write(JsonConvert.SerializeObject(itemFile, Formatting.Indented));
					}
				}
			}
		}

		public static TextFile.ItemFile GetItemTexts(Mod mod)
		{
			var items = typeof(Mod).GetField("items", BindingFlags.Instance | BindingFlags.NonPublic).
					GetValue(mod) as Dictionary<string, ModItem>;
			var itemFile = new TextFile.ItemFile();
			foreach (var itemPair in items)
			{
				// Get basic info (name and tooltip)
				var itemTranslation = new TextFile.ItemTranslation(itemPair.Value);
				itemFile.Items.Add(itemPair.Key, itemTranslation);

				// Get setbonus
				var updateArmorSetMethod = itemPair.Value.GetType()
					.GetMethod("UpdateArmorSet", BindingFlags.Instance | BindingFlags.Public);
				var instructions = ILTool.GetInstructions(updateArmorSetMethod);
				var target = instructions.Find(i => i.opcode == OpCodes.Stfld && i.operand.ToString().Contains("setBonus"));
				if (target != null)
				{
					var setBonus = ILTool.GetStrBeforeInstruction(instructions, target);
					if (!string.IsNullOrWhiteSpace(setBonus))
					{
						itemFile.SetBonus.Add(itemPair.Key, new TextFile.SetBonusTranslation(setBonus));
					}
				}
			}

			return itemFile;
		}

		public static void ExportNPCTexts(Mod mod, string path)
		{
			if (mod != null)
			{
				var npcFile = GetNPCTexts(mod);

				using (var fs = new FileStream(Path.Combine(path, "NPCs.json"), FileMode.Create))
				{
					using (var sw = new StreamWriter(fs))
					{
						sw.Write(JsonConvert.SerializeObject(npcFile, Formatting.Indented));
					}
				}
			}
		}

		public static TextFile.NPCFile GetNPCTexts(Mod mod)
		{
			var npcs = typeof(Mod).GetField("npcs", BindingFlags.Instance | BindingFlags.NonPublic)
					.GetValue(mod) as Dictionary<string, ModNPC>;
			var npcFile = new TextFile.NPCFile();
			foreach (var npcPair in npcs)
			{
				// Get name
				var npcTranslation = new TextFile.NPCTranslation(npcPair.Value);
				npcFile.NPCs.Add(npcPair.Key, npcTranslation);

				// Get chat
				var getChatMethod = npcPair.Value.GetType().GetMethod("GetChat", BindingFlags.Instance | BindingFlags.Public);
				var instructions = ILTool.GetInstructions(getChatMethod);
				var chatlines = new List<ILInstruction>();
				for (int i = 0; i < instructions.Count; i++)
				{
					if (instructions[i].opcode == OpCodes.Ldstr)
					{
						if (i + 1 < instructions.Count &&
						    instructions[i + 1].operand != null &&
						    !instructions[i + 1].operand.ToString().Contains("GetTextValue"))
						{
							chatlines.Add(instructions[i]);
						}
					}
				}

				if (chatlines != null && chatlines.Count > 0)
				{
					var chatLineTranslations = new List<TextFile.ChatLineTranslation>();
					foreach (var line in chatlines)
					{
						chatLineTranslations.Add(new TextFile.ChatLineTranslation(line.operand.ToString()));
					}

					if (chatLineTranslations.Count > 0)
					{
						npcFile.ChatLines.Add(npcPair.Key, chatLineTranslations);
					}
				}

				// Get button
				var setChatButtonsMethod = npcPair.Value.GetType()
					.GetMethod("SetChatButtons", BindingFlags.Instance | BindingFlags.Public);
				instructions = ILTool.GetInstructions(setChatButtonsMethod);
				var buttons = new List<ILInstruction>();
				for (int i = 0; i < instructions.Count; i++)
				{
					if (instructions[i].opcode == OpCodes.Ldstr)
					{
						if (i + 1 < instructions.Count &&
						    instructions[i + 1].operand != null &&
						    !instructions[i + 1].operand.ToString().Contains("GetTextValue"))
						{
							buttons.Add(instructions[i]);
						}
					}
				}

				if (buttons != null && buttons.Count > 0)
				{
					var chatButtonsTranslations = new List<TextFile.ChatButtonTranslation>();
					foreach (var line in buttons)
					{
						chatButtonsTranslations.Add(new TextFile.ChatButtonTranslation(line.operand.ToString()));
					}

					npcFile.ChatButtons.Add(npcPair.Key, chatButtonsTranslations);
				}
			}

			return npcFile;
		}

		public static void ExportBuffTexts(Mod mod, string path)
		{
			if (mod != null)
			{
				var buffFile = GetBuffTexts(mod);

				using (var fs = new FileStream(Path.Combine(path, "Buffs.json"), FileMode.Create))
				{
					using (var sw = new StreamWriter(fs))
					{
						sw.Write(JsonConvert.SerializeObject(buffFile, Formatting.Indented));
					}
				}
			}
		}

		public static TextFile.BuffFile GetBuffTexts(Mod mod)
		{
			var buffs = typeof(Mod).GetField("buffs", BindingFlags.Instance | BindingFlags.NonPublic)
					.GetValue(mod) as Dictionary<string, ModBuff>;
			var buffFile = new TextFile.BuffFile();
			foreach (var buffPair in buffs)
			{
				var buffTranslation = new TextFile.BuffTranslation(buffPair.Value);
				buffFile.Buffs.Add(buffPair.Key, buffTranslation);
			}

			return buffFile;
		}

		public static void ExportMiscTexts(Mod mod, string path)
		{
			if (mod != null)
			{
				var miscFile = GetMiscTexts(mod);

				using (var fs = new FileStream(Path.Combine(path, "Miscs.json"), FileMode.Create))
				{
					using (var sw = new StreamWriter(fs))
					{
						sw.Write(JsonConvert.SerializeObject(miscFile, Formatting.Indented));
					}
				}
			}
		}

		public static TextFile.MiscFile GetMiscTexts(Mod mod)
		{
			var translations =
				typeof(Mod).GetField("translations", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(mod) as
					Dictionary<string, ModTranslation>;
			var miscFile = new TextFile.MiscFile();
			foreach (var translation in translations)
			{
				miscFile.Miscs.Add(translation.Key.Replace(string.Format("Mods.{0}.", mod.Name), ""),
					new TextFile.MiscTranslation(translation.Value));
			}

			return miscFile;
		}
	}
}
