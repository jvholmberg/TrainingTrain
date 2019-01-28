using System;

namespace Application.Exceptions
{
	public class NotAllowedException : Exception
	{
		public NotAllowedException(string message)
			: base(message) { }
	}
}
