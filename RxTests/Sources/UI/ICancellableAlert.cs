using System;
using System.Threading;
using System.Runtime.CompilerServices;

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
		ICancellableAlert DisplayTimeRemaining (string time);

		event EventHandler<bool> OnResult;
		IObservable<bool> WhenResult { get; }
	}
}

