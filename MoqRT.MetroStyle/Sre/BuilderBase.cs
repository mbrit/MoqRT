using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq.Reflection.Emit;

namespace System.Reflection.Emit
{
    public abstract class BuilderBase : EmitWrapper
    {
        protected BuilderBase(object inner)
            : base(inner)
        {
        }

        internal void SetCustomAttribute(CustomAttributeBuilder builder)
        {
            throw new NotImplementedException();
        }

        internal ILGenerator GetILGenerator()
        {
            var result = this.Invoke();
            return new ILGenerator(result);
        }
    }
}
