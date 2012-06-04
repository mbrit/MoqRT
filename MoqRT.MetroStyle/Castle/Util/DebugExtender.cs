using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Diagnostics
{
	internal static class DebugExtender
	{
		internal static void Assert(bool ok, string shortMessage, string longMessage)
		{
			Debug.Assert(ok, shortMessage);
		}
	}
}
