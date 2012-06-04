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
        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public Type DeclaringType
        {
            get
            {
                throw new NotImplementedException("This operation has not been implemented.");
            }
        }

        public bool IsInternal
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsFamily
        {
            get
            {
                throw new NotImplementedException("This operation has not been implemented.");
            }
        }

        public bool IsAssembly
        {
            get
            {
                throw new NotImplementedException("This operation has not been implemented.");
            }
        }

        public bool IsFamilyOrAssembly
        {
            get
            {
                throw new NotImplementedException("This operation has not been implemented.");
            }
        }

        public bool IsStatic
        {
            get
            {
                throw new NotImplementedException("This operation has not been implemented.");
            }
        }

        public object GetNonInheritableAttributes()
        {
            // not in SR, this is an extension operation...
            throw new NotImplementedException();
        }

        public bool IsDefined(Type type)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetCustomAttributes(bool inherit)
        {
            throw new NotImplementedException();
        }

        public Type ReflectedType()
        {
            throw new NotImplementedException();
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
