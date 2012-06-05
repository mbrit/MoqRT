using MoqRT.Reflection;

namespace System.Reflection.Emit
{
    public class ConstructorBuilder : MethodBuilder, IConstructorInfo
    {
        internal ConstructorBuilder(object inner)
            : base(inner)
        {
        }
    }
}
