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
    public class CallbacksFixture
	{
		[TestMethod]
		public void ExecutesCallbackWhenVoidMethodIsCalled()
		{
			var mock = new Mock<IFoo>();
			bool called = false;
			mock.Setup(x => x.Submit()).Callback(() => called = true);

			mock.Object.Submit();
			Assert.IsTrue(called);
		}

		[TestMethod]
		public void ExecutesCallbackWhenNonVoidMethodIsCalled()
		{
			var mock = new Mock<IFoo>();
			bool called = false;
			mock.Setup(x => x.Execute("ping")).Callback(() => called = true).Returns("ack");

			Assert.AreEqual("ack", mock.Object.Execute("ping"));
			Assert.IsTrue(called);
		}

		[TestMethod]
		public void CallbackCalledWithoutArgumentsForMethodCallWithArguments()
		{
			var mock = new Mock<IFoo>();
			bool called = false;
			mock.Setup(x => x.Submit(It.IsAny<string>())).Callback(() => called = true);

			mock.Object.Submit("blah");
			Assert.IsTrue(called);
		}

		[TestMethod]
		public void FriendlyErrorWhenCallbackArgumentCountNotMatch()
		{
			var mock = new Mock<IFoo>();

			AssertHelper.Throws<ArgumentException>(() => 
				mock.Setup(x => x.Submit(It.IsAny<string>()))
					.Callback((string s1, string s2) => Debug.WriteLine(s1 + s2)));
		}

		[TestMethod]
		public void CallbackCalledWithOneArgument()
		{
			var mock = new Mock<IFoo>();
			string callbackArg = null;
			mock.Setup(x => x.Submit(It.IsAny<string>())).Callback((string s) => callbackArg = s);

			mock.Object.Submit("blah");
			Assert.AreEqual("blah", callbackArg);
		}

		[TestMethod]
		public void CallbackCalledWithTwoArguments()
		{
			var mock = new Mock<IFoo>();
			string callbackArg1 = null;
			string callbackArg2 = null;
			mock.Setup(x => x.Submit(It.IsAny<string>(), It.IsAny<string>()))
				.Callback((string s1, string s2) => { callbackArg1 = s1; callbackArg2 = s2; });

			mock.Object.Submit("blah1", "blah2");
			Assert.AreEqual("blah1", callbackArg1);
			Assert.AreEqual("blah2", callbackArg2);
		}

		[TestMethod]
		public void CallbackCalledWithThreeArguments()
		{
			var mock = new Mock<IFoo>();
			string callbackArg1 = null;
			string callbackArg2 = null;
			string callbackArg3 = null;
			mock.Setup(x => x.Submit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Callback((string s1, string s2, string s3) => { callbackArg1 = s1; callbackArg2 = s2; callbackArg3 = s3; });

			mock.Object.Submit("blah1", "blah2", "blah3");
			Assert.AreEqual("blah1", callbackArg1);
			Assert.AreEqual("blah2", callbackArg2);
			Assert.AreEqual("blah3", callbackArg3);
		}

		[TestMethod]
		public void CallbackCalledWithFourArguments()
		{
			var mock = new Mock<IFoo>();
			string callbackArg1 = null;
			string callbackArg2 = null;
			string callbackArg3 = null;
			string callbackArg4 = null;
			mock.Setup(x => x.Submit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Callback((string s1, string s2, string s3, string s4) => { callbackArg1 = s1; callbackArg2 = s2; callbackArg3 = s3; callbackArg4 = s4; });

			mock.Object.Submit("blah1", "blah2", "blah3", "blah4");
			Assert.AreEqual("blah1", callbackArg1);
			Assert.AreEqual("blah2", callbackArg2);
			Assert.AreEqual("blah3", callbackArg3);
			Assert.AreEqual("blah4", callbackArg4);
		}

		[TestMethod]
		public void CallbackCalledWithFiveArguments()
		{
			var mock = new Mock<IFoo>();
			string callbackArg1 = null;
			string callbackArg2 = null;
			string callbackArg3 = null;
			string callbackArg4 = null;
			string callbackArg5 = null;
			mock.Setup(x => x.Submit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Callback((string s1, string s2, string s3, string s4, string s5) => { callbackArg1 = s1; callbackArg2 = s2; callbackArg3 = s3; callbackArg4 = s4; callbackArg5 = s5; });

			mock.Object.Submit("blah1", "blah2", "blah3", "blah4", "blah5");
			Assert.AreEqual("blah1", callbackArg1);
			Assert.AreEqual("blah2", callbackArg2);
			Assert.AreEqual("blah3", callbackArg3);
			Assert.AreEqual("blah4", callbackArg4);
			Assert.AreEqual("blah5", callbackArg5);
		}

		[TestMethod]
		public void CallbackCalledWithSixArguments()
		{
			var mock = new Mock<IFoo>();
			string callbackArg1 = null;
			string callbackArg2 = null;
			string callbackArg3 = null;
			string callbackArg4 = null;
			string callbackArg5 = null;
			string callbackArg6 = null;
			mock.Setup(x => x.Submit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Callback((string s1, string s2, string s3, string s4, string s5, string s6) => { callbackArg1 = s1; callbackArg2 = s2; callbackArg3 = s3; callbackArg4 = s4; callbackArg5 = s5; callbackArg6 = s6; });

			mock.Object.Submit("blah1", "blah2", "blah3", "blah4", "blah5", "blah6");
			Assert.AreEqual("blah1", callbackArg1);
			Assert.AreEqual("blah2", callbackArg2);
			Assert.AreEqual("blah3", callbackArg3);
			Assert.AreEqual("blah4", callbackArg4);
			Assert.AreEqual("blah5", callbackArg5);
			Assert.AreEqual("blah6", callbackArg6);
		}

		[TestMethod]
		public void CallbackCalledWithSevenArguments()
		{
			var mock = new Mock<IFoo>();
			string callbackArg1 = null;
			string callbackArg2 = null;
			string callbackArg3 = null;
			string callbackArg4 = null;
			string callbackArg5 = null;
			string callbackArg6 = null;
			string callbackArg7 = null;
			mock.Setup(x => x.Submit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Callback((string s1, string s2, string s3, string s4, string s5, string s6, string s7) => { callbackArg1 = s1; callbackArg2 = s2; callbackArg3 = s3; callbackArg4 = s4; callbackArg5 = s5; callbackArg6 = s6; callbackArg7 = s7; });

			mock.Object.Submit("blah1", "blah2", "blah3", "blah4", "blah5", "blah6", "blah7");
			Assert.AreEqual("blah1", callbackArg1);
			Assert.AreEqual("blah2", callbackArg2);
			Assert.AreEqual("blah3", callbackArg3);
			Assert.AreEqual("blah4", callbackArg4);
			Assert.AreEqual("blah5", callbackArg5);
			Assert.AreEqual("blah6", callbackArg6);
			Assert.AreEqual("blah7", callbackArg7);
		}

		[TestMethod]
		public void CallbackCalledWithEightArguments()
		{
			var mock = new Mock<IFoo>();
			string callbackArg1 = null;
			string callbackArg2 = null;
			string callbackArg3 = null;
			string callbackArg4 = null;
			string callbackArg5 = null;
			string callbackArg6 = null;
			string callbackArg7 = null;
			string callbackArg8 = null;
			mock.Setup(x => x.Submit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Callback((string s1, string s2, string s3, string s4, string s5, string s6, string s7, string s8) => { callbackArg1 = s1; callbackArg2 = s2; callbackArg3 = s3; callbackArg4 = s4; callbackArg5 = s5; callbackArg6 = s6; callbackArg7 = s7; callbackArg8 = s8; });

			mock.Object.Submit("blah1", "blah2", "blah3", "blah4", "blah5", "blah6", "blah7", "blah8");
			Assert.AreEqual("blah1", callbackArg1);
			Assert.AreEqual("blah2", callbackArg2);
			Assert.AreEqual("blah3", callbackArg3);
			Assert.AreEqual("blah4", callbackArg4);
			Assert.AreEqual("blah5", callbackArg5);
			Assert.AreEqual("blah6", callbackArg6);
			Assert.AreEqual("blah7", callbackArg7);
			Assert.AreEqual("blah8", callbackArg8);
		}

		[TestMethod]
		public void CallbackCalledWithOneArgumentForNonVoidMethod()
		{
			var mock = new Mock<IFoo>();
			string callbackArg1 = null;
			mock.Setup(x => x.Execute(It.IsAny<string>()))
				.Callback((string s1) => callbackArg1 = s1)
				.Returns("foo");

			mock.Object.Execute("blah1");
			Assert.AreEqual("blah1", callbackArg1);
		}

		[TestMethod]
		public void CallbackCalledWithTwoArgumentsForNonVoidMethod()
		{
			var mock = new Mock<IFoo>();
			string callbackArg1 = null;
			string callbackArg2 = null;
			mock.Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<string>()))
				.Callback((string s1, string s2) => { callbackArg1 = s1; callbackArg2 = s2; })
				.Returns("foo");

			mock.Object.Execute("blah1", "blah2");
			Assert.AreEqual("blah1", callbackArg1);
			Assert.AreEqual("blah2", callbackArg2);
		}

		[TestMethod]
		public void CallbackCalledWithThreeArgumentsForNonVoidMethod()
		{
			var mock = new Mock<IFoo>();
			string callbackArg1 = null;
			string callbackArg2 = null;
			string callbackArg3 = null;
			mock.Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Callback((string s1, string s2, string s3) => { callbackArg1 = s1; callbackArg2 = s2; callbackArg3 = s3; })
				.Returns("foo");

			mock.Object.Execute("blah1", "blah2", "blah3");
			Assert.AreEqual("blah1", callbackArg1);
			Assert.AreEqual("blah2", callbackArg2);
			Assert.AreEqual("blah3", callbackArg3);
		}

		[TestMethod]
		public void CallbackCalledWithFourArgumentsForNonVoidMethod()
		{
			var mock = new Mock<IFoo>();
			string callbackArg1 = null;
			string callbackArg2 = null;
			string callbackArg3 = null;
			string callbackArg4 = null;
			mock.Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Callback((string s1, string s2, string s3, string s4) => { callbackArg1 = s1; callbackArg2 = s2; callbackArg3 = s3; callbackArg4 = s4; })
				.Returns("foo");

			mock.Object.Execute("blah1", "blah2", "blah3", "blah4");
			Assert.AreEqual("blah1", callbackArg1);
			Assert.AreEqual("blah2", callbackArg2);
			Assert.AreEqual("blah3", callbackArg3);
			Assert.AreEqual("blah4", callbackArg4);
		}

		[TestMethod]
		public void CallbackCalledWithFiveArgumentsForNonVoidMethod()
		{
			var mock = new Mock<IFoo>();
			string callbackArg1 = null;
			string callbackArg2 = null;
			string callbackArg3 = null;
			string callbackArg4 = null;
			string callbackArg5 = null;
			mock.Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Callback((string s1, string s2, string s3, string s4, string s5) => { callbackArg1 = s1; callbackArg2 = s2; callbackArg3 = s3; callbackArg4 = s4; callbackArg5 = s5; })
				.Returns("foo");

			mock.Object.Execute("blah1", "blah2", "blah3", "blah4", "blah5");
			Assert.AreEqual("blah1", callbackArg1);
			Assert.AreEqual("blah2", callbackArg2);
			Assert.AreEqual("blah3", callbackArg3);
			Assert.AreEqual("blah4", callbackArg4);
			Assert.AreEqual("blah5", callbackArg5);
		}

		[TestMethod]
		public void CallbackCalledWithSixArgumentsForNonVoidMethod()
		{
			var mock = new Mock<IFoo>();
			string callbackArg1 = null;
			string callbackArg2 = null;
			string callbackArg3 = null;
			string callbackArg4 = null;
			string callbackArg5 = null;
			string callbackArg6 = null;
			mock.Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Callback((string s1, string s2, string s3, string s4, string s5, string s6) => { callbackArg1 = s1; callbackArg2 = s2; callbackArg3 = s3; callbackArg4 = s4; callbackArg5 = s5; callbackArg6 = s6; })
				.Returns("foo");

			mock.Object.Execute("blah1", "blah2", "blah3", "blah4", "blah5", "blah6");
			Assert.AreEqual("blah1", callbackArg1);
			Assert.AreEqual("blah2", callbackArg2);
			Assert.AreEqual("blah3", callbackArg3);
			Assert.AreEqual("blah4", callbackArg4);
			Assert.AreEqual("blah5", callbackArg5);
			Assert.AreEqual("blah6", callbackArg6);
		}

		[TestMethod]
		public void CallbackCalledWithSevenArgumentsForNonVoidMethod()
		{
			var mock = new Mock<IFoo>();
			string callbackArg1 = null;
			string callbackArg2 = null;
			string callbackArg3 = null;
			string callbackArg4 = null;
			string callbackArg5 = null;
			string callbackArg6 = null;
			string callbackArg7 = null;
			mock.Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Callback((string s1, string s2, string s3, string s4, string s5, string s6, string s7) => { callbackArg1 = s1; callbackArg2 = s2; callbackArg3 = s3; callbackArg4 = s4; callbackArg5 = s5; callbackArg6 = s6; callbackArg7 = s7; })
				.Returns("foo");

			mock.Object.Execute("blah1", "blah2", "blah3", "blah4", "blah5", "blah6", "blah7");
			Assert.AreEqual("blah1", callbackArg1);
			Assert.AreEqual("blah2", callbackArg2);
			Assert.AreEqual("blah3", callbackArg3);
			Assert.AreEqual("blah4", callbackArg4);
			Assert.AreEqual("blah5", callbackArg5);
			Assert.AreEqual("blah6", callbackArg6);
			Assert.AreEqual("blah7", callbackArg7);
		}

		[TestMethod]
		public void CallbackCalledWithEightArgumentsForNonVoidMethod()
		{
			var mock = new Mock<IFoo>();
			string callbackArg1 = null;
			string callbackArg2 = null;
			string callbackArg3 = null;
			string callbackArg4 = null;
			string callbackArg5 = null;
			string callbackArg6 = null;
			string callbackArg7 = null;
			string callbackArg8 = null;
			mock.Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Callback((string s1, string s2, string s3, string s4, string s5, string s6, string s7, string s8) => { callbackArg1 = s1; callbackArg2 = s2; callbackArg3 = s3; callbackArg4 = s4; callbackArg5 = s5; callbackArg6 = s6; callbackArg7 = s7; callbackArg8 = s8; })
				.Returns("foo");

			mock.Object.Execute("blah1", "blah2", "blah3", "blah4", "blah5", "blah6", "blah7", "blah8");
			Assert.AreEqual("blah1", callbackArg1);
			Assert.AreEqual("blah2", callbackArg2);
			Assert.AreEqual("blah3", callbackArg3);
			Assert.AreEqual("blah4", callbackArg4);
			Assert.AreEqual("blah5", callbackArg5);
			Assert.AreEqual("blah6", callbackArg6);
			Assert.AreEqual("blah7", callbackArg7);
			Assert.AreEqual("blah8", callbackArg8);
		}

		[TestMethod]
		public void CallbackCalledAfterReturnsCall()
		{
			var mock = new Mock<IFoo>();
			bool returnsCalled = false;
			bool beforeCalled = false;
			bool afterCalled = false;

			mock.Setup(foo => foo.Execute("ping"))
				.Callback(() => { Assert.IsFalse(returnsCalled); beforeCalled = true; })
				.Returns(() => { returnsCalled = true; return "ack"; })
				.Callback(() => { Assert.IsTrue(returnsCalled); afterCalled = true; });

			Assert.AreEqual("ack", mock.Object.Execute("ping"));

			Assert.IsTrue(beforeCalled);
			Assert.IsTrue(afterCalled);
		}

		[TestMethod]
		public void CallbackCalledAfterReturnsCallWithArg()
		{
			var mock = new Mock<IFoo>();
			bool returnsCalled = false;

			mock.Setup(foo => foo.Execute(It.IsAny<string>()))
				.Callback<string>(s => Assert.IsFalse(returnsCalled))
				.Returns(() => { returnsCalled = true; return "ack"; })
				.Callback<string>(s => Assert.IsTrue(returnsCalled));

			mock.Object.Execute("ping");

			Assert.IsTrue(returnsCalled);
		}

		[TestMethod]
		public void CallbackCanReceiveABaseClass()
		{
			var mock = new Mock<IInterface>(MockBehavior.Strict);
			mock.Setup(foo => foo.Method(It.IsAny<Derived>())).Callback<Derived>(TraceMe);

			mock.Object.Method(new Derived());
		}

		public interface IInterface
		{
			void Method(Derived b);
		}

		public class Base
		{
		}

		public class Derived : Base
		{
		}

		private void TraceMe(Base b)
		{
		}
		
		public interface IFoo
		{
			void Submit();
			void Submit(string command);
			string Submit(string arg1, string arg2);
			string Submit(string arg1, string arg2, string arg3);
			string Submit(string arg1, string arg2, string arg3, string arg4);
			void Submit(string arg1, string arg2, string arg3, string arg4, string arg5);
			void Submit(string arg1, string arg2, string arg3, string arg4, string arg5, string arg6);
			void Submit(string arg1, string arg2, string arg3, string arg4, string arg5, string arg6, string arg7);
			void Submit(string arg1, string arg2, string arg3, string arg4, string arg5, string arg6, string arg7, string arg8);
			string Execute(string command);
			string Execute(string arg1, string arg2);
			string Execute(string arg1, string arg2, string arg3);
			string Execute(string arg1, string arg2, string arg3, string arg4);
			string Execute(string arg1, string arg2, string arg3, string arg4, string arg5);
			string Execute(string arg1, string arg2, string arg3, string arg4, string arg5, string arg6);
			string Execute(string arg1, string arg2, string arg3, string arg4, string arg5, string arg6, string arg7);
			string Execute(string arg1, string arg2, string arg3, string arg4, string arg5, string arg6, string arg7, string arg8);

			int Value { get; set; }
		}
	}
}
