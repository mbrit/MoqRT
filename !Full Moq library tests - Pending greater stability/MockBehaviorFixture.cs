using System;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Moq;

#if !NETFX_CORE
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif

namespace Moq.Tests
{
    [TestClass]
    public class MockBehaviorFixture
	{
		[TestMethod]
		public void ShouldThrowIfStrictNoExpectation()
		{
			var mock = new Mock<IFoo>(MockBehavior.Strict);
			try
			{
				mock.Object.Do();
				Assert.IsTrue(false, "Should have thrown for unexpected call with MockBehavior.Strict");
			}
			catch (MockException mex)
			{
				Assert.AreEqual(MockException.ExceptionReason.NoSetup, mex.Reason);
			}
		}

		[TestMethod]
		public void ShouldReturnDefaultForLooseBehaviorOnInterface()
		{
			var mock = new Mock<IFoo>(MockBehavior.Loose);

			Assert.AreEqual(0, mock.Object.Get());
			Assert.IsNull(mock.Object.GetObject());
		}

		[TestMethod]
		public void ShouldReturnDefaultForLooseBehaviorOnAbstract()
		{
			var mock = new Mock<Foo>(MockBehavior.Loose);

			Assert.AreEqual(0, mock.Object.AbstractGet());
			Assert.IsNull(mock.Object.GetObject());
		}

		[TestMethod]
		public void ShouldReturnEmptyArrayOnLoose()
		{
			var mock = new Mock<IFoo>(MockBehavior.Loose);

			Assert.IsNotNull(mock.Object.GetArray());
			Assert.AreEqual(0, mock.Object.GetArray().Length);
		}

		[TestMethod]
		public void ShouldReturnEmptyArrayTwoDimensionsOnLoose()
		{
			var mock = new Mock<IFoo>(MockBehavior.Loose);

			Assert.IsNotNull(mock.Object.GetArrayTwoDimensions());
			Assert.AreEqual(0, mock.Object.GetArrayTwoDimensions().Length);
		}

		[TestMethod]
		public void ShouldReturnNullListOnLoose()
		{
			var mock = new Mock<IFoo>(MockBehavior.Loose);

			Assert.IsNull(mock.Object.GetList());
		}

		[TestMethod]
		public void ShouldReturnEmptyEnumerableStringOnLoose()
		{
			var mock = new Mock<IFoo>(MockBehavior.Loose);

			Assert.IsNotNull(mock.Object.GetEnumerable());
			Assert.AreEqual(0, mock.Object.GetEnumerable().Count());
		}

		[TestMethod]
		public void ShouldReturnEmptyEnumerableObjectsOnLoose()
		{
			var mock = new Mock<IFoo>(MockBehavior.Loose);

			Assert.IsNotNull(mock.Object.GetEnumerableObjects());
			Assert.AreEqual(0, mock.Object.GetEnumerableObjects().Cast<object>().Count());
		}

		[TestMethod]
		public void ShouldReturnDefaultGuidOnLoose()
		{
			var mock = new Mock<IFoo>(MockBehavior.Loose);
			Assert.AreEqual(default(Guid), mock.Object.GetGuid());
		}

		[TestMethod]
		public void ShouldReturnNullStringOnLoose()
		{
			var mock = new Mock<IFoo>(MockBehavior.Loose);

			Assert.IsNull(mock.Object.DoReturnString());
		}

		[TestMethod]
		public void ShouldReturnNullStringOnLooseWithExpect()
		{
			var mock = new Mock<IFoo>(MockBehavior.Loose);

			mock.Setup(x => x.DoReturnString());

			Assert.IsNull(mock.Object.DoReturnString());
		}

		[TestMethod]
		public void ReturnsMockDefaultValueForLooseBehaviorOnInterface()
		{
			var mock = new Mock<IFoo>(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock };

			var value = mock.Object.GetObject();

			Assert.IsTrue(value is IMocked);
		}

		[TestMethod]
		public void ReturnsMockDefaultValueForLooseBehaviorOnAbstract()
		{
			var mock = new Mock<Foo>(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock };

			var value = mock.Object.Bar;

			Assert.IsTrue(value is IMocked);

			value = mock.Object.GetBar();

			Assert.IsTrue(value is IMocked);
		}

