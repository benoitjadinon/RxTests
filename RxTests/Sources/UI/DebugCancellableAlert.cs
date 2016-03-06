using System;

using System.Diagnostics;
using RxTests;
using Xamarin.Forms;

namespace RxTests
{

	class DebugCancellableAlert : ObservableAlert
	{
		#region ICancellableAlert implementation

		override public ICancellableAlert Open ()
		{
			Debug.WriteLine ("alert open");
			return this;
		}
		override public ICancellableAlert Close ()
		{
			Debug.WriteLine ("alert close");
			return this;
		}
		override public ICancellableAlert SetMessage (string txt)
		{
			Debug.WriteLine ("alert set message to {0}", txt);
			return this;
		}
		override public ICancellableAlert SetTitle (string txt)
		{
			Debug.WriteLine ("alert set title to {0}", txt);
			return this;
		}
		override public ICancellableAlert SetCancelTitle (string title = "Cancel")
		{
			Debug.WriteLine ("alert set cancel button title to {0}", title);
			return this;
		}
		override public ICancellableAlert SetOKTitle (string title = "OK")
		{
			Debug.WriteLine ("alert set ok button title to {0}", title);
			return this;
		}

		override public ICancellableAlert DisplayTimeRemaining (string time)
		{
			Debug.WriteLine ("alert set time to {0}", time);
			return this;
		}

		override public void Dispose ()
		{
			base.Dispose ();
		}

		#endregion
		
	}
}
