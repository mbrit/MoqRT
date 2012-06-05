using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class Trace
    {
        internal static void Assert(bool p)
        {
            Assert(p, "Failed.");
        }

        internal static void Assert(bool p, string message)
        {
            if (!(p))
                throw new InvalidOperationException("Assertion failed: " + message);
        }
    }
}
