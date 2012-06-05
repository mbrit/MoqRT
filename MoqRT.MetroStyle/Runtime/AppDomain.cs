using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Moq.Reflection.Emit;

namespace System
{
    public class AppDomain : EmitWrapper
    {
        public static AppDomain CurrentDomain { get; private set; }

        private AppDomain(object inner)
            : base("System", "AppDomain", inner)
        {
        }

        static AppDomain()
        {
            // get...
            var type = EmitWrapper.Mscorlib.GetType("System.AppDomain");
            var prop = type.GetProperty("CurrentDomain", BindingFlags.Static | BindingFlags.Public);
            var instance = prop.GetValue(null);

            // create...
            CurrentDomain = new AppDomain(instance);
        }

        internal AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string folderPath)
        {
            var realAba = EmitWrapper.Mscorlib.GetType("System.Reflection.Emit.AssemblyBuilderAccess");
            var result = this.Invoke("DefineDynamicAssembly", new Type[] { typeof(AssemblyName), 
                realAba, typeof(string) }, name, access, folderPath);
            return new AssemblyBuilder(result);
        }

        internal AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access)
        {
            var realAba = EmitWrapper.Mscorlib.GetType("System.Reflection.Emit.AssemblyBuilderAccess");
            var result = this.Invoke("DefineDynamicAssembly", new Type[] { typeof(AssemblyName), 
                realAba }, name, access);
            return new AssemblyBuilder(result);
        }
    }
}
