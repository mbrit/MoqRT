using System;
using System.Diagnostics;
using System.Reflection;
using System.Linq.Expressions;
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
    public class MockFixture
	{
		[TestMethod]
		public void CreatesMockAndExposesInterface()
		{
			var mock = new Mock<IComparable>();

			IComparable comparable = mock.Object;

			Assert.IsNotNull(comparable);
		}

		[TestMethod]
		public void ThrowsIfNullExpectAction()
		{
			var mock = new Mock<IComparable>();

			AssertHelper.Throws<ArgumentNullException>(() => mock.Setup((Expression<Action<IComparable>>)null));
		}

		[TestMethod]
		public void ThrowsIfNullExpectFunction()
		{
			var mock = new Mock<IComparable>();

			AssertHelper.Throws<ArgumentNullException>(() => mock.Setup((Expression<Func<IComparable, string>>)null));
		}

		[TestMethod]
		public void ThrowsIfExpectationSetForField()
		{
			var mock = new Mock<FooBase>();

			AssertHelper.Throws<ArgumentException>(() => mock.Setup(x => x.ValueField));
		}

		[TestMethod]
		public void CallParameterCanBeVariable()
		{
			int value = 5;
			var mock = new Mock<IFoo>();

			mock.Setup(x => x.Echo(value)).Returns(() => value * 2);

			Assert.AreEqual(value * 2, mock.Object.Echo(value));
		}

		[TestMethod]
		public void CallParameterCanBeMethodCall()
		{
			int value = 5;
			var mock = new Mock<IFoo>();

			mock.Setup(x => x.Echo(GetValue(value))).Returns(() => value * 2);

			Assert.AreEqual(value * 2, mock.Object.Echo(value * 2));
		}

		private int GetValue(int value)
		{
			return value * 2;
		}

		[TestMethod]
		public void ExpectsVoidCall()
		{
			var mock = new Mock<IFoo>();

			mock.Setup(x => x.Submit());

			mock.Object.Submit();
		}

		[TestMethod]
		public void ThrowsIfExpectationThrows()
		{
			var mock = new Mock<IFoo>();

			mock.Setup(x => x.Submit()).Throws(new FormatException());

			AssertHelper.Throws<FormatException>(() => mock.Object.Submit());
		}

		[TestMethod]
		public void ThrowsIfExpectationThrowsWithGenericsExceptionType()
		{
			var mock = new Mock<IFoo>();

			mock.Setup(x => x.Submit()).Throws<FormatException>();

			AssertHelper.Throws<FormatException>(() => mock.Object.Submit());
		}

		[TestMethod]
		public void ReturnsServiceFromServiceProvider()
		{
			var provider = new Mock<IServiceProvider>();

			provider.Setup(x => x.GetService(typeof(IFooService))).Returns(new FooService());

			Assert.IsTrue(provider.Object.GetService(typeof(IFooService)) is FooService);
		}

		[TestMethod]
		public void InvokesLastExpectationThatMatches()
		{
			var mock = new Mock<IFoo>();
			mock.Setup(x => x.Execute(It.IsAny<string>())).Throws(new ArgumentException());
			mock.Setup(x => x.Execute("ping")).Returns("I'm alive!");

			Assert.AreEqual("I'm alive!", mock.Object.Execute("ping"));

			AssertHelper.Throws<ArgumentException>(() => mock.Object.Execute("asdf"));
		}

		[TestMethod]
		public void MockObjectIsAssignableToMockedInterface()
		{
			var mock = new Mock<IFoo>();
			Assert.IsTrue(typeof(IFoo).IsAssignableFrom(mock.Object.GetType()));
		}

		[TestMethod]
		public void MockObjectsEqualityIsReferenceEquals()
		{
			var mock1 = new Mock<IFoo>();
			var mock2 = new Mock<IFoo>();

			Assert.IsTrue(mock1.Object.Equals(mock1.Object));
			Assert.IsFalse(mock1.Object.Equals(mock2.Object));
		}

		[TestMethod]
		public void HashCodeIsDifferentForEachMock()
		{
			var mock1 = new Mock<IFoo>();
			var mock2 = new Mock<IFoo>();

			Assert.AreEqual(mock1.Object.GetHashCode(), mock1.Object.GetHashCode());
			Assert.AreEqual(mock2.Object.GetHashCode(), mock2.Object.GetHashCode());
			Assert.AreNotEqual(mock1.Object.GetHashCode(), mock2.Object.GetHashCode());
		}

		[TestMethod]
		public void ToStringIsNullOrEmpty()
		{
			var mock = new Mock<IFoo>();
			Assert.IsFalse(String.IsNullOrEmpty(mock.Object.ToString()));
		}

        //[Fact(Skip = "Castle.DynamicProxy2 doesn't seem to call interceptors for ToString, GetHashCode & Equals")]
        //public void OverridesObjectMethods()
        //{
        //    var mock = new Mock<IFoo>();
        //    mock.Setup(x => x.GetHashCode()).Returns(1);
        //    mock.Setup(x => x.ToString()).Returns("foo");
        //    mock.Setup(x => x.Equals(It.IsAny<object>())).Returns(true);

        //    Assert.AreEqual("foo", mock.Object.ToString());
        //    Assert.AreEqual(1, mock.Object.GetHashCode());
        //    Assert.IsTrue(mock.Object.Equals(new object()));
        //}

		[TestMethod]
		public void OverridesBehaviorFromAbstractClass()
		{
			var mock = new Mock<FooBase>();
			mock.CallBase = true;

			mock.Setup(x => x.Check("foo")).Returns(false);

			Assert.IsFalse(mock.Object.Check("foo"));
			Assert.IsTrue(mock.Object.Check("bar"));
		}

		[TestMethod]
		public void CallsUnderlyingClassEquals()
		{
			var mock = new Mock<FooOverrideEquals>();
			var mock2 = new Mock<FooOverrideEquals>();

			mock.CallBase = true;

			mock.Object.Name = "Foo";
			mock2.Object.Name = "Foo";

			Assert.IsTrue(mock.Object.Equals(mock2.Object));
		}

		[TestMethod]
		public void ThrowsIfSealedClass()
		{
			AssertHelper.Throws<NotSupportedException>(() => new Mock<FooSealed>());
		}

		[TestMethod]
		public void ThrowsIfExpectOnNonVirtual()
		{
			var mock = new Mock<FooBase>();

			AssertHelper.Throws<NotSupportedException>(() => mock.Setup(x => x.True()).Returns(false));
		}

		[TestMethod]
		public void OverridesPreviousExpectation()
		{
			var mock = new Mock<IFoo>();

			mock.Setup(x => x.Echo(1)).Returns(5);

			Assert.AreEqual(5, mock.Object.Echo(1));

			mock.Setup(x => x.Echo(1)).Returns(10);

			Assert.AreEqual(10, mock.Object.Echo(1));
		}

		[TestMethod]
		public void ConstructsObjectsWithCtorArguments()
		{
			var mock = new Mock<FooWithConstructors>(MockBehavior.Default, "Hello", 26);

			Assert.AreEqual("Hello", mock.Object.StringValue);
			Assert.AreEqual(26, mock.Object.IntValue);

			// Should also construct without args.
			mock = new Mock<FooWithConstructors>(MockBehavior.Default);

			Assert.AreEqual(null, mock.Object.StringValue);
			Assert.AreEqual(0, mock.Object.IntValue);
		}

		[TestMethod]
		public void ConstructsClassWithNoDefaultConstructor()
		{
			var mock = new Mock<ClassWithNoDefaultConstructor>(MockBehavior.Default, "Hello", 26);

			Assert.AreEqual("Hello", mock.Object.StringValue);
			Assert.AreEqual(26, mock.Object.IntValue);
		}

		[TestMethod]
		public void ConstructsClassWithNoDefaultConstructorAndNullValue()
		{
			var mock = new Mock<ClassWithNoDefaultConstructor>(MockBehavior.Default, null, 26);

			Assert.AreEqual(null, mock.Object.StringValue);
			Assert.AreEqual(26, mock.Object.IntValue);
		}

		[TestMethod]
		public void ThrowsIfNoMatchingConstructorFound()
		{
			AssertHelper.Throws<ArgumentException>(() =>
			{
				Debug.WriteLine(new Mock<ClassWithNoDefaultConstructor>(25, true).Object);
			});
		}

		[TestMethod]
		public void ThrowsIfArgumentsPassedForInterface()
		{
			AssertHelper.Throws<ArgumentException>(() => new Mock<IFoo>(25, true));
		}

		[TestMethod]
		public void ThrowsOnStrictWithExpectButNoReturns()
		{
			var mock = new Mock<IFoo>(MockBehavior.Strict);

			mock.Setup(x => x.Execute("ping"));

			try
			{
				mock.Object.Execute("ping");
				Assert.IsTrue(false, "SHould throw");
			}
			catch (MockException mex)
			{
				Assert.AreEqual(MockException.ExceptionReason.ReturnValueRequired, mex.Reason);
			}
		}

		[TestMethod]
		public void AllowsSetupNewHiddenProperties()
		{
			var value = new Mock<INewBar>().Object;

			var target = new Mock<INewFoo>();
			target.As<IFoo>().SetupGet(x => x.Bar).Returns(value);
			target.Setup(x => x.Bar).Returns(value);

			Assert.AreEqual(target.Object.Bar, target.As<IFoo>().Object.Bar);
		}

		[TestMethod]
		public void AllowsSetupNewHiddenBaseProperty()
		{
			var value = new Mock<INewBar>().Object;

			var target = new Mock<INewFoo>();
			target.As<IFoo>().SetupGet(x => x.Bar).Returns(value);

			Assert.AreEqual(value, target.As<IFoo>().Object.Bar);
			Assert.IsNull(target.Object.Bar);
		}

		[TestMethod]
		public void AllowsSetupNewHiddenInheritedProperty()
		{
			var value = new Mock<INewBar>().Object;

			var target = new Mock<INewFoo>();
			target.As<IFoo>();
			target.SetupGet(x => x.Bar).Returns(value);

			Assert.AreEqual(value, target.Object.Bar);
			Assert.IsNull(target.As<IFoo>().Object.Bar);
		}

		[TestMethod]
		public void ExpectsPropertySetter()
		{
			var mock = new Mock<IFoo>();

			int? value = 0;

			mock.SetupSet(foo => foo.Value = It.IsAny<int?>())
				.Callback<int?>(i => value = i);

			mock.Object.Value = 5;

			Assert.AreEqual(5, value);
		}

		[TestMethod]
		public void ExpectsPropertySetterLambda()
		{
			var mock = new Mock<IFoo>();

			mock.SetupSet(foo => foo.Count = 5);

			AssertHelper.Throws<MockVerificationException>(() => mock.VerifyAll());

			mock.Object.Count = 6;

			AssertHelper.Throws<MockVerificationException>(() => mock.VerifyAll());

			mock.Object.Count = 5;

			mock.VerifyAll();
		}

		[TestMethod]
		public void CallbackReceivesValueWithPropertySetterLambda()
		{
			var mock = new Mock<IFoo>();
			int value = 0;
			int value2 = 0;

			mock.SetupSet(foo => foo.Count = 6).Callback<int>(v => value = v);
			mock.SetupSet<int>(foo => foo.Count = 3).Callback(v => value2 = v);

			mock.Object.Count = 6;

			Assert.AreEqual(6, value);
			Assert.AreEqual(0, value2);

			mock.Object.Count = 3;

			Assert.AreEqual(3, value2);
		}

		[TestMethod]
		public void SetterLambdaUsesItIsAnyMatcher()
		{
			var mock = new Mock<IFoo>();

			mock.SetupSet(foo => foo.Count = It.IsAny<int>());

			AssertHelper.Throws<MockVerificationException>(() => mock.VerifyAll());

			mock.Object.Count = 6;

			mock.VerifyAll();
		}

		[TestMethod]
		public void SetterLambdaUsesItIsInRangeMatcher()
		{
			var mock = new Mock<IFoo>();

			mock.SetupSet(foo => foo.Count = It.IsInRange(1, 5, Range.Inclusive));

			AssertHelper.Throws<MockVerificationException>(() => mock.VerifyAll());

			mock.Object.Count = 6;

			AssertHelper.Throws<MockVerificationException>(() => mock.VerifyAll());

			mock.Object.Count = 5;

			mock.VerifyAll();
		}

		[TestMethod]
		public void SetterLambdaUsesItIsPredicateMatcher()
		{
			var mock = new Mock<IFoo>();

			mock.SetupSet(foo => foo.Count = It.Is<int>(v => v % 2 == 0));

			AssertHelper.Throws<MockVerificationException>(() => mock.VerifyAll());

			mock.Object.Count = 7;

			AssertHelper.Throws<MockVerificationException>(() => mock.VerifyAll());

			mock.Object.Count = 4;

			mock.VerifyAll();
		}

		[TestMethod]
		public void SetterLambdaCannotHaveMultipleSetups()
		{
			var mock = new Mock<IFoo>();

			mock.SetupSet(foo => foo.Count = It.IsAny<int>())
				.Throws<ArgumentOutOfRangeException>();
			AssertHelper.Throws<ArgumentOutOfRangeException>(() => mock.Object.Count = 5);

			mock.SetupSet(foo => foo.Count = It.IsInRange(1, 5, Range.Inclusive))
				.Throws<ArgumentException>();
			AssertHelper.Throws<ArgumentException>(() => mock.Object.Count = 5);
		}

		[TestMethod]
		public void SetterLambdaDoesNotCountAsInvocation()
		{
			var mock = new Mock<IFoo>();

			mock.SetupSet(foo => foo.Count = 5);

			AssertHelper.Throws<MockVerificationException>(() => mock.VerifyAll());

			mock.Object.Count = 5;
			mock.VerifyAll();
		}

		[TestMethod]
		public void SetterLambdaWithStrictMockWorks()
		{
			var mock = new Mock<IFoo>(MockBehavior.Strict);

			mock.SetupSet(foo => foo.Count = 5);
		}

		[TestMethod]
		public void ShouldAllowMultipleCustomMatcherWithArguments()
		{
			var mock = new Mock<IFoo>();

			mock.Setup(m => m.Echo(IsMultipleOf(2))).Returns(2);
			mock.Setup(m => m.Echo(IsMultipleOf(3))).Returns(3);

			Assert.AreEqual(2, mock.Object.Echo(4));
			Assert.AreEqual(2, mock.Object.Echo(8));
			Assert.AreEqual(3, mock.Object.Echo(9));
			Assert.AreEqual(3, mock.Object.Echo(3));
		}

		private int IsMultipleOf(int value)
		{
			return Match.Create<int>(i => i % value == 0);
		}

#pragma warning disable 618
		[Matcher]
		private static int OddInt()
		{
			return 0;
		}
#pragma warning restore 618

		private static bool OddInt(int value)
		{
			return value % 2 == 0;
		}

#pragma warning disable 618
		[Matcher]
		private int BigInt()
		{
			return 0;
		}
#pragma warning restore 618

		private bool BigInt(int value)
		{
			return value > 2;
		}

		[TestMethod]
		public void ExpectsPropertySetterLambdaCoercesNullable()
		{
			var mock = new Mock<IFoo>();

			mock.SetupSet(foo => foo.Value = 5);

			AssertHelper.Throws<MockVerificationException>(() => mock.VerifyAll());

			mock.Object.Value = 6;

			AssertHelper.Throws<MockVerificationException>(() => mock.VerifyAll());

			mock.Object.Value = 5;

			mock.VerifyAll();
		}

		[TestMethod]
		public void ExpectsPropertySetterLambdaValueReference()
		{
			var mock = new Mock<IFoo>();
			var obj = new object();

			mock.SetupSet(foo => foo.Object = obj);

			AssertHelper.Throws<MockVerificationException>(() => mock.VerifyAll());

			mock.Object.Object = new object();

			AssertHelper.Throws<MockVerificationException>(() => mock.VerifyAll());

			mock.Object.Object = obj;

			mock.VerifyAll();
		}

		[TestMethod]
		public void ExpectsPropertySetterLambdaRecursive()
		{
			var mock = new Mock<IFoo>();

			mock.SetupSet<string>(foo => foo.Bar.Value = "bar");

			AssertHelper.Throws<MockVerificationException>(() => mock.VerifyAll());

			mock.Object.Bar.Value = "bar";

			mock.VerifyAll();
		}

		[TestMethod]
		public void ExpectsPropertySetterWithNullValue()
		{
			var mock = new Mock<IFoo>(MockBehavior.Strict);
			mock.SetupSet(m => m.Value = null);

			AssertHelper.Throws<MockException>(() => { mock.Object.Value = 5; });
			AssertHelper.Throws<MockVerificationException>(() => mock.VerifyAll());

			mock.Object.Value = null;

			mock.VerifyAll();
			mock.VerifySet(m => m.Value = It.IsAny<int?>());
		}

		[TestMethod]
		public void ThrowsIfPropertySetterWithWrongValue()
		{
			var mock = new Mock<IFoo>(MockBehavior.Strict);
			mock.SetupSet(m => m.Value = 5);

			AssertHelper.Throws<MockException>(() => { mock.Object.Value = 6; });
		}

		[TestMethod]
		public void ExpectsPropertyGetter()
		{
			var mock = new Mock<IFoo>();

			bool called = false;

			mock.SetupGet(x => x.Value)
				.Callback(() => called = true)
				.Returns(25);

			Assert.AreEqual(25, mock.Object.Value);
			Assert.IsTrue(called);
		}

		[TestMethod]
		public void ThrowsIfExpectPropertySetterOnMethod()
		{
			var mock = new Mock<IFoo>();

			AssertHelper.Throws<ArgumentException>(() => mock.SetupSet(foo => foo.Echo(5)));
		}

		[TestMethod]
		public void ThrowsIfExpectPropertyGetterOnMethod()
		{
			var mock = new Mock<IFoo>();

			AssertHelper.Throws<ArgumentException>(() => mock.SetupGet(foo => foo.Echo(5)));
		}

		[TestMethod]
		public void DoesNotCallBaseClassVirtualImplementationByDefault()
		{
			var mock = new Mock<FooBase>();

			Assert.IsFalse(mock.Object.BaseCalled);
			mock.Object.BaseCall();

			Assert.IsFalse(mock.Object.BaseCalled);
		}

		[TestMethod]
		public void DoesNotCallBaseClassVirtualImplementationIfSpecified()
		{
			var mock = new Mock<FooBase>();

			mock.CallBase = false;

			Assert.IsFalse(mock.Object.BaseCalled);
			mock.Object.BaseCall();

			Assert.AreEqual(default(bool), mock.Object.BaseCall("foo"));
			Assert.IsFalse(mock.Object.BaseCalled);
		}

		[TestMethod]
		public void ExpectsGetIndexedProperty()
		{
			var mock = new Mock<IFoo>();

			mock.SetupGet(foo => foo[0])
				.Returns(1);
			mock.SetupGet(foo => foo[1])
				.Returns(2);

			Assert.AreEqual(1, mock.Object[0]);
			Assert.AreEqual(2, mock.Object[1]);
		}

		[TestMethod]
		public void ExpectAndExpectGetAreSynonyms()
		{
			var mock = new Mock<IFoo>();

			mock.SetupGet(foo => foo.Value)
				.Returns(1);
			mock.Setup(foo => foo.Value)
				.Returns(2);

			Assert.AreEqual(2, mock.Object.Value);
		}

		[TestMethod]
		public void ThrowsIfExpecationSetForNotOverridableMember()
		{
			var target = new Mock<Doer>();

			AssertHelper.Throws<NotSupportedException>(() => target.Setup(t => t.Do()));
		}

		[TestMethod]
		public void ExpectWithParamArrayEmptyMatchArguments()
		{
			string expected = "bar";
			string argument = "foo";

			var target = new Mock<IParams>();
			target.Setup(x => x.ExecuteByName(argument)).Returns(expected);

			string actual = target.Object.ExecuteByName(argument);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void ExpectWithParamArrayNotMatchDifferentLengthInArguments()
		{
			string notExpected = "bar";
			string argument = "foo";

			var target = new Mock<IParams>();
			target.Setup(x => x.ExecuteParams(argument, It.IsAny<string>())).Returns(notExpected);

			string actual = target.Object.ExecuteParams(argument);
			Assert.AreNotEqual(notExpected, actual);
		}

		[TestMethod]
		public void ExpectWithParamArrayMatchArguments()
		{
			string expected = "bar";
			string argument = "foo";

			var target = new Mock<IParams>();
			target.Setup(x => x.ExecuteParams(argument, It.IsAny<string>())).Returns(expected);

			string ret = target.Object.ExecuteParams(argument, "baz");
			Assert.AreEqual(expected, ret);
		}

		[TestMethod]
		public void ExpectWithArrayNotMatchTwoDifferentArrayInstances()
		{
			string expected = "bar";
			string argument = "foo";

			var target = new Mock<IParams>();
			target.Setup(x => x.ExecuteArray(new string[] { argument, It.IsAny<string>() })).Returns(expected);

			string ret = target.Object.ExecuteArray(new string[] { argument, "baz" });
			Assert.AreEqual(null, ret);
		}

		[TestMethod]
		public void ExpectGetAndExpectSetMatchArguments()
		{
			var target = new Mock<IFoo>();
			target.SetupGet(d => d.Value).Returns(1);
			target.SetupSet(d => d.Value = 2);

			target.Object.Value = target.Object.Value + 1;

			target.VerifyAll();
		}

		[TestMethod]
		public void ArgumentNullMatchProperCtor()
		{
			var target = new Mock<Foo>(null);
			Assert.IsNull(target.Object.Bar);
		}

		[TestMethod]
		public void DistinguishesSameMethodsWithDifferentGenericArguments()
		{
			var mock = new Mock<FooBase>();

			mock.Setup(foo => foo.Generic<int>()).Returns(2);
			mock.Setup(foo => foo.Generic<string>()).Returns(3);

			Assert.AreEqual(2, mock.Object.Generic<int>());
			Assert.AreEqual(3, mock.Object.Generic<string>());
		}

		//[TestMethod]
		//public void CanCreateMockOfInternalInterface()
		//{
		//    Assert.IsNotNull(new Mock<ClassLibrary1.IFooInternal>().Object);
		//}

		public class Foo
		{
			public Foo() : this(new Bar()) { }

			public Foo(IBar bar)
			{
				this.Bar = bar;
			}

			public IBar Bar { get; private set; }
		}

		public class Bar : IBar
		{
			public string Value { get; set; }
		}

		public interface IBar
		{
			string Value { get; set; }
		}

		interface IDo { void Do(); }

		public class Doer : IDo
		{
			public void Do()
			{
			}
		}

		public sealed class FooSealed { }
		class FooService : IFooService { }
		interface IFooService { }

		public class FooWithPrivateSetter
		{
			public virtual string Foo { get; private set; }
		}

		public class ClassWithNoDefaultConstructor
		{
			public ClassWithNoDefaultConstructor(string stringValue, int intValue)
			{
				this.StringValue = stringValue;
				this.IntValue = intValue;
			}

			public string StringValue { get; set; }
			public int IntValue { get; set; }
		}

		public abstract class FooWithConstructors
		{
			public FooWithConstructors(string stringValue, int intValue)
			{
				this.StringValue = stringValue;
				this.IntValue = intValue;
			}

			public FooWithConstructors()
			{
			}

			public override string ToString()
			{
				return base.ToString();
			}

			public string StringValue { get; set; }
			public int IntValue { get; set; }
		}

		public class FooOverrideEquals
		{
			public string Name { get; set; }

			public override bool Equals(object obj)
			{
				return (obj is FooOverrideEquals) &&
					((FooOverrideEquals)obj).Name == this.Name;
			}

			public override int GetHashCode()
			{
				return Name.GetHashCode();
			}
		}

		public interface IFoo
		{
			object Object { get; set; }
			IBar Bar { get; set; }
			int Count { set; }
			int? Value { get; set; }
			int Echo(int value);
			void Submit();
			string Execute(string command);
			int this[int index] { get; set; }
		}

		public interface IParams
		{
			string ExecuteByName(string name, params object[] args);
			string ExecuteParams(params string[] args);
			string ExecuteArray(string[] args);
		}

		public abstract class FooBase
		{
			public int ValueField;
			public abstract void Do(int value);

			public virtual bool Check(string value)
			{
				return true;
			}

			public bool GetIsProtected()
			{
				return IsProtected();
			}

			protected virtual bool IsProtected()
			{
				return true;
			}

			public bool True()
			{
				return true;
			}

			public bool BaseCalled = false;

			public virtual void BaseCall()
			{
				BaseCalled = true;
			}

			public virtual int Generic<T>()
			{
				return 0;
			}

			public bool BaseReturnCalled = false;

			public virtual bool BaseCall(string value)
			{
				BaseReturnCalled = true;
				return default(bool);
			}
		}

		public interface INewFoo : IFoo
		{
			new INewBar Bar { get; set; }
		}

		public interface INewBar : IBar
		{
		}
	}
}