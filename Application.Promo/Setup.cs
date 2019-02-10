
using Microsoft.Extensions.DependencyInjection;

namespace Application.Promo
{
    public static class Setup
	{

		public static void SetupApplicationPromo(this IServiceCollection services, string connectionString)
		{
			
			//// Establish database connection
			//services.AddEntityFrameworkNpgsql().AddDbContext<Context.UserContext>(options => options.UseNpgsql(connectionString));

			//// Add service
			//services.AddScoped<Services.IUserService, Services.UserService>();
		}
	}
}