using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class IgnoreBakingAttribute : Attribute
    {
    }
}
