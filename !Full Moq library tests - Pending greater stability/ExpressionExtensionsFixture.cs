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
    using MoqRT;
    public class ExpressionExtensionsFixture
	{
		[TestMethod]
		public void FixesGenericMethodName()
		{
			Expression<Action<ExpressionExtensionsFixture>> expr1 = f => f.Do("");
			Expression<Action<ExpressionExtensionsFixture>> expr2 = f => f.Do(5);

			Assert.AreNotEqual(expr1.ToStringFixed(), expr2.ToStringFixed());
		}

		[TestMethod]
		public void PrefixesStaticMethodWithClass()
		{
			Expression<Action> expr = () => DoStatic(5);

			var value = expr.ToStringFixed();

			AssertHelper.Contains("ExpressionExtensionsFixture.DoStatic(5)", value);
		}

		[TestMethod]
		public void PrefixesStaticGenericMethodWithClass()
		{
			Expression<Action> expr = () => DoStaticGeneric(5);

			var value = expr.ToStringFixed();

			AssertHelper.Contains("ExpressionExtensionsFixture.DoStaticGeneric<Int32>(5)", value);
		}

		[TestMethod]
		public void ToLambdaThrowsIfNullExpression()
		{
			AssertHelper.Throws<ArgumentNullException>(() => ExpressionExtensions.ToLambda(null));
		}

		[TestMethod]
		public void ToLambdaThrowsIfExpressionNotLambda()
		{
			AssertHelper.Throws<ArgumentException>(() => Expression.Constant(5).ToLambda());
		}

		[TestMethod]
		public void ToLambdaRemovesConvert()
		{
			var lambda = ToExpression<object>(() => (object)5);

			var result = lambda.ToLambda();

			Assert.AreEqual(typeof(int), result.Compile().GetMethodInfo().ReturnType);
		}

		[TestMethod]
		public void IsPropertyLambdaTrue()
		{
			var expr = ToExpression<IFoo, int>(f => f.Value).ToLambda();

			Assert.IsTrue(expr.IsProperty());
		}

		[TestMethod]
		public void IsPropertyLambdaFalse()
		{
			var expr = ToExpression<IFoo>(f => f.Do()).ToLambda();

			Assert.IsFalse(expr.IsProperty());
		}

		[TestMethod]
		public void IsPropertyExpressionTrue()
		{
			var expr = ToExpression<IFoo, int>(f => f.Value).ToLambda().Body;

			Assert.IsTrue(expr.IsProperty());
		}

		[TestMethod]
		public void IsPropertyExpressionFalse()
		{
			var expr = ToExpression<IFoo>(f => f.Do()).ToLambda().Body;

			Assert.IsFalse(expr.IsProperty());
		}

		[TestMethod]
		public void IsPropertyIndexerExpressionTrue()
		{
			var expr = ToExpression<IFoo, object>(f => f[5]).ToLambda().Body;

			Assert.IsTrue(expr.IsPropertyIndexer());
		}

		[TestMethod]
		public void ToMethodCallThrowsIfNotMethodCall()
		{
			var expr = ToExpression<IFoo, object>(f => f.Value).ToLambda();

			AssertHelper.Throws<ArgumentException>(() => expr.ToMethodCall());
		}

		[TestMethod]
		public void ToMethodCallConvertsLambda()
		{
			var expr = ToExpression<IFoo>(f => f.Do()).ToLambda();

			Assert.AreEqual(typeof(IFoo).GetMethod("Do"), expr.ToMethodCall().Method);
		}

		[TestMethod]
		public void ToPropertyInfoConvertsExpression()
		{
			
		}

		private Expression ToExpression<T>(Expression<Func<T>> expression)
		{
			return expression;
		}

		private Expression ToExpression<T>(Expression<Action<T>> expression)
		{
			return expression;
		}

		private Expression ToExpression<T, TResult>(Expression<Func<T, TResult>> expression)
		{
			return expression;
		}

		private void Do<T>(T value) { }

		private static void DoStatic(int value) { }
		private static void DoStaticGeneric<T>(T value) { }

		public interface IFoo
		{
			int Value { get; set; }
			void Do();
			object this[int index] { get; set; }
		}
	}
}
