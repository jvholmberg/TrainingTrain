namespace Application.Views
{
	public class RegisterRequest
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string PasswordVerify { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}

	public class RegisterResponse
	{
		public string Email { get; set; }
	}
}
