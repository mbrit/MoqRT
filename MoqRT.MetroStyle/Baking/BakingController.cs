using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq.Proxy;

namespace MoqRT.Baking
{
    internal class BakingController : IDisposable
    {
        public void Dispose()
        {
            var genereator = CastleProxyFactory.GetPersistentBuilder();
            genereator.SaveAssembly();
        }
    }
}
