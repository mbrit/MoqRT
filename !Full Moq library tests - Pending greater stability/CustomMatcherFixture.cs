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
    public class CustomMatcherFixture
	{
		[TestMethod]
		public void UsesCustomMatcher()
		{
			var mock = new Mock<IFoo>();

			mock.Setup(m => m.Do(Any<string>())).Returns(true);

			Assert.IsTrue(mock.Object.Do("foo"));
		}

		[TestMethod]
		public void UsesCustomMatcherWithArgument()
		{
			var mock = new Mock<IFoo>();

			mock.Setup(m => m.Do(Between(1, 5, Range.Inclusive))).Returns(true);

			Assert.IsFalse(mock.Object.Do(6));
			Assert.IsTrue(mock.Object.Do(1));
			Assert.IsTrue(mock.Object.Do(5));
		}

		public TValue Any<TValue>()
		{
			return Match.Create<TValue>(v => true);
		}

		public TValue Between<TValue>(TValue from, TValue to, Range rangeKind)
			where TValue : IComparable
		{
			return Match.Create<TValue>(value =>
			{
				if (value == null)
				{
					return false;
				}

				if (rangeKind == Range.Exclusive)
				{
					return value.CompareTo(from) > 0 &&
						value.CompareTo(to) < 0;
				}
				else
				{
					return value.CompareTo(from) >= 0 &&
						value.CompareTo(to) <= 0;
				}
			});
		}

		public interface IFoo
		{
			bool Do(string value);
			bool Do(int value);
		}
	}
}
