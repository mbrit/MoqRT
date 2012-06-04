using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Reflection.Emit
{
    public abstract class BuilderBase
    {
        internal void SetCustomAttribute(CustomAttributeBuilder builder)
        {
            throw new NotImplementedException();
        }

        internal ILGenerator GetILGenerator()
        {
            throw new NotImplementedException();
        }
    }
}
