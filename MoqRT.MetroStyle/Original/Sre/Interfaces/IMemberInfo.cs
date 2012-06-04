using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MoqRT.Reflection
{
    public interface IMemberInfo : IReflectableBase
    {
        Type DeclaringType
        {
            get;
        }

        string Name
        {
            get;
        }

        bool IsDefined(Type type);

        object[] GetCustomAttributes(bool inherit);

        Type ReflectedType();
    }
}
