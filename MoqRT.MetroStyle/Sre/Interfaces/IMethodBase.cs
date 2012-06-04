using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MoqRT.Reflection
{
    public interface IMethodBase : IMemberInfo
    {
        bool IsAbstract
        {
            get;
        }

        bool IsGenericMethod
        {
            get;
        }

        bool IsGenericMethodDefinition
        {
            get;
        }

        bool IsFinal
        {
            get;
        }

        bool IsVirtual
        {
            get;
        }

        bool IsFamily
        {
            get;
        }

        bool IsAssembly
        {
            get;
        }

        bool IsFamilyOrAssembly
        {
            get;
        }

        bool IsFamilyAndAssembly
        {
            get;
        }

        bool IsPrivate
        {
            get;
        }

        bool IsPublic
        {
            get;
        }

        bool IsHideBySig
        {
            get;
        }

        ParameterInfo[] GetParameters();
    }
}
