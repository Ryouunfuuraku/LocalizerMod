using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using Harmony.ILCopying;
using Newtonsoft.Json;
using Terraria;
using Terraria.ModLoader;
using Localizer.DataStructures;

namespace Localizer
{
	public class ILHelper
	{
		/// <summary>
		/// Find ldstr instruction before the target and return it's operand as string
		/// </summary>
		/// <param name="instructions"></param>
		/// <param name="il"></param>
		/// <returns></returns>
		public static string GetStrBeforeInstruction(List<ILInstruction> instructions, ILInstruction il)
		{
			var ldstr = instructions.Find(i => i.opcode == OpCodes.Ldstr && i.offset < il.offset);

			if (ldstr != null)
				return ldstr.operand.ToString();
			else
				return null;
		}
	}
}
