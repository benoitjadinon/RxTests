using System;
using ReactiveUI;
using System.Reactive.Linq;
using System.Diagnostics;
using System.Reactive.Concurrency;
using Trash.Service.Namur;
using NodaTime;

namespace RxTests
{
	public class HomeViewModel : ReactiveObject
	{
		const int TimerStartTime = 10;
		const int TimerIntervalMillisec = 1000;

		public HomeViewModel (Func<ICancellableAlert> alertFactory, IScheduler scheduler = null)
		{
			if (scheduler == null)
				scheduler = RxApp.MainThreadScheduler;
			
			var alert = alertFactory?.Invoke ();

			alert
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
				.Take (1) // first onNext passes through then calls onComplete
				;
			canGoNext.Subscribe (
					onNext: alertResult => {
						if (alertResult)
							Debug.WriteLine ("OK");
						else
							Debug.WriteLine ("canceled");
					}
				);

			var trashService = new TrashCollectService ();
			Observable.Timer (TimeSpan.FromSeconds (3))
				.SelectMany(
					trashService.GetMunicipalitiesObservable ()
						.Timeout (TimeSpan.FromSeconds (5))
						.Retry(1)
				)
				.SelectMany(mu => trashService.GetSubMunicipalitiesObservable(mu[0]))
				.SelectMany(sm => trashService.GetCollectDaysObservable(sm[0], 
					new Interval(SystemClock.Instance.Now, SystemClock.Instance.Now.Plus(Duration.FromStandardDays(4)))
				))
				.ObserveOn(scheduler)
				.Subscribe (onNext: l => Debug.WriteLine ($"days {l.Count}"), onError: Debug.WriteLine)
				;

			Observable.Interval (TimeSpan.FromSeconds (1), scheduler)
				.Delay(TimeSpan.FromSeconds(1))
				.Select(i => i + 1)
				.Take (12)
				.Select(seconds => $"{seconds.ToString ()} seconds since subscribed")
				.Subscribe (Debug.WriteLine);

			/*
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
				.Subscribe (onNext: repos => Debug.WriteLine (repos[0]), onError: Debug.WriteLine);
				;
*/
			//var result = await github.GetUserObservable("benoitjadinon")
			//	.Timeout(TimeSpan.FromSeconds(10));
		}
	}
}

