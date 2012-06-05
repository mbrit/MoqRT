using System;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
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
    public class ExtensibilityFixture
    {
        [TestMethod]
        public void ShouldExtendMatching()
        {
            var mock = new Mock<IOrderRepository>();
            mock.Setup(repo => repo.Save(OrderIs.Big()))
                .Throws(new InvalidOperationException());

            try
            {
                mock.Object.Save(new Order { Amount = 1000 });

                Assert.IsTrue(false, "Should have failed for big order");
            }
            catch (InvalidOperationException)
            {
            }
        }

        [TestMethod]
        public void ShouldExtendWithSimpleMatchers()
        {
            var order = new Order();
            var mock = new Mock<IOrderRepository>();

            Order repo = Match.Create<Order>(r => true);

            mock.Setup(x => x.Save(Orders.Contains(order)))
                 .Throws<ArgumentException>();

            AssertHelper.Throws<ArgumentException>(() => mock.Object.Save(new[] { order }));
        }

        [TestMethod]
        public void ShouldExtendWithPropertyMatchers()
        {
            var mock = new Mock<IOrderRepository>();
            mock.Setup(repo => repo.Save(Orders.IsSmall))
                .Throws(new InvalidOperationException());

            try
            {
                mock.Object.Save(new Order { Amount = 50 });

                Assert.IsTrue(false, "Should have failed for small order");
            }
            catch (InvalidOperationException)
            {
            }
        }

        [TestMethod]
        public void SetterMatcherRendersNicely()
        {
            var mock = new Mock<IOrderRepository>();

            try
            {
                mock.VerifySet(repo => repo.Value = It.IsAny<int>());
            }
            catch (MockException me)
            {
                Debug.WriteLine(me.Message);
            }

            mock.Object.Value = 25;

            mock.VerifySet(repo => repo.Value = It.IsInRange(10, 25, Range.Inclusive));
        }
    }

    public static class Orders
    {
        public static IEnumerable<Order> Contains(Order order)
        {
            return Match.Create<IEnumerable<Order>>(orders => orders.Contains(order));
        }

        public static Order IsBig()
        {
            return Match.Create<Order>(o => o.Amount >= 1000);
        }

        public static Order IsSmall
        {
            get
            {
                return Match.Create<Order>(o => o.Amount <= 1000);
            }
        }
    }

    public interface IOrderRepository
    {
        void Save(Order order);
        void Save(IEnumerable<Order> orders);
        int Value { get; set; }
    }

    public static class OrderIs
    {
        [AdvancedMatcher(typeof(BigOrderMatcher))]
        public static Order Big()
        {
            return null;
        }

        public class BigOrderMatcher : IMatcher
        {
            public void Initialize(System.Linq.Expressions.Expression matcherExpression)
            {
            }

            public bool Matches(object value)
            {
                if (value is Order &&
                    ((Order)value).Amount >= 1000)
                    return true;

                return false;
            }
        }
    }

    public class Order
    {
        public int Amount { get; set; }
    }
}
