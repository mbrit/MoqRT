using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using Moq;
using MoqRT;

#if !NETFX_CORE
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif

namespace Moq.Tests.Linq
{
    [TestClass]
    public class UnsupportedQuerying
	{
		public class GivenAReadonlyNonVirtualProperty
		{
			[TestMethod]
			public void WhenQueryingDirect_ThenThrowsNotSupportedException()
			{
				AssertHelper.Throws<NotSupportedException>(() => Mock.Of<Bar>(x => x.NonVirtualValue == "bar"));
			}

			[TestMethod]
			public void WhenQueryingOnFluent_ThenThrowsNotSupportedException()
			{
				AssertHelper.Throws<NotSupportedException>(() => Mock.Of<Foo>(x => x.VirtualBar.NonVirtualValue == "bar"));
			}

			[TestMethod]
			public void WhenQueryingOnIntermediateFluentReadonly_ThenThrowsNotSupportedException()
			{
				AssertHelper.Throws<NotSupportedException>(() => Mock.Of<Foo>(x => x.NonVirtualBar.VirtualValue == "bar"));
			}

			public class Bar
			{
				public string NonVirtualValue { get { return "foo"; } }
				public virtual string VirtualValue { get; set; }
			}

			public class Foo
			{
				public virtual Bar VirtualBar { get; set; }
				public Bar NonVirtualBar { get; set; }
			}
		}

		public class GivenAField
		{
			[TestMethod]
			public void WhenQueryingField_ThenThrowsNotSupportedException()
			{
				AssertHelper.Throws<NotSupportedException>(() => Mock.Of<Bar>(x => x.FieldValue == "bar"));
			}

			[TestMethod]
			public void WhenQueryingOnFluent_ThenThrowsNotSupportedException()
			{
				AssertHelper.Throws<NotSupportedException>(() => Mock.Of<Foo>(x => x.VirtualBar.FieldValue == "bar"));
			}

			[TestMethod]
			public void WhenIntermediateFluentReadonly_ThenThrowsNotSupportedException()
			{
				AssertHelper.Throws<NotSupportedException>(() => Mock.Of<Foo>(x => x.Bar.VirtualValue == "bar"));
			}

			public class Bar
			{
				public string FieldValue = "foo";
				public virtual string VirtualValue { get; set; }
			}

			public class Foo
			{
				public Bar Bar = new Bar();

				public virtual Bar VirtualBar { get; set; }
			}
		}

		public class GivenANonVirtualMethod
		{
			[TestMethod]
			public void WhenQueryingDirect_ThenThrowsNotSupportedException()
			{
				AssertHelper.Throws<NotSupportedException>(() => Mock.Of<Bar>(x => x.NonVirtual() == "foo"));
			}

			[TestMethod]
			public void WhenQueryingOnFluent_ThenThrowsNotSupportedException()
			{
				AssertHelper.Throws<NotSupportedException>(() => Mock.Of<Foo>(x => x.Virtual().NonVirtual() == "foo"));
			}

			[TestMethod]
			public void WhenQueryingOnIntermediateFluentNonVirtual_ThenThrowsNotSupportedException()
			{
				AssertHelper.Throws<NotSupportedException>(() => Mock.Of<Foo>(x => x.NonVirtual().Virtual() == "foo"));
			}

			public class Bar
			{
				public string NonVirtual()
				{
					return string.Empty;
				}

				public virtual string Virtual()
				{
					return string.Empty;
				}
			}

			public class Foo
			{
				public Bar NonVirtual()
				{
					return new Bar();
				}

				public virtual Bar Virtual()
				{
					return new Bar();
				}
			}
		}

		public class GivenAnInterface
		{
			[TestMethod]
			public void WhenQueryingSingle_ThenThrowsNotSupportedException()
			{
				AssertHelper.Throws<NotSupportedException>(() => Mocks.Of<IFoo>().Single());
			}

			[TestMethod]
			public void WhenQueryingSingleOrDefault_ThenThrowsNotSupportedException()
			{
				AssertHelper.Throws<NotSupportedException>(() => Mocks.Of<IFoo>().SingleOrDefault());
			}

			[TestMethod]
			public void WhenQueryingAll_ThenThrowsNotSupportedException()
			{
				AssertHelper.Throws<NotSupportedException>(() => Mocks.Of<IFoo>().All(x => x.Value == "Foo"));
			}

			[TestMethod]
			public void WhenQueryingAny_ThenThrowsNotSupportedException()
			{
				AssertHelper.Throws<NotSupportedException>(() => Mocks.Of<IFoo>().Any());
			}

			[TestMethod]
			public void WhenQueryingLast_ThenThrowsNotSupportedException()
			{
				AssertHelper.Throws<NotSupportedException>(() => Mocks.Of<IFoo>().Last());
			}

			[TestMethod]
			public void WhenQueryingLastOrDefault_ThenThrowsNotSupportedException()
			{
				AssertHelper.Throws<NotSupportedException>(() => Mocks.Of<IFoo>().LastOrDefault());
			}

			[TestMethod]
			public void WhenOperatorIsNotEqual_ThenThrowsNotSupportedException()
			{
				AssertHelper.Throws<NotSupportedException>(() => Mock.Of<IFoo>(x => x.Value != "foo"));
			}

			[TestMethod]
			public void WhenOperatorIsGreaterThan_ThenThrowsNotSupportedException()
			{
				AssertHelper.Throws<NotSupportedException>(() => Mock.Of<IFoo>(x => x.Count > 5));
			}

			[TestMethod]
			public void WhenOperatorIsGreaterThanOrEqual_ThenThrowsNotSupportedException()
			{
				AssertHelper.Throws<NotSupportedException>(() => Mock.Of<IFoo>(x => x.Count >= 5));
			}

			[TestMethod]
			public void WhenOperatorIsLessThan_ThenThrowsNotSupportedException()
			{
				AssertHelper.Throws<NotSupportedException>(() => Mock.Of<IFoo>(x => x.Count < 5));
			}

			[TestMethod]
			public void WhenOperatorIsLessThanOrEqual_ThenThrowsNotSupportedException()
			{
				AssertHelper.Throws<NotSupportedException>(() => Mock.Of<IFoo>(x => x.Count <= 5));
			}

			[TestMethod]
			public void WhenCombiningWithOrRatherThanLogicalAnd_ThenThrowsNotSupportedException()
			{
				AssertHelper.Throws<NotSupportedException>(() => Mock.Of<IFoo>(x => x.Count == 5 || x.Value == "foo"));
			}

			public interface IFoo
			{
				string Value { get; set; }
				int Count { get; set; }
			}
		}
	}
}