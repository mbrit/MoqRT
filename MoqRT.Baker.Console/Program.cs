using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;

namespace MoqRTPoc.Baker
{
    class Program
    {
        private static string RootFolder = @"C:\Code\MoqRT\MoqRT.MetroStyle.Tests\bin\Debug\AppX\";

        static void Main(string[] args)
        {
            try
            {
                // load the test assembly...
                var path = Path.Combine(RootFolder, "MoqRT.MetroStyle.Tests.dll");
                var asm = Assembly.LoadFrom(path);

                // find the moqrt runtime class - we have to do this via reflection...
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

                // load real moq...
                Type rt = null;
                foreach (var referenceName in asm.GetReferencedAssemblies().ToList().Where(v => v.Name.StartsWith("MoqRT")))
                {
                    var reference = Assembly.Load(referenceName);

                    // get...
                    rt = reference.GetType("MoqRT.MoqRTRuntime");
                    if (rt != null)
                        break;
                }

                // run...
                var initializeMethod = rt.GetMethod("InitializeBaking", BindingFlags.Static | BindingFlags.Public);

                // THIS NEEDS CHANGING - the path needs to be where it's COMPILED TO, not where
                // it's DEPLOYED TO...
                initializeMethod.Invoke(null, new object[] { path, RootFolder });

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
                                testInstances.Add(Activator.CreateInstance(type));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
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
                                Console.Write(string.Format("Running '{0}' on '{1}': ", method.Name, testInstance.GetType().Name));

                                // tell it we're doing it...
                                //setRunningMethodMethod.Invoke(baker, new object[] { testInstance, method });

                                // run it...
                                try
                                {
                                    method.Invoke(testInstance, null);
                                    Console.Write("No failure");
                                }
                                catch (Exception ex)
                                {
                                    bool first = true;
                                    var walk = ex;
                                    while (walk != null)
                                    {
                                        if (first)
                                            first = false;
                                        else
                                            Console.Write(", ");

                                        Console.Write(walk.GetType().Name);
                                        walk = walk.InnerException;
                                    }
                                }

                                Console.WriteLine();
                            }
                        }
                    }
                }

                // finish...
                var finishMethod = rt.GetMethod("FinishBaking", BindingFlags.Static | BindingFlags.Public);
                finishMethod.Invoke(null, null);

                // load...
                Console.WriteLine("=========================================");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (Debugger.IsAttached)
                    Console.ReadLine();
            }
        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string name = args.Name;
            int index = args.Name.IndexOf(",");
            if (index != -1)
                name = name.Substring(0, index);
            string path = Path.Combine(@"C:\Code\MoqRT\MoqRT.MetroStyle\bin\Debug", name + ".dll");
            if (File.Exists(path))
                return Assembly.LoadFrom(path);
            else
                return null;
        }

        static void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(e.FullPath);
        }
    }
}
