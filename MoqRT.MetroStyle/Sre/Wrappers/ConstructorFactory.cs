using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Reflection
{
    internal static class ConstructorFactory
    {
        internal static IConstructorInfo GetConstructor(Type invocationType, IConstructorInfo constructor)
        {
            return TypeBuilder.GetConstructor(invocationType, constructor.AsConstructorInfo()).AsIConstructorInfo();
        }
    }
}
