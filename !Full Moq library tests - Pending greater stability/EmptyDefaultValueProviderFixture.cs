using System;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Emit;
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
    public class EmptyDefaultValueProviderFixture
	{
		[TestMethod]
		public void ProvidesNullString()
		{
			var provider = new EmptyDefaultValueProvider();

			var value = provider.ProvideDefault(typeof(IFoo).GetProperty("StringValue").GetGetMethod());

			Assert.IsNull(value);
		}

		[TestMethod]
		public void ProvidesDefaultInt()
		{
			var provider = new EmptyDefaultValueProvider();

			var value = provider.ProvideDefault(typeof(IFoo).GetProperty("IntValue").GetGetMethod());

			Assert.AreEqual(default(int), value);
		}

		[TestMethod]
		public void ProvidesNullInt()
		{
			var provider = new EmptyDefaultValueProvider();

			var value = provider.ProvideDefault(typeof(IFoo).GetProperty("NullableIntValue").GetGetMethod());

			Assert.IsNull(value);
		}

		[TestMethod]
		public void ProvidesDefaultBool()
		{
			var provider = new EmptyDefaultValueProvider();

			var value = provider.ProvideDefault(typeof(IFoo).GetProperty("BoolValue").GetGetMethod());

			Assert.AreEqual(default(bool), value);
		}

        [TestMethod]
		public void ProvidesDefaultEnum()
		{
			var provider = new EmptyDefaultValueProvider();

			var value = provider.ProvideDefault(typeof(IFoo).GetProperty("Platform").GetGetMethod());

			Assert.AreEqual(default(OpCode), value);
		}

		[TestMethod]
		public void ProvidesEmptyEnumerable()
		{
			var provider = new EmptyDefaultValueProvider();

			var value = provider.ProvideDefault(typeof(IFoo).GetProperty("Indexes").GetGetMethod());
			Assert.IsTrue(value is IEnumerable<int> && ((IEnumerable<int>)value).Count() == 0);
		}

		[TestMethod]
		public void ProvidesEmptyArray()
		{
			var provider = new EmptyDefaultValueProvider();

			var value = provider.ProvideDefault(typeof(IFoo).GetProperty("Bars").GetGetMethod());
			Assert.IsTrue(value is IBar[] && ((IBar[])value).Length == 0);
		}

		[TestMethod]
		public void ProvidesNullReferenceTypes()
		{
			var provider = new EmptyDefaultValueProvider();

			var value1 = provider.ProvideDefault(typeof(IFoo).GetProperty("Bar").GetGetMethod());
			var value2 = provider.ProvideDefault(typeof(IFoo).GetProperty("Object").GetGetMethod());

			Assert.IsNull(value1);
			Assert.IsNull(value2);
		}

		[TestMethod]
		public void ProvideEmptyQueryable()
		{
			var provider = new EmptyDefaultValueProvider();
			var value = provider.ProvideDefault(typeof(IFoo).GetProperty("Queryable").GetGetMethod());

			AssertHelper.IsAssignableFrom<IQueryable<int>>(value);
			Assert.AreEqual(0, ((IQueryable<int>)value).Count());
		}

		[TestMethod]
		public void ProvideEmptyQueryableObjects()
		{
			var provider = new EmptyDefaultValueProvider();
			var value = provider.ProvideDefault(typeof(IFoo).GetProperty("QueryableObjects").GetGetMethod());

			AssertHelper.IsAssignableFrom<IQueryable>(value);
			Assert.AreEqual(0, ((IQueryable)value).Cast<object>().Count());
		}

		public interface IFoo
		{
			object Object { get; set; }
			IBar Bar { get; set; }
			string StringValue { get; set; }
			int IntValue { get; set; }
			bool BoolValue { get; set; }
			int? NullableIntValue { get; set; }
			OpCode Platform { get; set; }
			IEnumerable<int> Indexes { get; set; }
			IBar[] Bars { get; set; }
			IQueryable<int> Queryable { get; }
			IQueryable QueryableObjects { get; }
		}

		public interface IBar { }
	}
}
