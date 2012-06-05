using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MoqRT.Baking.Data;

namespace MoqRT.Baking
{
    internal class BakingRunner
    {
        private BakingController Owner { get; set; }
        private Queue<BakingSettings> WorkItems { get; set; }
        private Thread Thread { get; set; }
        private AutoResetEvent Waiter { get; set; }

        internal BakingRunner(BakingController owner)
        {
            this.Owner = owner;
            this.WorkItems = new Queue<BakingSettings>();

            this.Waiter = new AutoResetEvent(false);
            this.Thread = new Thread(ThreadEntryPoint);
            this.Thread.Name = "Baker";
            this.Thread.Start();
        }

        private void ThreadEntryPoint()
        {
            while (true)
            {
                try
                {
                    while (this.WorkItems.Count > 0)
                    {
                        // run...
                        var item = this.WorkItems.Dequeue();

                        // create...
                        AppDomain domain = AppDomain.CreateDomain(item.AssemblyFilename);
                        var type = typeof(BakingPoke);
                        IBakingPoke poke = (IBakingPoke)domain.CreateInstanceAndUnwrap(type.Assembly.GetName().ToString(), type.FullName);
                        try
                        {
                            poke.Bake(this.Owner, item);
                        }
                        finally
                        {
                            AppDomain.Unload(domain);
                        }
                    }

                    // wait...
                    this.Waiter.WaitOne(TimeSpan.FromSeconds(5));
                    this.Waiter.Reset();
                }
                catch (ThreadAbortException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    this.Owner.Log("Unhandled baking exception: ", ex);
                }
            }
        }

        private void Log(string message)
        {
            this.Owner.Log(message);
        }

        internal void Enqueue(BakingSettings settings)
        {
            this.WorkItems.Enqueue(settings.Clone());
            this.Waiter.Set();
        }
    }
}
