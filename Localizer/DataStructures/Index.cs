using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localizer.DataStructures
{
	public class Index
	{
		public SubDir zh_hans { get; set; }

		public sealed class SubDir
		{
			public List<string> Items { get; set; }
		}
	}
}
