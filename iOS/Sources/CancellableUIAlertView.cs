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

		public override ICancellableAlert Open ()
		{
			alert.Show ();
			return this;
		}

		public override ICancellableAlert Close ()
		{
			alert.DismissWithClickedButtonIndex (-1, false);
			return this;
		}

		public override void Dispose ()
		{
			Close ();
		}

		public override ICancellableAlert SetMessage (string txt)
		{
			alert.Message = txt;
			return this;
		}

		public override ICancellableAlert SetTitle (string txt)
		{
			//TODO
			alert.Title = txt;
			return this;
		}

		public override ICancellableAlert SetCancelTitle (string title = "Cancel")
		{
			//TODO
			alert.AddButton (title);
			return this;
		}

		public override ICancellableAlert SetOKTitle (string title = "OK")
		{
			alert.AddButton (title);
			return this;
		}

		public override ICancellableAlert DisplayTimeRemaining (string time)
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
