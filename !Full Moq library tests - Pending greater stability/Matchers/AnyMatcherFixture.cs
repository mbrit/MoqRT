using System;
using System.Linq.Expressions;
using System.Diagnostics;
using System.Reflection;
using Moq;

#if !NETFX_CORE
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif

namespace Moq.Tests.Matchers
{
    [TestClass]
    public class AnyMatcherFixture
	{
		[TestMethod]
		public void MatchesNull()
		{
			var expr = ToExpression<object>(() => It.IsAny<object>()).ToLambda().Body;

			var matcher = MatcherFactory.CreateMatcher(expr, false);
			matcher.Initialize(expr);

			Assert.IsTrue(matcher.Matches(null));
		}

		[TestMethod]
		public void MatchesIfAssignableType()
		{
			var expr = ToExpression<object>(() => It.IsAny<object>()).ToLambda().Body;

			var matcher = MatcherFactory.CreateMatcher(expr, false);
			matcher.Initialize(expr);

			Assert.IsTrue(matcher.Matches("foo"));
		}

		[TestMethod]
		public void MatchesIfAssignableInterface()
		{
			var expr = ToExpression<IDisposable>(() => It.IsAny<IDisposable>()).ToLambda().Body;

			var matcher = MatcherFactory.CreateMatcher(expr, false);
			matcher.Initialize(expr);

			Assert.IsTrue(matcher.Matches(new Disposable()));
		}

		[TestMethod]
		public void DoesntMatchIfNotAssignableType()
		{
			var expr = ToExpression<IFormatProvider>(() => It.IsAny<IFormatProvider>()).ToLambda().Body;

			var matcher = MatcherFactory.CreateMatcher(expr, false);
			matcher.Initialize(expr);

			Assert.IsFalse(matcher.Matches("foo"));
		}

		private Expression ToExpression<TResult>(Expression<Func<TResult>> expr)
		{
			return expr;
		}

		class Disposable : IDisposable
		{
			public void Dispose()
			{
				throw new NotImplementedException();
			}
		}
	}
}
