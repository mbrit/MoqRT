using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MoqRT.Reflection
{
    public interface IMethodInfo : IMethodBase
    {
        Type ReturnType
        {
            get;
        }

        bool ContainsGenericParameters
        {
            get;
        }

        IMethodInfo GetGenericMethodDefinition();

        IMethodInfo MakeGenericMethod(Type[] type);

        Type[] GetGenericArguments();

        ParameterInfo ReturnParameter
        {
            get;
        }

        //bool IsPropertySetter();

        //bool IsPropertyGetter();

        //bool IsDestructor();

        //bool IsEventAttach();

        //bool IsEventDetach();

        IMethodInfo GetBaseDefinition();
    }
}
