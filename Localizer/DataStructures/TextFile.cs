using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Localizer.DataStructures
{
	public class TextFile
	{
		public sealed class TranslationInfo
		{
			public string Mod { get; set; }
			public string Translator { get; set; }
			public string Description { get; set; }
			private string _culture;
			[JsonIgnore]
			public GameCulture Culture {
				get
				{
					return GameCulture.FromName(_culture);
				}
			}

			public TranslationInfo() { }

			public TranslationInfo(Mod mod, GameCulture culture)
			{
				Mod = mod.Name;
				Translator = string.Empty;
				Description = string.Empty;
				_culture = culture.Name;
			}
		}

		public sealed class MiscFile
		{
			public Dictionary<string, MiscTranslation> Miscs { get; set; }

			public MiscFile()
			{
				Miscs = new Dictionary<string, MiscTranslation>();
			}
		}
		
		public sealed class MiscTranslation
		{
			public string Default { get; set; }
			public string Translation { get; set; }

			public MiscTranslation() { }

			public MiscTranslation(string str)
			{
				Translation = string.Empty;

				Default = str;
			}
		}

		#region Item
		public sealed class ItemFile
		{
			public Dictionary<string, ItemTranslation> Items { get; set; }
			public Dictionary<string, SetBonusTranslation> SetBonus { get; set; }
			public Dictionary<string, List<string>> ModifyTooltips { get; set; }

			public ItemFile()
			{
				Items = new Dictionary<string, ItemTranslation>();
				SetBonus = new Dictionary<string, SetBonusTranslation>();
				ModifyTooltips = new Dictionary<string, List<string>>();
			}
		}

		public sealed class ItemTranslation
		{
			public string Name { get; set; }
			public string NameTranslation { get; set; }
			public string Tooltip { get; set; }
			public string TooltipTranslation { get; set; }

			public ItemTranslation() { }

			public ItemTranslation(ModItem item)
			{
				NameTranslation = string.Empty;
				TooltipTranslation = string.Empty;

				Name = item.DisplayName.GetDefault();
				Tooltip = item.Tooltip.GetDefault();
			}
		}

		public sealed class SetBonusTranslation
		{
			public string SetBonus { get; set; }
			public string Translation { get; set; }

			public SetBonusTranslation() { }

			public SetBonusTranslation(string str)
			{
				SetBonus = str;
				Translation = string.Empty;
			}
		}
		#endregion


		#region NPC
		public sealed class NPCFile
		{
			public Dictionary<string, NPCTranslation> NPCs { get; set; }
			public Dictionary<string, List<ChatLineTranslation>> ChatLines { get; set; }
			public Dictionary<string, List<ChatButtonTranslation>> ChatButtons { get; set; }

			public NPCFile()
			{
				NPCs = new Dictionary<string, NPCTranslation>();
				ChatLines = new Dictionary<string, List<ChatLineTranslation>>();
				ChatButtons = new Dictionary<string, List<ChatButtonTranslation>>();
			}
		}

		public sealed class NPCTranslation
		{
			public string Name { get; set; }
			public string NameTranslation { get; set; }

			public NPCTranslation() { }

			public NPCTranslation(ModNPC npc)
			{
				NameTranslation = string.Empty;

				Name = npc.DisplayName.GetDefault();
			}
		}

		public sealed class ChatLineTranslation
		{
			public string ChatLine { get; set; }
			public string Translation { get; set; }

			public ChatLineTranslation() { }

			public ChatLineTranslation(string str)
			{
				ChatLine = str;
				Translation = string.Empty;
			}
		}

		public sealed class ChatButtonTranslation
		{
			public string ChatButton { get; set; }
			public string Translation { get; set; }

			public ChatButtonTranslation() { }

			public ChatButtonTranslation(string str)
			{
				ChatButton = str;
				Translation = string.Empty;
			}
		}
		#endregion


		#region Buff
		public sealed class BuffFile
		{
			public Dictionary<string, BuffTranslation> Buffs { get; set; }

			public BuffFile()
			{
				Buffs = new Dictionary<string, BuffTranslation>();
			}
		}

		public sealed class BuffTranslation
		{
			public string Name { get; set; }
			public string NameTranslation { get; set; }
			public string Tip { get; set; }
			public string TipTranslation { get; set; }

			public BuffTranslation() { }

			public BuffTranslation(ModBuff buff)
			{
				NameTranslation = string.Empty;
				TipTranslation = string.Empty;

				Name = buff.DisplayName.GetDefault();
				Tip = buff.Description.GetDefault();
			}
		}
		#endregion
	}
}
