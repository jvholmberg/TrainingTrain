using System;

namespace Application.Views
{
	public class LoginRequest
	{
		public string Email { get; set; }
		public string Password { get; set; }
	}

	public class LoginResponse
	{
		public string Email { get; set; }
		public string Token { get; set; }
		public DateTime TokenExpires { get; set; }
	}
}