using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Localizer
{
	public class CommonTools
	{
		
		public static T LoadJson<T>(string path)
		{
			using (var fs = new FileStream(path, FileMode.Open))
			{
				using (var sr = new StreamReader(fs))
				{
					return JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
				}
			}
		}

		public static void DumpJson(string path, object o)
		{
			using (var fs = new FileStream(path, FileMode.Create))
			{
				using (var sw = new StreamWriter(fs))
				{
					sw.Write(JsonConvert.SerializeObject(o, Formatting.Indented));
				}
			}
		}
	}
}
