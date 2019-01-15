using System;

namespace Application.Views
{

	public class RenewResponse
	{
		public string Email { get; set; }
		public string Token { get; set; }
		public DateTime TokenExpires { get; set; }
	}
}