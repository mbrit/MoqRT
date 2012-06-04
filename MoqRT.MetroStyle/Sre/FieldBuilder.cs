using MoqRT.Reflection;

namespace System.Reflection.Emit
{
    public class FieldBuilder : MemberBuilder, IFieldInfo
    {
        public Type FieldType
        {
            get { throw new NotImplementedException(); }
        }

        public FieldAttributes Attributes
        {
            get { throw new NotImplementedException(); }
        }

        internal FieldInfo ToFieldInfo()
        {
            throw new NotImplementedException();
        }
    }
}
