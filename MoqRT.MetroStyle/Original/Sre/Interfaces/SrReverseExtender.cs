using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Reflection
{
    internal static class SrReverseExtender
    {
        internal static Type ReflectedType(this IMemberInfo member)
        {
            return member.AsMemberInfo().ReflectedType();
        }

        internal static MemberInfo AsMemberInfo(this IMemberInfo wrapper)
        {
            throw new NotImplementedException("This operation has not been implemented.");
        }

        internal static MethodInfo AsMethodInfo(this IMethodInfo wrapper)
        {
            throw new NotImplementedException("This operation has not been implemented.");
        }

        internal static FieldInfo AsFieldInfo(this IFieldInfo wrapper)
        {
            throw new NotImplementedException("This operation has not been implemented.");
        }

        internal static PropertyInfo AsPropertyInfo(this IPropertyInfo wrapper)
        {
            throw new NotImplementedException("This operation has not been implemented.");
        }

        internal static ConstructorInfo AsConstructorInfo(this IConstructorInfo wrapper)
        {
            throw new NotImplementedException("This operation has not been implemented.");
        }
    }
}
