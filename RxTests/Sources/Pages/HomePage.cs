using System;
using System.Linq;
using Xamarin.Forms;
using ReactiveUI;
using System.Reactive.Linq;
using System.Reactive;
using Splat;
using ReactiveUI.XamForms;
using GitHub;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RxTests
{
	public class HomePage : ReactiveContentPage<HomeViewModel>
	{
		//readonly Button openButt;
		readonly Editor textEditor;
		readonly ListView listView;
		readonly ProgressBar loading;

		#region IViewFor implementation

		public HomeViewModel ViewModel { get; set; }
		object IViewFor.ViewModel {
			get { return ViewModel; }
			set { ViewModel = (HomeViewModel)value; }
		}

		#endregion


		public HomePage (Func<ICancellableAlert> alertFactory)
		{
			//ViewModel = Locator.Current.GetService<HomeViewModel> ();
			BindingContext = ViewModel = new HomeViewModel(alertFactory);

			Content = new StackLayout {
				VerticalOptions = LayoutOptions.Center,
				Padding = new Thickness(14),
				Children = {
					new Label {
						HorizontalTextAlignment = TextAlignment.Center,
						Text = ""
					},
					(textEditor = new Editor {
						Text = "",
						BackgroundColor = Color.Gray,
					}),
					(loading = new ProgressBar{
						IsVisible = false,

					}),
					/*
					(openButt = new Button {
						Text = "show popup",
					}),
					*/
					(listView = new ListView())
				}
			};

			var githubAPI = new GitHubApi ();
			IDisposable subscription = Observable.FromEventPattern<EventHandler<TextChangedEventArgs>, TextChangedEventArgs>
				(ah => textEditor.TextChanged += ah, rh => textEditor.TextChanged -= rh)
				.Throttle (TimeSpan.FromMilliseconds(400))
				.Select (query => query.EventArgs.NewTextValue.Trim())
				.Where (query => query.Length > 0)
				.ObserveOn(RxApp.MainThreadScheduler)
				.Do(q => listView.ItemsSource = null)
				.Do(q => loading.IsVisible = true)
				//.Timeout(TimeSpan.FromSeconds(7))
				.Select (query => githubAPI.GetUserObservable (query))
				.Switch ()
				//.Catch((Refit.ApiException e) => Observable.Return(new User()))
				.ObserveOn(RxApp.MainThreadScheduler)
				.Do(q => loading.ProgressTo(1,2, Easing.Linear))
				.Select (user => githubAPI.GetReposOwned(user.Login))
				.Switch ()
				.ObserveOn(RxApp.MainThreadScheduler)
				.Do(q => loading.ProgressTo(2,2, Easing.Linear))
				.Select (list => list.Where(repo => repo.Private != true))
				.Do(r => loading.IsVisible = false)
				.Retry()
				.Subscribe (
					onNext: list => listView.ItemsSource = list
				);

			/*
			IObservable<long> refresButtonClickedObservable = Observable.Return(1L);
			Observable.Merge (
				refresButtonClickedObservable,
				Observable.Interval (TimeSpan.FromMinutes (1))				
			).Subscribe (_ => Refresh());

			this.BindCommand (ViewModel, vm => vm.ButtonCommand, v => v.butt);

			MainPage.BindingContext = 
				ViewModel = 
					new HomeViewModel (GetNewAlert, RxApp.MainThreadScheduler);
			*/
			/*
			IObservable<string> latestAuthTokenObservable = Observable.Return ("");
			IObservable<string> latestSecureFileUrlObservable = Observable.Return ("");

			//...
			Observable.CombineLatest (
				latestSecureFileUrlObservable,
				latestAuthTokenObservable,
				(str, str2) => string.Join
			).Subscribe (LoadFile);
			*/
			/*
			const TimeSpan maxTimeBetweenClicks = TimeSpan.FromMilliseconds(300);
			Observable.FromEventPattern (a => butt.Clicked += a, r => butt.Clicked -= r)
				.TimeInterval ()
				.Buffer (3, 1)
				.Select (timeIntervals => {
					foreach (var ti in timeIntervals)
						if (ti.Interval > maxTimeBetweenClicks)
							return false;
					return true;
					}
				)
				.ObserveOn (RxApp.MainThreadScheduler)
				.Subscribe ();
			*/

			//openButt.Clicked += async (sender, e) => await ViewModel.OpenPopupCommand.ExecuteAsync ();
		}
	}
}