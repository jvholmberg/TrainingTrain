namespace MsvcAuth.Entities
{
	public class Auth
	{
		public int Id{ get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Role { get; set; }
		public string RefreshToken { get; set; }
	}
}
