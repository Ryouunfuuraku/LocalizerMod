using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using Harmony.ILCopying;
using Newtonsoft.Json;
using Terraria;
using Terraria.ModLoader;
using Localizer.DataStructures;

namespace Localizer
{
	public class ExportTool
	{
		public static void ExportItemTexts(Mod mod, string path)
		{
			if (mod != null)
			{
				var items = typeof(Mod).GetField("items", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(mod) as Dictionary<string, ModItem>;
				var itemFile = new TextFile.ItemFile();
				foreach (var itemPair in items)
				{
					// Get basic info (name and tooltip)
					var itemTranslation = new TextFile.ItemTranslation(itemPair.Value);
					itemFile.Items.Add(itemPair.Key, itemTranslation);

					// Get setbonus
					var updateArmorSetMethod = itemPair.Value.GetType().GetMethod("UpdateArmorSet", BindingFlags.Instance | BindingFlags.Public);
					var dummy = new DynamicMethod("Dummy", typeof(void), new Type[] { });
					var instructions = MethodBodyReader.GetInstructions(dummy.GetILGenerator(), updateArmorSetMethod);
					var target = instructions.Find(i => i.opcode == OpCodes.Stfld && i.operand.ToString().Contains("setBonus"));
					if(target != null)
					{
						var setBonus = ILHelper.GetStrBeforeInstruction(instructions, target);
						if (!string.IsNullOrWhiteSpace(setBonus))
						{
							itemFile.SetBonus.Add(itemPair.Key, new TextFile.SetBonusTranslation(setBonus));
						}
					}
				}

				using (var fs = new FileStream(Path.Combine(path, "Items.json"), FileMode.Create))
				{
					using (var sw = new StreamWriter(fs))
					{
						sw.Write(JsonConvert.SerializeObject(itemFile, Formatting.Indented));
					}
				}
			}
		}

		public static void ExportNPCTexts(Mod mod, string path)
		{
			if (mod != null)
			{
				var npcs = typeof(Mod).GetField("npcs", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(mod) as Dictionary<string, ModNPC>;
				var npcFile = new TextFile.NPCFile();
				foreach (var npcPair in npcs)
				{
					// Get name
					var npcTranslation = new TextFile.NPCTranslation(npcPair.Value);
					npcFile.NPCs.Add(npcPair.Key, npcTranslation);
				}

				using (var fs = new FileStream(Path.Combine(path, "NPCs.json"), FileMode.Create))
				{
					using (var sw = new StreamWriter(fs))
					{
						sw.Write(JsonConvert.SerializeObject(npcFile, Formatting.Indented));
					}
				}
			}
		}

		public static void ExportBuffTexts(Mod mod, string path)
		{
			if (mod != null)
			{
				var buffs = typeof(Mod).GetField("buffs", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(mod) as Dictionary<string, ModBuff>;
				var buffFile = new TextFile.BuffFile();
				foreach (var buffPair in buffs)
				{
					var buffTranslation = new TextFile.BuffTranslation(buffPair.Value);
					buffFile.Buffs.Add(buffPair.Key, buffTranslation);
				}

				using (var fs = new FileStream(Path.Combine(path, "Buffs.json"), FileMode.Create))
				{
					using (var sw = new StreamWriter(fs))
					{
						sw.Write(JsonConvert.SerializeObject(buffFile, Formatting.Indented));
					}
				}
			}
		}
	}
}
