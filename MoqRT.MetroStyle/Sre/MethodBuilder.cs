using Castle.DynamicProxy.Generators.Emitters;
using MoqRT.Reflection;

namespace System.Reflection.Emit
{
    public class MethodBuilder : MethodBaseBuilder, IMethodInfo
    {
        public Type ReturnType
        {
            get { throw new NotImplementedException(); }
        }

        public bool ContainsGenericParameters
        {
            get { throw new NotImplementedException(); }
        }

        public IMethodInfo GetGenericMethodDefinition()
        {
            throw new NotImplementedException();
        }

        public IMethodInfo MakeGenericMethod(Type[] type)
        {
            throw new NotImplementedException();
        }

        public bool IsGetType()
        {
            throw new NotImplementedException();
        }

        public bool IsMemberwiseClone()
        {
            throw new NotImplementedException();
        }

        public bool IsFinalizer()
        {
            throw new NotImplementedException();
        }

        public Type[] GetGenericArguments()
        {
            throw new NotImplementedException();
        }

        public ParameterInfo ReturnParameter
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsPropertySetter()
        {
            throw new NotImplementedException();
        }

        public bool IsPropertyGetter()
        {
            throw new NotImplementedException();
        }

        public bool IsDestructor()
        {
            throw new NotImplementedException();
        }

        public bool IsEventAttach()
        {
            throw new NotImplementedException();
        }

        public bool IsEventDetach()
        {
            throw new NotImplementedException();
        }

        public bool IsAccessible()
        {
            throw new NotImplementedException();
        }

        public IMethodInfo GetBaseDefinition()
        {
            throw new NotImplementedException();
        }

        internal void SetReturnType(Type returnType)
        {
            throw new NotImplementedException();
        }

        internal void SetSignature(Type returnType, object p1, object p2, Type[] parameters, object p3, object p4)
        {
            throw new NotImplementedException();
        }

        internal IMethodInfo AsIMethodInfo()
        {
            return this;
        }

        internal ApplyGenArgs DefineGenericParameters
        {
            get
            {
                throw new NotImplementedException("This operation has not been implemented.");
            }
        }
    }
}