		[TestMethod]
		public void ReturnsEmptyArrayOnLooseWithMockDefaultValue()
		{
			var mock = new Mock<IFoo>(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock };

			Assert.IsNotNull(mock.Object.GetArray());
			Assert.AreEqual(0, mock.Object.GetArray().Length);
		}

		[TestMethod]
		public void ReturnsEmptyArrayTwoDimensionsOnLooseWithMockDefaultValue()
		{
			var mock = new Mock<IFoo>(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock };

			Assert.IsNotNull(mock.Object.GetArrayTwoDimensions());
			Assert.AreEqual(0, mock.Object.GetArrayTwoDimensions().Length);
		}

		[TestMethod]
		public void ReturnsMockListOnLooseWithMockDefaultValue()
		{
			var mock = new Mock<IFoo>(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock };

			Assert.IsNotNull(mock.Object.GetList());

			var list = mock.Object.GetList();

			list.Add("foo");

			Assert.AreEqual("foo", list[0]);
		}

		[TestMethod]
		public void ReturnsEmptyEnumerableStringOnLooseWithMockDefaultValue()
		{
			var mock = new Mock<IFoo>(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock };

			Assert.IsNotNull(mock.Object.GetEnumerable());
			Assert.AreEqual(0, mock.Object.GetEnumerable().Count());
		}

		[TestMethod]
		public void ReturnsEmptyQueryableStringOnLooseWithMockDefaultValue()
		{
			var mock = new Mock<IFoo>(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock };

			Assert.IsNotNull(mock.Object.GetQueryable());
			Assert.AreEqual(0, mock.Object.GetQueryable().Count());
		}

		[TestMethod]
		public void ReturnsEmptyEnumerableObjectsOnLooseWithMockDefaultValue()
		{
			var mock = new Mock<IFoo>(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock };

			Assert.IsNotNull(mock.Object.GetEnumerableObjects());
			Assert.AreEqual(0, mock.Object.GetEnumerableObjects().Cast<object>().Count());
		}

		[TestMethod]
		public void ReturnsEmptyQueryableObjectsOnLooseWithMockDefaultValue()
		{
			var mock = new Mock<IFoo>(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock };

			Assert.IsNotNull(mock.Object.GetQueryableObjects());
			Assert.AreEqual(0, mock.Object.GetQueryableObjects().Cast<object>().Count());
		}

		[TestMethod]
		public void ReturnsDefaultGuidOnLooseWithMockDefaultValueWithMockDefaultValue()
		{
			var mock = new Mock<IFoo>(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock };
			Assert.AreEqual(default(Guid), mock.Object.GetGuid());
		}

		[TestMethod]
		public void ReturnsNullStringOnLooseWithMockDefaultValue()
		{
			var mock = new Mock<IFoo>(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock };

			Assert.IsNull(mock.Object.DoReturnString());
		}

		public interface IFoo
		{
			void Do();
			int Get();
			Guid GetGuid();
			object GetObject();
			string[] GetArray();
			string[][] GetArrayTwoDimensions();
			List<string> GetList();
			IEnumerable<string> GetEnumerable();
			IEnumerable GetEnumerableObjects();
			string DoReturnString();
			IQueryable<string> GetQueryable();
			IQueryable GetQueryableObjects();
		}

		public interface IBar { }

		public abstract class Foo : IFoo
		{
			public abstract IBar Bar { get; set; }
			public abstract IBar GetBar();

			public abstract void Do();
			public abstract object GetObject();
			public abstract string DoReturnString();

			public void DoNonVirtual() { }
			public virtual void DoVirtual() { }

			public int NonVirtualGet()
			{
				return 0;
			}

			public int VirtualGet()
			{
				return 0;
			}

			public virtual int Get()
			{
				return AbstractGet();
			}

			public abstract int AbstractGet();

			public string[] GetArray()
			{
				return new string[0];
			}

			public string[][] GetArrayTwoDimensions()
			{
				return new string[0][];
			}

			public List<string> GetList()
			{
				return null;
			}

			public IEnumerable<string> GetEnumerable()
			{
				return new string[0];
			}

			public IEnumerable GetEnumerableObjects()
			{
				return new object[0];
			}


			public Guid GetGuid()
			{
				return default(Guid);
			}

			public IQueryable<string> GetQueryable()
			{
				return new string[0].AsQueryable();
			}

			public IQueryable GetQueryableObjects()
			{
				return new object[0].AsQueryable();
			}
		}
	}
}
