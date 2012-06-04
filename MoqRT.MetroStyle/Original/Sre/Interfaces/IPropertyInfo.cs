using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MoqRT.Reflection
{
    public interface IPropertyInfo : IMemberInfo
    {
        bool CanWrite
        {
            get;
        }

        bool CanRead
        {
            get;
        }

        IMethodInfo GetGetMethod(bool nonPublic);

        IMethodInfo GetSetMethod(bool nonPublic);

        ParameterInfo[] GetIndexParameters();

        Type PropertyType
        {
            get;
        }

        object GetValue(object obj, object[] index);
    }
}
