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
    public class ExtensionsFixture
	{
		[TestMethod]
		public void IsMockeableReturnsFalseForValueType()
		{
			Assert.IsFalse(typeof(int).IsMockeable());
		}
	}
}
