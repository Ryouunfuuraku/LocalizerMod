using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace Localizer
{
	public class Logger
	{
		public static string LogPath = Path.Combine(Main.SavePath, "Logs/Logs.md");
		public static string TextUpdateLogPath = Path.Combine(Main.SavePath, "Logs/TextUpdateLogs.md");

		public static void Log(object content)
		{
			Log(content.ToString());
		}

		public static void Log(string content)
		{
			var sb = new StringBuilder();
			sb.AppendFormat("#### {0}  \n---  \n{1}  \n", DateTime.Now, content);
			File.AppendAllText(LogPath, sb.ToString());
		}

		public static void DebugLog(object content)
		{
#if DEBUG
			Log(content);
#endif
		}

		public static void TextUpdateLog(object content)
		{
			var sb = new StringBuilder();
			sb.AppendFormat("#### {0}  \n---  \n{1}  \n", DateTime.Now, content);
			File.AppendAllText(TextUpdateLogPath, sb.ToString());
		}
	}
}
