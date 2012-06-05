using System;
using System.Diagnostics;
using System.Reflection;
using Moq;

#if !NETFX_CORE
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif

namespace Moq.Tests
{
    [TestClass]
    public class ConditionalSetupFixture
	{
		[TestMethod]
		public void ChooseAffirmativeExpectationOnMethod()
		{
			var mock = new Mock<IFoo>();

			var when = true;

			mock.When(() => when).Setup(x => x.Foo()).Returns("bar");
			mock.When(() => !when).Setup(x => x.Foo()).Returns("no bar");

			Assert.AreEqual("bar", mock.Object.Foo());

			when = false;
			Assert.AreEqual("no bar", mock.Object.Foo());

			when = true;
			Assert.AreEqual("bar", mock.Object.Foo());
		}

		[TestMethod]
		public void ChooseAffirmativeExpetationOnVoidMethod()
		{
			var mock = new Mock<IFoo>();

			var when = true;
			var positive = false;
			var negative = false;

			mock.When(() => when).Setup(x => x.Bar()).Callback(() => positive = true);
			mock.When(() => !when).Setup(x => x.Bar()).Callback(() => negative = true);

			mock.Object.Bar();

			Assert.IsTrue(positive);
			Assert.IsFalse(negative);

			when = false;
			positive = false;
			mock.Object.Bar();

			Assert.IsFalse(positive);
			Assert.IsTrue(negative);

			when = true;
			negative = false;
			mock.Object.Bar();

			Assert.IsTrue(positive);
			Assert.IsFalse(negative);
		}

		[TestMethod]
		public void ChooseAffirmativeExpectationOnPropertyGetter()
		{
			var mock = new Mock<IFoo>();

			var first = true;

			mock.When(() => first).SetupGet(x => x.Value).Returns("bar");
			mock.When(() => !first).SetupGet(x => x.Value).Returns("no bar");

			Assert.AreEqual("bar", mock.Object.Value);
			first = false;
			Assert.AreEqual("no bar", mock.Object.Value);
			first = true;
			Assert.AreEqual("bar", mock.Object.Value);
		}

		[TestMethod]
		public void ChooseAffirmativeExpetationOnPropertySetter()
		{
			var mock = new Mock<IFoo>();

			var when = true;
			var positive = false;
			var negative = false;

			mock.When(() => when).SetupSet(x => x.Value = "foo").Callback(() => positive = true);
			mock.When(() => !when).SetupSet(x => x.Value = "foo").Callback(() => negative = true);

			mock.Object.Value = "foo";

			Assert.IsTrue(positive);
			Assert.IsFalse(negative);

			when = false;
			positive = false;
			mock.Object.Value = "foo";

			Assert.IsFalse(positive);
			Assert.IsTrue(negative);

			when = true;
			negative = false;
			mock.Object.Value = "foo";

			Assert.IsTrue(positive);
			Assert.IsFalse(negative);
		}

		[TestMethod]
		public void ChooseAffirmativeExpetationOnTypedPropertySetter()
		{
			var mock = new Mock<IFoo>();

			var when = true;
			var positive = false;
			var negative = false;

			mock.When(() => when).SetupSet<string>(x => x.Value = "foo").Callback(s => positive = true);
			mock.When(() => !when).SetupSet<string>(x => x.Value = "foo").Callback(s => negative = true);

			mock.Object.Value = "foo";

			Assert.IsTrue(positive);
			Assert.IsFalse(negative);

			when = false;
			positive = false;
			mock.Object.Value = "foo";

			Assert.IsFalse(positive);
			Assert.IsTrue(negative);

			when = true;
			negative = false;
			mock.Object.Value = "foo";

			Assert.IsTrue(positive);
			Assert.IsFalse(negative);
		}

		[TestMethod]
		public void ChooseAffirmativeExpectationOnPropertyIndexer()
		{
			var mock = new Mock<IFoo>();

			var first = true;

			mock.When(() => first).Setup(x => x[0]).Returns("bar");
			mock.When(() => !first).Setup(x => x[0]).Returns("no bar");

			Assert.AreEqual("bar", mock.Object[0]);
			first = false;
			Assert.AreEqual("no bar", mock.Object[0]);
			first = true;
			Assert.AreEqual("bar", mock.Object[0]);
		}

		public interface IFoo
		{
			string Value { get; set; }
			string this[int index] { get; }
			void Bar();
			string Foo();
		}
	}
}