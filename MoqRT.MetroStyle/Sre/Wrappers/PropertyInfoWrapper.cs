using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Reflection
{
    [Serializable]
    internal class PropertyInfoWrapper : MemberInfoReflectionWrapper<PropertyInfo>, IPropertyInfo
    {
        // only create via a factory! Castle depends on single instances of wrappers...
        internal PropertyInfoWrapper(PropertyInfo inner)
            : base(inner)
        {
        }

        public bool CanWrite
        {
            get
            {
                return this.Inner.CanWrite;
            }
        }

        public bool CanRead
        {
            get
            {
                return this.Inner.CanRead;
            }
        }

        public IMethodInfo GetGetMethod(bool nonPublic)
        {
            return this.Inner.GetGetMethod(nonPublic).AsIMethodInfo();
        }

        public IMethodInfo GetSetMethod(bool nonPublic)
        {
            return this.Inner.GetSetMethod(nonPublic).AsIMethodInfo();
        }

        public ParameterInfo[] GetIndexParameters()
        {
            return this.Inner.GetIndexParameters();
        }

        public Type PropertyType
        {
            get
            {
                return this.Inner.PropertyType;
            }
        }

        public object GetValue(object original, params object[] index)
        {
            return this.Inner.GetValue(original, index);
        }
    }
}
