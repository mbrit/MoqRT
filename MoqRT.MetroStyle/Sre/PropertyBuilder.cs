using MoqRT.Reflection;

namespace System.Reflection.Emit
{
    public class PropertyBuilder : MemberBuilder, IPropertyInfo
    {
        public bool CanWrite
        {
            get { throw new NotImplementedException(); }
        }

        public bool CanRead
        {
            get { throw new NotImplementedException(); }
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
            get { throw new NotImplementedException(); }
        }

        public object GetValue(object obj, params object[] index)
        {
            throw new NotImplementedException();
        }

        internal void SetSetMethod(MethodBuilder methodBuilder)
        {
            throw new NotImplementedException();
        }

        internal void SetGetMethod(MethodBuilder methodBuilder)
        {
            throw new NotImplementedException();
        }
    }
}
