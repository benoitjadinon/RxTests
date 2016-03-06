using System;
using Android.App;
using Android.Content;
using Android.Widget;
using System.Collections.Generic;
using Android.Hardware;

namespace RxTests.Droid
{
	//TODO : <AD, ADBuilder> to support support v7 library alertdialog classes
	public class CancellableAlert : ObservableAlert
	{
		readonly Dictionary<DialogButtonType, Func<AlertDialog.Builder, string, EventHandler<DialogClickEventArgs>, AlertDialog.Builder>> funcButtonsMap;
		
		readonly AlertDialog.Builder builder;
		AlertDialog dialogShown;

		public CancellableAlert (Context context)
		{
			funcButtonsMap = new Dictionary<DialogButtonType, Func<AlertDialog.Builder, string, EventHandler<DialogClickEventArgs>, AlertDialog.Builder>>()
			{
				{ DialogButtonType.Positive, (b, title, cb) => b.SetPositiveButton(title, cb) },
				{ DialogButtonType.Negative, (b, title, cb) => b.SetNegativeButton(title, cb) }
			};
			
			builder = new AlertDialog.Builder (context);

			ChangeButton (DialogButtonType.Negative, "Cancel", () => (s,e) => OnResult(s, false));
			ChangeButton (DialogButtonType.Positive, "OK",     () => (s,e) => OnResult(s, true));
		}

		#region ICancellableAlert implementation

		public override event EventHandler<bool> OnResult;

		public override ICancellableAlert Open ()
		{
			CloseDialogIfShown ();

			dialogShown = builder.Show ();

			return this;
		}

		public override ICancellableAlert Close ()
		{
			CloseDialogIfShown ();

			return this;
		}

		public override ICancellableAlert SetCancelTitle (string title = "Cancel") => 
			ChangeButton (DialogButtonType.Negative, title);

		public override ICancellableAlert SetOKTitle (string title = "OK") =>
			ChangeButton (DialogButtonType.Positive, title);

		public override ICancellableAlert SetTitle (string txt) {
			if (dialogShown != null)
				dialogShown.SetTitle (txt);
			else
				builder.SetTitle (txt);
			return this;
		}

		public override ICancellableAlert SetMessage (string txt) {
			if (dialogShown != null)
				dialogShown.SetMessage (txt);
			else
				builder.SetMessage (txt);
			return this;
		}

		public override ICancellableAlert DisplayTimeRemaining (string time)
		{
			SetOKTitle (string.Format ("OK ({0})", time));
			return this;
		}

		public override void Dispose ()
		{
			CloseDialogIfShown ();
			base.Dispose ();
		}

		#endregion


		void CloseDialogIfShown ()
		{
			if (dialogShown != null && dialogShown.IsShowing)
				dialogShown.Dismiss ();
		}

		protected ICancellableAlert ChangeButton (DialogButtonType type, string title, Func<EventHandler> callback = null)
		{
			Button button;
			if (dialogShown != null && (button = dialogShown.GetButton((int)type)) != null){
				if (callback != null)
					button.Click += (s,e) => callback()(s,e);
				button.Text = title;
				button.Invalidate();
			} else {
				funcButtonsMap [type]?.Invoke (builder, title, (s,e) => callback?.Invoke()(s,e));
			}

			return this;
		}
	}
}

