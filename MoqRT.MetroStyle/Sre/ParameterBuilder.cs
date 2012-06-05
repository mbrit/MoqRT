using Moq.Reflection.Emit;

namespace System.Reflection.Emit
{
    public class ParameterBuilder : EmitWrapper
    {
        internal ParameterBuilder(object inner)
            : base(inner)
        {
        }

        internal void SetCustomAttribute(CustomAttributeBuilder attribute)
        {
            this.Invoke("SetCustomAttribute", attribute.Inner);
        }
    }
}
