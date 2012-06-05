using System.Collections;
using System.Collections.Generic;
using Castle.DynamicProxy.Generators.Emitters;
using MoqRT.Reflection;

namespace System.Reflection.Emit
{
    public class MethodBuilder : MethodBaseBuilder, IMethodInfo, IMethodInfoOwner
    {
        internal MethodBuilder(object inner)
            : base(inner)
        {
        }

        public Type ReturnType
        {
            get
            {
                return this.GetProperty<Type>();
            }
        }

        public bool ContainsGenericParameters
        {
            get
            {
                return this.GetProperty<bool>();
            }
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
            throw new NotImplementedException("This operation has not been implemented.");
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
            this.Invoke("SetReturnType", returnType);
        }

        internal void SetSignature(Type returnType, Type[] rtrcm, Type[] rtocm, Type[] parameterTypes, 
            Type[][] ptrcm, Type[][] ptocm)
        {
            //this.Invoke("SetSignature", new Type[] { typeof(Type), typeof(Type[]), typeof(Type[]), typeof(Type[][]), 
            //    typeof(Type[][]) }, returnType, rtrcm, rtocm, parameterTypes, ptrcm, ptocm);
            this.Invoke("SetSignature", returnType, rtrcm, rtocm, parameterTypes, ptrcm, ptocm);
        }

        internal IMethodInfo AsIMethodInfo()
        {
            return this;
        }

        internal ApplyGenArgs DefineGenericParameters
        {
            get
            {
                return new ApplyGenArgs((names) =>
                {
                    var results = this.Invoke("DefineGenericParameters", new object[] { names });

                    // copy...
                    var mapped = new List<GenericTypeParameterBuilder>();
                    foreach (var result in (IEnumerable)results)
                        mapped.Add(new GenericTypeParameterBuilder(result));
                    return mapped.ToArray();
                });
            }
        }

        MethodInfo IMethodInfoOwner.MethodInfo
        {
            get
            {
                return (MethodInfo)this.Inner;
            }
        }
    }
}
