using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq.Reflection.Emit;

namespace System.Reflection.Emit
{
    public class Label : EmitWrapper
    {
        internal Label(object inner)
            : base(inner)
        {
        }
    }
}
