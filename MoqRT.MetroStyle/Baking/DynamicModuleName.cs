using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Moq.Baking
{
    public class DynamicModuleName
    {
        private long Index { get; set; }
        internal string StrongAssembly { get; private set; }
        internal string StrongPath { get; private set; }
        internal string WeakAssembly { get; private set; }
        internal string WeakPath { get; private set; }

        private static long _counter;

        internal DynamicModuleName()
        {
            this.Index = Interlocked.Increment(ref _counter);

            this.StrongAssembly = string.Format("MoqRT-Baked-Strong-{0}.dll", this.Index);
            this.WeakAssembly = string.Format("MoqRT-Baked-Strong-{0}.dll", this.Index);

            this.StrongPath = StrongAssembly;
            this.WeakPath = WeakAssembly;
        }
    }
}
