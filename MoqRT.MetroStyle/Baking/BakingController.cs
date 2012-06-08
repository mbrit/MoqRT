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
using Windows.Storage;

namespace MoqRT.Baking
{
    internal class BakingController : IDisposable
    {
        private string TestAssemblyPath { get; set; }
        internal string AppxPath { get; set; }
        private string DatabasePath { get; set; }
        private Guid Signal { get; set; }

        internal const string DatabaseName = "MoqRT.Baked.dll.db";
        private const string SignalFilename = "MoqRT.signal";

        internal BakingController(string testAssembly, string appxPath, string bakingPath)
        {
            this.TestAssemblyPath = testAssembly;
            this.AppxPath = bakingPath;
            this.DatabasePath = Path.Combine(bakingPath, DatabaseName);
            this.Signal = Guid.NewGuid();

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
