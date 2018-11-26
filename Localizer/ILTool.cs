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
		public static string GetStrBeforeInstruction(List<ILInstruction> instructions, ILInstruction il)
		{
			var ldstr = instructions.Find(i => i.opcode == OpCodes.Ldstr && i.offset < il.offset);

			if (ldstr != null)
				return ldstr.operand.ToString();
			else
				return null;
		}

		public static List<ILInstruction> GetInstructions(MethodInfo method)
		{
			var dummy = new DynamicMethod("Dummy", typeof(void), new Type[] { });
			return MethodBodyReader.GetInstructions(dummy.GetILGenerator(), method);
		}
	}
}
