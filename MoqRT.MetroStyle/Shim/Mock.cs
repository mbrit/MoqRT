using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Moq.Baking;

namespace Moq
{
    // @mbrit - 2012-06-02 - we're abstract because we're always generated...
    public abstract class MockX
    {
        private IDefaultValueProvider defaultValueProvider = new EmptyDefaultValueProvider();

        protected MockX()
        {
        }

        public static Mock<T> Create<T>()
            where T : class
        {
            if (MoqRTRuntime.IsBaking)
                return new BakingMock<T>();
            else
            {
                // this is where we'd punt over to an IoC container...
                var name = new AssemblyName()
                {
                    Name = "MoqRTPoc.WinRTTest.Cheating"
                };
                var asm = Assembly.Load(name);
                var type = asm.GetType("MoqRTPoc.Baked.Instances.BakedForPocTestTestDoMagic");
                return (Mock<T>)Activator.CreateInstance(type);
            }
        }
    }
}
