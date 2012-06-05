using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoqRT.Reflection;

namespace System.Reflection.Emit
{
    public abstract class MemberBuilder : BuilderBase, IMemberInfo
    {
        protected MemberBuilder(object inner)
            : base(inner)
        {
        }

        public string Name
        {
            get
            {
                return this.GetProperty<string>();
            }
        }

        public Type DeclaringType
        {
            get
            {
                return this.GetProperty<Type>();
            }
        }

        public bool IsInternal
        {
            get
            {
                return this.GetProperty<bool>();
            }
        }

        public bool IsFamily
        {
            get
            {
                return this.GetProperty<bool>();
            }
        }

        public bool IsAssembly
        {
            get
            {
                return this.GetProperty<bool>();
            }
        }

        public bool IsFamilyOrAssembly
        {
            get
            {
                return this.GetProperty<bool>();
            }
        }

        public bool IsStatic
        {
            get
            {
                return this.GetProperty<bool>();
            }
        }

        public object GetNonInheritableAttributes()
        {
            // not in SR, this is an extension operation...
            throw new NotImplementedException();
        }

        public bool IsDefined(Type type)
        {
            return (bool)this.Invoke("IsDefined", type);
        }

        public IEnumerable<object> GetCustomAttributes(bool inherit)
        {
            throw new NotImplementedException();
        }

        public Type ReflectedType()
        {
            // it's a property in the underlying member - this is to simulate
            // the WinRT shim...
            return this.GetProperty<Type>();
        }

        public string Key
        {
            get { throw new NotImplementedException(); }
        }

        internal IMemberInfo AsIMemberInfo()
        {
            throw new NotImplementedException();
        }
    }
}
