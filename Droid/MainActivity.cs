using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using ReactiveUI;

namespace RxTests.Droid
{
	[Activity (Label = "RxTests.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Forms.Init (this, bundle);

			LoadApplication (new AndroidApp ());
		}
	}

	class AndroidApp : App
	{
		protected override ICancellableAlert CreateAlert ()
		{
			return new CancellableAlert (Forms.Context);
		}
	}
}

