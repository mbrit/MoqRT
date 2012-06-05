using System;
using System.Diagnostics;
using System.Reflection;
using System.Linq.Expressions;
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
    public class AdvancedMatcherAttributeFixture
    {
        [TestMethod]
        public void ShouldThrowIfNullMatcherType()
        {
            AssertHelper.Throws<ArgumentNullException>(() => new AdvancedMatcherAttribute(null));
        }

        [TestMethod]
        public void ShouldThrowIfMatcherNotIExpressionMatcher()
        {
            AssertHelper.Throws<ArgumentException>(() => new AdvancedMatcherAttribute(typeof(object)));
        }

        [TestMethod]
        public void ShouldCreateMatcher()
        {
            var attr = new AdvancedMatcherAttribute(typeof(MockMatcher));
            var matcher = attr.CreateMatcher();

            Assert.IsNotNull(matcher);
        }

        [TestMethod]
        public void ShouldExposeMatcherType()
        {
            var attr = new AdvancedMatcherAttribute(typeof(MockMatcher));

            Assert.AreEqual(typeof(MockMatcher), attr.MatcherType);
        }

        [TestMethod]
        public void ShouldThrowRealException()
        {
            var attr = new AdvancedMatcherAttribute(typeof(ThrowingMatcher));
            AssertHelper.Throws<ArgumentException>(() => attr.CreateMatcher());
        }

        public class MockMatcher : IMatcher
        {
            #region IMatcher Members

            public void Initialize(Expression matcherExpression)
            {
            }

            public bool Matches(object value)
            {
                return false;
            }

            #endregion
        }

        public class ThrowingMatcher : IMatcher
        {
            public ThrowingMatcher()
            {
                throw new ArgumentException();
            }

            public void Initialize(Expression matcherExpression)
            {
            }

            public bool Matches(object value)
            {
                return false;
            }
        }
    }
}