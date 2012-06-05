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
        private Queue<WorkItem> WorkItems { get; set; }
        private Thread Thread { get; set; }
        private AutoResetEvent Waiter { get; set; }

        internal BakingRunner(BakingController owner)
        {
            this.Owner = owner;
            this.WorkItems = new Queue<WorkItem>();

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
                    List<WorkItem> requeue = new List<WorkItem>();
                    while (this.WorkItems.Count > 0)
                    {
                        // run...
                        var item = this.WorkItems.Dequeue();

                        // ok...
                        if (item.OkToRun)
                        {
                            // create...
                            AppDomain domain = AppDomain.CreateDomain(item.Settings.AssemblyFilename);
                            var type = typeof(BakingPoke);
                            IBakingPoke poke = (IBakingPoke)domain.CreateInstanceAndUnwrap(type.Assembly.GetName().ToString(), type.FullName);
                            try
                            {
                                poke.Bake(this.Owner, item.Settings);
                            }
                            finally
                            {
                                AppDomain.Unload(domain);
                            }
                        }
                        else
                            requeue.Add(item);
                    }

                    // walk...
                    foreach (var item in requeue)
                        this.WorkItems.Enqueue(item);

                    // wait...
                    this.Waiter.WaitOne(1000);
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
            this.Enqueue(settings, DateTime.MinValue);
        }

        internal void Enqueue(BakingSettings settings, DateTime dt)
        {
            var existing = this.WorkItems.Where(v => v.Settings.AssemblyPath == settings.AssemblyPath && 
                v.AtOrAfter != DateTime.MinValue).FirstOrDefault();
            if (existing != null)
            {
                existing.AtOrAfter = dt;
                return;
            }
            else
            {
                this.WorkItems.Enqueue(new WorkItem(settings.Clone(), dt));
                this.Waiter.Set();
            }
        }
    }
}
