using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Reflection
{
    [Serializable]
    internal abstract class MethodBaseReflectionWrapper<T> : MemberInfoReflectionWrapper<T>, IMethodBase
        where T : MethodBase
    {
        protected MethodBaseReflectionWrapper(T inner)
            : base(inner)
        {
        }

        public bool IsAbstract
        {
            get
            {
                return this.Inner.IsAbstract;
            }
        }

        public bool IsGenericMethod
        {
            get
            {
                return this.Inner.IsGenericMethod;
            }
        }

        public bool IsGenericMethodDefinition
        {
            get
            {
                return this.Inner.IsGenericMethodDefinition;
            }
        }

        public bool IsFinal
        {
            get
            {
                return this.Inner.IsFinal;
            }
        }

        public bool IsVirtual
        {
            get
            {
                return this.Inner.IsVirtual;
            }
        }

        public bool IsFamily
        {
            get
            {
                return this.Inner.IsFamily;
            }
        }

        public bool IsAssembly
        {
            get
            {
                return this.Inner.IsAssembly;
            }
        }

        public bool IsFamilyOrAssembly
        {
            get
            {
                return this.Inner.IsFamilyOrAssembly;
            }
        }

        public bool IsFamilyAndAssembly
        {
            get
            {
                return this.Inner.IsFamilyAndAssembly;
            }
        }

        public bool IsPrivate
        {
            get
            {
                return this.Inner.IsPrivate;
            }
        }

        public bool IsPublic
        {
            get
            {
                return this.Inner.IsPublic;
            }
        }

        public bool IsHideBySig
        {
            get
            {
                return this.Inner.IsHideBySig;
            }
        }

        public ParameterInfo[] GetParameters()
        {
            return this.Inner.GetParameters();
        }
    }
}
