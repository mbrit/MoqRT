using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Moq.Driver
{
    internal static class RealMockFactory
    {
        private static MethodInfo RealMockCreateMethod { get; set; }
        private static Dictionary<Type, MethodInfo> FactoryMethods { get; set; }
        private static object _factoryMethodsLock = new object();

        static RealMockFactory()
        {
            FactoryMethods = new Dictionary<Type, MethodInfo>();

            // get a reference to "old moq"...
            var name = new AssemblyName()
            {
                Name = "Moq"
            };
            var asm = Assembly.Load(name);

            var type = asm.GetType("Moq.Mock");
            RealMockCreateMethod = type.GetMethod("Create", BindingFlags.Public | BindingFlags.Static);
        }

        internal static RealMockWrapper<T> Create<T>()
            where T : class
        {
            // reach into Old Moq and pull out a proper proxy...
            MethodInfo method = null;
            lock (_factoryMethodsLock)
            {
                if(!(FactoryMethods.ContainsKey(typeof(T))))
                {
                    method = RealMockCreateMethod.MakeGenericMethod(typeof(T));
                    FactoryMethods[typeof(T)] = method;
                }
                else
                    method = FactoryMethods[typeof(T)];
            }

            // ok...
            return new RealMockWrapper<T>(method.Invoke(null, null));
        }
    }
}
