using System;

using System.Diagnostics;
using RxTests;
using Xamarin.Forms;

namespace RxTests
{

	class DebugCancellableAlert : ICancellableAlert
	{
		#region ICancellableAlert implementation

		public event EventHandler OnOK;
		public event EventHandler OnCancel;

		public ICancellableAlert Open ()
		{
			Debug.WriteLine ("alert open");
			return this;
		}
		public ICancellableAlert Close ()
		{
			Debug.WriteLine ("alert close");
			return this;
		}
		public ICancellableAlert SetMessage (string txt)
		{
			Debug.WriteLine ("alert set message to {0}", txt);
			return this;
		}
		public ICancellableAlert SetTitle (string txt)
		{
			Debug.WriteLine ("alert set title to {0}", txt);
			return this;
		}
		public ICancellableAlert SetCancelTitle (string title = "Cancel")
		{
			Debug.WriteLine ("alert set cancel button title to {0}", title);
			return this;
		}
		public ICancellableAlert SetOKTitle (string title = "OK")
		{
			Debug.WriteLine ("alert set ok button title to {0}", title);
			return this;
		}

		public ICancellableAlert DisplayTimeRemaining (string time)
		{
			Debug.WriteLine ("alert set time to {0}", time);
			return this;
		}

		public void Dispose ()
		{
		}

		#endregion
		
	}
}
