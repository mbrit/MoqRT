using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
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
    public class QueryableMocksFixture
	{
		[TestMethod]
		public void ShouldSupportReturningMultipleMocks()
		{
			var target = (from foo in Mocks.Of<IFoo>()
						  from bar in Mocks.Of<IBar>()
						  where
							foo.Name == "Foo" &&
							foo.Find("1").Baz(It.IsAny<string>()).Value == 1 &&
							bar.Id == "A"
						  select new { Foo = foo, Bar = bar })
						  .First();

			Assert.AreEqual(target.Foo.Name, "Foo");
			Assert.AreEqual(target.Foo.Find("1").Baz("hello").Value, 1);
			Assert.AreEqual(target.Bar.Id, "A");
		}

		[TestMethod]
		public void ShouldSupportMultipleSetups()
		{
			var target = (from f in Mocks.Of<IFoo>()
						  where
							f.Name == "Foo" &&
							f.Find("1").Baz(It.Is<string>(s => s.Length > 0)).Value == 99 &&
							f.Bar.Id == "25" &&
							f.Bar.Ping(It.IsAny<string>()) == "ack" &&
							f.Bar.Ping("error") == "error" &&
							f.Bar.Baz(It.IsAny<string>()).Value == 5
						  select f)
						  .First();

			Assert.AreEqual(target.Name, "Foo");
			Assert.AreEqual(target.Find("1").Baz("asdf").Value, 99);
			Assert.AreEqual(target.Bar.Id, "25");
			Assert.AreEqual(target.Bar.Ping("blah"), "ack");
			Assert.AreEqual(target.Bar.Ping("error"), "error");
			Assert.AreEqual(target.Bar.Baz("foo").Value, 5);
		}

		[TestMethod]
		public void ShouldSupportEnum()
		{
			var target = Mocks.Of<IFoo>().First(f => f.Targets == AttributeTargets.Class);

			Assert.AreEqual(AttributeTargets.Class, target.Targets);
		}

		[TestMethod]
		public void ShoulSupportMethod()
		{
			var expected = new Mock<IBar>().Object;
			var target = Mocks.Of<IFoo>().First(x => x.Find(It.IsAny<string>()) == expected);

			Assert.AreEqual(expected, target.Find("3"));
		}

		[TestMethod]
		public void ShouldSupportIndexer()
		{
			var target = Mocks.Of<IBaz>().First(x => x["3", It.IsAny<bool>()] == 10);

			Assert.AreNotEqual(10, target["1", true]);
			Assert.AreEqual(10, target["3", true]);
			Assert.AreEqual(10, target["3", false]);
		}

		[TestMethod]
		public void ShouldSupportBooleanMethod()
		{
			var target = Mocks.Of<IBaz>().First(x => x.HasElements("3"));

			Assert.IsTrue(target.HasElements("3"));
		}

		[TestMethod]
		public void ShouldSupportBooleanMethodNegation()
		{
			var target = Mocks.Of<IBaz>().First(x => !x.HasElements("3"));

			Assert.IsFalse(target.HasElements("3"));
		}

		[TestMethod]
		public void ShouldSupportMultipleMethod()
		{
			var target = Mocks.Of<IBaz>().First(x => !x.HasElements("1") && x.HasElements("2"));

			Assert.IsFalse(target.HasElements("1"));
			Assert.IsTrue(target.HasElements("2"));
		}

		[TestMethod]
		public void ShouldSupportMocksFirst()
		{
			var target = Mocks.Of<IBaz>().First();

			Assert.IsNotNull(target);
		}

		[TestMethod]
		public void ShouldSupportMocksFirstOrDefault()
		{
			var target = Mocks.Of<IBaz>().FirstOrDefault();

			Assert.IsNotNull(target);
		}

		[TestMethod]
		public void ShouldSupportSettingDtoPropertyValue()
		{
			//var target = Mock.Of<IFoo>(x => x.Bar.Id == "foo");
			var target = Mock.Of<Dto>(x => x.Value == "foo");

			Assert.AreEqual("foo", target.Value);
		}

		[TestMethod]
		public void ShouldOneOfCreateNewMock()
		{
			var target = Mock.Of<IFoo>();

			Assert.IsNotNull(Mock.Get(target));
		}

		[TestMethod]
		public void ShouldOneOfWithPredicateCreateNewMock()
		{
			var target = Mock.Of<IFoo>(x => x.Name == "Foo");

			Assert.IsNotNull(Mock.Get(target));
			Assert.AreEqual("Foo", target.Name);
		}

		[TestMethod]
		public void ShouldAllowFluentOnReadOnlyGetterProperty()
		{
			var target = Mock.Of<IFoo>(x => x.Bars == new[] 
			{ 
				Mock.Of<IBar>(b => b.Id == "1"), 
				Mock.Of<IBar>(b => b.Id == "2"), 
			});

			Assert.IsNotNull(Mock.Get(target));
			Assert.AreEqual(2, target.Bars.Count());
		}

		[TestMethod]
		public void ShouldAllowFluentOnNonVirtualReadWriteProperty()
		{
			var target = Mock.Of<Dto>(x => x.Value == "foo");

			Assert.IsNotNull(Mock.Get(target));
			Assert.AreEqual("foo", target.Value);
		}

		public void Do()
		{
			Debug.WriteLine("Done");
		}

		public class Dto
		{
			public string Value { get; set; }
		}

		public interface IFoo
		{
			IBar Bar { get; set; }
			string Name { get; set; }
			IBar Find(string id);
			AttributeTargets Targets { get; set; }
			IEnumerable<IBar> Bars { get; }
		}

		public interface IBar
		{
			IBaz Baz(string value);
			string Id { get; set; }
			string Ping(string command);
		}

		public interface IBaz
		{
			int Value { get; set; }
			int this[string key1, bool key2] { get; set; }
			bool IsValid { get; set; }
			bool HasElements(string key1);
		}
	}
}