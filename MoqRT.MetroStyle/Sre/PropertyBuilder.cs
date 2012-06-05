using MoqRT.Reflection;

namespace System.Reflection.Emit
{
    public class PropertyBuilder : MemberBuilder, IPropertyInfo
    {
        internal PropertyBuilder(object inner)
            : base(inner)
        {
        }

        public bool CanWrite
        {
            get
            {
                return this.GetProperty<bool>();
            }
        }

        public bool CanRead
        {
            get
            {
                return this.GetProperty<bool>();
            }
        }

        public IMethodInfo GetGetMethod(bool nonPublic)
        {
            throw new NotImplementedException();
        }

        public IMethodInfo GetSetMethod(bool nonPublic)
        {
            throw new NotImplementedException();
        }

        public ParameterInfo[] GetIndexParameters()
        {
            throw new NotImplementedException();
        }

        public Type PropertyType
        {
            get
            {
                return this.GetProperty<Type>();
            }
        }

        public object GetValue(object obj, params object[] index)
        {
            throw new NotImplementedException();
        }

        internal void SetSetMethod(MethodBuilder method)
        {
            this.Invoke("SetSetMethod", method.Inner);
        }

        internal void SetGetMethod(MethodBuilder method)
        {
            this.Invoke("SetGetMethod", method.Inner);
        }
    }
}
