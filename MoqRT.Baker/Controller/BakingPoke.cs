using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MoqRT.Baking.Data;
using MoqRT.Logging;

namespace MoqRT.Baking
{
    internal class BakingPoke : MarshalByRefObject, IBakingPoke
    {
        public void RunWorkItem(BakingController owner, WorkItem item)
        {
            using (new Resolver(item.Settings))
            {
                // find the WinRT version of the library...
                Type rt = null;
                var asm = Assembly.LoadFrom(item.Settings.AssemblyPath);
                foreach (var referenceName in asm.GetReferencedAssemblies().ToList().Where(v => v.Name.StartsWith("MoqRT")))
                {
                    var reference = Assembly.Load(referenceName);

                    // get...
                    rt = reference.GetType("MoqRT.MoqRTRuntime");
                    if (rt != null)
                        break;
                }

                // ensure...
                SQLiteHelper.EnsureSQLiteInFolder(item.Settings.AssemblyFolder);

                // go...
                item.Run(new BakingContext(owner, rt, asm));
            }
        }

        private class Resolver : IDisposable
        {
            private BakingSettings Settings { get; set; }

            internal Resolver(BakingSettings settings)
            {
                this.Settings = settings;
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            }

            Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
            {
                string name = args.Name;
                int index = args.Name.IndexOf(",");
                if (index != -1)
                    name = name.Substring(0, index);
                string path = Path.Combine(this.Settings.AssemblyFolder, name + ".dll");
                if (File.Exists(path))
                    return Assembly.LoadFrom(path);
                else
                    return null;
            }

            public void Dispose()
            {
                AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
            }
        }

        public void SetupLogging(ILogWriter writer)
        {
            Logger.RegisterLogWriter(writer);
        }
    }
}
