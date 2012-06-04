using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Reflection
{
    [Serializable]
    internal class MethodInfoWrapper : MethodBaseReflectionWrapper<MethodInfo>, IMethodInfo
    {
        // only create via a factory! Castle depends on single instances of wrappers...
        internal MethodInfoWrapper(MethodInfo inner)
            : base(inner)
        {
        }

        public Type ReturnType
        {
            get
            {
                return this.Inner.ReturnType;
            }
        }

        public bool ContainsGenericParameters
        {
            get
            {
                return this.Inner.ContainsGenericParameters;
            }
        }

        public IMethodInfo GetGenericMethodDefinition()
        {
            return this.Inner.GetGenericMethodDefinition().AsIMethodInfo();
        }

        public IMethodInfo MakeGenericMethod(Type[] type)
        {
            return this.Inner.MakeGenericMethod(type).AsIMethodInfo();
        }

        public Type[] GetGenericArguments()
        {
            return this.Inner.GetGenericArguments();
        }

        public ParameterInfo ReturnParameter
        {
            get
            {
                if (this.Inner.ReturnParameter != null)
                    return this.Inner.ReturnParameter;
                else
                    return null;
            }
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
            throw new NotImplementedException("This operation has not been implemented.");
        }

        public IMethodInfo GetBaseDefinition()
        {
            throw new NotImplementedException();
        }
    }
}
