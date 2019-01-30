namespace MsvcAuth.Views
{
	public class MsvcAuthLoginRequestBody
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}

	public class MsvcAuthRegisterRequestBody : MsvcAuthLoginRequestBody
	{
		public string PasswordVerify { get; set; }
	}
}
