using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MoqRT.Reflection
{
    internal static class SrExtender
    {
        internal static Type DeclaringType(this MemberInfo member)
        {
            return member.DeclaringType;
        }

        internal static IMethodInfo AsIMethodInfo(this MethodInfo method)
        {
            throw new NotImplementedException("This operation has not been implemented.");
        }

        internal static IEnumerable<IMethodInfo> AsIMethodInfos(this IEnumerable<MethodInfo> methods)
        {
            var results = new List<IMethodInfo>();
            foreach (var member in methods)
                results.Add(member.AsIMethodInfo());
            return results;
        }

        internal static IFieldInfo AsIFieldInfo(this FieldInfo field)
        {
            throw new NotImplementedException("This operation has not been implemented.");
        }

        internal static IEnumerable<IFieldInfo> AsIFieldInfos(this IEnumerable<FieldInfo> fields)
        {
            var results = new List<IFieldInfo>();
            foreach (var member in fields)
                results.Add(member.AsIFieldInfo());
            return results;
        }

        internal static IPropertyInfo AsIPropertyInfo(this PropertyInfo prop)
        {
            throw new NotImplementedException("This operation has not been implemented.");
        }

        internal static IEnumerable<IPropertyInfo> AsIPropertyInfos(this IEnumerable<PropertyInfo> props)
        {
            var results = new List<IPropertyInfo>();
            foreach (var member in props)
                results.Add(member.AsIPropertyInfo());
            return results;
        }

        internal static IConstructorInfo AsIConstructorInfo(this ConstructorInfo constructor)
        {
            throw new NotImplementedException("This operation has not been implemented.");
        }

        internal static IEnumerable<IConstructorInfo> AsIConstructorInfos(this IEnumerable<ConstructorInfo> constructors)
        {
            var results = new List<IConstructorInfo>();
            foreach (var member in constructors)
                results.Add(member.AsIConstructorInfo());
            return results;
        }

        internal static IMemberInfo AsIMemberInfo(this MemberInfo member)
        {
            throw new NotImplementedException("This operation has not been implemented.");
        }

        internal static IEnumerable<IMemberInfo> AsIMemberInfos(this IEnumerable<MemberInfo> members)
        {
            var results = new List<IMemberInfo>();
            foreach (var member in members)
                results.Add(member.AsIMemberInfo());
            return results;
        }

        internal static Type AsType(this GenericTypeParameterBuilder builder)
        {
            throw new NotImplementedException("This operation has not been implemented.");
        }

        internal static IEnumerable<Type> AsTypes(this GenericTypeParameterBuilder[] builders)
        {
            List<Type> results = new List<Type>();
            foreach (var builder in builders)
                results.Add(builder.AsType());
            return results;
        }

        internal static TypeCode GetTypeCode(this Enum e)
        {
            throw new NotImplementedException("This operation has not been implemented.");
        }
    }
}
