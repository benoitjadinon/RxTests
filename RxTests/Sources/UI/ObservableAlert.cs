using System;
using System.Reactive.Linq;

namespace RxTests
{
	public abstract class ObservableAlert : ICancellableAlert
	{
		#region ICancellableAlert implementation

		public IObservable<bool> WhenResult {
			get{
				return Observable.FromEventPattern<EventHandler<bool>, bool> (h => OnResult += h, h => OnResult -= h)
					.Select (p => p.EventArgs)
					.Take(1)
					;
			}
		}

		public virtual event EventHandler<bool> OnResult;

		public abstract ICancellableAlert Open ();

		public abstract ICancellableAlert Close ();

		public abstract ICancellableAlert SetMessage (string txt);

		public abstract ICancellableAlert SetTitle (string txt);

		public abstract ICancellableAlert SetCancelTitle (string title = "Cancel");

		public abstract ICancellableAlert SetOKTitle (string title = "OK");

		public abstract ICancellableAlert DisplayTimeRemaining (string time);

		#endregion

		#region IDisposable implementation

		public virtual void Dispose ()
		{
			
		}

		#endregion
	}
}

