using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using Terraria.ModLoader;
using Harmony;

namespace Localizer
{
	public class Patches
	{
		[HarmonyPatch(typeof(NPCLoader))]
		[HarmonyPatch("SetChatButtons")]
		class AddChatButtonHook
		{
			static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
			{
				int setChatButtonIndex = -1;

				var codes = new List<CodeInstruction>(instructions);
				for (int i = 0; i < codes.Count; i++)
				{
					if (codes[i].opcode == OpCodes.Callvirt && codes[i].operand.ToString().Contains("SetChatButton"))
					{
						setChatButtonIndex = i;
					}
				}
				if (setChatButtonIndex > -1)
				{
					codes[setChatButtonIndex] = new CodeInstruction(OpCodes.Call, typeof(Hooks).GetMethod("SetChatButton"));
					codes.Insert(setChatButtonIndex - 2, new CodeInstruction(OpCodes.Ldloc_0));
				}

				return codes.AsEnumerable();
			}
		}
		
		[HarmonyPatch(typeof(ItemLoader))]
		[HarmonyPatch("UpdateArmorSet")]
		class AddUpdateArmorSetHook
		{
			static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
			{
				List<int> updateArmorSetIndex = new List<int>();

				var codes = new List<CodeInstruction>(instructions);
				for (int i = 0; i < codes.Count; i++)
				{
					if (codes[i].opcode == OpCodes.Callvirt)
					{
						var m = codes[i].operand as MethodInfo;
						if(m.DeclaringType == typeof(ModItem) && m.Name.Contains("UpdateArmorSet"))
							updateArmorSetIndex.Add(i);
					}
				}

				for (int i = 0; i < updateArmorSetIndex.Count; i++)
				{
					var index = updateArmorSetIndex[i];
					codes[index] = new CodeInstruction(OpCodes.Call, typeof(Hooks).GetMethod("UpdateArmorSet"));
					codes[index - 2] = new CodeInstruction(OpCodes.Nop);
				}

				return codes.AsEnumerable();
			}
		}
	}
}
