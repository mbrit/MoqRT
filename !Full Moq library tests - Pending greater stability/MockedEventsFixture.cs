using System;
using System.Diagnostics;
using System.Reflection;
using System.ComponentModel;
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
    public class MockedEventsFixture
	{
		[TestMethod]
		public void ShouldExpectAddHandler()
		{
			var view = new Mock<IFooView>();

			var presenter = new FooPresenter(view.Object);
			bool fired = false;

			presenter.Fired += (sender, args) =>
			{
				fired = true;
				Assert.IsTrue(args is FooArgs);
				Assert.AreEqual("foo", ((FooArgs)args).Value);
			};

			view.Raise(v => v.FooSelected += null, new FooArgs { Value = "foo" });

			Assert.IsTrue(fired);
		}

		[TestMethod]
		public void ShouldRaiseEventIfAttachedAfterUse()
		{
			var view = new Mock<IFooView>();
			var presenter = new FooPresenter(view.Object);

			Assert.IsFalse(presenter.Canceled);

			view.Raise(v => v.Canceled += null, EventArgs.Empty);

			Assert.IsTrue(presenter.Canceled);
		}

		[TestMethod]
		public void ShouldExpectAddGenericHandler()
		{
			var view = new Mock<IFooView>();
			var presenter = new FooPresenter(view.Object);

			Assert.IsFalse(presenter.Canceled);

			view.Raise(v => v.Canceled += null, EventArgs.Empty);

			Assert.IsTrue(presenter.Canceled);
		}

		[TestMethod]
		public void ShouldNotThrowIfEventIsNotMocked()
		{
			var view = new Mock<IFooView>();

			// Presenter class attaches to the event and nothing happens.
			var presenter = new FooPresenter(view.Object);
		}

		[TestMethod]
		public void ShouldRaiseEventWhenExpectationMet()
		{
			var mock = new Mock<IAdder<string>>();

			var raised = false;
			mock.Setup(add => add.Add(It.IsAny<string>()))
				.Raises(m => m.Added += null, EventArgs.Empty);

			mock.Object.Added += (s, e) => raised = true;

			mock.Object.Add("foo");

			Assert.IsTrue(raised);
		}

		[TestMethod]
		public void ShouldRaiseEventWithArgWhenExpectationMet()
		{
			var mock = new Mock<IAdder<string>>();

			var raised = false;
			mock.Setup(add => add.Add(It.IsAny<string>()))
				.Raises(m => m.Added += null, EventArgs.Empty);

			mock.Object.Added += (s, e) => raised = true;

			mock.Object.Add("foo");

			Assert.IsTrue(raised);
		}

		[TestMethod]
		public void ShouldRaiseEventWhenExpectationMetReturn()
		{
			var mock = new Mock<IAdder<string>>();

			var raised = false;
			mock.Object.Added += (s, e) => raised = true;

			mock.Setup(add => add.Insert(It.IsAny<string>(), 0))
				.Returns(1)
				.Raises(m => m.Added += null, EventArgs.Empty);

			int value = mock.Object.Insert("foo", 0);

			Assert.AreEqual(1, value);
			Assert.IsTrue(raised);
		}

		[TestMethod]
		public void ShouldPreserveStackTraceWhenRaisingEvent()
		{
			var mock = new Mock<IAdder<string>>();
			mock.Object.Added += (s, e) => { throw new InvalidOperationException(); };

			AssertHelper.Throws<InvalidOperationException>(() => mock.Raise(m => m.Added += null, EventArgs.Empty));
		}

		[TestMethod]
		public void ShouldRaiseEventWithFunc()
		{
			var mock = new Mock<IAdder<string>>();

			var raised = false;
			mock.Setup(add => add.Add(It.IsAny<string>()))
				.Raises(m => m.Added += null, () => EventArgs.Empty);

			mock.Object.Added += (s, e) => raised = true;
			mock.Object.Add("foo");

			Assert.IsTrue(raised);
		}

		[TestMethod]
		public void ShouldRaiseEventWithFuncOneArg()
		{
			var mock = new Mock<IAdder<string>>();

			mock.Setup(add => add.Add(It.IsAny<string>()))
				.Raises(m => m.Added += null, (string s) => new FooArgs { Value = s });

			var raised = false;

			mock.Object.Added += (sender, args) =>
			{
				raised = true;
				Assert.IsTrue(args is FooArgs);
				Assert.AreEqual("foo", ((FooArgs)args).Value);
			};

			mock.Object.Add("foo");

			Assert.IsTrue(raised);
		}

		[TestMethod]
		public void ShouldRaiseEventWithFuncTwoArgs()
		{
			var mock = new Mock<IAdder<string>>();

			mock.Setup(add => add.Do(It.IsAny<string>(), It.IsAny<int>()))
				.Raises(m => m.Added += null, (string s, int i) => new FooArgs { Args = new object[] { s, i } });

			var raised = false;

			mock.Object.Added += (sender, args) =>
			{
				raised = true;
				Assert.IsTrue(args is FooArgs);
				Assert.AreEqual("foo", ((FooArgs)args).Args[0]);
				Assert.AreEqual(5, ((FooArgs)args).Args[1]);
			};

			mock.Object.Do("foo", 5);

			Assert.IsTrue(raised);
		}

		[TestMethod]
		public void ShouldRaiseEventWithFuncThreeArgs()
		{
			var mock = new Mock<IAdder<string>>();

			mock.Setup(add => add.Do(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()))
				.Raises(m => m.Added += null, (string s, int i, bool b) => new FooArgs { Args = new object[] { s, i, b } });

			var raised = false;

			mock.Object.Added += (sender, args) =>
			{
				raised = true;
				Assert.IsTrue(args is FooArgs);
				Assert.AreEqual("foo", ((FooArgs)args).Args[0]);
				Assert.AreEqual(5, ((FooArgs)args).Args[1]);
				Assert.AreEqual(true, ((FooArgs)args).Args[2]);
			};

			mock.Object.Do("foo", 5, true);

			Assert.IsTrue(raised);
		}

		[TestMethod]
		public void ShouldRaiseEventWithFuncFourArgs()
		{
			var mock = new Mock<IAdder<string>>();

			mock.Setup(add => add.Do(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()))
				.Raises(m => m.Added += null, (string s, int i, bool b, string v) => new FooArgs { Args = new object[] { s, i, b, v } });

			var raised = false;

			mock.Object.Added += (sender, args) =>
			{
				raised = true;
				Assert.IsTrue(args is FooArgs);
				Assert.AreEqual("foo", ((FooArgs)args).Args[0]);
				Assert.AreEqual(5, ((FooArgs)args).Args[1]);
				Assert.AreEqual(true, ((FooArgs)args).Args[2]);
				Assert.AreEqual("bar", ((FooArgs)args).Args[3]);
			};

			mock.Object.Do("foo", 5, true, "bar");

			Assert.IsTrue(raised);
		}

		[TestMethod]
		public void ShouldAttachToInheritedEvent()
		{
			var bar = new Mock<IDerived>(MockBehavior.Strict);
			bar.Object.Event += (o, e) => { ;}; // Exception Fired here
		}

		[TestMethod]
		public void ShouldAttachAndDetachListener()
		{
			var parent = new Mock<IParent>(MockBehavior.Strict);
			var raised = false;
			EventHandler<EventArgs> listener = (sender, args) => raised = true;

			parent.Object.Event += listener;

			parent.Raise(p => p.Event += null, EventArgs.Empty);

			Assert.IsTrue(raised);

			raised = false;

			parent.Object.Event -= listener;

			parent.Raise(p => p.Event += null, EventArgs.Empty);

			Assert.IsFalse(raised);
		}

		bool raisedField = false;

		[TestMethod]
		public void ShouldAttachAndDetachListenerMethod()
		{
			var parent = new Mock<IParent>(MockBehavior.Strict);
			raisedField = false;

			parent.Object.Event += this.OnRaised;

			parent.Raise(p => p.Event += null, EventArgs.Empty);

			Assert.IsTrue(raisedField);

			raisedField = false;

			parent.Object.Event -= OnRaised;

			parent.Raise(p => p.Event += null, EventArgs.Empty);

			Assert.IsFalse(raisedField);
		}

		[TestMethod]
		public void ShouldAllowListenerListToBeModifiedDuringEventHandling()
		{
			var parent = new Mock<IParent>(MockBehavior.Strict);

			parent.Object.Event += delegate
			{
				parent.Object.Event += delegate { raisedField = true; };
			};

			parent.Raise(p => p.Event += null, EventArgs.Empty);

			// we don't expect the inner event to be raised the first time
			Assert.IsFalse(raisedField);

			// the second time around, the event handler added the first time
			// should kick in
			parent.Raise(p => p.Event += null, EventArgs.Empty);

			Assert.IsTrue(raisedField);
		}

		[TestMethod]
		public void RaisesEvent()
		{
			var mock = new Mock<IAdder<string>>();

			bool raised = false;
			mock.Object.Added += (sender, args) => raised = true;

			mock.Raise(a => a.Added -= null, EventArgs.Empty);

			Assert.IsTrue(raised);
		}

		[TestMethod]
		public void RaisesPropertyChanged()
		{
			var mock = new Mock<IParent>();

			var prop = "";
			mock.Object.PropertyChanged += (sender, args) => prop = args.PropertyName;

			mock.Raise(x => x.PropertyChanged -= It.IsAny<PropertyChangedEventHandler>(), new PropertyChangedEventArgs("foo"));

			Assert.AreEqual("foo", prop);
		}

		[TestMethod]
		public void FailsIfArgumentException()
		{
			var mock = new Mock<IParent>();

			var prop = "";
			mock.Object.PropertyChanged += (sender, args) => prop = args.PropertyName;

			AssertHelper.Throws<ArgumentException>(() => mock.Raise(x => x.PropertyChanged -= null, EventArgs.Empty));
		}

		[TestMethod]
		public void DoesNotRaiseEventOnSubObject()
		{
			var mock = new Mock<IParent> { DefaultValue = DefaultValue.Mock };

			bool raised = false;
			mock.Object.Adder.Added += (sender, args) => raised = true;

			AssertHelper.Same(mock.Object.Adder, mock.Object.Adder);

			mock.Raise(p => p.Adder.Added += null, EventArgs.Empty);

			Assert.IsFalse(raised);
		}

		[TestMethod]
		public void RaisesEventWithActionLambda()
		{
			var mock = new Mock<IWithEvent>();

			mock.SetupSet(m => m.Value = It.IsAny<int>()).Raises(m => m.InterfaceEvent += null, EventArgs.Empty);

			var raised = false;
			mock.Object.InterfaceEvent += (sender, args) => raised = true;

			mock.Object.Value = 5;

			Assert.IsTrue(raised);
		}

		[TestMethod]
		public void RaisesEventWithActionLambdaOnClass()
		{
			var mock = new Mock<WithEvent>();

			mock.SetupSet(m => m.Value = It.IsAny<int>()).Raises(m => m.VirtualEvent += null, EventArgs.Empty);

			var raised = false;
			mock.Object.VirtualEvent += (sender, args) => raised = true;

			mock.Object.Value = 5;

			Assert.IsTrue(raised);
		}

		[TestMethod]
		public void RaisesThrowsIfEventNonVirtual()
		{
			var mock = new Mock<WithEvent>();

			AssertHelper.Throws<ArgumentException>(
				() => mock.SetupSet(m => m.Value = It.IsAny<int>()).Raises(m => m.ClassEvent += null, EventArgs.Empty));
		}

        //[Fact(Skip = "Events on non-virtual events not supported yet")]
        //public void EventRaisingFailsOnNonVirtualEvent()
        //{
        //    var mock = new Mock<WithEvent>();

        //    var raised = false;
        //    mock.Object.ClassEvent += delegate { raised = true; };

        //    // TODO: fix!!! We should go the GetInvocationList route here...
        //    mock.Raise(x => x.ClassEvent += null, EventArgs.Empty);

        //    Assert.IsTrue(raised);
        //}

		[TestMethod]
		public void EventRaisingSucceedsOnVirtualEvent()
		{
			var mock = new Mock<WithEvent>();

			var raised = false;
			mock.Object.VirtualEvent += (s, e) => raised = true;

			// TODO: fix!!! We should go the GetInvocationList route here...
			mock.Raise(x => x.VirtualEvent += null, EventArgs.Empty);

			Assert.IsTrue(raised);
		}

		[TestMethod]
		public void RaisesEventWithActionLambdaOneArg()
		{
			var mock = new Mock<IAdder<int>>();

			mock.Setup(m => m.Do("foo")).Raises<string>(m => m.Done += null, s => new DoneArgs { Value = s });

			DoneArgs args = null;
			mock.Object.Done += (sender, e) => args = e;

			mock.Object.Do("foo");

			Assert.IsNotNull(args);
			Assert.AreEqual("foo", args.Value);
		}

		[TestMethod]
		public void RaisesEventWithActionLambdaTwoArgs()
		{
			var mock = new Mock<IAdder<int>>();

			mock.Setup(m => m.Do("foo", 5))
				.Raises(m => m.Done += null, (string s, int i) => new DoneArgs { Value = s + i });

			DoneArgs args = null;
			mock.Object.Done += (sender, e) => args = e;

			mock.Object.Do("foo", 5);

			Assert.IsNotNull(args);
			Assert.AreEqual("foo5", args.Value);
		}

		[TestMethod]
		public void RaisesEventWithActionLambdaThreeArgs()
		{
			var mock = new Mock<IAdder<int>>();

			mock.Setup(m => m.Do("foo", 5, true))
				.Raises(m => m.Done += null, (string s, int i, bool b) => new DoneArgs { Value = s + i + b });

			DoneArgs args = null;
			mock.Object.Done += (sender, e) => args = e;

			mock.Object.Do("foo", 5, true);

			Assert.IsNotNull(args);
			Assert.AreEqual("foo5True", args.Value);
		}

		[TestMethod]
		public void RaisesEventWithActionLambdaFourArgs()
		{
			var mock = new Mock<IAdder<int>>();

			mock.Setup(m => m.Do("foo", 5, true, "bar"))
				.Raises(m => m.Done += null, (string s, int i, bool b, string s1) => new DoneArgs { Value = s + i + b + s1 });

			DoneArgs args = null;
			mock.Object.Done += (sender, e) => args = e;

			mock.Object.Do("foo", 5, true, "bar");

			Assert.IsNotNull(args);
			Assert.AreEqual("foo5Truebar", args.Value);
		}

		[TestMethod]
		public void RaisesEventWithActionLambdaFiveArgs()
		{
			var mock = new Mock<IAdder<int>>();

			mock.Setup(m => m.Do("foo", 5, true, "bar", 5))
				.Raises(m => m.Done += null, (string s, int i, bool b, string s1, int arg5) => new DoneArgs { Value = s + i + b + s1 + arg5 });

			DoneArgs args = null;
			mock.Object.Done += (sender, e) => args = e;

			mock.Object.Do("foo", 5, true, "bar", 5);

			Assert.IsNotNull(args);
			Assert.AreEqual("foo5Truebar5", args.Value);
		}

		[TestMethod]
		public void RaisesEventWithActionLambdaSixArgs()
		{
			var mock = new Mock<IAdder<int>>();

			mock.Setup(m => m.Do("foo", 5, true, "bar", 5, 6))
				.Raises(m => m.Done += null, (string s, int i, bool b, string s1, int arg5, int arg6) => new DoneArgs { Value = s + i + b + s1 + arg5 + arg6 });

			DoneArgs args = null;
			mock.Object.Done += (sender, e) => args = e;

			mock.Object.Do("foo", 5, true, "bar", 5, 6);

			Assert.IsNotNull(args);
			Assert.AreEqual("foo5Truebar56", args.Value);
		}

		[TestMethod]
		public void RaisesEventWithActionLambdaSevenArgs()
		{
			var mock = new Mock<IAdder<int>>();

			mock.Setup(m => m.Do("foo", 5, true, "bar", 5, 6, 7))
				.Raises(m => m.Done += null, (string s, int i, bool b, string s1, int arg5, int arg6, int arg7) => new DoneArgs { Value = s + i + b + s1 + arg5 + arg6 + arg7 });

			DoneArgs args = null;
			mock.Object.Done += (sender, e) => args = e;

			mock.Object.Do("foo", 5, true, "bar", 5, 6, 7);

			Assert.IsNotNull(args);
			Assert.AreEqual("foo5Truebar567", args.Value);
		}

		[TestMethod]
		public void RaisesEventWithActionLambdaEightArgs()
		{
			var mock = new Mock<IAdder<int>>();

			mock.Setup(m => m.Do("foo", 5, true, "bar", 5, 6, 7, 8))
				.Raises(m => m.Done += null, (string s, int i, bool b, string s1, int arg5, int arg6, int arg7, int arg8) => new DoneArgs { Value = s + i + b + s1 + arg5 + arg6 + arg7 + arg8 });

			DoneArgs args = null;
			mock.Object.Done += (sender, e) => args = e;

			mock.Object.Do("foo", 5, true, "bar", 5, 6, 7, 8);

			Assert.IsNotNull(args);
			Assert.AreEqual("foo5Truebar5678", args.Value);
		}

		[TestMethod]
		public void RaisesCustomEventWithLambda()
		{
			var mock = new Mock<IWithEvent>();
			string message = null;
			int? value = null;

			mock.Object.CustomEvent += (s, i) => { message = s; value = i; };

			mock.Raise(x => x.CustomEvent += null, "foo", 5);

			Assert.AreEqual("foo", message);
			Assert.AreEqual(5, value);
		}

		[TestMethod]
		public void RaisesCustomEventWithLambdaOnPropertySet()
		{
			var mock = new Mock<IWithEvent>();
			string message = null;
			int? value = null;

			mock.Object.CustomEvent += (s, i) => { message = s; value = i; };
			mock.SetupSet(w => w.Value = 5).Raises(x => x.CustomEvent += null, "foo", 5);

			mock.Object.Value = 5;

			Assert.AreEqual("foo", message);
			Assert.AreEqual(5, value);
		}

		public delegate void CustomEvent(string message, int value);

		public interface IWithEvent
		{
			event EventHandler InterfaceEvent;
			event CustomEvent CustomEvent;
			object Value { get; set; }
		}

		public class WithEvent : IWithEvent
		{
			public event EventHandler InterfaceEvent = (s, e) => { };
			public event EventHandler ClassEvent = (s, e) => { };
			public event CustomEvent CustomEvent = (s, e) => { };
			public virtual event EventHandler VirtualEvent = (s, e) => { };
			public virtual object Value { get; set; }
		}

		private void OnRaised(object sender, EventArgs e)
		{
			raisedField = true;
		}

		public class DoneArgs : EventArgs
		{
			public string Value { get; set; }
		}

		public interface IAdder<T>
		{
			event EventHandler<DoneArgs> Done;
			event EventHandler Added;
			void Add(T value);
			int Insert(T value, int index);
			void Do(string s);
			void Do(string s, int i);
			void Do(string s, int i, bool b);
			void Do(string s, int i, bool b, string v);
			void Do(string s, int i, bool b, string v, int arg5);
			void Do(string s, int i, bool b, string v, int arg5, int arg6);
			void Do(string s, int i, bool b, string v, int arg5, int arg6, int arg7);
			void Do(string s, int i, bool b, string v, int arg5, int arg6, int arg7, int arg8);
		}

		public class FooPresenter
		{
			public event EventHandler Fired;

			public FooPresenter(IFooView view)
			{
				view.FooSelected += (s, e) => Fired(s, e);
				view.Canceled += (s, e) => Canceled = true;
			}

			public bool Canceled { get; set; }
		}

		public class FooArgs : EventArgs
		{
			public object Value { get; set; }
			public object[] Args { get; set; }
		}

		public interface IFooView
		{
			event EventHandler<FooArgs> FooSelected;
			event EventHandler Canceled;
		}

		public interface IParent : INotifyPropertyChanged
		{
			event EventHandler<EventArgs> Event;
			IAdder<int> Adder { get; set; }
		}

		public interface IDerived : IParent
		{
		}
	}
}
