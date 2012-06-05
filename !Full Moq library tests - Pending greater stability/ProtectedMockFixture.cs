using System;
using System.Diagnostics;
using System.Reflection;
using Moq;
using Moq.Protected;
using MoqRT;

#if !NETFX_CORE
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif

namespace Moq.Tests
{
    [TestClass]
    public partial class ProtectedMockFixture
	{
		[TestMethod]
		public void ThrowsIfNullMock()
		{
			AssertHelper.Throws<ArgumentNullException>(() => ProtectedExtension.Protected((Mock<string>)null));
		}

		[TestMethod]
		public void ThrowsIfSetupNullVoidMethodName()
		{
			AssertHelper.Throws<ArgumentNullException>(() => new Mock<FooBase>().Protected().Setup(null));
		}

		[TestMethod]
		public void ThrowsIfSetupEmptyVoidMethodName()
		{
			AssertHelper.Throws<ArgumentException>(() => new Mock<FooBase>().Protected().Setup(string.Empty));
		}

		[TestMethod]
		public void ThrowsIfSetupResultNullMethodName()
		{
			AssertHelper.Throws<ArgumentNullException>(() => new Mock<FooBase>().Protected().Setup<int>(null));
		}

		[TestMethod]
		public void ThrowsIfSetupResultEmptyMethodName()
		{
			AssertHelper.Throws<ArgumentException>(() => new Mock<FooBase>().Protected().Setup<int>(string.Empty));
		}

		[TestMethod]
		public void ThrowsIfSetupVoidMethodNotFound()
		{
			AssertHelper.Throws<ArgumentException>(() => new Mock<FooBase>().Protected().Setup("Foo"));
		}

		[TestMethod]
		public void ThrowsIfSetupResultMethodNotFound()
		{
			AssertHelper.Throws<ArgumentException>(() => new Mock<FooBase>().Protected().Setup<int>("Foo"));
		}

		[TestMethod]
		public void ThrowsIfSetupPublicVoidMethod()
		{
			AssertHelper.Throws<ArgumentException>(() => new Mock<FooBase>().Protected().Setup("Public"));
		}

		[TestMethod]
		public void ThrowsIfSetupPublicResultMethod()
		{
			AssertHelper.Throws<ArgumentException>(() => new Mock<FooBase>().Protected().Setup<int>("PublicInt"));
		}

		[TestMethod]
		public void ThrowsIfSetupNonVirtualVoidMethod()
		{
			AssertHelper.Throws<NotSupportedException>(() => new Mock<FooBase>().Protected().Setup("NonVirtual"));
		}

		[TestMethod]
		public void ThrowsIfSetupNonVirtualResultMethod()
		{
			AssertHelper.Throws<NotSupportedException>(() => new Mock<FooBase>().Protected().Setup<int>("NonVirtualInt"));
		}

		[TestMethod]
		public void SetupAllowsProtectedInternalVoidMethod()
		{
			var mock = new Mock<FooBase>();
			mock.Protected().Setup("ProtectedInternal");
			mock.Object.ProtectedInternal();

			mock.VerifyAll();
		}

		[TestMethod]
		public void SetupAllowsProtectedInternalResultMethod()
		{
			var mock = new Mock<FooBase>();
			mock.Protected()
				.Setup<int>("ProtectedInternalInt")
				.Returns(5);

			Assert.AreEqual(5, mock.Object.ProtectedInternalInt());
		}

		[TestMethod]
		public void SetupAllowsProtectedVoidMethod()
		{
			var mock = new Mock<FooBase>();
			mock.Protected().Setup("Protected");
			mock.Object.DoProtected();

			mock.VerifyAll();
		}

		[TestMethod]
		public void SetupAllowsProtectedResultMethod()
		{
			var mock = new Mock<FooBase>();
			mock.Protected()
				.Setup<int>("ProtectedInt")
				.Returns(5);

			Assert.AreEqual(5, mock.Object.DoProtectedInt());
		}

		[TestMethod]
		public void ThrowsIfSetupVoidMethodIsProperty()
		{
			AssertHelper.Throws<ArgumentException>(() => new Mock<FooBase>().Protected().Setup("ProtectedValue"));
		}

		[TestMethod]
		public void SetupResultAllowsProperty()
		{
			var mock = new Mock<FooBase>();
			mock.Protected()
				.Setup<string>("ProtectedValue")
				.Returns("foo");

			Assert.AreEqual("foo", mock.Object.GetProtectedValue());
		}

