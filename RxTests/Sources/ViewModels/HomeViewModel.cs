using System;
using ReactiveUI;
using System.Reactive.Linq;
using System.Diagnostics;
using System.Threading;

namespace RxTests
{
	public class HomeViewModel : ReactiveObject
	{
		const int TimerStartTime = 10;
		const int TimerIntervalMillisec = 1000;

		public HomeViewModel (Func<ICancellableAlert> alertFactory)
		{
			var alert = alertFactory?.Invoke ();
			alert
				.SetTitle ("title")
				.SetMessage ("msg")
				.Open ();

			var onOKPressed = Observable.FromEventPattern<EventHandler, EventArgs> (h => alert.OnOK += h, h => alert.OnOK -= h);
			onOKPressed.Subscribe (_ => Go ());

			var onCancelPressed = Observable.FromEventPattern<EventHandler, EventArgs> (h => alert.OnCancel += h, h => alert.OnCancel -= h);
			onCancelPressed.Subscribe (_ => Cancel ());

			var oneSec = TimeSpan.FromMilliseconds (TimerIntervalMillisec);
			Observable.Generate (
				initialState: TimerStartTime,
				condition: i => i >= 0,
				iterate: i => i - 1,
				resultSelector: i => i,
				timeSelector: i => oneSec
			)
				.TakeUntil (onOKPressed)
				.TakeUntil (onCancelPressed)
				.ObserveOn (SynchronizationContext.Current)
				.Subscribe (
				onNext: seconds => alert.SetTimeRemaining (millisec: seconds * TimerIntervalMillisec),
				onCompleted: () => {
					alert.Close ();
					Go ();
				}
			);
		}

		void Cancel ()
		{
			Debug.WriteLine ("canceled");
		}

		void Go ()
		{
			Debug.WriteLine ("next!");
		}
	}
}

