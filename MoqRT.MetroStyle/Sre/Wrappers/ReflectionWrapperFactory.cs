using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Reflection
{
    // @mbrit - 2012-06-04 - castle proxy relies on only having single instances for 
    // hash keys, so we must not generate multiple wrappers for the same object...
    internal static class ReflectionWrapperFactory
    {
        private static Dictionary<object, IReflectableBase> Wrappers { get; set; }
        private static object _wrappersLock = new object();

        static ReflectionWrapperFactory()
        {
            Wrappers = new Dictionary<object, IReflectableBase>();
        }

        internal static T Get<T>(object item)
            where T : IReflectableBase
        {
            if (item == null)
                return default(T);

            lock (_wrappersLock)
            {
                if(!(Wrappers.ContainsKey(item)))
                {
                    if (item is MethodInfo)
                        return (T)(IReflectableBase)new MethodInfoWrapper((MethodInfo)item);
                    else if (item is ConstructorInfo)
                        return (T)(IReflectableBase)new ConstructorInfoWrapper((ConstructorInfo)item);
                    else if (item is PropertyInfo)
                        return (T)(IReflectableBase)new PropertyInfoWrapper((PropertyInfo)item);
                    else if (item is FieldInfo)
                        return (T)(IReflectableBase)new FieldInfoWrapper((FieldInfo)item);
                    else if (item is ParameterInfo)
                        return (T)(IReflectableBase)new ParameterInfoWrapper((ParameterInfo)item);
                    else
                        throw new NotSupportedException(string.Format("Cannot handle '{0}'.", item.GetType()));
                }

                return (T)Wrappers[item];
            }
        }
    }
}
