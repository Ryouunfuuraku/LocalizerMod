using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Localizer
{
	public class LocalizeTile : ModTile
	{
		static FieldInfo typeField;

		public override void SetDefaults()
		{
			typeField = typeof(ModTile).GetField("<Type>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		public void AddMapEntryTranslation(ModTile tile, ModTranslation translation)
		{
			typeField.SetValue(this, tile.Type);

			AddMapEntry(Color.White, translation);
		}
	}
}
