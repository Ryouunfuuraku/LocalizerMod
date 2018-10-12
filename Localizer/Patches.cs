using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Harmony;
using System.Reflection.Emit;

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
	}
}
