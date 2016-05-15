using NUnit.Framework;
using Microsoft.Reactive.Testing;
using System;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI.Testing;

// only works in Visual Studio

namespace RxTests.Tests
{
	[TestFixture ()]
	public class ViewModelTest
	{
		[Test ()]
		public void TestHotObservable ()
		{
			TestScheduler scheduler = new TestScheduler();

			var xs = scheduler.CreateHotObservable (
				scheduler.OnNextAt(100, "a"), // only works in Visual Studio
				scheduler.OnNextAt(200, "b"),
				scheduler.OnCompletedAt<string>(300)
			);

			var observer = scheduler.CreateObserver<string>();
			scheduler.Schedule(TimeSpan.FromTicks(220), 
				(sched, state) => xs.Subscribe(observer));

			scheduler.Start();

			observer.Messages.AssertEqual (
				new Recorded<Notification<string>>(250, Notification.CreateOnCompleted<string>())
			);
		}

		[Test ()]
		public void TestTimer ()
		{
			TestScheduler scheduler = new TestScheduler();

			HomeViewModel vm = new HomeViewModel (null, scheduler);

			//vm.GetObservable()
		}
	}
}

