using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Terraria;

namespace Localizer
{

	public class Configuration
	{
		public static string ConfigPath = Path.Combine(Main.SavePath, "LocalizerConfig.json");

		public bool EnableModAutoUpdate = true;
		public bool EnableTextAutoUpdate = true;
		public bool CompatibleMode = false;

		public static Configuration Read()
		{
			// TODO: Change to true path
			if (!File.Exists(ConfigPath))
			{
				var config =  new Configuration();

				config.Write();

				return config;
			}

			using (var fs = new FileStream(ConfigPath, FileMode.Open))
			{
				using (var sr = new StreamReader(fs))
				{
					return JsonConvert.DeserializeObject<Configuration>(sr.ReadToEnd());
				}
			}
		}

		public void Write()
		{
			using (var fs = new FileStream(ConfigPath, FileMode.Create))
			{
				using (var sw = new StreamWriter(fs))
				{
					sw.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
				}
			}
		}
	}
}