		[TestMethod]
		public void ThrowsIfSetupGetNullPropertyName()
		{
			AssertHelper.Throws<ArgumentNullException>(() => new Mock<FooBase>().Protected().SetupGet<string>(null));
		}

		[TestMethod]
		public void ThrowsIfSetupGetEmptyPropertyName()
		{
			AssertHelper.Throws<ArgumentException>(() => new Mock<FooBase>().Protected().SetupGet<string>(string.Empty));
		}

		[TestMethod]
		public void ThrowsIfSetupGetPropertyNotFound()
		{
			AssertHelper.Throws<ArgumentException>(() => new Mock<FooBase>().Protected().SetupGet<int>("Foo"));
		}

		[TestMethod]
		public void ThrowsIfSetupGetPropertyWithoutPropertyGet()
		{
			AssertHelper.Throws<ArgumentException>(() => new Mock<FooBase>().Protected().SetupGet<int>("OnlySet"));
		}

		[TestMethod]
		public void ThrowsIfSetupGetPublicPropertyGet()
		{
			AssertHelper.Throws<ArgumentException>(() => new Mock<FooBase>().Protected().SetupGet<int>("PublicValue"));
		}

		[TestMethod]
		public void ThrowsIfSetupGetNonVirtualProperty()
		{
			AssertHelper.Throws<NotSupportedException>(() => new Mock<FooBase>().Protected().SetupGet<string>("NonVirtualValue"));
		}

		[TestMethod]
		public void SetupGetAllowsProtectedInternalPropertyGet()
		{
			var mock = new Mock<FooBase>();
			mock.Protected()
				.SetupGet<string>("ProtectedInternalValue")
				.Returns("foo");

			Assert.AreEqual("foo", mock.Object.ProtectedInternalValue);
		}

		[TestMethod]
		public void SetupGetAllowsProtectedPropertyGet()
		{
			var mock = new Mock<FooBase>();
			mock.Protected()
				.SetupGet<string>("ProtectedValue")
				.Returns("foo");

			Assert.AreEqual("foo", mock.Object.GetProtectedValue());
		}

		[TestMethod]
		public void ThrowsIfSetupSetNullPropertyName()
		{
			AssertHelper.Throws<ArgumentNullException>(
				() => new Mock<FooBase>().Protected().SetupSet<string>(null, ItExpr.IsAny<string>()));
		}

		[TestMethod]
		public void ThrowsIfSetupSetEmptyPropertyName()
		{
			AssertHelper.Throws<ArgumentException>(
				() => new Mock<FooBase>().Protected().SetupSet<string>(string.Empty, ItExpr.IsAny<string>()));
		}

		[TestMethod]
		public void ThrowsIfSetupSetPropertyNotFound()
		{
			AssertHelper.Throws<ArgumentException>(
				() => new Mock<FooBase>().Protected().SetupSet<int>("Foo", ItExpr.IsAny<int>()));
		}

		[TestMethod]
		public void ThrowsIfSetupSetPropertyWithoutPropertySet()
		{
			AssertHelper.Throws<ArgumentException>(
				() => new Mock<FooBase>().Protected().SetupSet<int>("OnlyGet", ItExpr.IsAny<int>()));
		}

		[TestMethod]
		public void ThrowsIfSetupSetPublicPropertySet()
		{
			AssertHelper.Throws<ArgumentException>(
				() => new Mock<FooBase>().Protected().SetupSet<int>("PublicValue", ItExpr.IsAny<int>()));
		}

		[TestMethod]
		public void ThrowsIfSetupSetNonVirtualProperty()
		{
			AssertHelper.Throws<ArgumentException>(
				() => new Mock<FooBase>().Protected().SetupSet<string>("NonVirtualValue", ItExpr.IsAny<string>()));
		}

		[TestMethod]
		public void SetupSetAllowsProtectedInternalPropertySet()
		{
			var mock = new Mock<FooBase>();
			var value = string.Empty;
			mock.Protected()
				.SetupSet<string>("ProtectedInternalValue", ItExpr.IsAny<string>())
				.Callback(v => value = v);

			mock.Object.ProtectedInternalValue = "foo";

			Assert.AreEqual("foo", value);
			mock.VerifyAll();
		}

