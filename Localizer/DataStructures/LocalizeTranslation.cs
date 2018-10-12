using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;

namespace Localizer.DataStructures
{
	public class LocalizeTranslation
	{
		public const int FALLBACK = 1;

		public Dictionary<int, string> translations;

		public LocalizeTranslation(GameCulture culture, string translation)
		{
			if(translations == null)
				translations = new Dictionary<int, string>();

			AddTranslation(culture, translation);
		}

		public void AddTranslation(GameCulture culture, string translation)
		{
			translations.Add(culture.LegacyId, translation);
		}
		
		public string GetTranslation()
		{
			return GetTranslation(LanguageManager.Instance.ActiveCulture);
		}

		public string GetTranslation(GameCulture culture)
		{
			return GetTranslation(culture.LegacyId);
		}

		public string GetTranslation(int culture)
		{
			if (translations.ContainsKey(culture))
			{
				return translations[culture];
			}
			else
			{
				return translations[FALLBACK];
			}
		}
	}
}
