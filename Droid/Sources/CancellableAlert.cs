using System;

namespace RxTests.Droid
{
	public class CancellableAlert : ICancellableAlert
	{
		public CancellableAlert ()
		{
		}

		#region ICancellableAlert implementation

		public void Open ()
		{
			throw new NotImplementedException ();
		}

		public void Close ()
		{
			throw new NotImplementedException ();
		}

		public void SetButton (string title = "OK", int atPosition = 0, Action onClick = null)
		{
			throw new NotImplementedException ();
		}

		public string Message {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public string Title {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		#endregion
	}
}

