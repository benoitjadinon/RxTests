﻿using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using RxTests;
using BlueMarin;

namespace RxTests.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();

			// Code for starting up the Xamarin Test Cloud Agent
			#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
			#endif

			LoadApplication (new IOSApp ());

			return base.FinishedLaunching (app, options);
		}
	}

	class IOSApp : App
	{
		protected override ICancellableAlert CreateAlert(){
			//return new CancellableUIAlertView ();
			return new CancellableUIAlertController ();
		}
	}

}

