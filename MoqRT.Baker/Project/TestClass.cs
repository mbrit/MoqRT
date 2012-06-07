using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Baking
{
    [Serializable]
    public class TestClass : TestItem
    {
        public List<TestMethod> Methods { get; private set; }
        public TestMethod InitializeMethod { get; private set; }
        public TestMethod CleanupMethod { get; private set; }

        internal TestClass(Type type, ILog log)
            : base(type.FullName)
        {
            this.Methods = new List<TestMethod>();

            // walk the methods...
            foreach (var method in type.GetMethods())
            {
                var names = new List<string>();
                foreach (var attr in method.GetCustomAttributes(false))
                    names.Add(attr.GetType().Name);

                // fudge - easier than referencing the VS test framework...
                if (!(names.Contains("IgnoreBakingAttribute")) && !(names.Contains("IgnoreAttribute")))
                {
                    // if...
                    if (names.Contains("TestMethodAttribute")) 
                    {
                        log.Log(string.Format("Found test method '{0}'...", method.Name));
                        this.Methods.Add(new TestMethod(method, log));
                    }

                    // if...
                    if (names.Contains("TestInitializeAttribute"))
                    {
                        log.Log(string.Format("Found initialize method '{0}'...", method.Name));
                        this.Methods.Add(new TestMethod(method, log));
                    }

                    // if...
                    if (names.Contains("TestCleanupAttribute"))
                    {
                        log.Log(string.Format("Found cleanup method '{0}'...", method.Name));
                        this.Methods.Add(new TestMethod(method, log));
                    }
                }
            }
        }
    }
}
