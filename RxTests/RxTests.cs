
using ReactiveUI;
using RxTests;
using Xamarin.Forms;
using Splat;

namespace RxTests
{
	public class App : Application
	{
		public App ()
		{
			Locator.CurrentMutable.RegisterLazySingleton(() => new HomeModel(), typeof(IHomeModel));

			//Locator.CurrentMutable.Register(() => new HomePage(), typeof(IViewFor<HomeViewModel>));
			//MainPage = (Page)Locator.Current.GetService<IViewFor<HomeViewModel>>();

			MainPage = new HomePage (CreateAlert);
		}


		protected virtual ICancellableAlert CreateAlert ()
		{
			return new DebugCancellableAlert ();
		}

		protected override void OnStart ()
		{
		}

		protected override void OnSleep ()
		{
		}

		protected override void OnResume ()
		{
		}

	}

}