using Moq.Reflection.Emit;
using MoqRT;
using MoqRT.Reflection;

namespace System.Reflection.Emit
{
    public class CustomAttributeBuilder : EmitWrapper
    {
        public CustomAttributeBuilder(ConstructorInfo ctor, params object[] ctorArgs)
            : this(ctor.AsIConstructorInfo(), ctorArgs)
        {
        }

        public CustomAttributeBuilder(IConstructorInfo ctor, params object[] ctorArgs)
            : base(null)
        {
        }

        public CustomAttributeBuilder(ConstructorInfo ctor, object[] ctorArgs, IPropertyInfo[] properties, 
            object[] propertyValues, IFieldInfo[] fields, object[] fieldValues)
            : this(ctor, properties, propertyValues, fields, fieldValues)
        {
        }

        public CustomAttributeBuilder(IConstructorInfo ctor, object[] ctorArgs, IPropertyInfo[] properties, 
            object[] propertyValues, IFieldInfo[] fields, object[] fieldValues)
            : base(null)
        {
        }
    }
}
