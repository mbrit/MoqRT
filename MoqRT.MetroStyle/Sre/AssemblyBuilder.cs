using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq.Reflection.Emit;

namespace System.Reflection.Emit
{
    public class AssemblyBuilder : EmitWrapper
    {
        internal AssemblyBuilder(object inner)
            : base(inner)
        {
        }

        internal ModuleBuilder DefineDynamicModule(string moduleName, bool args)
        {
            var result = this.Invoke("DefineDynamicModule", new Type[] { typeof(string), typeof(bool) }, 
                moduleName, args);
            return new ModuleBuilder(result);
        }

        internal ModuleBuilder DefineDynamicModule(string moduleName, string filename, bool args)
        {
            var result = this.Invoke("DefineDynamicModule", new Type[] { typeof(string), typeof(string), typeof(bool) },
                moduleName, filename, args);
            return new ModuleBuilder(result);
        }

        internal void Save(string filename)
        {
            this.Invoke("Save", filename);
        }
    }
}
