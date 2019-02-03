using System;

namespace MsvcUser.Entities
{
	public class User
	{
		public int Id { get; set; }
        public int AuthId { get; set; }

        public string FirstName { get; set; }
		public string LastName { get; set; }

		public Language Language { get; set; }
		
		public DateTime CreatedAt { get; set; }
		public DateTime LastModified { get; set; }	
	}
}