using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MoqRT.Baking.Data;

namespace MoqRT.Baking
{
    internal class BakingPoke : MarshalByRefObject, IBakingPoke
    {
        public void Bake(BakingController owner, BakingSettings settings)
        {
            owner.HandleBakingStarted();
            owner.Log("Starting baking process...");
            try
            {
                using (new Resolver(settings))
                {
                    // find the WinRT version of the library...
                    Type rt = null;
                    var asm = Assembly.LoadFrom(settings.AssemblyPath);
                    foreach (var referenceName in asm.GetReferencedAssemblies().ToList().Where(v => v.Name.StartsWith("MoqRT")))
                    {
                        var reference = Assembly.Load(referenceName);

                        // get...
                        rt = reference.GetType("MoqRT.MoqRTRuntime");
                        if (rt != null)
                            break;
                    }

                    // ensure...
                    SQLiteHelper.EnsureSQLiteInFolder(settings.AssemblyFolder);

                    // run...
                    owner.Log("Initializing...");
                    var initializeMethod = rt.GetMethod("InitializeBaking", BindingFlags.Static | BindingFlags.Public);
                    initializeMethod.Invoke(null, new object[] { settings.AssemblyPath, settings.AppxPath });

                    //// more...
                    //var setRunningMethodMethod = bakerType.GetMethod("SetRunningMethod", BindingFlags.Instance | BindingFlags.Public);

                    // walk...
                    var testInstances = new List<object>();
                    foreach (var type in asm.GetTypes())
                    {
                        // walk the attributes...
                        foreach (var attr in type.GetCustomAttributes())
                        {
                            if (attr.GetType().Name == "TestClassAttribute") // fudge - easier than referencing the WinRT test framework...
                            {
                                try
                                {
                                    owner.Log(string.Format("Found test class '{0}'...", type.FullName));
                                    testInstances.Add(Activator.CreateInstance(type));
                                }
                                catch (Exception ex)
                                {
                                    owner.Log(string.Format("Failed to create instance of '{0}' --> {1}", type, ex.Message));
                                }
                            }
                        }
                    }

                    // walk each type...
                    foreach (var testInstance in testInstances)
                    {
                        foreach (var method in testInstance.GetType().GetMethods())
                        {
                            foreach (var attr in method.GetCustomAttributes())
                            {
                                if (attr.GetType().Name == "TestMethodAttribute")
                                {
                                    owner.Log(string.Format("Running '{0}' on '{1}'...", method.Name, testInstance.GetType().Name));

                                    // tell it we're doing it...
                                    //setRunningMethodMethod.Invoke(baker, new object[] { testInstance, method });

                                    // run it...
                                    try
                                    {
                                        method.Invoke(testInstance, null);
                                        owner.Log("...No failure");
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

                                        owner.Log(builder.ToString());
                                    }
                                }
                            }
                        }
                    }

                    // finish...
                    owner.Log("Finishing up...");
                    var finishMethod = rt.GetMethod("FinishBaking", BindingFlags.Static | BindingFlags.Public);
                    string bakedPath = (string)finishMethod.Invoke(null, null);

                    owner.Log("Baking process complete. Baked assembly at: " + bakedPath);
                }
            }
            finally
            {
                owner.HandleBakingFinished();
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

    }
}
