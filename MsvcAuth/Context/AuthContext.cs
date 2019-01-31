using Microsoft.EntityFrameworkCore;

namespace MsvcAuth.Context
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options)
            : base(options)
        {
        }

		public DbSet<Entities.Auth> Auth { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
		}
	}
}
