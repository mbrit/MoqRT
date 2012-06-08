using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MoqRT.Baking.Data;
using MoqRT.Logging;

namespace MoqRT.Baking
{
    internal class BakingRunner : ILoggable, IDisposable
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
                            poke.SetupLogging(new PinnedLogDistributor());
                            this.Owner.HandleWorkItemStarted();
                            try
                            {
                                poke.RunWorkItem(this.Owner, item);

                                if (item.Waiter != null)
                                    item.Waiter.Set();
                            }
                            finally
                            {
                                AppDomain.Unload(domain);
                                this.Owner.HandleWorkItemFinished();
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
                    this.Log("Unhandled baking exception: ", ex);
                }
            }
        }

        internal void EnqueueScan(BakingSettings settings)
        {
            this.EnqueueScan(settings, DateTime.MinValue);
        }

        internal void EnqueueScan(BakingSettings settings, DateTime dt)
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
                this.WorkItems.Enqueue(new ScanWorkItem(settings.Clone(), dt));
                this.Waiter.Set();
            }
        }

        internal void EnqueueBaking(BakingSettings settings, ManualResetEvent waiter = null)
        {
            this.WorkItems.Enqueue(new BakingWorkItem(settings.Clone(), DateTime.MinValue, waiter));
            this.Waiter.Set();
        }

        public void Dispose()
        {
            try
            {
                this.Thread.Abort();
            }
            finally
            {
                this.Thread = null;
            }
        }
    }
}
