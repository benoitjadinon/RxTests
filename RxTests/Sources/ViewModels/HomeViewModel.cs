using System;
using ReactiveUI;
using System.Reactive.Linq;
using System.Diagnostics;
using System.Reactive.Concurrency;
using GitHub;

namespace RxTests
{
	public class HomeViewModel : ReactiveObject
	{
		const int TimerStartTime = 10;
		const int TimerIntervalMillisec = 1000;

		IHomeModel model;

		public HomeViewModel (Func<ICancellableAlert> alertFactory, IScheduler scheduler = null, IHomeModel mod = null)
		{
			//model = Locator.Current.GetService<IHomeModel> ();
			
			if (scheduler == null)
				scheduler = RxApp.MainThreadScheduler;
			
			var alert = alertFactory ()
				.SetTitle ("some title")
				.SetMessage ("some message")
				.Open ();

			var onAlertResult = alert.AsObservable ();

			var canGoNext = onAlertResult
				.Merge (
					Observable.Interval (TimeSpan.FromMilliseconds (TimerIntervalMillisec), scheduler) // the actual timer
						.Select (p => TimerStartTime - p - 1)          // inversing countdown
						.StartWith (Convert.ToInt64 (TimerStartTime))  // starting so it displays 10, otherwise nothing for 1 sec
						.Do (sec => alert.DisplayTimeRemaining (sec.ToString ())) // updating alert
						.Select (sec => sec <= 0)                      // returns true only at end of countdown
						.Where (isEnded => isEnded)                    // only forwarding if true
						.Do (_ => alert.Close ())                      // close popup
				)
				.Take (1) // first onNext passes through then this will call onComplete
				;
			canGoNext.Subscribe (
					onNext: alertResult => {
						if (alertResult)
							Debug.WriteLine ("OK");
						else
							Debug.WriteLine ("canceled");
					}
				);

			var github = new GitHubApi ();
			var subscription = github.GetUserObservable ("benoitjadinon")
				.SubscribeOn (RxApp.TaskpoolScheduler)
				.Timeout (TimeSpan.FromSeconds (10))          // throws TimeOutException
				.DelaySubscription (TimeSpan.FromSeconds (3)) // though we subscribe directly, it'll wait for 3 seconds before doing the actual call
				.Do (onNext: Debug.WriteLine)       // debugs the actual call result
				.Delay (TimeSpan.FromSeconds (2))   // will hold the result for 2 more seconds
				.Select (u => u.Login)              // forwards only the login, instead of the whole User object
				.SelectMany (github.GetReposOwned)  // do an other call
				.ObserveOn (scheduler)
				.Subscribe (
					onNext: repos => Debug.WriteLine (repos[0]), 
					onError: Debug.WriteLine
				);

			//var result = await github.GetUserObservable("benoitjadinon")
			//	.Timeout(TimeSpan.FromSeconds(10));
		}
	}
}

