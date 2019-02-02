using Microsoft.EntityFrameworkCore;

namespace MsvcUser.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
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
				.HasOne(usr => usr.Role);
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
