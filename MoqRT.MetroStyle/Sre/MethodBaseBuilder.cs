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

        internal ParameterBuilder DefineParameter(int p1, ParameterAttributes parameterAttributes, string p2)
        {
            throw new NotImplementedException();
        }

        internal void SetParameters(Type[] paramTypes)
        {
            throw new NotImplementedException();
        }

        public ParameterInfo[] GetParameters()
        {
            throw new NotImplementedException();
        }

        public bool IsAbstract
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsGenericMethod
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsGenericMethodDefinition
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsFinal
        {
            get { throw new NotImplementedException(); }
        }

        public new bool IsInternal
        {
            get { throw new NotImplementedException(); }
        }

        public new bool IsFamily
        {
            get { throw new NotImplementedException(); }
        }

        public new bool IsAssembly
        {
            get { throw new NotImplementedException(); }
        }

        public new bool IsFamilyOrAssembly
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsPrivate
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsPublic
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsVirtual
        {
            get { throw new NotImplementedException(); }
        }


        public bool IsFamilyAndAssembly
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsHideBySig
        {
            get { throw new NotImplementedException(); }
        }
    }
}
