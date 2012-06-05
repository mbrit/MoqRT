using Moq.Reflection.Emit;

namespace System.Reflection.Emit
{
    public class GenericTypeParameterBuilder : EmitWrapper
    {
        internal GenericTypeParameterBuilder(object inner)
            : base(inner)
        {
        }

        public string Name
        {
            get
            {
                return this.GetProperty<string>();
            }
        }

        internal Type MakeArrayType()
        {
            return (Type)this.Invoke("MakeArrayType", new Type[] { }, new object[] { });
        }

        internal Type MakeArrayType(int rank)
        {
            return (Type)this.Invoke("MakeArrayType", new Type[] { typeof(int) }, new object[] { rank });
        }

        internal void SetGenericParameterAttributes(GenericParameterAttributes attributes)
        {
            this.Invoke("SetGenericParameterAttributes", attributes);
        }

        internal void SetInterfaceConstraints(Type[] constraints)
        {
            this.Invoke("SetInterfaceConstraints", new object[] { constraints });
        }

        internal void SetCustomAttribute(CustomAttributeBuilder attribute)
        {
            this.Invoke("SetCustomAttribute", attribute.Inner);
        }
    }
}
