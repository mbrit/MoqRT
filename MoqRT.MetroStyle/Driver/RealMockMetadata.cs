using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Moq.Driver
{
    internal static class RealMockMetadata
    {
        private static Dictionary<string, MethodInfo> Methods { get; set; }
        private static object _lock = new object();

        static RealMockMetadata()
        {
            Methods = new Dictionary<string, MethodInfo>();
        }

        internal static MethodInfo GetDeferralMethod(Type type, string name, Type[] genericTypes, Type[] argTypes)
        {
            var builder = new StringBuilder();
            builder.Append(type.FullName);
            builder.Append("|");
            builder.Append(name);
            builder.Append("|");
            if (genericTypes != null && genericTypes.Length > 0)
            {
                builder.Append(genericTypes.Length);
                foreach (var genericType in genericTypes)
                {
                    builder.Append(",");
                    builder.Append(genericType.FullName);
                }
            }
            else
                builder.Append("x");
            if (argTypes != null && argTypes.Length > 0)
            {
                builder.Append(argTypes.Length);
                foreach (var argType in argTypes)
                {
                    builder.Append(",");
                    builder.Append(argType.FullName);
                }
            }
            else
                builder.Append("x");
            var key = builder.ToString();

            lock (_lock)
            {
                if (!(Methods.ContainsKey(key)))
                {
                    // are we matching a generic type?
                    MethodInfo method = null;
                    if (genericTypes != null && genericTypes.Length > 0)
                    {
                        var methods = type.GetMethods().ToList().Where(v => v.Name == name);
                        //foreach(var walk in methods)
                        //{

                        //}
                        method = methods.Last().MakeGenericMethod(genericTypes);
                    }
                    else
                        method = type.GetMethod(name, argTypes);

                    // set...
                    Methods[key] = method;
                }

                return Methods[key];
            }
        }
    }
}
