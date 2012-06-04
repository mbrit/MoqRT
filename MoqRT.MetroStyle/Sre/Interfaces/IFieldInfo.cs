using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MoqRT.Reflection
{
    public interface IFieldInfo : IMemberInfo
    {
        Type FieldType
        {
            get;
        }
     
        FieldAttributes Attributes
        {
            get;
        }
    }
}
