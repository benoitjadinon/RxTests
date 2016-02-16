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
			var onCancelPressed = Observable.FromEventPattern<EventHandler, EventArgs> (h => alert.OnCancel += h, h => alert.OnCancel -= h);

			var loop = Observable.Merge (
					Observable.Return (Convert.ToInt64 (TimerStartTime)), // cold obs to show the first '10' while waiting for 1st interval iteration
					Observable.Interval (TimeSpan.FromMilliseconds (TimerIntervalMillisec)) // the actual timer
						.Take (TimerStartTime + 1)            // +1 to show '0' for a second
						.Select (p => TimerStartTime - p - 1)  // inverse countdown
			    )
				.TakeUntil (onOKPressed)
				.TakeUntil (onCancelPressed)
				.ObserveOn (SynchronizationContext.Current)
				.Subscribe (
				    onNext: seconds => {
						if (seconds >= 0)
							alert.DisplayTimeRemaining (time: seconds.ToString ());
					},
				    onCompleted: () => {
						alert.Close ();
						Go ();
					}
				);
			
			onOKPressed.Subscribe (_ => {
				loop.Dispose ();
				Go ();
			});

			onCancelPressed.Subscribe (_ => {
				loop.Dispose ();
				Cancel ();
			});
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

