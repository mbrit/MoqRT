using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq.Proxy;
using SQLite;
using Moq;
using Moq.Database;
using System.Reflection;

namespace MoqRT.Baking
{
    internal class BakingController : IDisposable
    {
        private string TestAssemblyPath { get; set; }
        internal string AppxPath { get; set; }
        private string DatabasePath { get; set; }

        internal const string DatabaseName = "MoqRTBaking.db";

        internal BakingController(string testAssembly, string appxPath)
        {
            this.TestAssemblyPath = testAssembly;
            this.AppxPath = appxPath;
            this.DatabasePath = Path.Combine(appxPath, DatabaseName);

            // setup...
            using (var conn = GetDatabase())
                conn.CreateTable<GeneratedType>();
        }

        private SQLiteConnection GetDatabase()
        {
            return new SQLiteConnection(this.DatabasePath);
        }

        private string TestAssemblyName
        {
            get
            {
                return Path.GetFileName(this.TestAssemblyPath);
            }
        }

        public void Dispose()
        {
            var genereator = CastleProxyFactory.GetPersistentBuilder();
            genereator.SaveAssembly();
        }

        internal void ProxyCreated(Mock mock, object value)
        {
            GeneratedType item = new GeneratedType();
            item.TargetType = mock.ProxyKey;
            item.ProxyType = value.GetType().FullName;

            using (var conn = this.GetDatabase())
                conn.Insert(item);
        }
    }
}
