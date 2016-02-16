using System;
using UIKit;
using System.Linq;
using Xamarin.Forms;

namespace RxTests.iOS
{
	public class CancellableUIAlertController : ICancellableAlert
	{
		UIAlertController alert;

		bool animated = false;

		string title = "";


		public CancellableUIAlertController (bool animated = false)
		{
			this.animated = animated;
			alert = UIAlertController.Create ("", "", UIAlertControllerStyle.Alert);
			alert.AddAction (UIAlertAction.Create ("OK", UIAlertActionStyle.Default, _ => OnOK?.Invoke(this, null)));
			alert.AddAction (UIAlertAction.Create ("Cancel", UIAlertActionStyle.Cancel, _ => OnCancel?.Invoke(this, null)));
		}


		#region ICancellableAlert implementation

		public event EventHandler OnOK;

		public event EventHandler OnCancel;


		public ICancellableAlert Open ()
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

		public ICancellableAlert Close ()
		{
			if (alert != null && !alert.IsBeingDismissed)
				alert.DismissViewController(animated, null);

			return this;
		}

		public ICancellableAlert SetMessage (string txt)
		{
			alert.Message = txt;
			return this;
		}

		public ICancellableAlert SetTitle (string txt)
		{
			title = txt;
			alert.Title = txt;
			return this;
		}

		public ICancellableAlert SetCancelTitle (string title = "Cancel")
		{
			//impossible :(
			//alert.Actions.ToList ().First (a => a.Style == UIAlertActionStyle.Cancel).Title = title;
			return this;
		}

		public ICancellableAlert SetOKTitle (string title = "OK")
		{
			//impossible :(
			//alert.Actions.ToList ().First (a => a.Style == UIAlertActionStyle.Default).Title = title;
			return this;
		}

		public ICancellableAlert SetTimeRemaining (long millisec)
		{
			var tit = string.Format(title + " ({0})", Math.Round (millisec / 1000d));
			alert.Title = tit;
			return this;
		}

		#endregion

		#region IDisposable implementation

		public void Dispose ()
		{
			Close ();
		}

		#endregion
	}
}

