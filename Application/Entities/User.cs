using System;

namespace Application.Entities
{
	public class User
	{
		public int Id { get; set; }
		
		public string Username { get; set; }
		public string Password { get; set; }
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public Language Language { get; set; }

		public Role Role { get; set; }
		public bool Activated { get; set; }
		
		public DateTime CreatedAt { get; set; }
		public DateTime LastModified { get; set; }	
	}
}