using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Moq.Driver
{
    internal abstract class RealWrapper
    {
        private object Real { get; set; }

        protected RealWrapper(object real)
        {
            this.Real = real;
        }

        internal object Invoke(string name, Type[] genericArgs, Type[] argTypes, params object[] args)
        {
            // get it...
            var method = GetDeferralMethod(name, genericArgs, argTypes, args);
            return method.Invoke(this.Real, args);
        }

        private MethodInfo GetDeferralMethod(string name, Type[] genericArgs, Type[] argTypes, object[] args)
        {
            return RealMockMetadata.GetDeferralMethod(this.Real.GetType(), name, genericArgs, argTypes);
        }
    }
}
