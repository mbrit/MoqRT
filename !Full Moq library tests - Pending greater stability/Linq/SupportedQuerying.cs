using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using Moq;

#if !NETFX_CORE
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif

namespace Moq.Tests.Linq
{
    [TestClass]
    public class SupportedQuerying
	{
		public class GivenABooleanProperty
		{
			[TestMethod]
			public void WhenImplicitlyQueryingTrueOneOf_ThenSetsPropertyToTrue()
			{
				var target = Mock.Of<IFoo>(x => x.IsValid);

				Assert.IsTrue(target.IsValid);
			}

			[TestMethod]
			public void WhenImplicitlyQueryingTrueWhere_ThenSetsPropertyToTrue()
			{
				var target = Mocks.Of<IFoo>().Where(x => x.IsValid);

				Assert.IsTrue(target.First().IsValid);
			}

			[TestMethod]
			public void WhenImplicitlyQueryingTrueFirst_ThenSetsPropertyToTrue()
			{
				var target = Mocks.Of<IFoo>().First(x => x.IsValid);

				Assert.IsTrue(target.IsValid);
			}

			[TestMethod]
			public void WhenImplicitlyQueryingTrueFirstOrDefault_ThenSetsPropertyToTrue()
			{
				var target = Mocks.Of<IFoo>().FirstOrDefault(x => x.IsValid);

				Assert.IsTrue(target.IsValid);
			}

			[TestMethod]
			public void WhenExplicitlyQueryingTrueOneOf_ThenSetsPropertyToTrue()
			{
				var target = Mock.Of<IFoo>(x => x.IsValid == true);

				Assert.IsTrue(target.IsValid);
			}

			[TestMethod]
			public void WhenExplicitlyQueryingTrueWhere_ThenSetsPropertyToTrue()
			{
				var target = Mocks.Of<IFoo>().Where(x => x.IsValid == true);

				Assert.IsTrue(target.First().IsValid);
			}

			[TestMethod]
			public void WhenExplicitlyQueryingTrueFirst_ThenSetsPropertyToTrue()
			{
				var target = Mocks.Of<IFoo>().First(x => x.IsValid == true);

				Assert.IsTrue(target.IsValid);
			}

			[TestMethod]
			public void WhenExplicitlyQueryingTrueFirstOrDefault_ThenSetsPropertyToTrue()
			{
				var target = Mocks.Of<IFoo>().FirstOrDefault(x => x.IsValid == true);

				Assert.IsTrue(target.IsValid);
			}

			[TestMethod]
			public void WhenQueryingOnFluent_ThenSetsPropertyToTrue()
			{
				var target = Mocks.Of<IFluent>().FirstOrDefault(x => x.Foo.IsValid == true);

				Assert.IsTrue(target.Foo.IsValid);
			}

			[TestMethod]
			public void WhenQueryingWithFalse_ThenSetsProperty()
			{
				var target = Mock.Of<FooDefaultIsValid>(x => x.IsValid == false);

				Assert.IsFalse(target.IsValid);
			}

			[TestMethod]
			public void WhenQueryingTrueEquals_ThenSetsProperty()
			{
				var target = Mock.Of<IFoo>(x => true == x.IsValid);

				Assert.IsTrue(target.IsValid);
			}

			[TestMethod]
			public void WhenQueryingFalseEquals_ThenSetsProperty()
			{
				var target = Mock.Of<FooDefaultIsValid>(x => false == x.IsValid);

				Assert.IsFalse(target.IsValid);
			}

			[TestMethod]
			public void WhenQueryingNegatedProperty_ThenSetsProperty()
			{
				var target = Mock.Of<FooDefaultIsValid>(x => !x.IsValid);

				Assert.IsFalse(target.IsValid);
			}

			[TestMethod]
			public void WhenQueryingWithNoValue_ThenAlwaysHasPropertyStubBehavior()
			{
				var foo = Mock.Of<IFoo>();

				foo.IsValid = true;

				Assert.IsTrue(foo.IsValid);

				foo.IsValid = false;

				Assert.IsFalse(foo.IsValid);
			}

			public class FooDefaultIsValid : IFoo
			{
				public FooDefaultIsValid()
				{
					this.IsValid = true;
				}

