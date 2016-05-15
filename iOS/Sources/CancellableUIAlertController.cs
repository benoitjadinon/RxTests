using System;
using UIKit;
using System.Linq;
using Xamarin.Forms;

namespace RxTests.iOS
{
	public class CancellableUIAlertController : ObservableAlert
	{
		UIAlertController alert;

		bool animated = false;

		string title = "";


		public CancellableUIAlertController (bool animated = false)
		{
			this.animated = animated;
			alert = UIAlertController.Create ("", "", UIAlertControllerStyle.Alert);
			alert.AddAction (UIAlertAction.Create ("OK", UIAlertActionStyle.Default, _ => OnResult?.Invoke(this, true)));
			alert.AddAction (UIAlertAction.Create ("Cancel", UIAlertActionStyle.Cancel, _ => OnResult?.Invoke(this, false)));
		}


		#region ICancellableAlert implementation

		public override event EventHandler<bool> OnResult;

		public override ICancellableAlert Open ()
		{
			//TODO: pass current controller instead somehow
			var window= UIApplication.SharedApplication.KeyWindow;
			var vc = window.RootViewController;
			while (vc.PresentedViewController != null)
			{
				vc = vc.PresentedViewController;
			}
			vc.PresentViewController (alert, animated, completionHandler: null);
			return this;
		}

		public override ICancellableAlert Close ()
		{
			if (alert != null && !alert.IsBeingDismissed)
				alert.DismissViewController(animated, null);

			return this;
		}

		public override ICancellableAlert SetMessage (string txt)
		{
			alert.Message = txt;
			return this;
		}

		public override ICancellableAlert SetTitle (string txt)
		{
			title = txt;
			alert.Title = txt;
			return this;
		}

		public override ICancellableAlert SetCancelTitle (string title = "Cancel")
		{
			//impossible :(
			//alert.Actions.ToList ().First (a => a.Style == UIAlertActionStyle.Cancel).Title = title;
			return this;
		}

		public override ICancellableAlert SetOKTitle (string title = "OK")
		{
			//impossible :(
			//alert.Actions.ToList ().First (a => a.Style == UIAlertActionStyle.Default).Title = title;
			return this;
		}

		public override ICancellableAlert DisplayTimeRemaining (string time)
		{
			var tit = string.Format(title + " ({0})", time);
			alert.Title = tit;
			return this;
		}

		#endregion

		#region IDisposable implementation

		public override void Dispose ()
		{
			Close ();
			base.Dispose ();
		}

		#endregion
	}
}

