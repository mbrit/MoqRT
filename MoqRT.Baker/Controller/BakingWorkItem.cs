using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MoqRT.Logging;

namespace MoqRT.Baking
{
    [Serializable]
    internal class BakingWorkItem : ScanWorkItem
    {
        private const string SignalFilename = "MoqRT.signal";

        internal BakingWorkItem(BakingSettings settings, DateTime atOrAfter, ManualResetEvent waiter)
            : base(settings, atOrAfter, waiter)
        {
        }

        internal override void Run(BakingContext context)
        {
            base.Run(context);

            // next...
            context.Owner.HandleBakingStarted();
            this.Log("Starting baking process...");
            try
            {
                // run...
                this.Log("Initializing...");
                var initializeMethod = context.RuntimeType.GetMethod("InitializeBaking", BindingFlags.Static | BindingFlags.Public);
                initializeMethod.Invoke(null, new object[] { this.Settings.AssemblyPath, this.Settings.AppxPath, this.Settings.BakingPath });

                // go through the project...
                foreach (var c in context.Owner.ActiveProject.Classes)
                {
                    if (c.Include)
                    {
                        // load...
                        object instance = null;
                        try
                        {
                            var type = context.TestAssembly.GetType(c.Name);
                            instance = Activator.CreateInstance(type);
                        }
                        catch(Exception ex)
                        {
                            context.Log(string.Format("An error occurred whilst creating test '{0}'.", c.Name), ex);
                        }

                        // if...
                        if(instance != null)
                        {
                            foreach(var m in c.Methods)
                            {
                                if(m.Include)
                                {
                                    // get...
                                    MethodInfo info = null;
                                    try
                                    {
                                        info = instance.GetType().GetMethod(m.Name);
                                    }
                                    catch (Exception ex)
                                    {
                                        context.Log(string.Format("An error occurred whilst obtaining method '{0}'.", m.Name), ex);
                                    }

                                    if (info != null)
                                    {
                                        context.Log(string.Format("Running '{0}'...", m.Name));
                                        try
                                        {
                                            info.Invoke(instance, null);
                                            this.Log("...No failure");
                                        }
                                        catch (Exception ex)
                                        {
                                            bool first = true;
                                            var walk = ex;
                                            StringBuilder builder = new StringBuilder();
                                            builder.Append("...");
                                            while (walk != null)
                                            {
                                                if (first)
                                                    first = false;
                                                else
                                                    builder.Append(", ");

                                                builder.Append(walk.GetType().Name);
                                                walk = walk.InnerException;
                                            }

                                            this.Log(builder.ToString());
                                        }
                                    }
                                }
                                else
                                    this.Log(string.Format("Ignoring method '{0}'...", m.Name));
                            }
                        }
                    }
                    else
                        this.Log(string.Format("Ignoring class '{0}'...", c.Name));
                }

                // finish...
                context.Owner.Log("Finishing up...");
                var finishMethod = context.RuntimeType.GetMethod("FinishBaking", BindingFlags.Static | BindingFlags.Public);
                finishMethod.Invoke(null, null);

                // move the database...
                const string databaseName = "MoqRT.Baked.dll.db";
                var dbPath = Path.Combine(this.Settings.BakingPath, databaseName);
                var finalDbPath = Path.Combine(this.Settings.AppxPath, databaseName);
                OptimisticDelete(finalDbPath);
                File.Copy(dbPath, finalDbPath);

                // log...
                context.Owner.Log("Baking process complete.");
            }
            finally
            {
                context.Owner.HandleBakingFinished();
            }
        }

        private void OptimisticDelete(string path)
        {
            var until = Environment.TickCount + 5000;
            while (Environment.TickCount < until)
            {
                try
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                        return;
                    }
                    else
                        return;
                }
                catch 
                {
                }

                // wait...
                Thread.Sleep(250);
            }

            throw new InvalidOperationException(string.Format("The file '{0}' could not be deleted.", path));
        }
    }
}
