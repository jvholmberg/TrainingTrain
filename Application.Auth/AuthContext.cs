using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Application.Auth.Context
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options)
            : base(options)
        {
        }

		public DbSet<Entities.User> User { get; set; }
		public DbSet<Entities.Role> Role { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder
				.Entity<Entities.User>()
				.ToTable("user");
            modelBuilder
                .Entity<Entities.User>()
                .HasOne(usr => usr.Role);

            modelBuilder
                .Entity<Entities.Role>()
                .ToTable("role");
        }
	}

	public class AuthContextFactory : IDesignTimeDbContextFactory<AuthContext>
	{
		public AuthContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<AuthContext>();
			optionsBuilder.UseNpgsql("User ID=postgres;Server=localhost;Database=msvc_auth");
			return new AuthContext(optionsBuilder.Options);
		}
	}
}
