using System;
using ReactiveUI;
using System.Reactive.Linq;
using System.Diagnostics;
using System.Reactive.Concurrency;

namespace RxTests
{	
	public class HomeViewModel : ReactiveObject
	{
		const int TimerStartTime = 10;
		const int TimerIntervalMillisec = 1000;

		public ReactiveCommand<System.Reactive.Unit> OpenPopupCommand { get; protected set; }

		public HomeViewModel (Func<ICancellableAlert> alertFactory, IScheduler scheduler = null, IHomeModel mod = null)
		{
			/*
			if (scheduler == null)
				scheduler = RxApp.MainThreadScheduler;
			
			var alert = alertFactory ()
				.SetTitle ("some title")
				.SetMessage ("some message")
				;

			var automaticGoAlert = alert.WhenResult
				.Merge (
					Observable.Interval (TimeSpan.FromMilliseconds (TimerIntervalMillisec), scheduler) // the actual timer
						.Select (p => TimerStartTime - p - 1)          // inversing countdown
						.StartWith (Convert.ToInt64 (TimerStartTime))  // starting so it displays 10, otherwise nothing for 1 sec
						.Do (sec => alert.DisplayTimeRemaining (sec.ToString ())) // updating alert
						.Select (sec => sec <= 0)                      // returns true only at end of countdown
						.Where (b => b)                                  // only goes forward if true
						.Do (_ => alert.Close ())                      // close popup
				)
				.Take (1) // first onNext passes through then this will call onComplete
				;

			OpenPopupCommand = ReactiveCommand.CreateAsyncTask(async _ => { 
				alert.Open(); 
				automaticGoAlert.Subscribe (
					onNext: alertResult => {
						if (alertResult)
							alertFactory ().SetMessage ("OK").Open ();
						else
							alertFactory ().SetMessage ("Cancelled").Open ();
					}
				);
			}, RxApp.MainThreadScheduler);
			*/

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
				.Subscribe (
					onNext: repos => Debug.WriteLine (repos[0]), 
					onError: Debug.WriteLine
				);
			*/

			//var result = await github.GetUserObservable("benoitjadinon")
			//	.Timeout(TimeSpan.FromSeconds(10));

		}
	}

	/*
	class MainWindowViewModel : ReactiveObject
	{
		private string passwordText;
		public string PasswordText {
			get { return passwordText; }
			set { this.RaiseAndSetIfChanged(ref passwordText, value); }
		}
		public ReactiveCommand LoginCommand { get; private set; }

		public MainWindowViewModel()
		{
			var canLoginObservable = this.WhenAny(vm => vm.PasswordText, 
				s => !string.IsNullOrWhiteSpace(s.Value));

			LoginCommand = new ReactiveCommand(canLoginObservable);
			LoginCommand.Subscribe(p => MessageBox.Show("I was clicked"));
		}
	}
	*/
}

