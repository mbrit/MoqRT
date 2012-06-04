using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moq.Driver
{
    internal class RealMockWrapper<T> : RealWrapper
        where T : class
    {
        internal RealMockWrapper(object real)
            : base(real)
        {
        }
    }
}
