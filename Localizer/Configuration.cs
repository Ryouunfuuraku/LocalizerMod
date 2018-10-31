using Systempath;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Localizer
{
	public class Configuration
	{
		public static string Path = "LocalizerConfig.json";

		public static Configuration Read()
		{
			// TODO: Change to true path
			if (!File.Exists(Path))
			{
				var config =  new Configuration();

				config.Write();

				return config;
			}

			using (var fs = new FileStream(Path, FileMode.Open))
			{
				using (var sr = new StreamReader(fs))
				{
					return JsonConvert.DeserializeObject<Configuration>(sr.ReadToEnd());
				}
			}
		}

		public void Write()
		{
			using (var fs = new FileStream(Path, FileMode.Create))
			{
				using (var sw = new StreamWriter(fs))
				{
					sw.Write(JsonConvert.SerializeObject(this));
				}
			}
		}
	}
}
