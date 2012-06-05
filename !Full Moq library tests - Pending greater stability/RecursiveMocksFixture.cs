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
    public class RecursiveMocksFixture
	{
		[TestMethod]
		public void CreatesMockForAccessedProperty()
		{
			var mock = new Mock<IFoo>();

			mock.SetupGet(m => m.Bar.Value).Returns(5);

			Assert.AreEqual(5, mock.Object.Bar.Value);
		}

		[TestMethod]
		public void RetrievesSameMockForProperty()
		{
			var mock = new Mock<IFoo> { DefaultValue = DefaultValue.Mock };

			var b1 = mock.Object.Bar;
			var b2 = mock.Object.Bar;

			AssertHelper.Same(b1, b2);
		}

		[TestMethod]
		public void NewMocksHaveSameBehaviorAndDefaultValueAsOwner()
		{
			var mock = new Mock<IFoo>();

			mock.SetupGet(m => m.Bar.Value).Returns(5);

			var barMock = Mock.Get(mock.Object.Bar);

			Assert.AreEqual(mock.Behavior, barMock.Behavior);
			Assert.AreEqual(mock.DefaultValue, barMock.DefaultValue);
		}


		[TestMethod]
		public void CreatesMockForAccessedPropertyWithMethod()
		{
			var mock = new Mock<IFoo>();

			mock.Setup(m => m.Bar.Do("ping")).Returns("ack");
			mock.Setup(m => m.Bar.Baz.Do("ping")).Returns("ack");

			Assert.AreEqual("ack", mock.Object.Bar.Do("ping"));
			Assert.AreEqual("ack", mock.Object.Bar.Baz.Do("ping"));
			Assert.AreEqual(default(string), mock.Object.Bar.Do("foo"));
		}

		[TestMethod]
		public void CreatesMockForAccessedPropertyWithVoidMethod()
		{
			var mock = new Mock<IFoo>();

			mock.Setup(m => m.Bar.Baz.Do());

			//AssertHelper.Throws<MockVerificationException>(() => mock.VerifyAll());

			Assert.IsNotNull(mock.Object.Bar);
			Assert.IsNotNull(mock.Object.Bar.Baz);

			mock.Object.Bar.Baz.Do();

			mock.Verify(m => m.Bar.Baz.Do());
		}

		[TestMethod]
		public void CreatesMockForAccessedPropertyWithSetterWithValue()
		{
			var mock = new Mock<IFoo>();

			mock.SetupSet(m => m.Bar.Value = 5);

			Assert.IsNotNull(mock.Object.Bar);
			AssertHelper.Throws<MockVerificationException>(() => mock.VerifyAll());

			mock.Object.Bar.Value = 5;

			mock.VerifyAll();
		}

		[TestMethod]
		public void CreatesMockForAccessedPropertyWithSetter()
		{
			var mock = new Mock<IFoo>();

			mock.SetupSet(m => m.Bar.Value = It.IsAny<int>());

			Assert.IsNotNull(mock.Object.Bar);
			AssertHelper.Throws<MockVerificationException>(() => mock.VerifyAll());

			mock.Object.Bar.Value = 5;

			mock.VerifyAll();
		}

		[TestMethod]
		public void VerifiesAllHierarchy()
		{
			var mock = new Mock<IFoo>();

			mock.Setup(m => m.Bar.Do("ping")).Returns("ack");
			mock.Setup(m => m.Do("ping")).Returns("ack");

			mock.Object.Do("ping");
			var bar = mock.Object.Bar;

			AssertHelper.Throws<MockVerificationException>(() => mock.VerifyAll());
		}

		[TestMethod]
		public void VerifiesHierarchy()
		{
			var mock = new Mock<IFoo>();

			mock.Setup(m => m.Bar.Do("ping")).Returns("ack").Verifiable();
			mock.Setup(m => m.Do("ping")).Returns("ack");

			mock.Object.Do("ping");
			var bar = mock.Object.Bar;

			AssertHelper.Throws<MockVerificationException>(() => mock.Verify());
		}

		[TestMethod]
		public void VerifiesHierarchyMethodWithExpression()
		{
			var mock = new Mock<IFoo>();

			AssertHelper.Throws<MockException>(() => mock.Verify(m => m.Bar.Do("ping")));

			mock.Object.Bar.Do("ping");
			mock.Verify(m => m.Bar.Do("ping"));
		}

		[TestMethod]
		public void VerifiesHierarchyPropertyGetWithExpression()
		{
			var mock = new Mock<IFoo>();

			AssertHelper.Throws<MockException>(() => mock.VerifyGet(m => m.Bar.Value));

			var value = mock.Object.Bar.Value;
			mock.VerifyGet(m => m.Bar.Value);
		}

		[TestMethod]
		public void VerifiesHierarchyPropertySetWithExpression()
		{
			var mock = new Mock<IFoo>();

			AssertHelper.Throws<MockException>(() => mock.VerifySet(m => m.Bar.Value = It.IsAny<int>()));

			mock.Object.Bar.Value = 5;
			mock.VerifySet(m => m.Bar.Value = It.IsAny<int>());
		}

		[TestMethod]
		public void VerifiesReturnWithExpression()
		{
			var mock = new Mock<IFoo>();

			mock.Setup(m => m.Bar.Do("ping")).Returns("ack").Verifiable();

			AssertHelper.Throws<MockException>(() => mock.Verify(m => m.Bar.Do("ping")));

			var result = mock.Object.Bar.Do("ping");

			Assert.AreEqual("ack", result);
			mock.Verify(m => m.Bar.Do("ping"));
		}

		[TestMethod]
		public void VerifiesVoidWithExpression()
		{
			var mock = new Mock<IFoo>();

			mock.Setup(m => m.Bar.Baz.Do());

			AssertHelper.Throws<MockException>(() => mock.Verify(m => m.Bar.Baz.Do()));

			mock.Object.Bar.Baz.Do();

			mock.Verify(m => m.Bar.Baz.Do());
		}

		[TestMethod]
		public void VerifiesGetWithExpression()
		{
			var mock = new Mock<IFoo>();

			mock.SetupGet(m => m.Bar.Value).Returns(5);

			AssertHelper.Throws<MockException>(() => mock.VerifyGet(m => m.Bar.Value));

			var result = mock.Object.Bar.Value;

			Assert.AreEqual(5, result);

			mock.VerifyGet(m => m.Bar.Value);
		}

		[TestMethod]
		public void VerifiesGetWithExpression2()
		{
			var mock = new Mock<IFoo>();

			mock.Setup(m => m.Bar.Value).Returns(5);

			AssertHelper.Throws<MockException>(() => mock.Verify(m => m.Bar.Value));

			var result = mock.Object.Bar.Value;

			Assert.AreEqual(5, result);

			mock.Verify(m => m.Bar.Value);
		}

		[TestMethod]
		public void VerifiesSetWithExpression()
		{
			var mock = new Mock<IFoo>();

			mock.SetupSet(m => m.Bar.Value = It.IsAny<int>());

			AssertHelper.Throws<MockException>(() => mock.VerifySet(m => m.Bar.Value = It.IsAny<int>()));

			mock.Object.Bar.Value = 5;

			mock.VerifySet(m => m.Bar.Value = It.IsAny<int>());
		}

		[TestMethod]
		public void VerifiesSetWithExpressionAndValue()
		{
			var mock = new Mock<IFoo>();

			mock.SetupSet(m => m.Bar.Value = 5);

			AssertHelper.Throws<MockException>(() => mock.VerifySet(m => m.Bar.Value = 5));

			mock.Object.Bar.Value = 5;

			mock.VerifySet(m => m.Bar.Value = 5);
		}

		[TestMethod]
		public void FieldAccessNotSupported()
		{
			var mock = new Mock<Foo>();

			AssertHelper.Throws<NotSupportedException>(() => mock.Setup(m => m.BarField.Do("ping")));
		}

		[TestMethod]
		public void NonMockeableTypeThrows()
		{
			var mock = new Mock<IFoo>();

			AssertHelper.Throws<NotSupportedException>(() => mock.Setup(m => m.Bar.Value.ToString()));
		}

		[TestMethod]
		public void IntermediateIndexerAccessIsSupported()
		{
			var mock = new Mock<IFoo>();

			mock.Setup(m => m[0].Do("ping")).Returns("ack");

			var result = mock.Object[0].Do("ping");

			Assert.AreEqual("ack", result);
		}

		[TestMethod]
		public void IntermediateMethodInvocationAreSupported()
		{
			var mock = new Mock<IFoo>();

			mock.Setup(m => m.GetBar().Do("ping")).Returns("ack");

			var result = mock.Object.GetBar().Do("ping");

			Assert.AreEqual("ack", result);
		}

		[TestMethod]
		public void FullMethodInvocationsSupportedInsideFluent()
		{
			var fooMock = new Mock<IFoo>(MockBehavior.Strict);
			fooMock.Setup(f => f.Bar.GetBaz("hey").Value).Returns(5);

			Assert.AreEqual(5, fooMock.Object.Bar.GetBaz("hey").Value);
		}

		[TestMethod]
		public void FullMethodInvocationInsideFluentCanUseMatchers()
		{
			var fooMock = new Mock<IFoo>(MockBehavior.Strict);
			fooMock.Setup(f => f.Bar.GetBaz(It.IsAny<string>()).Value).Returns(5);

			Assert.AreEqual(5, fooMock.Object.Bar.GetBaz("foo").Value);
		}

		public class Foo : IFoo
		{

			public IBar BarField;
			public IBar Bar { get; set; }
			public IBar GetBar() { return null; }
			public IBar this[int index] { get { return null; } set { } }

			public string Do(string command)
			{
				throw new NotImplementedException();
			}
		}

		public interface IFoo
		{
			IBar Bar { get; set; }
			IBar this[int index] { get; set; }
			string Do(string command);
			IBar GetBar();
		}

		public interface IBar
		{
			int Value { get; set; }
			string Do(string command);
			IBaz Baz { get; set; }
			IBaz GetBaz(string value);
		}

		public interface IBaz
		{
			int Value { get; set; }
			string Do(string command);
			void Do();
		}
	}
}
