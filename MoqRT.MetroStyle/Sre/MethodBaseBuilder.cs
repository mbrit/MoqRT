using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoqRT.Reflection;

namespace System.Reflection.Emit
{
    public abstract class MethodBaseBuilder : MemberBuilder, IMethodBase
    {
        private MethodImplAttributes Flags { get; set; }

        protected MethodBaseBuilder(object inner)
            : base(inner)
        {
        }

        public void SetImplementationFlags(MethodImplAttributes attrs)
        {
            this.Flags = attrs;
        }

        internal MethodImplAttributes GetMethodImplementationFlags()
        {
            return this.Flags;
        }

        public MethodImplAttributes GetImplementationFlags()
        {
            return this.Flags;
        }

        internal ParameterBuilder DefineParameter(int position, ParameterAttributes parameterAttributes, string name)
        {
            var result = this.Invoke("DefineParameter", new Type[] { typeof(int), typeof(ParameterAttributes), 
                typeof(string) }, position, parameterAttributes, name);
            return new ParameterBuilder(result);
        }

        internal void SetParameters(params Type[] paramTypes)
        {
            this.Invoke("SetParameters", new Type[] { typeof(Type[]) }, new object[] { paramTypes });
        }

        public ParameterInfo[] GetParameters()
        {
            throw new NotImplementedException();
        }

        public bool IsAbstract
        {
            get
            {
                return this.GetProperty<bool>();
            }
        }

        public bool IsGenericMethod
        {
            get
            {
                return this.GetProperty<bool>();
            }
        }

        public bool IsGenericMethodDefinition
        {
            get
            {
                return this.GetProperty<bool>();
            }
        }

        public bool IsFinal
        {
            get
            {
                return this.GetProperty<bool>();
            }
        }

        public bool IsPrivate
        {
            get
            {
                return this.GetProperty<bool>();
            }
        }

        public bool IsPublic
        {
            get
            {
                return this.GetProperty<bool>();
            }
        }

        public bool IsVirtual
        {
            get
            {
                return this.GetProperty<bool>();
            }
        }

        public bool IsFamilyAndAssembly
        {
            get
            {
                return this.GetProperty<bool>();
            }
        }

        public bool IsHideBySig
        {
            get
            {
                return this.GetProperty<bool>();
            }
        }
    }
}
