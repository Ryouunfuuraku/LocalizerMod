using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using Terraria;
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
		
		public static void DoManualPatches()
		{
			MenuButtons.Execute();
		}

		public class MenuButtons
		{
			public static void Execute()
			{
				var original = typeof(Mod).Module.GetType("Terraria.ModLoader.Interface").GetMethod("AddMenuButtons", BindingFlags.NonPublic | BindingFlags.Static);
				var transpiler = typeof(MenuButtons).GetMethod("AddMenuButtonsTranspiler", BindingFlags.NonPublic | BindingFlags.Static);

				Localizer.harmony.Patch(original, null, null, new HarmonyMethod(transpiler));

				original = typeof(Mod).Module.GetType("Terraria.ModLoader.Interface").GetMethod("ModLoaderMenus", BindingFlags.NonPublic | BindingFlags.Static);
				transpiler = typeof(MenuButtons).GetMethod("ModLoaderMenusTranspiler", BindingFlags.NonPublic | BindingFlags.Static);

				Localizer.harmony.Patch(original, null, null, new HarmonyMethod(transpiler));
			}

			public static void AddButtons(Main main, int selectedMenu, string[] buttonNames, float[] buttonScales, ref int offY, ref int spacing, ref int buttonIndex, ref int numButtons)
			{
				buttonNames[buttonIndex] = "Localize Manager";
				if (selectedMenu == buttonIndex)
				{
					Main.PlaySound(10, -1, -1, 1, 1f, 0f);
					Main.menuMode = 100000;
				}
				buttonIndex++;
				numButtons++;
			}

			public static bool ModLoaderMenus(Main main, int selectedMenu, string[] buttonNames, float[] buttonScales, int[] buttonVerticalSpacing, ref int offY, ref int spacing, ref int numButtons, ref bool backButtonDown)
			{
				if (Main.menuMode == 100000)
				{
					Localizer.TurnToManager();
					return true;
				}

				return false;
			}

			static IEnumerable<CodeInstruction> AddMenuButtonsTranspiler(IEnumerable<CodeInstruction> instructions)
			{
				List<CodeInstruction> addButton = new List<CodeInstruction>();

				addButton.Add(new CodeInstruction(OpCodes.Ldarg_S, 0));
				addButton.Add(new CodeInstruction(OpCodes.Ldarg_S, 1));
				addButton.Add(new CodeInstruction(OpCodes.Ldarg_S, 2));
				addButton.Add(new CodeInstruction(OpCodes.Ldarg_S, 3));
				addButton.Add(new CodeInstruction(OpCodes.Ldarg_S, 4));
				addButton.Add(new CodeInstruction(OpCodes.Ldarg_S, 5));
				addButton.Add(new CodeInstruction(OpCodes.Ldarg_S, 6));
				addButton.Add(new CodeInstruction(OpCodes.Ldarg_S, 7));
				addButton.Add(new CodeInstruction(OpCodes.Call, typeof(MenuButtons).GetMethod("AddButtons", BindingFlags.Public | BindingFlags.Static)));

				var result = addButton.Concat(instructions);

				return result;
			}

			static IEnumerable<CodeInstruction> ModLoaderMenusTranspiler(IEnumerable<CodeInstruction> instructions)
			{
				var result = instructions.ToList();
				List<CodeInstruction> menus = new List<CodeInstruction>();

				menus.Add(new CodeInstruction(OpCodes.Ldarg_S, 0));
				menus.Add(new CodeInstruction(OpCodes.Ldarg_S, 1));
				menus.Add(new CodeInstruction(OpCodes.Ldarg_S, 2));
				menus.Add(new CodeInstruction(OpCodes.Ldarg_S, 3));
				menus.Add(new CodeInstruction(OpCodes.Ldarg_S, 4));
				menus.Add(new CodeInstruction(OpCodes.Ldarg_S, 5));
				menus.Add(new CodeInstruction(OpCodes.Ldarg_S, 6));
				menus.Add(new CodeInstruction(OpCodes.Ldarg_S, 7));
				menus.Add(new CodeInstruction(OpCodes.Ldarg_S, 8));
				menus.Add(new CodeInstruction(OpCodes.Call, typeof(MenuButtons).GetMethod("ModLoaderMenus", BindingFlags.Public | BindingFlags.Static)));
				menus.Add(new CodeInstruction(OpCodes.Brtrue, result[result.Count-15].operand));

				return menus.Concat(result);
			}
		}
	}
}
