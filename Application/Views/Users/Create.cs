using System.Collections.Generic;

namespace Application.Views.Users
{
	public class CreateRequest
	{
		public string Username { get; set; }
		public string Password { get; set; }
		public string PasswordVerify { get; set; }
	}

	public class CreateResponse
	{
		public string Username { get; set; }
		public IEnumerable<string> Errors { get; set; }
	}
}