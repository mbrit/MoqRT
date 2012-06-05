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
    public class MockedFixture
	{
		[TestMethod]
		public void InterfaceMockedShouldImplementMocked()
		{
			Mock<IFoo> mock = new Mock<IFoo>();
			IFoo mocked = mock.Object;
			Assert.IsTrue(mocked is IMocked<IFoo>);
		}

		[TestMethod]
		public void MockOfMockedInterfaceShouldReturnSame()
		{
			Mock<IFoo> mock = new Mock<IFoo>();
			IMocked<IFoo> mocked = mock.Object as IMocked<IFoo>;
			AssertHelper.Same(mock, mocked.Mock);
		}

		[TestMethod]
		public void ClassMockedShouldImplementMocked()
		{
			Mock<Foo> mock = new Mock<Foo>();
			Foo mocked = mock.Object;
			Assert.IsTrue(mocked is IMocked<Foo>);
		}

		[TestMethod]
		public void MockOfMockedClassShouldReturnSame()
		{
			Mock<Foo> mock = new Mock<Foo>();
			IMocked<Foo> mocked = mock.Object as IMocked<Foo>;
			AssertHelper.Same(mock, mocked.Mock);
		}

		public class FooWithCtor
		{
			public FooWithCtor(int a) { }
		}

		[TestMethod]
		public void ClassWithCtorMockedShouldImplementMocked()
		{
			Mock<FooWithCtor> mock = new Mock<FooWithCtor>(5);
			FooWithCtor mocked = mock.Object;
			Assert.IsTrue(mocked is IMocked<FooWithCtor>);
		}

		[TestMethod]
		public void MockOfMockedClassWithCtorShouldReturnSame()
		{
			Mock<FooWithCtor> mock = new Mock<FooWithCtor>(5);
			IMocked<FooWithCtor> mocked = mock.Object as IMocked<FooWithCtor>;
			AssertHelper.Same(mock, mocked.Mock);
		}

		[TestMethod]
		public void GetReturnsMockForAMocked()
		{
			var mock = new Mock<IFoo>();
			var mocked = mock.Object;
			AssertHelper.Same(mock, Mock.Get(mocked));
		}

		[TestMethod]
		public void GetReturnsMockForAMockedAbstract()
		{
			var mock = new Mock<FooBase>();
			var mocked = mock.Object;
			AssertHelper.Same(mock, Mock.Get(mocked));
		}

		[TestMethod]
		public void GetThrowsIfObjectIsNotMocked()
		{
			AssertHelper.Throws<ArgumentException>(
				"Object instance was not created by Moq.\r\nParameter name: mocked",
				() => Mock.Get("foo"));
		}

		public class FooBase
		{
		}
		
		public class Foo : FooBase
		{
		}
		
		public interface IFoo
		{
		}
	}
}