using System;
using System.Diagnostics;
using System.Reflection;
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
    public class TimesFixture
	{
		[TestMethod]
		public void AtLeastOnceRangesBetweenOneAndMaxValue()
		{
			var target = Times.AtLeastOnce();

			Assert.IsFalse(target.Verify(-1));
			Assert.IsFalse(target.Verify(0));
			Assert.IsTrue(target.Verify(1));
			Assert.IsTrue(target.Verify(5));
			Assert.IsTrue(target.Verify(int.MaxValue));
		}

		[TestMethod]
		public void AtLeastThrowsIfTimesLessThanOne()
		{
			AssertHelper.Throws<ArgumentOutOfRangeException>(() => Times.AtLeast(0));
			AssertHelper.Throws<ArgumentOutOfRangeException>(() => Times.AtLeast(-1));
		}

		[TestMethod]
		public void AtLeastRangesBetweenTimesAndMaxValue()
		{
			var target = Times.AtLeast(10);

			Assert.IsFalse(target.Verify(-1));
			Assert.IsFalse(target.Verify(0));
			Assert.IsFalse(target.Verify(9));
			Assert.IsTrue(target.Verify(10));
			Assert.IsTrue(target.Verify(int.MaxValue));
		}

		[TestMethod]
		public void AtMostOnceRangesBetweenZeroAndOne()
		{
			var target = Times.AtMostOnce();

			Assert.IsFalse(target.Verify(-1));
			Assert.IsTrue(target.Verify(0));
			Assert.IsTrue(target.Verify(1));
			Assert.IsFalse(target.Verify(5));
			Assert.IsFalse(target.Verify(int.MaxValue));
		}

		[TestMethod]
		public void AtMostThrowsIfTimesLessThanZero()
		{
			AssertHelper.Throws<ArgumentOutOfRangeException>(() => Times.AtMost(-1));
			AssertHelper.Throws<ArgumentOutOfRangeException>(() => Times.AtMost(-2));
		}

		[TestMethod]
		public void AtMostRangesBetweenZeroAndTimes()
		{
			var target = Times.AtMost(10);

			Assert.IsFalse(target.Verify(-1));
			Assert.IsTrue(target.Verify(0));
			Assert.IsTrue(target.Verify(6));
			Assert.IsTrue(target.Verify(10));
			Assert.IsFalse(target.Verify(11));
			Assert.IsFalse(target.Verify(int.MaxValue));
		}

		[TestMethod]
		public void BetweenInclusiveThrowsIfFromLessThanZero()
		{
			AssertHelper.Throws<ArgumentOutOfRangeException>(() => Times.Between(-1, 10, Range.Inclusive));
			AssertHelper.Throws<ArgumentOutOfRangeException>(() => Times.Between(-2, 3, Range.Inclusive));
		}

		[TestMethod]
		public void BetweenInclusiveThrowsIfFromGreaterThanTo()
		{
			AssertHelper.Throws<ArgumentOutOfRangeException>(() => Times.Between(3, 2, Range.Inclusive));
			AssertHelper.Throws<ArgumentOutOfRangeException>(() => Times.Between(-3, -2, Range.Inclusive));
			AssertHelper.Throws<ArgumentOutOfRangeException>(() => Times.Between(0, -2, Range.Inclusive));
		}

		[TestMethod]
		public void BetweenInclusiveRangesBetweenFromAndTo()
		{
			var target = Times.Between(10, 20, Range.Inclusive);

			Assert.IsFalse(target.Verify(0));
			Assert.IsFalse(target.Verify(9));
			Assert.IsTrue(target.Verify(10));
			Assert.IsTrue(target.Verify(14));
			Assert.IsTrue(target.Verify(20));
			Assert.IsFalse(target.Verify(21));
			Assert.IsFalse(target.Verify(int.MaxValue));
		}

		[TestMethod]
		public void BetweenExclusiveThrowsIfFromLessThanZero()
		{
			AssertHelper.Throws<ArgumentOutOfRangeException>(() => Times.Between(-1, 10, Range.Exclusive));
			AssertHelper.Throws<ArgumentOutOfRangeException>(() => Times.Between(-2, 3, Range.Exclusive));
		}

		[TestMethod]
		public void BetweenExclusiveThrowsIfFromPlusOneGreaterThanToMinusOne()
		{
			AssertHelper.Throws<ArgumentOutOfRangeException>(() => Times.Between(2, 3, Range.Exclusive));
			AssertHelper.Throws<ArgumentOutOfRangeException>(() => Times.Between(3, 2, Range.Exclusive));
			AssertHelper.Throws<ArgumentOutOfRangeException>(() => Times.Between(0, -2, Range.Exclusive));
		}

		[TestMethod]
		public void BetweenExclusiveRangesBetweenFromPlusOneAndToMinusOne()
		{
			var target = Times.Between(10, 20, Range.Exclusive);

			Assert.IsFalse(target.Verify(0));
			Assert.IsFalse(target.Verify(10));
			Assert.IsTrue(target.Verify(11));
			Assert.IsTrue(target.Verify(14));
			Assert.IsTrue(target.Verify(19));
			Assert.IsFalse(target.Verify(20));
			Assert.IsFalse(target.Verify(int.MaxValue));
		}

		[TestMethod]
		public void ExactlyThrowsIfTimesLessThanZero()
		{
			AssertHelper.Throws<ArgumentOutOfRangeException>(() => Times.Exactly(-1));
			AssertHelper.Throws<ArgumentOutOfRangeException>(() => Times.Exactly(-2));
		}

		[TestMethod]
		public void ExactlyCheckExactTimes()
		{
			var target = Times.Exactly(10);

			Assert.IsFalse(target.Verify(-1));
			Assert.IsFalse(target.Verify(0));
			Assert.IsFalse(target.Verify(9));
			Assert.IsTrue(target.Verify(10));
			Assert.IsFalse(target.Verify(11));
			Assert.IsFalse(target.Verify(int.MaxValue));
		}

		[TestMethod]
		public void NeverChecksZeroTimes()
		{
			var target = Times.Never();

			Assert.IsFalse(target.Verify(-1));
			Assert.IsTrue(target.Verify(0));
			Assert.IsFalse(target.Verify(1));
			Assert.IsFalse(target.Verify(int.MaxValue));
		}

		[TestMethod]
		public void OnceChecksOneTime()
		{
			var target = Times.Once();

			Assert.IsFalse(target.Verify(-1));
			Assert.IsFalse(target.Verify(0));
			Assert.IsTrue(target.Verify(1));
			Assert.IsFalse(target.Verify(int.MaxValue));
		}
	}
}