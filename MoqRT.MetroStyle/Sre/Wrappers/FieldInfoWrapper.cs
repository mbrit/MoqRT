using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Reflection
{
    [Serializable]
    internal class FieldInfoWrapper : MemberInfoReflectionWrapper<FieldInfo>, IFieldInfo
    {
        // only create via a factory! Castle depends on single instances of wrappers...
        internal FieldInfoWrapper(FieldInfo inner)
            : base(inner)
        {
        }

        public Type FieldType
        {
            get
            {
                return this.Inner.FieldType;
            }
        }

        public FieldAttributes Attributes
        {
            get
            {
                return this.Inner.Attributes;
            }
        }
    }
}
