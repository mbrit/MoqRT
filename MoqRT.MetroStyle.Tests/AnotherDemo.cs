using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

#if !NETFX_CORE
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif

namespace Moq.Tests
{
    [TestClass]
    public class AnotherDemo
    {
        [TestMethod]
        public void TestMore()
        {
            var mock = new Mock<IFoo>();
            mock.Setup(foo => foo.DoSomething("ping")).Returns(false);

            Assert.IsTrue(mock.Object.DoSomething("ping"));
        }

        [TestMethod]
        public void TestYetMore()
        {
            var mock = new Mock<IFoo>();
            mock.Setup(foo => foo.DoSomething("ping")).Returns(false);

            Assert.IsTrue(mock.Object.DoSomething("ping"));
        }

        [TestMethod]
        public void TestYetEvenMore()
        {
            var mock = new Mock<IFoo2>();
            mock.Setup(foo => foo.DoSomething("ping")).Returns(true);

            Assert.IsTrue(mock.Object.DoSomething("ping"));
        }

        [TestMethod]
        public void TestYetEvenMore3()
        {
            var mock = new Mock<IFoo3>();
            mock.Setup(foo => foo.DoSomething("ping")).Returns(true);

            Assert.IsTrue(mock.Object.DoSomething("ping"));
        }

        [TestMethod]
        public void TestYetEvenMore4()
        {
            var mock = new Mock<IFoo3>();
            mock.Setup(foo => foo.DoSomethingElse(27)).Returns("abc");

            Assert.AreEqual("abc", mock.Object.DoSomethingElse(27));
        }

        public interface IFoo
        {
            bool DoSomething(string p);
            string DoSomethingElse(int i);
        }

        public interface IFoo2 : IFoo
        {
        }

        public interface IFoo3 : IFoo2
        {
        }

        public interface IFoo4 : IFoo2
        {
        }
    }
}
