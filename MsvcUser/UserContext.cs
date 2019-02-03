using Microsoft.EntityFrameworkCore;

namespace MsvcUser.Context
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
        }

		public DbSet<Entities.User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// User
			modelBuilder.Entity<Entities.User>()
				.ToTable("User");
			modelBuilder.Entity<Entities.User>()
				.HasOne(usr => usr.Language);

			// Role
			modelBuilder.Entity<Entities.Role>()
				.ToTable("Role");
			
			// Language
			modelBuilder.Entity<Entities.Language>()
				.ToTable("Language");
		}
	}
}
