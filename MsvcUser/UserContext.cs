using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

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
				.ToTable("user");
			modelBuilder.Entity<Entities.User>()
				.HasOne(usr => usr.Language);
			
			// Language
			modelBuilder.Entity<Entities.Language>()
				.ToTable("language");
		}
	}

	public class UserContextFactory : IDesignTimeDbContextFactory<UserContext>
	{
		public UserContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<UserContext>();
			optionsBuilder.UseNpgsql("User ID=postgres;Server=localhost;Database=msvc_user");
			return new UserContext(optionsBuilder.Options);
		}
	}
}
