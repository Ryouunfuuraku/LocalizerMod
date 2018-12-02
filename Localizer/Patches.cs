using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Harmony;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.IO;

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
			if (!Localizer.Config.CompatibleMode)
			{
				MenuButtons.Execute();
			}

			if (Localizer.Config.EnableModBrowserMirror &&
			    LanguageManager.Instance.ActiveCulture == GameCulture.Chinese)
			{
				ModBrowserMirror.Execute();
			}
		}

		public class ModBrowserMirror
		{
			public static void Execute()
			{
				var original = typeof(Mod).Module.GetType("Terraria.ModLoader.UI.UIModBrowser").GetMethod("PopulateModBrowser", BindingFlags.NonPublic | BindingFlags.Instance);
				var transpiler = typeof(ModBrowserMirror).GetMethod("PopulateModBrowserTranspiler", BindingFlags.NonPublic | BindingFlags.Static);

				Localizer.harmony.Patch(original, null, null, new HarmonyMethod(transpiler));

				original = typeof(Mod).Module.GetType("Terraria.ModLoader.UI.UIModBrowser").GetMethod("PopulateFromJSON", BindingFlags.NonPublic | BindingFlags.Instance);
				transpiler = typeof(ModBrowserMirror).GetMethod("PopulateFromJSONTranspiler", BindingFlags.NonPublic | BindingFlags.Static);

				Localizer.harmony.Patch(original, null, null, new HarmonyMethod(transpiler));
			}

			static IEnumerable<CodeInstruction> PopulateModBrowserTranspiler(IEnumerable<CodeInstruction> instructions)
			{
				var result = instructions.ToList();

				// Change server address
				var serverAddrIns = result.Find(i => i.opcode == OpCodes.Ldstr && (string)i.operand == "http://javid.ddns.net/tModLoader/listmods.php");
				serverAddrIns.operand = "https://raw.githubusercontent.com/TrBossServer/ModBrowserTest/master/modlist.json";

				// Use OpenRead
				var ldstrPOSTIndex = result.FindIndex(i => i.opcode == OpCodes.Ldstr && (string)i.operand == "POST");
				result[ldstrPOSTIndex] = new CodeInstruction(OpCodes.Nop);
				result[ldstrPOSTIndex + 1] = new CodeInstruction(OpCodes.Nop);
				result[ldstrPOSTIndex + 2] = new CodeInstruction(OpCodes.Call, typeof(ModBrowserMirror).GetMethod("OpenRead", BindingFlags.Public | BindingFlags.Static));

				// Change event
				result[ldstrPOSTIndex - 6].operand = typeof(ModBrowserMirror).GetMethod("OnOpenReadCompleted", BindingFlags.Public | BindingFlags.Static);
				result[ldstrPOSTIndex - 5].operand = typeof(OpenReadCompletedEventHandler).GetConstructors()[0];
				result[ldstrPOSTIndex - 4].operand = typeof(WebClient).GetMethod("add_OpenReadCompleted", BindingFlags.Public | BindingFlags.Instance);
				
				return result;
			}

			static IEnumerable<CodeInstruction> PopulateFromJSONTranspiler(IEnumerable<CodeInstruction> instructions)
			{
				var result = instructions.ToList();
				result[81] = new CodeInstruction(OpCodes.Ldstr, "");
				for (int i = 138; i < 170; i++)
				{
					result[i] = new CodeInstruction(OpCodes.Nop);
				}
				return result;
			}

			public static void OnOpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
			{
				var interfaceType = typeof(Mod).Module.GetType("Terraria.ModLoader.Interface");
				var modBrowser = Traverse.Create(interfaceType).Field("modBrowser").GetValue();
				var modBrowserTraverse = Traverse.Create(modBrowser);
				var reloadButton = modBrowserTraverse.Field("reloadButton").GetValue() as UITextPanel<string>;
				var loading = modBrowserTraverse.Field("loading");

				if (e.Error != null)
				{
					if (e.Cancelled)
					{
					}
					else
					{
						//HttpStatusCode httpStatusCode = GetHttpStatusCode(e.Error);
						//if (httpStatusCode == HttpStatusCode.ServiceUnavailable)
						//{
						//	SetHeading(Language.GetTextValue("tModLoader.MenuModBrowser") + " " + Language.GetTextValue("tModLoader.MBOfflineWithReason", Language.GetTextValue("tModLoader.MBBusy")));
						//}
						//else
						//{
						//	SetHeading(Language.GetTextValue("tModLoader.MenuModBrowser") + " " + Language.GetTextValue("tModLoader.MBOfflineWithReason", Language.GetTextValue("tModLoader.MBUnknown")));
						//}
					}
					loading.SetValue(false);
					reloadButton.SetText(Language.GetTextValue("tModLoader.MBReloadBrowser"));
				}
				else if (!e.Cancelled)
				{
					reloadButton.SetText(Language.GetTextValue("tModLoader.MBPopulatingBrowser"));
					Stream result = e.Result;
					var sr = new StreamReader(result);
					string response = sr.ReadToEnd();
					if (SynchronizationContext.Current == null)
						SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
					Task.Factory
						.StartNew(() =>
						{
							var modloader = Traverse.Create(typeof(ModLoader).FullName);
							//var findModResult = modloader.Method("FindMods").GetValue();
							//Logger.Log(findModResult);
							return;
						})
						.ContinueWith(task =>
						{
							var populateFromJSON = typeof(Mod).Module.GetType("Terraria.ModLoader.UI.UIModBrowser")
								.GetMethod("PopulateFromJSON", BindingFlags.NonPublic | BindingFlags.Instance);
							populateFromJSON.Invoke(modBrowser, BindingFlags.Default, null, 
								new object[]{ null, response}, CultureInfo.CurrentCulture);
							loading.SetValue(false);
							reloadButton.SetText(Language.GetTextValue("tModLoader.MBReloadBrowser"));
						}, TaskScheduler.FromCurrentSynchronizationContext());
				}
			}

			public static void OpenRead(WebClient client, Uri uri)
			{
				client.OpenReadAsync(uri);
			}

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
				addButton.Add(new CodeInstruction(OpCodes.Call, typeof(Interface).GetMethod("AddButtons", BindingFlags.Public | BindingFlags.Static)));

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
				menus.Add(new CodeInstruction(OpCodes.Call, typeof(Interface).GetMethod("LocalizerMenus", BindingFlags.Public | BindingFlags.Static)));
				menus.Add(new CodeInstruction(OpCodes.Brtrue, result[result.Count-15].operand));

				return menus.Concat(result);
			}
		}
	}
}
