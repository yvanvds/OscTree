using shortid;
using System;
using System.Collections.Generic;
using System.Text;

namespace OscTree
{
	internal static class Utils
	{
		internal static string GenerateID()
		{
			return ShortId.Generate(true);
		}
	}
}
