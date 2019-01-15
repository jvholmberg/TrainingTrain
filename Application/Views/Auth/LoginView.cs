using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
		public DateTime ValidUntil { get; set; }
	}
}