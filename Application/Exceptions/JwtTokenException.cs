using System;

namespace Application.Exceptions
{
	public class JwtTokenException : Exception
	{
		public JwtTokenException(string message)
			: base(message) { }
	}
}
