using MoqRT.Reflection;

namespace System.Reflection.Emit
{
    public class FieldBuilder : MemberBuilder, IFieldInfo
    {
        internal FieldBuilder(object inner)
            : base(inner)
        {
        }

        public Type FieldType
        {
            get { return GetProperty<Type>(); }
        }

        public FieldAttributes Attributes
        {
            get { return GetProperty<FieldAttributes>(); }
        }

        internal FieldInfo ToFieldInfo()
        {
            return (FieldInfo)this.Inner;
        }
    }
}
