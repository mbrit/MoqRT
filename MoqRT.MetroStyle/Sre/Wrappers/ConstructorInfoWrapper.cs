using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Reflection
{
    [Serializable]
    internal class ConstructorInfoWrapper : MethodBaseReflectionWrapper<ConstructorInfo>, IConstructorInfo
    {
        // only create via a factory! Castle depends on single instances of wrappers...
        internal ConstructorInfoWrapper(ConstructorInfo inner)
            : base(inner)
        {
        }
    }
}
