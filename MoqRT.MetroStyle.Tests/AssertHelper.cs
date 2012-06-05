using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

#if !NETFX_CORE
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif

namespace MoqRT
{
    internal static class AssertHelper
    {
        internal static T Throws<T>(Action d)
            where T : Exception
        {
            return Throws<T>("Failed.", d);
        }

        internal static T Throws<T>(string message, Action d)
            where T : Exception
        {
            try
            {
                d();
                throw new InvalidOperationException("Exception not thrown.");
            }
            catch (Exception ex)
            {
                if (typeof(T).IsAssignableFrom(ex.GetType()))
                    return (T)ex;
                else
                    throw new InvalidOperationException(string.Format("Exception mismatch: {0} cf {1}.", ex.GetType(), typeof(T)), ex);
            }
        }

        internal static void Contains(string check, string message)
        {
            Assert.IsTrue(message.Contains(check));
        }

        internal static void Same(object a, object b)
        {
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        internal static void IsAssignableFrom<T>(object value)
        {
            Assert.IsTrue(typeof(T).IsAssignableFrom(value.GetType()));
        }
    }
}
