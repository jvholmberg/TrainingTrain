using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MsvcUser
{
    public static class Setup
	{

		public static void SetupMsvcUser(this IServiceCollection services, string connectionString)
		{
			
			// Establish database connection
			services.AddEntityFrameworkNpgsql().AddDbContext<Context.UserContext>(options => options.UseNpgsql(connectionString));

			// Add service
			services.AddScoped<Services.IUserService, Services.UserService>();
		}

		public static void SetupMsvcAuth(this IApplicationBuilder app)
		{
			app.UseAuthentication();
		}
	}
}