		[TestMethod]
		public void SetupSetAllowsProtectedPropertySet()
		{
			var mock = new Mock<FooBase>();
			var value = string.Empty;
			mock.Protected()
				.SetupSet<string>("ProtectedValue", ItExpr.IsAny<string>())
				.Callback(v => value = v);

			mock.Object.SetProtectedValue("foo");

			Assert.AreEqual("foo", value);
			mock.VerifyAll();
		}

		[TestMethod]
		public void ThrowsIfNullArgs()
		{
			AssertHelper.Throws<ArgumentException>(() => new Mock<FooBase>().Protected()
				.Setup<string>("StringArg", null)
				.Returns("null"));
		}

		[TestMethod]
		public void AllowMatchersForArgs()
		{
			var mock = new Mock<FooBase>();

			mock.Protected()
				.Setup<string>("StringArg", ItExpr.IsNull<string>())
				.Returns("null");

			Assert.AreEqual("null", mock.Object.DoStringArg(null));

			mock.Protected()
				.Setup<string>("StringArg", ItExpr.Is<string>(s => s == "bar"))
				.Returns("baz");

			Assert.AreEqual("baz", mock.Object.DoStringArg("bar"));

			mock = new Mock<FooBase>();

			mock.Protected()
				.Setup<string>("StringArg", ItExpr.Is<string>(s => s.Length >= 2))
				.Returns("long");
			mock.Protected()
				.Setup<string>("StringArg", ItExpr.Is<string>(s => s.Length < 2))
				.Returns("short");

			Assert.AreEqual("short", mock.Object.DoStringArg("f"));
			Assert.AreEqual("long", mock.Object.DoStringArg("foo"));

			mock = new Mock<FooBase>();
			mock.CallBase = true;

			mock.Protected()
				.Setup<string>("TwoArgs", ItExpr.IsAny<string>(), 5)
				.Returns("done");

			Assert.AreEqual("done", mock.Object.DoTwoArgs("foobar", 5));
			Assert.AreEqual("echo", mock.Object.DoTwoArgs("echo", 15));

			mock = new Mock<FooBase>();
			mock.CallBase = true;

			mock.Protected()
				.Setup<string>("TwoArgs", ItExpr.IsAny<string>(), ItExpr.IsInRange(1, 3, Range.Inclusive))
				.Returns("inrange");

			Assert.AreEqual("inrange", mock.Object.DoTwoArgs("foobar", 2));
			Assert.AreEqual("echo", mock.Object.DoTwoArgs("echo", 4));
		}

		[TestMethod]
		public void ResolveOverloads()
		{
			// NOTE: There are two overloads named "Do" and "DoReturn"
			var mock = new Mock<MethodOverloads>();
			mock.Protected().Setup("Do", 1, 2).Verifiable();
			mock.Protected().Setup<string>("DoReturn", "1", "2").Returns("3").Verifiable();

			mock.Object.ExecuteDo(1, 2);
			Assert.AreEqual("3", mock.Object.ExecuteDoReturn("1", "2"));

			mock.Verify();
		}

		[TestMethod]
		public void ThrowsIfSetReturnsForVoidMethod()
		{
			AssertHelper.Throws<ArgumentException>(
				() => new Mock<MethodOverloads>().Protected().Setup<string>("Do", "1", "2").Returns("3"));
		}

		[TestMethod]
		public void SetupResultAllowsProtectedMethodInBaseClass()
		{
			var mock = new Mock<FooDerived>();
			mock.Protected()
				.Setup<int>("ProtectedInt")
				.Returns(5);

			Assert.AreEqual(5, mock.Object.DoProtectedInt());
		}

		[TestMethod]
		public void ThrowsIfVerifyNullVoidMethodName()
		{
			AssertHelper.Throws<ArgumentNullException>(() => new Mock<FooBase>().Protected().Verify(null, Times.Once()));
		}

		[TestMethod]
		public void ThrowsIfVerifyEmptyVoidMethodName()
		{
			AssertHelper.Throws<ArgumentException>(() => new Mock<FooBase>().Protected().Verify(string.Empty, Times.Once()));
		}

		[TestMethod]
		public void ThrowsIfVerifyNullResultMethodName()
		{
			AssertHelper.Throws<ArgumentNullException>(() => new Mock<FooBase>().Protected().Verify<int>(null, Times.Once()));
		}

		[TestMethod]
		public void ThrowsIfVerifyEmptyResultMethodName()
		{
			AssertHelper.Throws<ArgumentException>(() => new Mock<FooBase>().Protected().Verify<int>(string.Empty, Times.Once()));
		}

