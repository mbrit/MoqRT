using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MoqRT.Baking;

namespace MoqRT
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

        public static void InitializeBaking()
        {
            Baker = new BakingController();
        }

        public static void FinishBaking()
        {
            try
            {
                Baker.Dispose();
            }
            finally
            {
                Baker = null;
            }
        }
    }
}
