using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Moq.Reflection.Emit
{
    public class EmitWrapper
    {
        protected Type InnerType { get; set; }
        internal object Inner { get; set; }

        internal static Assembly Mscorlib { get; private set; }

        internal EmitWrapper(object inner)
        {
            Initialize("System.Reflection.Emit", this.GetType().Name, inner);
        }

        internal EmitWrapper(string ns, string name, object inner)
        {
            this.Initialize(ns, name, inner);
        }

        private void Initialize(string ns, string name, object inner)
        {
            var fullName = string.Concat(ns, ".", name);

            this.InnerType = Mscorlib.GetType(fullName);
            if (this.InnerType == null)
                throw new InvalidOperationException(string.Format("The type '{0}' could not be found.", fullName));

            this.Inner = inner;
        }

        static EmitWrapper()
        {
            Mscorlib = typeof(string).Assembly();
        }

        internal object Invoke([CallerMemberName] string method = null, params object[] args)
        {
            return this.Invoke(method, null, args);
        }

        internal object Invoke(string method, Type[] argTypes, params object[] args)
        {
            MethodInfo info = null;
            if (argTypes != null)
                info = this.InnerType.GetMethod(method, argTypes);
            else
                info = this.InnerType.GetMethod(method);
            if(info == null)
                throw new InvalidOperationException(string.Format("Method '{0}' not found on '{1}'.", method, this.InnerType.Name));

            try
            {
                return info.Invoke(this.Inner, args);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(string.Format("Invocation of '{0}' on '{1}' failed.", method, this.InnerType.Name), ex);
            }
        }

        internal T GetProperty<T>([CallerMemberName] string name = null)
        {
            var prop = this.InnerType.GetProperty(name);
            if (prop == null)
                throw new InvalidOperationException(string.Format("Property '{0}' not found on '{1}'.", prop, this.InnerType.Name));
            return (T)prop.GetValue(this.Inner);
        }
    }
}
