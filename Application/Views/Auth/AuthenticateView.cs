using System;

namespace Application.Views
{

	public class AuthenticateResponse
	{
		public string Email { get; set; }
		public string Token { get; set; }
		public DateTime TokenExpires { get; set; }
	}
}