				public virtual bool IsValid { get; set; }
			}

			public interface IFoo
			{
				bool IsValid { get; set; }
			}

			public interface IFluent
			{
				IFoo Foo { get; set; }
			}
		}

		public class GivenAnEnumProperty
		{
			[TestMethod]
			public void WhenQueryingWithEnumValue_ThenSetsPropertyValue()
			{
				var target = Mocks.Of<IFoo>().First(f => f.Targets == AttributeTargets.Class);

				Assert.AreEqual(AttributeTargets.Class, target.Targets);
			}

            //[Fact(Skip = "Not implemented yet. Need to refactor old matcher stuff to the new one, require the MatcherAttribute on matchers, and verify it.")]
            //public void WhenQueryingWithItIsAny_ThenThrowsNotSupportedException()
            //{
            //    var target = Mocks.Of<IFoo>().First(f => f.Targets == It.IsAny<AttributeTargets>());

            //    Assert.AreEqual(AttributeTargets.Class, target.Targets);
            //}

			public interface IFoo
			{
				AttributeTargets Targets { get; set; }
			}
		}

		public class GivenTwoProperties
		{
			[TestMethod]
			public void WhenCombiningQueryingWithImplicitBoolean_ThenSetsBothProperties()
			{
				var target = Mock.Of<IFoo>(x => x.IsValid && x.Value == "foo");

				Assert.IsTrue(target.IsValid);
				Assert.AreEqual("foo", target.Value);
			}

			[TestMethod]
			public void WhenCombiningQueryingWithExplicitBoolean_ThenSetsBothProperties()
			{
				var target = Mock.Of<IFoo>(x => x.IsValid == true && x.Value == "foo");

				Assert.IsTrue(target.IsValid);
				Assert.AreEqual("foo", target.Value);
			}

			public interface IFoo
			{
				string Value { get; set; }
				bool IsValid { get; set; }
			}
		}

		public class GivenAMethodWithOneParameter
		{
			[TestMethod]
			public void WhenUsingSpecificArgumentValue_ThenSetsReturnValue()
			{
				var foo = Mock.Of<IFoo>(x => x.Do(5) == "foo");

				Assert.AreEqual("foo", foo.Do(5));
			}

			[TestMethod]
			public void WhenUsingItIsAnyForArgument_ThenSetsReturnValue()
			{
				var foo = Mock.Of<IFoo>(x => x.Do(It.IsAny<int>()) == "foo");

				Assert.AreEqual("foo", foo.Do(5));
			}

			[TestMethod]
			public void WhenUsingItIsForArgument_ThenSetsReturnValue()
			{
				var foo = Mock.Of<IFoo>(x => x.Do(It.Is<int>(i => i > 0)) == "foo");

				Assert.AreEqual("foo", foo.Do(5));
				Assert.AreEqual(default(string), foo.Do(-5));
			}

			[TestMethod]
			public void WhenUsingCustomMatcherForArgument_ThenSetsReturnValue()
			{
				var foo = Mock.Of<IFoo>(x => x.Do(Any<int>()) == "foo");

				Assert.AreEqual("foo", foo.Do(5));
			}

			public TValue Any<TValue>()
			{
				return Match.Create<TValue>(v => true);
			}

			public interface IFoo
			{
				string Do(int value);
			}
		}

		public class GivenAClassWithNonVirtualProperties
		{
			[TestMethod]
			public void WhenQueryingByProperties_ThenSetsThemDirectly()
			{
				var foo = Mock.Of<Foo>(x => x.Id == 1 && x.Value == "hello");

				Assert.AreEqual(1, foo.Id);
				Assert.AreEqual("hello", foo.Value);
			}

			public class Foo
			{
				public int Id { get; set; }
				public string Value { get; set; }
			}
		}

		public class GivenAReadonlyProperty
		{
			[TestMethod]
			public void WhenQueryingByProperties_ThenSetsThemDirectly()
			{
				var foo = Mock.Of<Foo>(x => x.Id == 1);

				Assert.AreEqual(1, foo.Id);
			}

			public class Foo
			{
				public virtual int Id { get { return 0; } }
			}
		}
	}
}