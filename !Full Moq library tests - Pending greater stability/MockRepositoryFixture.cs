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

namespace Moq.Tests
{
    [TestClass]
    public class MockRepositoryFixture
	{
		[TestMethod]
		public void ShouldCreateFactoryWithMockBehaviorAndVerificationBehavior()
		{
			var repository = new MockRepository(MockBehavior.Loose);

			Assert.IsNotNull(repository);
		}

		[TestMethod]
		public void ShouldCreateMocksWithFactoryBehavior()
		{
			var repository = new MockRepository(MockBehavior.Loose);

			var mock = repository.Create<IFormatProvider>();

			Assert.AreEqual(MockBehavior.Loose, mock.Behavior);
		}

		[TestMethod]
		public void ShouldCreateMockWithConstructorArgs()
		{
			var repository = new MockRepository(MockBehavior.Loose);

			var mock = repository.Create<BaseClass>("foo");

			Assert.AreEqual("foo", mock.Object.Value);
		}

		[TestMethod]
		public void ShouldVerifyAll()
		{
			try
			{
				var repository = new MockRepository(MockBehavior.Default);
				var mock = repository.Create<IFoo>();

				mock.Setup(foo => foo.Do());

				repository.VerifyAll();
			}
			catch (MockException mex)
			{
				Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
			}
		}

		[TestMethod]
		public void ShouldVerifyVerifiables()
		{
			try
			{
				var repository = new MockRepository(MockBehavior.Default);
				var mock = repository.Create<IFoo>();

				mock.Setup(foo => foo.Do());
				mock.Setup(foo => foo.Undo()).Verifiable();

				repository.Verify();
			}
			catch (MockException mex)
			{
				Assert.AreEqual(MockException.ExceptionReason.VerificationFailed, mex.Reason);
				Expression<Action<IFoo>> doExpr = foo => foo.Do();
				Assert.IsFalse(mex.Message.Contains(doExpr.ToString()));
			}
		}

		[TestMethod]
		public void ShouldAggregateFailures()
		{
			try
			{
				var repository = new MockRepository(MockBehavior.Loose);
				var foo = repository.Create<IFoo>();
				var bar = repository.Create<IBar>();

				foo.Setup(f => f.Do());
				bar.Setup(b => b.Redo());

				repository.VerifyAll();
			}
			catch (MockException mex)
			{
				Expression<Action<IFoo>> fooExpect = f => f.Do();
				Assert.IsTrue(mex.Message.Contains(fooExpect.ToString()));

				Expression<Action<IBar>> barExpect = b => b.Redo();
				Assert.IsTrue(mex.Message.Contains(barExpect.ToString()));
			}
		}

		[TestMethod]
		public void ShouldOverrideDefaultBehavior()
		{
			var repository = new MockRepository(MockBehavior.Loose);
			var mock = repository.Create<IFoo>(MockBehavior.Strict);

			Assert.AreEqual(MockBehavior.Strict, mock.Behavior);
		}

		[TestMethod]
		public void ShouldOverrideDefaultBehaviorWithCtorArgs()
		{
			var repository = new MockRepository(MockBehavior.Loose);
			var mock = repository.Create<BaseClass>(MockBehavior.Strict, "Foo");

			Assert.AreEqual(MockBehavior.Strict, mock.Behavior);
			Assert.AreEqual("Foo", mock.Object.Value);
		}

		[TestMethod]
		public void ShouldCreateMocksWithFactoryDefaultValue()
		{
			var repository = new MockRepository(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock };

			var mock = repository.Create<IFoo>();

			Assert.IsNotNull(mock.Object.Bar());
		}

		[TestMethod]
		public void ShouldCreateMocksWithFactoryCallBase()
		{
			var repository = new MockRepository(MockBehavior.Loose);

			var mock = repository.Create<BaseClass>();

			mock.Object.BaseMethod();

			Assert.IsFalse(mock.Object.BaseCalled);

			repository.CallBase = true;

			mock = repository.Create<BaseClass>();

			mock.Object.BaseMethod();

			Assert.IsTrue(mock.Object.BaseCalled);
		}

		public interface IFoo
		{
			void Do();
			void Undo();
			IBar Bar();
		}

		public interface IBar { void Redo(); }

		public abstract class BaseClass
		{
			public bool BaseCalled;

			public BaseClass()
			{
			}

			public BaseClass(string value)
			{
				this.Value = value;
			}

			public string Value { get; set; }

			public virtual void BaseMethod()
			{
				BaseCalled = true;
			}
		}
	}
}
