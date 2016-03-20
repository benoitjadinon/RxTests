using System;
using Xamarin.Forms;
using ReactiveUI;
using System.Reactive.Linq;
using System.Reactive;
using Splat;

namespace RxTests
{
	public class HomePage : ContentPage, IViewFor<HomeViewModel>
	{
		public Button butt;

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
				Children = {
					new Label {
						HorizontalTextAlignment = TextAlignment.Center,
						Text = "Reactive Tests"
					},
					(butt = new Button {
						Text = "test",
					})
				}
			};

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
		}


	}
}

