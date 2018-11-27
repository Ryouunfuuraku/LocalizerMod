using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Localizer.DataStructures;
using Terraria.ModLoader.IO;

namespace Localizer
{
	public class UpdateTool
	{
		public DiffResult<T> UpdateDict<T>(Dictionary<string, T> oldDict, Dictionary<string, T> newDict)
		{
			var result = new DiffResult<T>();
			foreach (var newItem in newDict)
			{
				oldDict.TryGetValue(newItem.Key, out T oldItem);
				if (oldItem != null)
				{
					result.Change.Add(newItem.Key, newItem.Value);
				}
				else
				{
					oldDict.Add(newItem.Key, newItem.Value);
					result.New.Add(newItem.Key, newItem.Value);
				}
			}

			return result;
		}

		#region Log Format
		internal static string NewLine = "---  \n";
		internal static string NewItemLogFormat = 
@"##### New Item: {0}  
**Name:** {1}  
**Tooltip:** {2}  
---  
";
		internal static string ItemChangeLogFormat =
@"##### Item Changed: {0}  
";
		internal static string ItemNameChangeLogFormat =
@"**Name:** {0} => {1}  
";
		internal static string ItemTooltipChangeLogFormat =
@"**Tooltip:** {0} => {1}  
";
		internal static string NewSetBonusLogFormat =
@"##### New SetBonus: {0}  
**Description:** {1}  
---  
";
		internal static string SetBonusChangeLogFormat =
@"##### SetBonus Changed: {0}  
**SetBonus Changed:** {1} => {2}  
---  
";

		internal static string NewNPCLogFormat =
@"##### New NPC: {0}  
**Name:** {1}   
---  
";
		internal static string NPCChangeLogFormat =
@"##### NPC Changed: {0}  
**NPC Changed:** {1} => {2}  
---  
";
		internal static string NewBuffLogFormat =
@"##### New Buff: {0}  
**Name:** {1}   
**Tip:** {2}   
---  
";
		internal static string BuffChangeLogFormat =
@"##### Buff Changed: {0}  
";
		internal static string BuffNameChangeLogFormat =
@"**Name:** {0} => {1}  
";
		internal static string BuffTipChangeLogFormat =
@"**Tip:** {0} => {1}  
";
		internal static string NewMiscLogFormat =
@"##### New Misc: {0}  
**Default:** {1}   
---  
";
		internal static string MiscChangeLogFormat =
@"##### Misc Changed: {0}  
**Misc Changed:** {1} => {2}  
---  
";
		#endregion
		public void UpdateItemsText(TextFile.ItemFile oldFile, TextFile.ItemFile newFile)
		{
			var sb = new StringBuilder();

			// Update items
			var itemResult = UpdateDict(oldFile.Items, newFile.Items);
			// New items
			foreach (var item in itemResult.New)
			{
				sb.AppendFormat(NewItemLogFormat, item.Key, item.Value.Name, item.Value.Tooltip);
			}
			// Changed items
			foreach (var item in itemResult.Change)
			{
				oldFile.Items.TryGetValue(item.Key, out TextFile.ItemTranslation oldItem);
				if (oldItem != null)
				{
					sb.AppendFormat(ItemChangeLogFormat, item.Key);
					if (oldItem.Name != item.Value.Name)
					{
						sb.AppendFormat(ItemNameChangeLogFormat, oldItem.Name, item.Value.Name);
						oldItem.Name = item.Value.Name;
					}
					if (oldItem.Tooltip != item.Value.Tooltip)
					{
						sb.AppendFormat(ItemTooltipChangeLogFormat, oldItem.Tooltip, item.Value.Tooltip);
						oldItem.Tooltip = item.Value.Tooltip;
					}

					sb.Append(NewLine);
				}
			}

			// Update set bonus
			var setBonusResult = UpdateDict(oldFile.SetBonus, newFile.SetBonus);
			foreach (var setbonus in setBonusResult.New)
			{
				sb.AppendFormat(NewSetBonusLogFormat, setbonus.Key, setbonus.Value.SetBonus);
			}

			foreach (var setbonus in setBonusResult.Change)
			{
				oldFile.SetBonus.TryGetValue(setbonus.Key, out TextFile.SetBonusTranslation old);
				if (old != null && old.SetBonus != setbonus.Value.SetBonus)
				{
					sb.AppendFormat(SetBonusChangeLogFormat, setbonus.Key, old.SetBonus, setbonus.Value.SetBonus);
					old.SetBonus = setbonus.Value.SetBonus;
				}
			}

			Logger.TextUpdateLog(sb.ToString());
		}

