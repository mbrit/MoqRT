using System;
using System.IO;
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
	public class StreamFixture
	{
		[TestMethod]
		public void ShouldMockStream()
		{
			var mockStream = new Mock<Stream>();

			mockStream.Setup(stream => stream.Seek(0, SeekOrigin.Begin)).Returns(0L);

			var position = mockStream.Object.Seek(0, SeekOrigin.Begin);

			Assert.Equal(0, position);

			mockStream.Setup(stream => stream.Flush());
			mockStream.Setup(stream => stream.SetLength(100));

			mockStream.Object.Flush();
			mockStream.Object.SetLength(100);

			mockStream.VerifyAll();
		}
	}
}
