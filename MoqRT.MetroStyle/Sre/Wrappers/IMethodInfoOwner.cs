using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Reflection
{
    internal interface IMethodInfoOwner
    {
        MethodInfo MethodInfo
        {
            get;
        }
    }
}
