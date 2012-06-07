using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Baking
{
    [Serializable]
    internal class BakingWorkItem : WorkItem
    {
        internal BakingWorkItem(BakingSettings settings, DateTime atOrAfter)
            : base(settings, atOrAfter)
        {
        }

        internal override void Run(BakingContext context)
        {
            context.Owner.HandleBakingStarted();
            context.Owner.Log("Starting baking process...");
            try
            {
                // run...
                context.Owner.Log("Initializing...");
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
                                            context.Owner.Log("...No failure");
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

                                            context.Owner.Log(builder.ToString());
                                        }
                                    }
                                }
                                else
                                    context.Log(string.Format("Ignoring method '{0}'...", c.Name));
                            }
                        }
                    }
                    else
                        context.Log(string.Format("Ignoring class '{0}'...", c.Name));
                }

                // finish...
                context.Owner.Log("Finishing up...");
                var finishMethod = context.RuntimeType.GetMethod("FinishBaking", BindingFlags.Static | BindingFlags.Public);
                string bakedPath = (string)finishMethod.Invoke(null, null);
                context.Owner.Log("Baking process complete.");

                // copy...
                var finalPath = Path.Combine(this.Settings.AppxPath, this.Settings.AssemblyFilename);
                if (File.Exists(finalPath))
                    File.Delete(finalPath);
                File.Copy(bakedPath, finalPath);
                context.Owner.Log("Final target assembly: " + this.Settings.AppxPath);

                const string databaseName = "MoqRT.Baked.dll.db";
                var dbPath = Path.Combine(this.Settings.BakingPath, databaseName);
                var finalDbPath = Path.Combine(this.Settings.AppxPath, databaseName);
                if (File.Exists(finalDbPath))
                    File.Delete(finalDbPath);
                File.Copy(dbPath, finalDbPath);
                context.Owner.Log("Final target tracking database: " + finalDbPath);
            }
            finally
            {
                context.Owner.HandleBakingFinished();
            }
        }
    }
}
