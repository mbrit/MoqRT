using System;
using System.Diagnostics;
using System.Reflection;
using Moq;
using MoqRT;

#if !NETFX_CORE
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif

namespace Moq.Tests
{
    [TestClass]
    public class SequenceExtensionsFixture
	{
		[TestMethod]
		public void PerformSequence()
		{
			var mock = new Mock<IFoo>();

			mock.SetupSequence(x => x.Do())
				.Returns(2)
				.Returns(3)
				.Throws<InvalidOperationException>();

			Assert.AreEqual(2, mock.Object.Do());
			Assert.AreEqual(3, mock.Object.Do());
			AssertHelper.Throws<InvalidOperationException>(() => mock.Object.Do());
		}

		[TestMethod]
		public void PerformSequenceOnProperty()
		{
			var mock = new Mock<IFoo>();

			mock.SetupSequence(x => x.Value)
				.Returns("foo")
				.Returns("bar")
#if !NETFX_CORE
				.Throws<SystemException>();
#else
                .Throws<Exception>();
#endif

			string temp;
			Assert.AreEqual("foo", mock.Object.Value);
			Assert.AreEqual("bar", mock.Object.Value);

#if !NETFX_CORE
            AssertHelper.Throws<SystemException>(() => temp = mock.Object.Value);
#else
            AssertHelper.Throws<Exception>(() => temp = mock.Object.Value);
#endif
		}

		public interface IFoo
		{
			string Value { get; set; }
			int Do();
		}
	}
}