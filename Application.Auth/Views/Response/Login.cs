namespace Application.Auth.Views.Response
{
	public class Login
	{
		public string AccessToken { get; set; }
		public string Expiry { get; set; }
		public string RefreshToken { get; set; }
		public string Message { get; set; }
	}
}
