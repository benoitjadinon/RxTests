using System;
using System.Threading;

namespace RxTests
{
	public interface ICancellableAlert
	{
		void Open();
		void Close();
		string Message { get; set; }
		string Title { get; set; }
		void SetButton(string title = "OK", int atPosition = 0, Action onClick = null);
	}
}

