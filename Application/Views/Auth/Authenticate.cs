namespace Application.Views.Auth
{
	public class AuthenticateRequest
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}

	public class AuthenticateResponse
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
		public string Expiry { get; set; }
	}
}
