using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Moq.Baking;
using SQLite;

namespace Moq.Baking
{
    public class BakingController
    {
        internal string BakingDatabasePath { get; private set; }
        internal object ProxyGenerator { get; private set; }
        internal MethodItem RunningMethod { get; private set; }

        public void InitializeBaking(string path, object proxyGenerator)
        {
            this.BakingDatabasePath = path;
            this.ProxyGenerator = proxyGenerator;

            using (var conn = GetBakingConnection())
            {
                conn.CreateTable<InstanceItem>();
                conn.CreateTable<MethodItem>();
                conn.CreateTable<BakingItem>();
            }

            MoqRTRuntime.Baker = this;
        }

        public void SetRunningMethod(object instance, MethodInfo method)
        {
            using (var conn = GetBakingConnection())
            {
                // find the instance...
                var hash = instance.GetHashCode();
                var instanceItem = conn.Table<InstanceItem>().Where(v => v.HashCode == hash).FirstOrDefault();
                if (instanceItem == null)
                {
                    instanceItem = new InstanceItem()
                    {
                        HashCode = instance.GetHashCode(),
                        Type = instance.GetType().AssemblyQualifiedName
                    };
                    conn.Insert(instanceItem);
                }

                // create the method...
                var methodItem = new MethodItem()
                {
                    InstanceItemId = instanceItem.Id,
                    Name = method.Name
                };
                conn.Insert(methodItem);

                // set...
                RunningMethod = methodItem;
            }
        }

        private SQLiteConnection GetBakingConnection()
        {
            return new SQLiteConnection(this.BakingDatabasePath);
        }
    }
}
