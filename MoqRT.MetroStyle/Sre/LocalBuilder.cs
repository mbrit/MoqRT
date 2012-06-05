using Moq.Reflection.Emit;

namespace System.Reflection.Emit
{
    public class LocalBuilder : EmitWrapper
    {
        internal LocalBuilder(object inner)
            : base(inner)
        {
        }
    }
}
