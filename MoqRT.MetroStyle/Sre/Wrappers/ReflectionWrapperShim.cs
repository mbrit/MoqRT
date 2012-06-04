//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using MoqRT.Reflection;

//namespace Moq.Reflection
//{
//    internal static class ReflectionWrapperShim
//    {
//        internal static MemberInfo AsMemberInfo(this IMemberInfo info)
//        {
//            if (info is PropertyInfoWrapper)
//                return ((PropertyInfoWrapper)info).Inner;
//            else if (info is MethodInfoWrapper)
//                return ((MethodInfoWrapper)info).Inner;
//            else if (info is FieldInfoWrapper)
//                return ((FieldInfoWrapper)info).Inner;
//            else
//                throw new NotSupportedException(string.Format("Cannot handle '{0}'.", info.GetType()));
//        }

//        internal static MethodInfo AsMethodInfo(this IMethodInfo info)
//        {
//            if (info is MethodInfoWrapper)
//                return ((MethodInfoWrapper)info).Inner;
//            else
//                throw new NotSupportedException(string.Format("Cannot handle '{0}'.", info.GetType()));
//        }

//        internal static IEnumerable<MethodInfo> AsMethodInfos(IEnumerable<IMethodInfo> infos)
//        {
//            var results = new List<MethodInfo>();
//            foreach (var info in infos)
//                results.Add(info.AsMethodInfo());
//            return results;
//        }

//        internal static FieldInfo AsFieldInfo(this IFieldInfo info)
//        {
//            if (info is FieldInfoWrapper)
//                return ((FieldInfoWrapper)info).Inner;
//            else
//                throw new NotSupportedException(string.Format("Cannot handle '{0}'.", info.GetType()));
//        }

//        internal static IEnumerable<FieldInfo> AsFieldInfos(IEnumerable<IFieldInfo> infos)
//        {
//            var results = new List<FieldInfo>();
//            foreach (var info in infos)
//                results.Add(info.AsFieldInfo());
//            return results;
//        }

//        internal static PropertyInfo AsPropertyInfo(this IPropertyInfo info)
//        {
//            if (info is PropertyInfoWrapper)
//                return ((PropertyInfoWrapper)info).Inner;
//            else
//                throw new NotSupportedException(string.Format("Cannot handle '{0}'.", info.GetType()));
//        }

//        internal static IEnumerable<PropertyInfo> AsPropertyInfos(IEnumerable<IPropertyInfo> infos)
//        {
//            var results = new List<PropertyInfo>();
//            foreach (var info in infos)
//                results.Add(info.AsPropertyInfo());
//            return results;
//        }

//        internal static ConstructorInfo AsConstructorInfo(this IConstructorInfo info)
//        {
//            if (info == null)
//                return null;

//            if (info is ConstructorInfoWrapper)
//                return ((ConstructorInfoWrapper)info).Inner;
//            else
//                throw new NotSupportedException(string.Format("Cannot handle '{0}'.", info.GetType()));
//        }

//        internal static Type DeclaringType(this MemberInfo member)
//        {
//            return member.DeclaringType;
//        }

//        internal static IMethodInfo AsIMethodInfo(this MethodInfo method)
//        {
//            return ReflectionWrapperFactory.Get<IMethodInfo>(method);
//        }

//        internal static IEnumerable<IMethodInfo> AsIMethodInfos(this IEnumerable<MethodInfo> methods)
//        {
//            var results = new List<IMethodInfo>();
//            foreach (var member in methods)
//                results.Add(member.AsIMethodInfo());
//            return results;
//        }

//        internal static IFieldInfo AsIFieldInfo(this FieldInfo field)
//        {
//            return ReflectionWrapperFactory.Get<IFieldInfo>(field);
//        }

//        internal static IEnumerable<IFieldInfo> AsIFieldInfos(this IEnumerable<FieldInfo> fields)
//        {
//            var results = new List<IFieldInfo>();
//            foreach (var member in fields)
//                results.Add(member.AsIFieldInfo());
//            return results;
//        }

//        internal static IPropertyInfo AsIPropertyInfo(this PropertyInfo prop)
//        {
//            return ReflectionWrapperFactory.Get<IPropertyInfo>(prop);
//        }

//        internal static IEnumerable<IPropertyInfo> AsIPropertyInfos(this IEnumerable<PropertyInfo> props)
//        {
//            var results = new List<IPropertyInfo>();
//            foreach (var member in props)
//                results.Add(member.AsIPropertyInfo());
//            return results;
//        }

//        internal static IConstructorInfo AsIConstructorInfo(this ConstructorInfo constructor)
//        {
//            return ReflectionWrapperFactory.Get<IConstructorInfo>(constructor);
//        }

//        internal static IEnumerable<IConstructorInfo> AsIConstructorInfos(this IEnumerable<ConstructorInfo> constructors)
//        {
//            var results = new List<IConstructorInfo>();
//            foreach (var member in constructors)
//                results.Add(member.AsIConstructorInfo());
//            return results;
//        }

//        internal static IMemberInfo AsIMemberInfo(this MemberInfo member)
//        {
//            throw new NotImplementedException("This operation has not been implemented.");
//        }

//        internal static IEnumerable<IMemberInfo> AsIMemberInfos(this IEnumerable<MemberInfo> members)
//        {
//            var results = new List<IMemberInfo>();
//            foreach (var member in members)
//                results.Add(member.AsIMemberInfo());
//            return results;
//        }
//    }
//}
