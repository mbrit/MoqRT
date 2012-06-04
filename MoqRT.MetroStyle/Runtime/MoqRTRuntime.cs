using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Moq.Baking;
using SQLite;

namespace Moq
{
    public static class MoqRTRuntime
    {
        internal static BakingController Baker { get; set; }

        internal static bool IsBaking
        {
            get
            {
                return !(Baker == null);
            }
        }
    }
}
