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
    public class OutRefFixture
	{
		[TestMethod]
		public void ExpectsOutArgument()
		{
			var mock = new Mock<IFoo>();
			var expected = "ack";

			mock.Setup(m => m.Execute("ping", out expected)).Returns(true);

			string actual;
			var ok = mock.Object.Execute("ping", out actual);

			Assert.IsTrue(ok);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void ExpectsOutEagerlyEvaluates()
		{
			var mock = new Mock<IFoo>();
			string expected = "ack";

			mock.Setup(m => m.Execute("ping", out expected)).Returns(true);

			expected = "foo";

			string actual;
			bool ok = mock.Object.Execute("ping", out actual);

			Assert.IsTrue(ok);
			Assert.AreEqual("ack", actual);
		}

		[TestMethod]
		public void ExpectsRefArgument()
		{
			var mock = new Mock<IFoo>();
			string expected = "ack";

			mock.Setup(m => m.Echo(ref expected)).Returns<string>(s => s);

			string actual = mock.Object.Echo(ref expected);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void RefOnlyMatchesSameInstance()
		{
			var mock = new Mock<IFoo>(MockBehavior.Strict);
			string expected = "ack";

			mock.Setup(m => m.Echo(ref expected)).Returns<string>(s => s);

			string actual = null;
			AssertHelper.Throws<MockException>(() => mock.Object.Echo(ref actual));
		}

		[TestMethod]
		public void RefTakesGuidParameter()
		{
			var mock = new Mock<IFoo>(MockBehavior.Strict);
			var expected = Guid.NewGuid();

			mock.Setup(m => m.GuidMethod(ref expected)).Returns(true);

			Assert.AreEqual(true, mock.Object.GuidMethod(ref expected));
		}

		[TestMethod]
		public void RefWorksWithOtherValueTypes()
		{
			var mock = new Mock<IFoo>(MockBehavior.Strict);
			var expected = 5;

			mock.Setup(m => m.IntMethod(ref expected)).Returns(true);

			Assert.AreEqual(true, mock.Object.IntMethod(ref expected));
		}

		// ThrowsIfOutIsNotConstant
		// ThrowsIfRefIsNotConstant

		public interface IFoo
		{
			T Echo<T>(ref T value);
			bool Execute(string command, out string result);
			void Submit(string command, ref string result);
			int Value { get; set; }
			bool GuidMethod(ref Guid guid);
			bool IntMethod(ref int value);
		}
	}
}
