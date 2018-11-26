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
	}
}
