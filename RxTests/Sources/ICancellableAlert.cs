using System;
using System.Threading;

namespace RxTests
{
	public interface ICancellableAlert : IDisposable
	{
		ICancellableAlert Open();
		ICancellableAlert Close();
		ICancellableAlert SetMessage (string txt);
		ICancellableAlert SetTitle (string txt);
		ICancellableAlert SetCancelTitle (string title = "Cancel");
		ICancellableAlert SetOKTitle (string title = "OK");
		ICancellableAlert SetTimeRemaining (long millisec);

		event EventHandler OnOK;
		event EventHandler OnCancel;
	}
}