		[TestMethod]
		public void ThrowsIfVerifyVoidMethodNotFound()
		{
			AssertHelper.Throws<ArgumentException>(() => new Mock<FooBase>().Protected().Verify("Foo", Times.Once()));
		}

		[TestMethod]
		public void ThrowsIfVerifyResultMethodNotFound()
		{
			AssertHelper.Throws<ArgumentException>(() => new Mock<FooBase>().Protected().Verify<int>("Foo", Times.Once()));
		}

		[TestMethod]
		public void ThrowsIfVerifyPublicVoidMethod()
		{
			AssertHelper.Throws<ArgumentException>(() => new Mock<FooBase>().Protected().Verify("Public", Times.Once()));
		}

		[TestMethod]
		public void ThrowsIfVerifyPublicResultMethod()
		{
			AssertHelper.Throws<ArgumentException>(
				() => new Mock<FooBase>().Protected().Verify<int>("PublicInt", Times.Once()));
		}

		[TestMethod]
		public void ThrowsIfVerifyNonVirtualVoidMethod()
		{
			AssertHelper.Throws<NotSupportedException>(() => new Mock<FooBase>().Protected().Verify("NonVirtual", Times.Once()));
		}

		[TestMethod]
		public void ThrowsIfVerifyNonVirtualResultMethod()
		{
			AssertHelper.Throws<NotSupportedException>(
				() => new Mock<FooBase>().Protected().Verify<int>("NonVirtualInt", Times.Once()));
		}

		[TestMethod]
		public void VerifyAllowsProtectedInternalVoidMethod()
		{
			var mock = new Mock<FooBase>();
			mock.Object.ProtectedInternal();

			mock.Protected().Verify("ProtectedInternal", Times.Once());
		}

		[TestMethod]
		public void VerifyAllowsProtectedInternalResultMethod()
		{
			var mock = new Mock<FooBase>();
			mock.Object.ProtectedInternalInt();

			mock.Protected().Verify<int>("ProtectedInternalInt", Times.Once());
		}

		[TestMethod]
		public void VerifyAllowsProtectedVoidMethod()
		{
			var mock = new Mock<FooBase>();
			mock.Object.DoProtected();

			mock.Protected().Verify("Protected", Times.Once());
		}

		[TestMethod]
		public void VerifyAllowsProtectedResultMethod()
		{
			var mock = new Mock<FooBase>();
			mock.Object.DoProtectedInt();

			mock.Protected().Verify<int>("ProtectedInt", Times.Once());
		}

		[TestMethod]
		public void ThrowsIfVerifyVoidMethodIsProperty()
		{
			AssertHelper.Throws<ArgumentException>(
				() => new Mock<FooBase>().Protected().Verify("ProtectedValue", Times.Once()));
		}

		[TestMethod]
		public void VerifyResultAllowsProperty()
		{
			var mock = new Mock<FooBase>();
			mock.Object.GetProtectedValue();

			mock.Protected().Verify<string>("ProtectedValue", Times.Once());
		}

		[TestMethod]
		public void ThrowsIfVerifyVoidMethodTimesNotReached()
		{
			var mock = new Mock<FooBase>();
			mock.Object.DoProtected();

			AssertHelper.Throws<MockException>(() => mock.Protected().Verify("Protected", Times.Exactly(2)));
		}

		[TestMethod]
		public void ThrowsIfVerifyResultMethodTimesNotReached()
		{
			var mock = new Mock<FooBase>();
			mock.Object.DoProtectedInt();

			AssertHelper.Throws<MockException>(() => mock.Protected().Verify("ProtectedInt", Times.Exactly(2)));
		}

		[TestMethod]
		public void DoesNotThrowIfVerifyVoidMethodTimesReached()
		{
			var mock = new Mock<FooBase>();
			mock.Object.DoProtected();
			mock.Object.DoProtected();

			mock.Protected().Verify("Protected", Times.Exactly(2));
		}

		[TestMethod]
		public void DoesNotThrowIfVerifyReturnMethodTimesReached()
		{
			var mock = new Mock<FooBase>();
			mock.Object.DoProtectedInt();
			mock.Object.DoProtectedInt();

			mock.Protected().Verify<int>("ProtectedInt", Times.Exactly(2));
		}

		[TestMethod]
		public void ThrowsIfVerifyPropertyTimesNotReached()
		{
			var mock = new Mock<FooBase>();
			mock.Object.GetProtectedValue();

			AssertHelper.Throws<MockException>(() => mock.Protected().Verify<string>("ProtectedValue", Times.Exactly(2)));
		}

