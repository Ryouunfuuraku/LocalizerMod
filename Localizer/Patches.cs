using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Harmony;

namespace Localizer
{
	public class Patches
	{
		[HarmonyPatch(typeof(NPCLoader))]
		[HarmonyPatch("SetChatButtons")]
		[HarmonyPatch(new Type[] { typeof(string), typeof(string)})]
		class AddChatButtonHook
		{
		}
	}
}
