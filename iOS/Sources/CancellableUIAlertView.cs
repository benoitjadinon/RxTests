using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using RxTests;
using BlueMarin;

namespace RxTests.iOS
{

	class CancellableUIAlertView : ObservableAlert
	{
		readonly UIAlertView alert;

		public CancellableUIAlertView ()
		{
			alert = new UIAlertView ();
				//NSBundle.MainBundle.LocalizedString ("Cancel", "Cancel"),
				//NSBundle.MainBundle.LocalizedString ("OK", "OK"));
			alert.ShouldEnableFirstOtherButton = ShouldEnableFirstOtherButton;
			alert.Clicked += OnClick;
		}

		void OnClick (object sender, UIButtonEventArgs e)
		{
			//buttonArgs.ButtonIndex;
		}

		#region ICancellableAlert implementation

		public ICancellableAlert Open ()
		{
			alert.Show ();
			return this;
		}

		public ICancellableAlert Close ()
		{
			alert.DismissWithClickedButtonIndex (-1, false);
			return this;
		}

		public void Dispose ()
		{
			Close ();
		}

		public ICancellableAlert SetMessage (string txt)
		{
			alert.Message = txt;
			return this;
		}

		public ICancellableAlert SetTitle (string txt)
		{
			//TODO
			alert.Title = txt;
			return this;
		}

		public ICancellableAlert SetCancelTitle (string title = "Cancel")
		{
			//TODO
			alert.AddButton (title);
			return this;
		}

		public ICancellableAlert SetOKTitle (string title = "OK")
		{
			alert.AddButton (title);
			return this;
		}

		public ICancellableAlert DisplayTimeRemaining (string time)
		{
			SetOKTitle (string.Format ("OK ({0})", time));
			return this;
		}

		#endregion

		bool ShouldEnableFirstOtherButton(UIAlertView theAlert)
		{
			return !theAlert.GetTextField (0).Text.IsEmpty ();
		}
	}
}
