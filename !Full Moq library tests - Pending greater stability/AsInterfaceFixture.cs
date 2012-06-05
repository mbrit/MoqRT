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
    public class AsInterfaceFixture
	{
		[TestMethod]
		public void ShouldThrowIfAsIsInvokedAfterInstanceIsRetrieved()
		{
			var mock = new Mock<IBag>();

			var instance = mock.Object;

			AssertHelper.Throws<InvalidOperationException>(() => mock.As<IFoo>());
		}

		[TestMethod]
		public void ShouldThrowIfAsIsInvokedWithANonInterfaceTypeParameter()
		{
			var mock = new Mock<IBag>();

			AssertHelper.Throws<ArgumentException>(() => mock.As<object>());
		}

		[TestMethod]
		public void ShouldExpectGetOnANewInterface()
		{
			var mock = new Mock<IBag>();

			bool called = false;

			mock.As<IFoo>().SetupGet(x => x.Value)
				.Callback(() => called = true)
				.Returns(25);

			Assert.AreEqual(25, ((IFoo)mock.Object).Value);
			Assert.IsTrue(called);
		}

		[TestMethod]
		public void ShouldExpectCallWithArgumentOnNewInterface()
		{
			var mock = new Mock<IBag>();
			mock.As<IFoo>().Setup(x => x.Execute("ping")).Returns("ack");

			Assert.AreEqual("ack", ((IFoo)mock.Object).Execute("ping"));
		}

		[TestMethod]
		public void ShouldExpectPropertySetterOnNewInterface()
		{
			bool called = false;
			int value = 0;
			var mock = new Mock<IBag>();
			mock.As<IFoo>().SetupSet(x => x.Value = 100).Callback<int>(i => { value = i; called = true; });

			((IFoo)mock.Object).Value = 100;

			Assert.AreEqual(100, value);
			Assert.IsTrue(called);
		}

		[TestMethod]
		public void MockAsExistingInterfaceAfterObjectSucceedsIfNotNew()
		{
			var mock = new Mock<IBag>();

			mock.As<IFoo>().SetupGet(x => x.Value).Returns(25);

			Assert.AreEqual(25, ((IFoo)mock.Object).Value);

			var fm = mock.As<IFoo>();

			fm.Setup(f => f.Execute());
		}

		[TestMethod]
		public void ThrowsWithTargetTypeName()
		{
			var bag = new Mock<IBag>();
			var foo = bag.As<IFoo>();

			bag.Setup(b => b.Add("foo", "bar")).Verifiable();
			foo.Setup(f => f.Execute()).Verifiable();

			try
			{
				bag.Verify();
			}
			catch (MockVerificationException me)
			{
				AssertHelper.Contains(typeof(IFoo).Name, me.Message);
				AssertHelper.Contains(typeof(IBag).Name, me.Message);
			}
		}

		[TestMethod]
		public void GetMockFromAddedInterfaceWorks()
		{
			var bag = new Mock<IBag>();
			var foo = bag.As<IFoo>();

			foo.SetupGet(x => x.Value).Returns(25);

			IFoo f = bag.Object as IFoo;

			var foomock = Mock.Get(f);

			Assert.IsNotNull(foomock);
		}

		[TestMethod]
		public void GetMockFromNonAddedInterfaceThrows()
		{
			var bag = new Mock<IBag>();
			bag.As<IFoo>();
			bag.As<IComparable>();
			object b = bag.Object;

			AssertHelper.Throws<ArgumentException>(() => Mock.Get(b));
		}

		[TestMethod]
		public void VerifiesExpectationOnAddedInterface()
		{
			var bag = new Mock<IBag>();
			var foo = bag.As<IFoo>();

			foo.Setup(f => f.Execute()).Verifiable();

			AssertHelper.Throws<MockVerificationException>(() => foo.Verify());
			AssertHelper.Throws<MockVerificationException>(() => foo.VerifyAll());
			AssertHelper.Throws<MockException>(() => foo.Verify(f => f.Execute()));

			foo.Object.Execute();

			foo.Verify();
			foo.VerifyAll();
			foo.Verify(f => f.Execute());
		}

		[TestMethod]
		public void VerifiesExpectationOnAddedInterfaceCastedDynamically()
		{
			var bag = new Mock<IBag>();
			bag.As<IFoo>();

			((IFoo)bag.Object).Execute();

			bag.As<IFoo>().Verify(f => f.Execute());
		}

		public interface IFoo
		{
			void Execute();
			string Execute(string command);
			string Execute(string arg1, string arg2);
			string Execute(string arg1, string arg2, string arg3);
			string Execute(string arg1, string arg2, string arg3, string arg4);

			int Value { get; set; }
		}

		public interface IBag
		{
			void Add(string key, object o);
			object Get(string key);
		}
	}
}
