using System;

using System.Diagnostics;
using RxTests;
using Xamarin.Forms;
using Trash.Service.Namur;
using System.Reactive;
using System.Reactive.Linq;
using System.Collections.Generic;
using Trash.Model;
using System.Threading.Tasks;
using Refit;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Globalization;
using System.Text;
using System.Reactive.Concurrency;
using ReactiveUI;

namespace RxTests
{
	public class App : Application
	{
		public App ()
		{
			Button butt;

			MainPage = new ContentPage {
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
				}
			};
		}


		protected virtual ICancellableAlert GetNewAlert ()
		{
			return new DebugCancellableAlert ();
		}

		protected override void OnStart ()
		{
			MainPage.BindingContext = new HomeViewModel (GetNewAlert, RxApp.MainThreadScheduler);
		}

		protected override void OnSleep ()
		{
		}

		protected override void OnResume ()
		{
		}
	}

}