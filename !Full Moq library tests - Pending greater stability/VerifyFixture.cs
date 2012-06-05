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
    public class VerifyFixture
	{
		[TestMethod]
		public void ThrowsIfVerifiableExpectationNotCalled()
		{
			var mock = new Mock<IFoo>();

			mock.Setup(x => x.Submit()).Verifiable();

			var mex = AssertHelper.Throws<MockVerificationException>(() => mock.Verify());
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
		}

		[TestMethod]
		public void ThrowsIfVerifiableExpectationNotCalledWithMessage()
		{
			var mock = new Mock<IFoo>();

			mock.Setup(x => x.Submit()).Verifiable("Kaboom!");

			var mex = AssertHelper.Throws<MockVerificationException>(() => mock.Verify());
			AssertHelper.Contains("Kaboom!", mex.Message);
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
		}

		[TestMethod]
		public void ThrowsWithEvaluatedExpressionsIfVerifiableExpectationNotCalled()
		{
			var expectedArg = "lorem,ipsum";
			var mock = new Mock<IFoo>();

			mock.Setup(x => x.Execute(expectedArg.Substring(0, 5)))
				.Returns("ack")
				.Verifiable();

			var mex = AssertHelper.Throws<MockVerificationException>(() => mock.Verify());
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
			Assert.IsTrue(mex.Message.Contains(@".Execute(""lorem"")"), "Contains evaluated expected argument.");
		}

		[TestMethod]
		public void ThrowsWithExpressionIfVerifiableExpectationWithLambdaMatcherNotCalled()
		{
			var mock = new Mock<IFoo>();

			mock.Setup(x => x.Execute(It.Is<string>(s => string.IsNullOrEmpty(s))))
				.Returns("ack")
				.Verifiable();

			var mex = AssertHelper.Throws<MockVerificationException>(() => mock.Verify());
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
			AssertHelper.Contains(@".Execute(It.Is<String>(s => String.IsNullOrEmpty(s)))", mex.Message);
		}

		[TestMethod]
		public void VerifiesNoOpIfNoVerifiableExpectations()
		{
			var mock = new Mock<IFoo>();

			mock.Verify();
		}

		[TestMethod]
		public void ThrowsIfVerifyAllNotMet()
		{
			var mock = new Mock<IFoo>();

			mock.Setup(x => x.Submit());

			var mex = AssertHelper.Throws<MockVerificationException>(() => mock.VerifyAll());
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
		}

		[TestMethod]
		public void ThrowsIfVerifyVoidMethodWithExpressionFails()
		{
			var mock = new Mock<IFoo>();

			var mex = AssertHelper.Throws<MockException>(() => mock.Verify(f => f.Submit()));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
		}

		[TestMethod]
		public void VerifiesVoidMethodWithExpression()
		{
			var mock = new Mock<IFoo>();
			mock.Object.Submit();

			mock.Verify(f => f.Submit());
		}

		[TestMethod]
		public void ThrowsIfVerifyReturningMethodWithExpressionFails()
		{
			var mock = new Mock<IFoo>();

			var mex = AssertHelper.Throws<MockException>(() => mock.Verify(f => f.Execute("ping")));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
		}

		[TestMethod]
		public void VerifiesReturningMethodWithExpression()
		{
			var mock = new Mock<IFoo>();
			mock.Object.Execute("ping");

			mock.Verify(f => f.Execute("ping"));
		}

		[TestMethod]
		public void VerifiesPropertyGetWithExpression()
		{
			var mock = new Mock<IFoo>();
			var v = mock.Object.Value;

			mock.VerifyGet(f => f.Value);
		}

		[TestMethod]
		public void VerifiesReturningMethodWithExpressionAndMessage()
		{
			var mock = new Mock<IFoo>();

			var me = AssertHelper.Throws<MockException>(
				() => mock.Verify(f => f.Execute("ping"), "Execute should have been invoked with 'ping'"));
			Assert.IsTrue(me.Message.Contains("Execute should have been invoked with 'ping'"));
			Assert.IsTrue(me.Message.Contains("f.Execute(\"ping\")"));
		}

		[TestMethod]
		public void VerifiesVoidMethodWithExpressionAndMessage()
		{
			var mock = new Mock<IFoo>();

			var me = AssertHelper.Throws<MockException>(
				() => mock.Verify(f => f.Submit(), "Submit should be invoked"));
			Assert.IsTrue(me.Message.Contains("Submit should be invoked"));
			Assert.IsTrue(me.Message.Contains("f.Submit()"));
		}

		[TestMethod]
		public void VerifiesPropertyGetWithExpressionAndMessage()
		{
			var mock = new Mock<IFoo>();

			var me = AssertHelper.Throws<MockException>(() => mock.VerifyGet(f => f.Value, "Nobody called .Value"));
			Assert.IsTrue(me.Message.Contains("Nobody called .Value"));
			Assert.IsTrue(me.Message.Contains("f.Value"));
		}

		[TestMethod]
		public void VerifiesPropertySetWithExpressionAndMessage()
		{
			var mock = new Mock<IFoo>();

			var me = AssertHelper.Throws<MockException>(() => mock.VerifySet(f => f.Value = It.IsAny<int?>(), "Nobody called .Value"));
			Assert.IsTrue(me.Message.Contains("Nobody called .Value"));
			Assert.IsTrue(me.Message.Contains("f.Value"));
		}

		[TestMethod]
		public void VerifiesPropertySetValueWithExpressionAndMessage()
		{
			var mock = new Mock<IFoo>();

			var e = AssertHelper.Throws<MockException>(() => mock.VerifySet(f => f.Value = 5, "Nobody called .Value"));
			AssertHelper.Contains("Nobody called .Value", e.Message);
			AssertHelper.Contains("f.Value", e.Message);
		}

		[TestMethod]
		public void AsInterfaceVerifiesReturningMethodWithExpressionAndMessage()
		{
			var disposable = new Mock<IDisposable>();
			var mock = disposable.As<IFoo>();

			var e = AssertHelper.Throws<MockException>(
				() => mock.Verify(f => f.Execute("ping"), "Execute should have been invoked with 'ping'"));

			Assert.IsTrue(e.Message.Contains("Execute should have been invoked with 'ping'"));
			Assert.IsTrue(e.Message.Contains("f.Execute(\"ping\")"));
		}

		[TestMethod]
		public void AsInferfaceVerifiesVoidMethodWithExpressionAndMessage()
		{
			var disposable = new Mock<IDisposable>();
			var mock = disposable.As<IFoo>();

			var e = AssertHelper.Throws<MockException>(() => mock.Verify(f => f.Submit(), "Submit should be invoked"));

			Assert.IsTrue(e.Message.Contains("Submit should be invoked"));
			Assert.IsTrue(e.Message.Contains("f.Submit()"));
		}

		[TestMethod]
		public void AsInterfaceVerifiesPropertyGetWithExpressionAndMessage()
		{
			var disposable = new Mock<IDisposable>();
			var mock = disposable.As<IFoo>();

			var e = AssertHelper.Throws<MockException>(() => mock.VerifyGet(f => f.Value, "Nobody called .Value"));
			Assert.IsTrue(e.Message.Contains("Nobody called .Value"));
			Assert.IsTrue(e.Message.Contains("f.Value"));
		}

		[TestMethod]
		public void AsInterfaceVerifiesPropertySetWithExpressionAndMessage()
		{
			var disposable = new Mock<IDisposable>();
			var mock = disposable.As<IBar>();

			var e = AssertHelper.Throws<MockException>(
				() => mock.VerifySet(f => f.Value = It.IsAny<int?>(), "Nobody called .Value"));
			Assert.IsTrue(e.Message.Contains("Nobody called .Value"));
			Assert.IsTrue(e.Message.Contains("f.Value"));
		}

		[TestMethod]
		public void AsInterfaceVerifiesPropertySetValueWithExpressionAndMessage()
		{
			var disposable = new Mock<IDisposable>();
			var mock = disposable.As<IBar>();

			var e = AssertHelper.Throws<MockException>(() => mock.VerifySet(f => f.Value = 5, "Nobody called .Value"));
			Assert.IsTrue(e.Message.Contains("Nobody called .Value"));
			Assert.IsTrue(e.Message.Contains("f.Value"));
		}

		[TestMethod]
		public void ThrowsIfVerifyPropertyGetWithExpressionFails()
		{
			var mock = new Mock<IFoo>();

			var e = AssertHelper.Throws<MockException>(() => mock.VerifyGet(f => f.Value));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, e.Reason);
		}

		[TestMethod]
		public void VerifiesPropertySetWithExpression()
		{
			var mock = new Mock<IFoo>();
			mock.Object.Value = 5;

			mock.VerifySet(f => f.Value = It.IsAny<int?>());
		}

		[TestMethod]
		public void ThrowsIfVerifyPropertySetWithExpressionFails()
		{
			var mock = new Mock<IFoo>();

			var e = AssertHelper.Throws<MockException>(() => mock.VerifySet(f => f.Value = It.IsAny<int?>()));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, e.Reason);
		}

		[TestMethod]
		public void VerifiesSetterWithAction()
		{
			var mock = new Mock<IFoo>();

			AssertHelper.Throws<MockException>(() => mock.VerifySet(m => m.Value = 2));
			mock.Object.Value = 2;

			mock.VerifySet(m => m.Value = 2);
		}

		[TestMethod]
		public void VerifiesSetterWithActionAndMessage()
		{
			var mock = new Mock<IFoo>();

			var me = AssertHelper.Throws<MockException>(() => mock.VerifySet(m => m.Value = 2, "foo"));
			AssertHelper.Contains("foo", me.Message);

			mock.Object.Value = 2;

			mock.VerifySet(m => m.Value = 2, "foo");
		}

		[TestMethod]
		public void VerifiesSetterWithActionAndMatcher()
		{
			var mock = new Mock<IFoo>();

			AssertHelper.Throws<MockException>(() => mock.VerifySet(m => m.Value = It.IsAny<int>()));
			mock.Object.Value = 2;

			mock.VerifySet(m => m.Value = It.IsAny<int>());
			mock.VerifySet(m => m.Value = It.IsInRange(1, 2, Range.Inclusive));
			mock.VerifySet(m => m.Value = It.Is<int>(i => i % 2 == 0));
		}

		[TestMethod]
		public void VerifiesRefWithExpression()
		{
			var mock = new Mock<IFoo>();
			var expected = "ping";

			AssertHelper.Throws<MockException>(() => mock.Verify(m => m.EchoRef(ref expected)));

			mock.Object.EchoRef(ref expected);

			mock.Verify(m => m.EchoRef(ref expected));
		}

		[TestMethod]
		public void VerifiesOutWithExpression()
		{
			var mock = new Mock<IFoo>();
			var expected = "ping";

			AssertHelper.Throws<MockException>(() => mock.Verify(m => m.EchoOut(out expected)));

			mock.Object.EchoOut(out expected);

			mock.Verify(m => m.EchoOut(out expected));
		}

		[TestMethod]
		public void ThrowsIfVerifyVoidAtMostOnceAndMoreThanOneCall()
		{
			var mock = new Mock<IFoo>();
			mock.Object.Submit();

			mock.Verify(foo => foo.Submit(), Times.AtMostOnce());

			mock.Object.Submit();

			var mex = AssertHelper.Throws<MockException>(() =>
				mock.Verify(foo => foo.Submit(), Times.AtMostOnce()));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
			Assert.IsTrue(mex.Message.StartsWith(
				"\r\nExpected invocation on the mock at most once, but was 2 times: foo => foo.Submit()"));
		}

		[TestMethod]
		public void ThrowsIfVerifyVoidAtMostAndMoreThanNCalls()
		{
			var mock = new Mock<IFoo>();
			mock.Object.Submit();
			mock.Object.Submit();

			mock.Verify(foo => foo.Submit(), Times.AtMost(2));

			mock.Object.Submit();

			var mex = AssertHelper.Throws<MockException>(() =>
				mock.Verify(foo => foo.Submit(), Times.AtMost(2)));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
			Assert.IsTrue(mex.Message.StartsWith(
				"\r\nExpected invocation on the mock at most 2 times, but was 3 times: foo => foo.Submit()"));
		}

		[TestMethod]
		public void ThrowsIfVerifyVoidNeverAndOneCall()
		{
			var mock = new Mock<IFoo>();

			mock.Verify(foo => foo.Submit(), Times.Never());

			mock.Object.Submit();

			var mex = AssertHelper.Throws<MockException>(() => mock.Verify(foo => foo.Submit(), Times.Never()));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
			Assert.IsTrue(mex.Message.StartsWith(
				"\r\nExpected invocation on the mock should never have been performed, but was 1 times: foo => foo.Submit()"));
		}

		[TestMethod]
		public void ThrowsIfVerifyVoidAtLeastOnceAndNotCalls()
		{
			var mock = new Mock<IFoo>();

			var mex = AssertHelper.Throws<MockException>(() => mock.Verify(foo => foo.Submit(), Times.AtLeastOnce()));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
			Assert.IsTrue(mex.Message.StartsWith(
				"\r\nExpected invocation on the mock at least once, but was never performed: foo => foo.Submit()"));

			mock.Object.Submit();

			mock.Verify(foo => foo.Submit(), Times.AtLeastOnce());
		}

		[TestMethod]
		public void ThrowsIfVerifyVoidAtLeastAndLessThanNCalls()
		{
			var mock = new Mock<IFoo>();

			mock.Object.Submit();
			mock.Object.Submit();

			var mex = AssertHelper.Throws<MockException>(() => mock.Verify(foo => foo.Submit(), Times.AtLeast(3)));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
			Assert.IsTrue(mex.Message.StartsWith(
				"\r\nExpected invocation on the mock at least 3 times, but was 2 times: foo => foo.Submit()"));

			mock.Object.Submit();

			mock.Verify(foo => foo.Submit(), Times.AtLeast(3));
		}

		[TestMethod]
		public void ThrowsIfVerifyVoidExactlyAndLessOrMoreThanNCalls()
		{
			var mock = new Mock<IFoo>();

			mock.Object.Submit();
			mock.Object.Submit();
			mock.Object.Submit();
			mock.Object.Submit();

			var mex = AssertHelper.Throws<MockException>(() => mock.Verify(foo => foo.Submit(), Times.Exactly(5)));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
			Assert.IsTrue(mex.Message.StartsWith(
				"\r\nExpected invocation on the mock exactly 5 times, but was 4 times: foo => foo.Submit()"));

			mock.Object.Submit();

			mock.Verify(foo => foo.Submit(), Times.Exactly(5));

			mock.Object.Submit();

			mex = AssertHelper.Throws<MockException>(() => mock.Verify(foo => foo.Submit(), Times.Exactly(5)));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
			Assert.IsTrue(mex.Message.StartsWith(
				"\r\nExpected invocation on the mock exactly 5 times, but was 6 times: foo => foo.Submit()"));
		}

		[TestMethod]
		public void ThrowsIfVerifyVoidOnceAndLessOrMoreThanACall()
		{
			var mock = new Mock<IFoo>();

			var mex = AssertHelper.Throws<MockException>(() => mock.Verify(foo => foo.Submit(), Times.Once()));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
			Assert.IsTrue(mex.Message.StartsWith(
				"\r\nExpected invocation on the mock once, but was 0 times: foo => foo.Submit()"));

			mock.Object.Submit();

			mock.Verify(foo => foo.Submit(), Times.Once());

			mock.Object.Submit();

			mex = AssertHelper.Throws<MockException>(() => mock.Verify(foo => foo.Submit(), Times.Once()));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
			Assert.IsTrue(mex.Message.StartsWith(
				"\r\nExpected invocation on the mock once, but was 2 times: foo => foo.Submit()"));
		}

		[TestMethod]
		public void ThrowsIfVerifyVoidBetweenExclusiveAndLessOrEqualsFromOrMoreOrEqualToCalls()
		{
			var mock = new Mock<IFoo>();

			mock.Object.Submit();

			var mex = AssertHelper.Throws<MockException>(
				() => mock.Verify(foo => foo.Submit(), Times.Between(1, 4, Range.Exclusive)));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
			Assert.IsTrue(mex.Message.StartsWith(
				"\r\nExpected invocation on the mock between 1 and 4 times (Exclusive), but was 1 times: foo => foo.Submit()"));

			mock.Object.Submit();

			mock.Verify(foo => foo.Submit(), Times.Between(1, 4, Range.Exclusive));

			mock.Object.Submit();

			mock.Verify(foo => foo.Submit(), Times.Between(1, 4, Range.Exclusive));

			mock.Object.Submit();

			mex = AssertHelper.Throws<MockException>(
				() => mock.Verify(foo => foo.Submit(), Times.Between(1, 4, Range.Exclusive)));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
			Assert.IsTrue(mex.Message.StartsWith(
				"\r\nExpected invocation on the mock between 1 and 4 times (Exclusive), but was 4 times: foo => foo.Submit()"));
		}

		[TestMethod]
		public void ThrowsIfVerifyVoidBetweenInclusiveAndLessFromOrMoreToCalls()
		{
			var mock = new Mock<IFoo>();
			mock.Object.Submit();

			var mex = AssertHelper.Throws<MockException>(
				() => mock.Verify(foo => foo.Submit(), Times.Between(2, 4, Range.Inclusive)));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
			Assert.IsTrue(mex.Message.StartsWith(
				"\r\nExpected invocation on the mock between 2 and 4 times (Inclusive), but was 1 times: foo => foo.Submit()"));

			mock.Object.Submit();

			mock.Verify(foo => foo.Submit(), Times.Between(2, 4, Range.Inclusive));

			mock.Object.Submit();

			mock.Verify(foo => foo.Submit(), Times.Between(2, 4, Range.Inclusive));

			mock.Object.Submit();
			mock.Object.Submit();

			mex = AssertHelper.Throws<MockException>(
				() => mock.Verify(foo => foo.Submit(), Times.Between(2, 4, Range.Inclusive)));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
			Assert.IsTrue(mex.Message.StartsWith(
				"\r\nExpected invocation on the mock between 2 and 4 times (Inclusive), but was 5 times: foo => foo.Submit()"));
		}

		[TestMethod]
		public void ThrowsIfVerifyReturningAtMostOnceAndMoreThanOneCall()
		{
			var mock = new Mock<IFoo>();
			mock.Object.Execute("");

			mock.Verify(foo => foo.Execute(""), Times.AtMostOnce());

			mock.Object.Execute("");

			MockException mex = AssertHelper.Throws<MockException>(
				() => mock.Verify(foo => foo.Execute(""), Times.AtMostOnce()));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
		}

		[TestMethod]
		public void ThrowsIfVerifyReturningAtMostAndMoreThanNCalls()
		{
			var mock = new Mock<IFoo>();
			mock.Object.Execute("");
			mock.Object.Execute("");

			mock.Verify(foo => foo.Execute(""), Times.AtMost(2));

			mock.Object.Execute("");

			MockException mex = AssertHelper.Throws<MockException>(
				() => mock.Verify(foo => foo.Execute(""), Times.AtMost(2)));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
		}

		[TestMethod]
		public void ThrowsIfVerifyReturningNeverAndOneCall()
		{
			var mock = new Mock<IFoo>();

			mock.Verify(foo => foo.Execute(""), Times.Never());

			mock.Object.Execute("");

			MockException mex = AssertHelper.Throws<MockException>(
				() => mock.Verify(foo => foo.Execute(""), Times.Never()));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
		}

		[TestMethod]
		public void ThrowsIfVerifyReturningAtLeastOnceAndNotCalls()
		{
			var mock = new Mock<IFoo>();

			MockException mex = AssertHelper.Throws<MockException>(
				() => mock.Verify(foo => foo.Execute(""), Times.AtLeastOnce()));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);

			mock.Object.Execute("");

			mock.Verify(foo => foo.Execute(""), Times.AtLeastOnce());
		}

		[TestMethod]
		public void ThrowsIfVerifyReturningAtLeastAndLessThanNCalls()
		{
			var mock = new Mock<IFoo>();

			mock.Object.Execute("");
			mock.Object.Execute("");

			MockException mex = AssertHelper.Throws<MockException>(
				() => mock.Verify(foo => foo.Execute(""), Times.AtLeast(3)));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);

			mock.Object.Execute("");

			mock.Verify(foo => foo.Execute(""), Times.AtLeast(3));
		}

		[TestMethod]
		public void ThrowsIfVerifyReturningExactlyAndLessOrMoreThanNCalls()
		{
			var mock = new Mock<IFoo>();

			mock.Object.Execute("");
			mock.Object.Execute("");
			mock.Object.Execute("");
			mock.Object.Execute("");

			MockException mex = AssertHelper.Throws<MockException>(
				() => mock.Verify(foo => foo.Execute(""), Times.Exactly(5)));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);

			mock.Object.Execute("");

			mock.Verify(foo => foo.Execute(""), Times.Exactly(5));

			mock.Object.Execute("");

			mex = AssertHelper.Throws<MockException>(() => mock.Verify(foo => foo.Execute(""), Times.Exactly(5)));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
		}

		[TestMethod]
		public void ThrowsIfVerifyReturningBetweenExclusiveAndLessOrEqualsFromOrMoreOrEqualToCalls()
		{
			var mock = new Mock<IFoo>();

			mock.Object.Execute("");

			MockException mex = AssertHelper.Throws<MockException>(
				() => mock.Verify(foo => foo.Execute(""), Times.Between(1, 4, Range.Exclusive)));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);

			mock.Object.Execute("");

			mock.Verify(foo => foo.Execute(""), Times.Between(1, 4, Range.Exclusive));

			mock.Object.Execute("");

			mock.Verify(foo => foo.Execute(""), Times.Between(1, 4, Range.Exclusive));

			mock.Object.Execute("");

			mex = AssertHelper.Throws<MockException>(() =>
				mock.Verify(foo => foo.Execute(""), Times.Between(1, 4, Range.Exclusive)));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
		}

		[TestMethod]
		public void ThrowsIfVerifyReturningBetweenInclusiveAndLessFromOrMoreToCalls()
		{
			var mock = new Mock<IFoo>();

			mock.Object.Execute("");

			MockException mex = AssertHelper.Throws<MockException>(() =>
				mock.Verify(foo => foo.Execute(""), Times.Between(2, 4, Range.Inclusive)));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);

			mock.Object.Execute("");

			mock.Verify(foo => foo.Execute(""), Times.Between(2, 4, Range.Inclusive));

			mock.Object.Execute("");

			mock.Verify(foo => foo.Execute(""), Times.Between(2, 4, Range.Inclusive));

			mock.Object.Execute("");
			mock.Object.Execute("");

			mex = AssertHelper.Throws<MockException>(
				() => mock.Verify(foo => foo.Execute(""), Times.Between(2, 4, Range.Inclusive)));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
		}

		[TestMethod]
		public void ThrowsIfVerifyGetGetAtMostOnceAndMoreThanOneCall()
		{
			var mock = new Mock<IFoo>();
			var value = mock.Object.Value;

			mock.VerifyGet(foo => foo.Value, Times.AtMostOnce());

			value = mock.Object.Value;

			MockException mex = AssertHelper.Throws<MockException>(
				() => mock.VerifyGet(foo => foo.Value, Times.AtMostOnce()));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
		}

		[TestMethod]
		public void ThrowsIfVerifyGetGetAtMostAndMoreThanNCalls()
		{
			var mock = new Mock<IFoo>();
			var value = mock.Object.Value;
			value = mock.Object.Value;

			mock.VerifyGet(foo => foo.Value, Times.AtMost(2));

			value = mock.Object.Value;

			MockException mex = AssertHelper.Throws<MockException>(
				() => mock.VerifyGet(foo => foo.Value, Times.AtMost(2)));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
		}

		[TestMethod]
		public void ThrowsIfVerifyGetGetNeverAndOneCall()
		{
			var mock = new Mock<IFoo>();

			mock.VerifyGet(foo => foo.Value, Times.Never());

			var value = mock.Object.Value;

			MockException mex = AssertHelper.Throws<MockException>(
				() => mock.VerifyGet(foo => foo.Value, Times.Never()));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
		}

		[TestMethod]
		public void ThrowsIfVerifyGetGetAtLeastOnceAndNotCalls()
		{
			var mock = new Mock<IFoo>();

			MockException mex = AssertHelper.Throws<MockException>(
				() => mock.VerifyGet(foo => foo.Value, Times.AtLeastOnce()));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);

			var value = mock.Object.Value;

			mock.VerifyGet(foo => foo.Value, Times.AtLeastOnce());
		}

		[TestMethod]
		public void ThrowsIfVerifyGetGetAtLeastAndLessThanNCalls()
		{
			var mock = new Mock<IFoo>();

			var value = mock.Object.Value;
			value = mock.Object.Value;

			MockException mex = AssertHelper.Throws<MockException>(
				() => mock.VerifyGet(foo => foo.Value, Times.AtLeast(3)));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);

			value = mock.Object.Value;

			mock.VerifyGet(foo => foo.Value, Times.AtLeast(3));
		}

		[TestMethod]
		public void ThrowsIfVerifyGetGetExactlyAndLessOrMoreThanNCalls()
		{
			var mock = new Mock<IFoo>();

			var value = mock.Object.Value;
			value = mock.Object.Value;
			value = mock.Object.Value;
			value = mock.Object.Value;

			MockException mex = AssertHelper.Throws<MockException>(
				() => mock.VerifyGet(foo => foo.Value, Times.Exactly(5)));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);

			value = mock.Object.Value;

			mock.VerifyGet(foo => foo.Value, Times.Exactly(5));

			value = mock.Object.Value;

			mex = AssertHelper.Throws<MockException>(() => mock.VerifyGet(foo => foo.Value, Times.Exactly(5)));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
		}

		[TestMethod]
		public void ThrowsIfVerifyGetGetBetweenExclusiveAndLessOrEqualsFromOrMoreOrEqualToCalls()
		{
			var mock = new Mock<IFoo>();

			var value = mock.Object.Value;

			MockException mex = AssertHelper.Throws<MockException>(
				() => mock.VerifyGet(foo => foo.Value, Times.Between(1, 4, Range.Exclusive)));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);

			value = mock.Object.Value;

			mock.VerifyGet(foo => foo.Value, Times.Between(1, 4, Range.Exclusive));

			value = mock.Object.Value;

			mock.VerifyGet(foo => foo.Value, Times.Between(1, 4, Range.Exclusive));

			value = mock.Object.Value;

			mex = AssertHelper.Throws<MockException>(() =>
				mock.VerifyGet(foo => foo.Value, Times.Between(1, 4, Range.Exclusive)));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
		}

		[TestMethod]
		public void ThrowsIfVerifyGetGetBetweenInclusiveAndLessFromOrMoreToCalls()
		{
			var mock = new Mock<IFoo>();

			var value = mock.Object.Value;

			MockException mex = AssertHelper.Throws<MockException>(
				() => mock.VerifyGet(foo => foo.Value, Times.Between(2, 4, Range.Inclusive)));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);

			value = mock.Object.Value;

			mock.VerifyGet(foo => foo.Value, Times.Between(2, 4, Range.Inclusive));

			value = mock.Object.Value;

			mock.VerifyGet(foo => foo.Value, Times.Between(2, 4, Range.Inclusive));

			value = mock.Object.Value;
			value = mock.Object.Value;

			mex = AssertHelper.Throws<MockException>(() =>
				mock.VerifyGet(foo => foo.Value, Times.Between(2, 4, Range.Inclusive)));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
		}

		[TestMethod]
		public void ThrowsIfVerifySetAtMostOnceAndMoreThanOneCall()
		{
			var mock = new Mock<IFoo>();
			mock.Object.Value = 3;

			mock.VerifySet(f => f.Value = 3, Times.AtMostOnce());

			mock.Object.Value = 3;

			MockException mex = AssertHelper.Throws<MockException>(
				() => mock.VerifySet(f => f.Value = 3, Times.AtMostOnce()));
			Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
		}

		[TestMethod]
		public void IncludesActualCallsInFailureMessage()
		{
			var mock = new Moq.Mock<IFoo>();

			mock.Object.Execute("ping");
			mock.Object.Echo(42);
			mock.Object.Submit();

			var mex = AssertHelper.Throws<MockException>(() => mock.Verify(f => f.Execute("pong")));

			AssertHelper.Contains(
				Environment.NewLine +
				"Performed invocations:" + Environment.NewLine +
				"IFoo.Execute(\"ping\")" + Environment.NewLine +
				"IFoo.Echo(42)" + Environment.NewLine +
				"IFoo.Submit()",
				mex.Message);
		}

		[TestMethod]
		public void IncludesMessageAboutNoActualCallsInFailureMessage()
		{
			var mock = new Moq.Mock<IFoo>();

			MockException mex = AssertHelper.Throws<MockException>(() => mock.Verify(f => f.Execute("pong")));

			AssertHelper.Contains(Environment.NewLine + "No invocations performed.", mex.Message);
		}


		public interface IBar
		{
			int? Value { get; set; }
		}

		public interface IFoo
		{
			int WriteOnly { set; }
			int? Value { get; set; }
			void EchoRef<T>(ref T value);
			void EchoOut<T>(out T value);
			int Echo(int value);
			void Submit();
			string Execute(string command);
		}
	}
}

namespace SomeNamespace
{
	public class VerifyExceptionsFixture
	{
		[TestMethod]
		public void RendersReadableMessageForVerifyFailures()
		{
			var mock = new Mock<Moq.Tests.VerifyFixture.IFoo>();

			mock.Setup(x => x.Submit());
			mock.Setup(x => x.Echo(1));
			mock.Setup(x => x.Execute("ping"));

			try
			{
				mock.VerifyAll();
				Assert.IsTrue(false, "Should have thrown");
			}
			catch (Exception ex)
			{
				Assert.IsTrue(ex.Message.Contains("x => x.Submit()"));
				Assert.IsTrue(ex.Message.Contains("x => x.Echo(1)"));
				Assert.IsTrue(ex.Message.Contains("x => x.Execute(\"ping\")"));
			}
		}
	}
}
