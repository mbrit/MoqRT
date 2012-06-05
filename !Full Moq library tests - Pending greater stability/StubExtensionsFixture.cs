using System;
using System.Diagnostics;
using System.Reflection;
using Moq;

#if !NETFX_CORE
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoqRT;
#endif

namespace Moq.Tests
{
    using MoqRT;
    public class StubExtensionsFixture
	{
		[TestMethod]
		public void ShouldStubPropertyWithoutInitialValue()
		{
			var mock = new Mock<IFoo>();

			mock.SetupProperty(f => f.ValueProperty);

			Assert.AreEqual(0, mock.Object.ValueProperty);

			mock.Object.ValueProperty = 5;

			Assert.AreEqual(5, mock.Object.ValueProperty);
		}

		[TestMethod]
		public void ShouldStubPropertyWithInitialValue()
		{
			var mock = new Mock<IFoo>();

			mock.SetupProperty(f => f.ValueProperty, 5);

			Assert.AreEqual(5, mock.Object.ValueProperty);

			mock.Object.ValueProperty = 15;

			Assert.AreEqual(15, mock.Object.ValueProperty);
		}

		[TestMethod]
		public void StubsAllProperties()
		{
			var mock = new Mock<IFoo>();

			mock.SetupAllProperties();

			mock.Object.ValueProperty = 5;
			Assert.AreEqual(5, mock.Object.ValueProperty);

			var obj = new object();
			mock.Object.Object = obj;
			AssertHelper.Same(obj, mock.Object.Object);

			var bar = new Mock<IBar>();
			mock.Object.Bar = bar.Object;
			AssertHelper.Same(bar.Object, mock.Object.Bar);
		}

		[TestMethod]
		public void StubsAllHierarchy()
		{
			var mock = new Mock<IFoo>() { DefaultValue = DefaultValue.Mock };

			mock.SetupAllProperties();

			mock.Object.Bar.Value = 5;
			Assert.AreEqual(5, mock.Object.Bar.Value);
		}

		[TestMethod]
		public void StubsInheritedInterfaceProperties()
		{
			var mock = new Mock<IBaz>();

			mock.SetupAllProperties();

			mock.Object.Value = 5;
			Assert.AreEqual(5, mock.Object.Value);

			mock.Object.Name = "foo";
			Assert.AreEqual("foo", mock.Object.Name);
		}

		[TestMethod]
		public void StubsInheritedClassProperties()
		{
			var mock = new Mock<Base>();

			mock.SetupAllProperties();

			mock.Object.BaseValue = 5;
			Assert.AreEqual(5, mock.Object.BaseValue);

			mock.Object.Value = 10;
			Assert.AreEqual(10, mock.Object.Value);
		}

		private object GetValue() { return new object(); }

		public interface IFoo
		{
			int ValueProperty { get; set; }
			object Object { get; set; }
			IBar Bar { get; set; }
		}

		public class Derived : Base
		{
			public string Name { get; set; }
		}

		public abstract class Base : IBar
		{
			public int BaseValue { get; set; }
			public int Value { get; set; }
		}

		public interface IBar
		{
			int Value { get; set; }
		}

		public interface IBaz : IBar
		{
			string Name { get; set; }
		}
	}
}