		[TestMethod]
		public void DoesNotThrowIfVerifyPropertyTimesReached()
		{
			var mock = new Mock<FooBase>();
			mock.Object.GetProtectedValue();
			mock.Object.GetProtectedValue();

			mock.Protected().Verify<string>("ProtectedValue", Times.Exactly(2));
		}

		[TestMethod]
		public void ThrowsIfVerifyGetNullPropertyName()
		{
			AssertHelper.Throws<ArgumentNullException>(() => new Mock<FooBase>().Protected().VerifyGet<int>(null, Times.Once()));
		}

		[TestMethod]
		public void ThrowsIfVerifyGetEmptyPropertyName()
		{
			AssertHelper.Throws<ArgumentException>(() => new Mock<FooBase>().Protected().VerifyGet<int>(string.Empty, Times.Once()));
		}

		[TestMethod]
		public void ThrowsIfVerifyGetPropertyNotFound()
		{
			AssertHelper.Throws<ArgumentException>(() => new Mock<FooBase>().Protected().VerifyGet<int>("Foo", Times.Once()));
		}

		[TestMethod]
		public void ThrowsIfVerifyGetPropertyWithoutPropertyGet()
		{
			AssertHelper.Throws<ArgumentException>(() => new Mock<FooBase>().Protected().VerifyGet<int>("OnlySet", Times.Once()));
		}

		[TestMethod]
		public void ThrowsIfVerifyGetIsPublicPropertyGet()
		{
			AssertHelper.Throws<ArgumentException>(
				() => new Mock<FooBase>().Protected().VerifyGet<string>("PublicValue", Times.Once()));
		}

		[TestMethod]
		public void VerifyGetAllowsProtectedInternalPropertyGet()
		{
			var mock = new Mock<FooBase>();
			var value = mock.Object.ProtectedInternalValue;

			mock.Protected().VerifyGet<string>("ProtectedInternalValue", Times.Once());
		}

		[TestMethod]
		public void VerifyGetAllowsProtectedPropertyGet()
		{
			var mock = new Mock<FooBase>();
			mock.Object.GetProtectedValue();

			mock.Protected().VerifyGet<string>("ProtectedValue", Times.Once());
		}

		[TestMethod]
		public void ThrowsIfVerifyGetNonVirtualPropertyGet()
		{
			AssertHelper.Throws<NotSupportedException>(
				() => new Mock<FooBase>().Protected().VerifyGet<string>("NonVirtualValue", Times.Once()));
		}

		[TestMethod]
		public void ThrowsIfVerifyGetTimesNotReached()
		{
			var mock = new Mock<FooBase>();
			mock.Object.GetProtectedValue();

			AssertHelper.Throws<MockException>(() => mock.Protected().VerifyGet<string>("ProtectedValue", Times.Exactly(2)));
		}

		[TestMethod]
		public void DoesNotThrowIfVerifyGetPropertyTimesReached()
		{
			var mock = new Mock<FooBase>();
			mock.Object.GetProtectedValue();
			mock.Object.GetProtectedValue();

			mock.Protected().VerifyGet<string>("ProtectedValue", Times.Exactly(2));
		}
	}

	public partial class ProtectedMockFixture
	{

		[TestMethod]
		public void ThrowsIfVerifySetNullPropertyName()
		{
			AssertHelper.Throws<ArgumentNullException>(
				() => new Mock<FooBase>().Protected().VerifySet<string>(null, Times.Once(), ItExpr.IsAny<string>()));
		}

		[TestMethod]
		public void ThrowsIfVerifySetEmptyPropertyName()
		{
			AssertHelper.Throws<ArgumentException>(
				() => new Mock<FooBase>().Protected().VerifySet<string>(string.Empty, Times.Once(), ItExpr.IsAny<int>()));
		}

		[TestMethod]
		public void ThrowsIfVerifySetPropertyNotFound()
		{
			AssertHelper.Throws<ArgumentException>(
				() => new Mock<FooBase>().Protected().VerifySet<int>("Foo", Times.Once(), ItExpr.IsAny<int>()));
		}

		[TestMethod]
		public void ThrowsIfVerifySetPropertyWithoutPropertySet()
		{
			AssertHelper.Throws<ArgumentException>(
				() => new Mock<FooBase>().Protected().VerifySet<int>("OnlyGet", Times.Once(), ItExpr.IsAny<int>()));
		}

