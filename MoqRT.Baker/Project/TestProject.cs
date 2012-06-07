using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Baking
{
    [Serializable]
    public class TestProject : TestItem
    {
        public List<TestClass> Classes { get; private set; }
        public List<TestSession> Sessions { get; private set; }

        internal TestProject(Assembly asm, ILog log)
            : base(asm.FullName)
        {
            this.Classes = new List<TestClass>();
            this.Sessions = new List<TestSession>();

            // walk...
            foreach (var type in asm.GetTypes())
            {
                // walk the attributes...
                var names = new List<string>();
                foreach (var attr in type.GetCustomAttributes(false))
                    names.Add(attr.GetType().Name);

                // fudge - easier than referencing the VS test framework...
                if(!(names.Contains("IgnoreBakingAttribute")))
                {
                    if (names.Contains("TestClassAttribute")) 
                    {
                        log.Log(string.Format("Found test class '{0}'...", type.FullName));
                        var testClass = new TestClass(type, log);
                        this.Classes.Add(testClass);

                        // if...
                        if (names.Contains("TestSessionAttribute"))
                        {
                            throw new NotImplementedException("This operation has not been implemented.");
                        }
                    }
                }
            }
        }

        internal void PatchIncludes(TestProject donor)
        {
            if (donor.Name != this.Name)
                return;

            // walk...
            foreach (var donorClass in donor.Classes)
            {
                var ourClass = this.Classes.Where(v => v.Name == donorClass.Name).FirstOrDefault();
                if(ourClass != null)
                {
                    ourClass.Include = donorClass.Include;

                    // walk...
                    foreach(var donorMethod in donorClass.Methods)
                    {
                        var ourMethod = ourClass.Methods.Where(v => v.Name == donorMethod.Name).FirstOrDefault();
                        if(ourMethod != null)
                            ourMethod.Include = donorMethod.Include;
                    }
                }
            }
        }
    }
}
