﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Localizer.DataStructures;
using Newtonsoft.Json;

namespace Localizer
{
	public class MachineTranslator
	{
		public static readonly string APIAddress = "http://api.fanyi.baidu.com/api/trans/vip/translate";
		public static readonly string AppID = "20181126000239553";
		public static readonly string Password = "1nnrCAk4cQeScmXD1jES";
		public static readonly string Salt = "123";
		public static readonly string RequestTemplate = "{0}?q={1}&from={2}&to={3}&appid={4}&salt={5}&sign={6}";

		private MD5 _md5;

		public MachineTranslator()
		{
			_md5 = MD5.Create();
		}

		public string Query(string raw, string from = "en", string to = "zh")
		{
			try
			{
				var sb = new StringBuilder();
				sb.Append(AppID).Append(raw).Append(Salt).Append(Password);
				var hashedDataBytes = _md5.ComputeHash(Encoding.GetEncoding("UTF-8").GetBytes(sb.ToString()));

				sb.Clear();
				foreach (byte i in hashedDataBytes)
				{
					sb.Append(i.ToString("x2"));
				}

				var sign = sb.ToString();

				var uri = string.Format(RequestTemplate, APIAddress, Uri.EscapeDataString(raw), from, to, AppID, Salt, sign);
				Logger.DebugLog(uri);
				var request = (HttpWebRequest) WebRequest.Create(uri);
				var response = (HttpWebResponse) request.GetResponse();
				var resText = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
				var result = JsonConvert.DeserializeObject<Result>(resText);

				sb.Clear();
				for (int i = 0; i < result.trans_result.Count; i++)
				{
					sb.Append(result.trans_result[i].dst);
					if(i < result.trans_result.Count - 1)
						sb.Append("\n");
				}
				return sb.ToString();
			}
			catch (Exception ex)
			{
				Logger.Log(ex);
			}

			return "";
		}

		public void TranslateFromPath(string path)
		{
			if (!Directory.Exists(path))
				return;
			Logger.DebugLog("Start to translate.");

			var items = CommonTools.LoadJson<TextFile.ItemFile>(Path.Combine(path, "Items.json"));
			TranslateItems(items);
			CommonTools.DumpJson(Path.Combine(path, "Items.json"), items);
			var npcs = CommonTools.LoadJson<TextFile.NPCFile>(Path.Combine(path, "NPCs.json"));
			TranslateNPCs(npcs);
			CommonTools.DumpJson(Path.Combine(path, "NPCs.json"), npcs);
			var buffs = CommonTools.LoadJson<TextFile.BuffFile>(Path.Combine(path, "Buffs.json"));
			TranslateBuffs(buffs);
			CommonTools.DumpJson(Path.Combine(path, "Buffs.json"), buffs);
			var miscs = CommonTools.LoadJson<TextFile.MiscFile>(Path.Combine(path, "Miscs.json"));
			TranslateMiscs(miscs);
			CommonTools.DumpJson(Path.Combine(path, "Miscs.json"), miscs);
		}

		static Regex reUnicode = new Regex(@"\\u([0-9a-fA-F]{4})", RegexOptions.Compiled);
		public static string UrlDecode(string s)
		{
			return reUnicode.Replace(s, m =>
			{
				short c;
				if (short.TryParse(m.Groups[1].Value, NumberStyles.HexNumber,
					CultureInfo.InvariantCulture, out c))
				{
					return "" + (char) c;
				}

				return m.Value;
			});
		}
		
		public void TranslateItems(TextFile.ItemFile items)
		{
			foreach (var item in items.Items.Values)
			{
				Logger.DebugLog(string.Format("Translating item: {0}", item.Name));
				item.NameTranslation = Query(item.Name);
				item.TooltipTranslation = Query(item.Tooltip);
			}

			foreach (var setbonus in items.SetBonus.Values)
			{
				Logger.DebugLog(string.Format("Translating setbonus: {0}", setbonus.SetBonus));
				setbonus.Translation = Query(setbonus.SetBonus);
			}
		}

		public void TranslateNPCs(TextFile.NPCFile npcs)
		{
			foreach (var npc in npcs.NPCs.Values)
			{
				Logger.DebugLog(string.Format("Translating npc: {0}", npc.Name));
				npc.NameTranslation = Query(npc.Name);
			}
		}

		public void TranslateBuffs(TextFile.BuffFile buffs)
		{
			foreach (var buff in buffs.Buffs.Values)
			{
				Logger.DebugLog(string.Format("Translating buff: {0}", buff.Name));
				buff.NameTranslation = Query(buff.Name);
				buff.TipTranslation = Query(buff.Tip);
			}
		}

		public void TranslateMiscs(TextFile.MiscFile miscs)
		{
			foreach (var misc in miscs.Miscs.Values)
			{
				Logger.DebugLog(string.Format("Translating misc: {0}", misc.Default));
				misc.Translation = Query(misc.Default);
			}
		}

		internal sealed class Result
		{
			public string from;
			public string to;
			public List<TransResult> trans_result;
		}

		internal sealed class TransResult
		{
			public string src;
			public string dst;
		}
	}
}
