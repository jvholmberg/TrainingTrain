using System;

namespace Application.Views
{
	public class User
	{
		public int Id { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime LastModified { get; set; }
		public bool Activated { get; set; }
		public string Role { get; set; }
		public Permission Permission { get; set; }
		public Language Language { get; set; }

		public User(Entities.User user)
		{
			Id = user.Id;
			Email = user.Email;
			FirstName = user.FirstName;
			LastName = user.LastName;
			CreatedAt = user.CreatedAt;
			LastModified = user.LastModified;
			Activated = user.Activated;
			Role = user?.Role?.Name;
			Permission = new Permission(user.Role);
			Language = new Language(user.Language);
		}
	}
}
