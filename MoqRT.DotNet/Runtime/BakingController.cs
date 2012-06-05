using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace MoqRT
{
    // .NET shim for baking - no-ops...
    internal class BakingController
    {
        internal void ProxyCreated(Mock mock, object value)
        {
            // no-op... - implemented in WinRT, not here...
            throw new NotImplementedException("This operation has not been implemented.");
        }
    }
}
