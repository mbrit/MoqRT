using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Moq.Database;
using Moq.Proxy;
using MoqRT.Baking;
using SQLite;
using Moq;
using Windows.Networking.Sockets;
using Windows.Networking;

namespace MoqRT
{
    public static class MoqRTRuntime
    {
        internal static bool IsWinRTProcess { get; set; }
        internal static BakingController Baker { get; set; }
        private static Assembly _bakedAssembly = null;

        static MoqRTRuntime()
        {
            try
            {
                var package = Windows.ApplicationModel.Package.Current;
                if (package == null)
                    throw new InvalidOperationException("'package' is null.");

                // ok...
                IsWinRTProcess = true;
            }
            catch
            {
                IsWinRTProcess = false;
            }
        }

        internal static bool IsBaking
        {
            get
            {
                return !(Baker == null);
            }
        }

        internal static bool UsingBakedProxies
        {
            get
            {
                return IsWinRTProcess;
            }
        }

        public static void InitializeBaking(string testAssembly, string appxPath, string bakingPath)
        {
            Baker = new BakingController(testAssembly, appxPath, bakingPath);
        }

        public static string FinishBaking()
        {
            try
            {
                var path = Path.Combine(Baker.AppxPath, ModuleScope.DEFAULT_FILE_NAME); 
                Baker.Dispose();

                // return...
                return path;
            }
            finally
            {
                Baker = null;
            }
        }

        internal static object GetBakedProxy(string key, IInterceptor interceptor, object[] args)
        {
            GeneratedType item = null;
            using (var conn = GetDatabase())
            {
                var query = conn.Table<GeneratedType>().Where(v => v.TargetType == key);
                item = query.FirstOrDefault();
                if (item == null)
                    throw new InvalidOperationException(string.Format("A proxy for '{0}' was not found.", key));
            }

            // get...
            var type = BakedAssembly.GetType(item.ProxyType);
            return Activator.CreateInstance(type, new IInterceptor[] { interceptor }, args);
        }

        private static SQLiteConnection GetDatabase()
        {
            return new SQLiteConnection(BakingController.DatabaseName);
        }

        private static Assembly BakedAssembly
        {
            get
            {
                if (_bakedAssembly == null)
                {
                    var name = new AssemblyName()
                    {
                        Name = Path.GetFileNameWithoutExtension(ModuleScope.DEFAULT_FILE_NAME)
                    };
                    _bakedAssembly = Assembly.Load(name);
                }
                return _bakedAssembly;
            }
        }

        /// <summary>
        /// Use this method in your tests to indicate that mock construction is finished
        /// and that the rest of the run should not be executed.
        /// </summary>
        public static void StopIfBaking()
        {
            if (IsBaking)
                throw new StopBakingException();
        }

        internal static void MockInstantiated(Mock mock)
        {
        }
    }
}
