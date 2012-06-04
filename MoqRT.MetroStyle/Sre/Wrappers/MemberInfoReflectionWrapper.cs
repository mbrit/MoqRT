using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Reflection
{
    [Serializable]
    internal class MemberInfoReflectionWrapper<T> : ReflectionWrapper<T>, IMemberInfo
        where T : MemberInfo
    {
        protected MemberInfoReflectionWrapper(T inner)
            : base(inner)
        {
        }

        public Type DeclaringType
        {
            get
            {
                return this.Inner.DeclaringType;
            }
        }

        public string Name
        {
            get
            {
                return this.Inner.Name;
            }
        }

        public bool IsDefined(Type type)
        {
            return this.Inner.IsDefined(type);
        }

        public IEnumerable<object> GetCustomAttributes(bool inherit)
        {
            return this.Inner.GetCustomAttributes(inherit);
        }

        public Type ReflectedType()
        {
            return this.Inner.ReflectedType();
        }
    }
}
