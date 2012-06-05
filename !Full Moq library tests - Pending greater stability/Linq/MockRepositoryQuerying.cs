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
	public class MockRepositoryQuerying
	{
        [TestClass]
        public class GivenAStrictFactory
		{
			private MockRepository repository;

			public GivenAStrictFactory()
			{
				this.repository = new MockRepository(MockBehavior.Strict);
			}

			[TestMethod]
			public void WhenQueryingSingle_ThenItIsStrict()
			{
				var foo = this.repository.OneOf<IFoo>();

				AssertHelper.Throws<MockException>(() => foo.Do());
			}

			[TestMethod]
			public void WhenQueryingMultiple_ThenItIsStrict()
			{
				var foo = this.repository.Of<IFoo>().First();

				AssertHelper.Throws<MockException>(() => foo.Do());
			}

			[TestMethod]
			public void WhenQueryingSingleWithProperty_ThenItIsStrict()
			{
				var foo = this.repository.OneOf<IFoo>(x => x.Id == "1");

				AssertHelper.Throws<MockException>(() => foo.Do());

				Mock.Get(foo).Verify();

				Assert.AreEqual("1", foo.Id);
			}

			[TestMethod]
			public void WhenQueryingMultipleWithProperty_ThenItIsStrict()
			{
				var foo = this.repository.Of<IFoo>(x => x.Id == "1").First();

				AssertHelper.Throws<MockException>(() => foo.Do());

				Mock.Get(foo).Verify();

				Assert.AreEqual("1", foo.Id);
			}
		}

		public interface IFoo
		{
			string Id { get; set; }
			bool Do();
		}
	}
}