		[TestMethod]
		public void ThrowsIfVerifySetPublicPropertySet()
		{
			AssertHelper.Throws<ArgumentException>(
				() => new Mock<FooBase>().Protected().VerifySet<int>("PublicValue", Times.Once(), ItExpr.IsAny<int>()));
		}

		[TestMethod]
		public void ThrowsIfVerifySetNonVirtualPropertySet()
		{
			AssertHelper.Throws<ArgumentException>(
				() => new Mock<FooBase>().Protected().VerifySet<string>("NonVirtualValue", Times.Once(), ItExpr.IsAny<string>()));
		}

		[TestMethod]
		public void VerifySetAllowsProtectedInternalPropertySet()
		{
			var mock = new Mock<FooBase>();
			mock.Object.ProtectedInternalValue = "foo";

			mock.Protected().VerifySet<string>("ProtectedInternalValue", Times.Once(), "bar");
		}

		[TestMethod]
		public void VerifySetAllowsProtectedPropertySet()
		{
			var mock = new Mock<FooBase>();
			mock.Object.SetProtectedValue("foo");

			mock.Protected().VerifySet<string>("ProtectedValue", Times.Once(), ItExpr.IsAny<string>());
		}

		[TestMethod]
		public void ThrowsIfVerifySetTimesNotReached()
		{
			var mock = new Mock<FooBase>();
			mock.Object.SetProtectedValue("Foo");

			AssertHelper.Throws<MockException>(
				() => mock.Protected().VerifySet<string>("ProtectedValue", Times.Exactly(2), ItExpr.IsAny<string>()));
		}

		[TestMethod]
		public void DoesNotThrowIfVerifySetPropertyTimesReached()
		{
			var mock = new Mock<FooBase>();
			mock.Object.SetProtectedValue("foo");
			mock.Object.SetProtectedValue("foo");

			mock.Protected().VerifySet<string>("ProtectedValue", Times.Exactly(2), ItExpr.IsAny<int>());
		}

		public class MethodOverloads
		{
			public void ExecuteDo(int a, int b)
			{
				this.Do(a, b);
			}

			public void ExecuteDo(string a, string b)
			{
				this.Do(a, b);
			}

			public int ExecuteDoReturn(int a, int b)
			{
				return this.DoReturn(a, b);
			}

			protected virtual void Do(int a, int b)
			{
			}

			protected virtual void Do(string a, string b)
			{
			}

			protected virtual int DoReturn(int a, int b)
			{
				return a + b;
			}

			public string ExecuteDoReturn(string a, string b)
			{
				return DoReturn(a, b);
			}

			protected virtual string DoReturn(string a, string b)
			{
				return a + b;
			}
		}

		public class FooBase
		{
			public virtual string PublicValue { get; set; }

			protected internal virtual string ProtectedInternalValue { get; set; }

			protected string NonVirtualValue { get; set; }

			protected virtual int OnlyGet
			{
				get { return 0; }
			}

			protected virtual int OnlySet
			{
				set { }
			}

			protected virtual string ProtectedValue { get; set; }

			protected virtual int this[int index]
			{
				get { return 0; }
				set { }
			}

			public void DoProtected()
			{
				this.Protected();
			}

			public int DoProtectedInt()
			{
				return this.ProtectedInt();
			}

			public string DoStringArg(string arg)
			{
				return this.StringArg(arg);
			}

			public string DoTwoArgs(string arg, int arg1)
			{
				return this.TwoArgs(arg, arg1);
			}

			public string GetProtectedValue()
			{
				return this.ProtectedValue;
			}

			public virtual void Public()
			{
			}

			public virtual int PublicInt()
			{
				return 10;
			}

			public void SetProtectedValue(string value)
			{
				this.ProtectedValue = value;
			}

			internal protected virtual void ProtectedInternal()
			{
			}

			internal protected virtual int ProtectedInternalInt()
			{
				return 0;
			}

			protected virtual void Protected()
			{
			}

			protected virtual int ProtectedInt()
			{
				return 2;
			}

			protected void NonVirtual()
			{
			}

			protected int NonVirtualInt()
			{
				return 2;
			}

			protected virtual string StringArg(string arg)
			{
				return arg;
			}

			protected virtual string TwoArgs(string arg, int arg1)
			{
				return arg;
			}
		}

		public class FooDerived : FooBase
		{
		}
	}
}