using Microsoft.EntityFrameworkCore;

namespace MsvcAuth.Context
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options)
            : base(options)
        {
        }

		public DbSet<Entities.User> Auth { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder
				.Entity<Entities.User>()
				.ToTable("user");
		}
	}
}
