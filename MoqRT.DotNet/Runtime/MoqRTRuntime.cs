using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq.Proxy;

namespace MoqRT
{
    // .NET shim for baking - no-ops...
    internal static class MoqRTRuntime
    {
        internal static BakingController Baker { get; set; }

        internal static bool IsBaking
        {
            get
            {
                // this should always return null - in .NET we're never baking...
                return Baker != null;
            }
        }

        internal static bool UsingBakedProxies
        {
            get
            {
                // this is always false in .NET as we're either running in "Old Moq" mode,
                // or we're baking...
                return false;
            }
        }

        internal static object GetBakedProxy(string key, ProxyInterceptor interceptor, object[] args)
        {
            throw new NotImplementedException();
        }
    }
}
