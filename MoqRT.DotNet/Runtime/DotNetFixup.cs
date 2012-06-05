using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using MoqRT;
using MoqRT.Reflection;

namespace MoqRT
{
    // DO NOT include this in the WinRT version...
    internal static class DotNetFixup
    {
        internal static MemberInfo AsMemberInfo(this IMemberInfo info)
        {
            if (info is PropertyInfoWrapper)
                return ((PropertyInfoWrapper)info).Inner;
            else if (info is MethodInfoWrapper)
                return ((MethodInfoWrapper)info).Inner;
            else if (info is FieldInfoWrapper)
                return ((FieldInfoWrapper)info).Inner;
            else
                throw new NotSupportedException(string.Format("Cannot handle '{0}'.", info.GetType()));
        }

        internal static MethodInfo AsMethodInfo(this IMethodInfo info)
        {
            if (info is MethodInfoWrapper)
                return ((MethodInfoWrapper)info).Inner;
            else
                throw new NotSupportedException(string.Format("Cannot handle '{0}'.", info.GetType()));
        }

        internal static IEnumerable<MethodInfo> AsMethodInfos(IEnumerable<IMethodInfo> infos)
        {
            var results = new List<MethodInfo>();
            foreach (var info in infos)
                results.Add(info.AsMethodInfo());
            return results;
        }

        internal static FieldInfo AsFieldInfo(this IFieldInfo info)
        {
            if (info is FieldInfoWrapper)
                return ((FieldInfoWrapper)info).Inner;
            else
                throw new NotSupportedException(string.Format("Cannot handle '{0}'.", info.GetType()));
        }

        internal static IEnumerable<FieldInfo> AsFieldInfos(IEnumerable<IFieldInfo> infos)
        {
            var results = new List<FieldInfo>();
            foreach (var info in infos)
                results.Add(info.AsFieldInfo());
            return results;
        }

        internal static PropertyInfo AsPropertyInfo(this IPropertyInfo info)
        {
            if (info is PropertyInfoWrapper)
                return ((PropertyInfoWrapper)info).Inner;
            else
                throw new NotSupportedException(string.Format("Cannot handle '{0}'.", info.GetType()));
        }

        internal static IEnumerable<PropertyInfo> AsPropertyInfos(IEnumerable<IPropertyInfo> infos)
        {
            var results = new List<PropertyInfo>();
            foreach (var info in infos)
                results.Add(info.AsPropertyInfo());
            return results;
        }

        internal static ConstructorInfo AsConstructorInfo(this IConstructorInfo info)
        {
            if (info == null)
                return null;

            if (info is ConstructorInfoWrapper)
                return ((ConstructorInfoWrapper)info).Inner;
            else
                throw new NotSupportedException(string.Format("Cannot handle '{0}'.", info.GetType()));
        }

        internal static Type DeclaringType(this MemberInfo member)
        {
            return member.DeclaringType;
        }

        internal static IMethodInfo AsIMethodInfo(this MethodInfo method)
        {
            return ReflectionWrapperFactory.Get<IMethodInfo>(method);
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
            return ReflectionWrapperFactory.Get<IFieldInfo>(field);
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
            return ReflectionWrapperFactory.Get<IPropertyInfo>(prop);
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
            return ReflectionWrapperFactory.Get<IConstructorInfo>(constructor);
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

        //internal static void Emit(this ILGenerator gen, OpCode opcode, IConstructorInfo con)
        //{
        //    gen.Emit(opcode, con.AsConstructorInfo());
        //}

        //internal static void Emit(this ILGenerator gen, OpCode opcode, IFieldInfo field)
        //{
        //    gen.Emit(opcode, field.AsFieldInfo());
        //}

        //internal static void Emit(this ILGenerator gen, OpCode opcode, IMethodInfo method)
        //{
        //    gen.Emit(opcode, method.AsMethodInfo());
        //}

        internal static void NonProxyableMemberNotification(this IProxyGenerationHook hook, Type type,
            MemberInfo memberInfo)
        {
            hook.NonProxyableMemberNotification(type, memberInfo.AsIMemberInfo());
        }

        internal static void DefineMethodOverride(this TypeBuilder builder, MethodBuilder methodInfoBody,
            IMethodInfo methodInfoDeclaration)
        {
            builder.DefineMethodOverride(methodInfoBody, methodInfoDeclaration.AsMethodInfo());
        }

        internal static FieldInfo ToFieldInfo(this FieldBuilder builder)
        {
            return builder;
        }

        internal static Type ReflectedType(this MemberInfo member)
        {
            return member.ReflectedType;
        }

        internal static Assembly Assembly(this ModuleBuilder module)
        {
            return module.Assembly;
        }
    }
}
