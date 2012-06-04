using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Reflection
{
    [Serializable]
    internal class ParameterInfoWrapper : ReflectionWrapper<ParameterInfo>
    {
        internal ParameterInfoWrapper(ParameterInfo inner)
            : base(inner)
        {
        }

        public Type ParameterType
        {
            get
            {
                return this.Inner.ParameterType;
            }
        }

        public int Position
        {
            get
            {
                return this.Inner.Position;
            }
        }

        public string Name
        {
            get
            {
                return this.Inner.Name;
            }
        }

        public ParameterAttributes Attributes
        {
            get
            {
                return this.Inner.Attributes;
            }
        }

        public IEnumerable<object> GetCustomAttributes(bool inherit)
        {
            return this.Inner.GetCustomAttributes(inherit);
        }

        public bool IsOut
        {
            get
            {
                return this.Inner.IsOut;
            }
        }

        public bool IsDefined(Type type, bool inherit)
        {
            return this.Inner.IsDefined(type, inherit);
        }

        public bool HasAttribute(Type type)
        {
            throw new NotImplementedException("This operation has not been implemented.");
        }
    }
}