		public void UpdateNPCsText(TextFile.NPCFile oldFile, TextFile.NPCFile newFile)
		{
			var sb = new StringBuilder();

			// Update npcs
			var npcResult = UpdateDict(oldFile.NPCs, newFile.NPCs);
			// New npc
			foreach (var npc in npcResult.New)
			{
				sb.AppendFormat(NewNPCLogFormat, npc.Key, npc.Value.Name);
			}
			// Changed npc
			foreach (var npc in npcResult.Change)
			{
				oldFile.NPCs.TryGetValue(npc.Key, out TextFile.NPCTranslation old);
				if (old != null && old.Name != npc.Value.Name)
				{
					sb.AppendFormat(NPCChangeLogFormat, npc.Key, old.Name, npc.Value.Name);
					old.Name = npc.Value.Name;
				}
			}

			Logger.TextUpdateLog(sb.ToString());
		}

		public void UpdateBuffsText(TextFile.BuffFile oldFile, TextFile.BuffFile newFile)
		{
			var sb = new StringBuilder();

			// Update npcs
			var buffResult = UpdateDict(oldFile.Buffs, newFile.Buffs);
			// New buff
			foreach (var buff in buffResult.New)
			{
				sb.AppendFormat(NewBuffLogFormat, buff.Key, buff.Value.Name, buff.Value.Tip);
			}
			// Changed buff
			foreach (var buff in buffResult.Change)
			{
				oldFile.Buffs.TryGetValue(buff.Key, out TextFile.BuffTranslation old);
				if (old != null)
				{
					sb.AppendFormat(BuffChangeLogFormat, buff.Key);
					if (old.Name != buff.Value.Name)
					{
						sb.AppendFormat(BuffNameChangeLogFormat, old.Name, buff.Value.Name);
						old.Name = buff.Value.Name;
					}
					if (old.Tip != buff.Value.Tip)
					{
						sb.AppendFormat(BuffTipChangeLogFormat, old.Tip, buff.Value.Tip);
						old.Tip = buff.Value.Tip;
					}
				}
			}

			Logger.TextUpdateLog(sb.ToString());
		}

		public void UpdateMiscsText(TextFile.MiscFile oldFile, TextFile.MiscFile newFile)
		{
			var sb = new StringBuilder();

			// Update miscs
			var miscResult = UpdateDict(oldFile.Miscs, newFile.Miscs);
			// New misc
			foreach (var misc in miscResult.New)
			{
				sb.AppendFormat(NewMiscLogFormat, misc.Key, misc.Value.Default);
			}
			// Changed misc
			foreach (var misc in miscResult.Change)
			{
				oldFile.Miscs.TryGetValue(misc.Key, out TextFile.MiscTranslation old);
				if (old != null && old.Default != misc.Value.Default)
				{
					sb.AppendFormat(MiscChangeLogFormat, misc.Key, old.Default, misc.Value.Default);
					old.Default = misc.Value.Default;
				}
			}

			Logger.TextUpdateLog(sb.ToString());
		}

		public sealed class DiffResult<T>
		{
			public Dictionary<string, T> New;
			public Dictionary<string, T> Change;
		}
	}
}
