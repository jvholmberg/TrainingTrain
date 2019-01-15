using System;

namespace Application.Entities
{
	public class User
	{
		public int Id { get; set; }
		
		public string Email { get; set; }
		public string Password { get; set; }
		public string Token { get; set; }
		public DateTime TokenExpires { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public Language Language { get; set; }

		public Role Role { get; set; }
		public bool Activated { get; set; }
		
		public DateTime CreatedAt { get; set; }
		public DateTime LastModified { get; set; }
		
	}
}