using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Baking
{
    [Serializable]
    public class TestMethod : TestItem
    {
        internal TestMethod(MethodInfo method)
            : base(method.Name)
        {
        }
    }
}
