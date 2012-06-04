using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moq.Driver
{
    internal class RealISetupWrapper<TMock, TResult> : RealWrapper
    {
        internal RealISetupWrapper(object real)
            : base(real)
        {
        }
    }
}
