using System;
namespace MsvcUser.Views
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Language Language { get; set; }

        public bool Activated { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }
    }
}
