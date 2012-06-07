using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Baking
{
    internal class BakingContext : ILog
    {
        internal BakingController Owner { get; private set; }
        internal Type RuntimeType { get; private set; }
        internal Assembly TestAssembly { get; private set; }

        internal BakingContext(BakingController owner, Type runtimeType, Assembly testAssembly)
        {
            this.Owner = owner;
            this.RuntimeType = runtimeType;
            this.TestAssembly = testAssembly;
        }

        public void Log(string message)
        {
            this.Owner.Log(message);
        }

        public void Log(string message, Exception ex)
        {
            this.Owner.Log(message, ex);
        }
    }
}
