using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Reflection
{
    [Serializable]
    internal abstract class ReflectionWrapper<T>
        where T : class
    {
        internal T Inner { get; private set; }

        protected ReflectionWrapper(T inner)
        {
            this.Inner = inner;
        }

        public string Key
        {
            get
            {
                return this.Inner.GetHashCode().ToString();
            }
        }
    }
}
