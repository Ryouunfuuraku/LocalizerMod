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
				foreach (var item in items)
				{
					// Get basic info (name and tooltip)
					var itemTranslation = new TextFile.ItemTranslation(item.Value);
					itemFile.Items.Add(item.Key, itemTranslation);

					// Get setbonus
					var updateArmorSetMethod = item.Value.GetType().GetMethod("UpdateArmorSet", BindingFlags.Instance | BindingFlags.Public);
					var dummy = new DynamicMethod("Dummy", typeof(void), new Type[] { });
					var instructions = MethodBodyReader.GetInstructions(dummy.GetILGenerator(), updateArmorSetMethod);
					var target = instructions.Find(i => i.opcode == OpCodes.Stfld && i.operand.ToString().Contains("setBonus"));
					if(target != null)
					{
						var setBonus = ILHelper.GetStrBeforeInstruction(instructions, target);
						if (!string.IsNullOrWhiteSpace(setBonus))
						{
							itemFile.SetBonus.Add(setBonus, "");
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
	}
}
