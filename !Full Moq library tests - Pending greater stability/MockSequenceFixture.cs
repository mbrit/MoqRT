using System;
using System.Diagnostics;
using System.Reflection;
using MoqRT;
using Moq;

#if !NETFX_CORE
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif

namespace Moq.Tests
{
    [TestClass]
    public class MockSequenceFixture
	{
		[TestMethod]
		public void RightSequenceSuccess()
		{
			var a = new Mock<IFoo>(MockBehavior.Strict);
			var b = new Mock<IFoo>(MockBehavior.Strict);

			var sequence = new MockSequence();
			a.InSequence(sequence).Setup(x => x.Do(100)).Returns(101);
			b.InSequence(sequence).Setup(x => x.Do(200)).Returns(201);

			a.Object.Do(100);
			b.Object.Do(200);
		}

		[TestMethod]
		public void InvalidSequenceFail()
		{
			var a = new Mock<IFoo>(MockBehavior.Strict);
			var b = new Mock<IFoo>(MockBehavior.Strict);

			var sequence = new MockSequence();
			a.InSequence(sequence).Setup(x => x.Do(100)).Returns(101);
			b.InSequence(sequence).Setup(x => x.Do(200)).Returns(201);

			AssertHelper.Throws<MockException>(() => b.Object.Do(200));
		}

		[TestMethod]
		public void NoCyclicSequenceFail()
		{
			var a = new Mock<IFoo>(MockBehavior.Strict);
			var b = new Mock<IFoo>(MockBehavior.Strict);

			var sequence = new MockSequence();
			a.InSequence(sequence).Setup(x => x.Do(100)).Returns(101);
			b.InSequence(sequence).Setup(x => x.Do(200)).Returns(201);

			Assert.AreEqual(101, a.Object.Do(100));
			Assert.AreEqual(201, b.Object.Do(200));

			AssertHelper.Throws<MockException>(() => a.Object.Do(100));
			AssertHelper.Throws<MockException>(() => b.Object.Do(200));
		}

		[TestMethod]
		public void CyclicSequenceSuccesss()
		{
			var a = new Mock<IFoo>(MockBehavior.Strict);
			var b = new Mock<IFoo>(MockBehavior.Strict);

			var sequence = new MockSequence { Cyclic = true };
			a.InSequence(sequence).Setup(x => x.Do(100)).Returns(101);
			b.InSequence(sequence).Setup(x => x.Do(200)).Returns(201);

			Assert.AreEqual(101, a.Object.Do(100));
			Assert.AreEqual(201, b.Object.Do(200));

			Assert.AreEqual(101, a.Object.Do(100));
			Assert.AreEqual(201, b.Object.Do(200));

			Assert.AreEqual(101, a.Object.Do(100));
			Assert.AreEqual(201, b.Object.Do(200));
		}

		public interface IFoo
		{
			int Do(int arg);
		}
	}
}