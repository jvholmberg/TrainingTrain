using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace MsvcUser
{
	public static class Setup
	{

		public static void SetupMsvcUser(this IServiceCollection services)
		{
			
			// Establish database connection
			services.AddEntityFrameworkNpgsql().AddDbContext<Context.AuthContext>(options => options.UseNpgsql(connectionString));

			// Add service
			services.AddScoped<Services.IUserService, Services.UserService>();
		}

		public static void SetupMsvcAuth(this IApplicationBuilder app)
		{
			app.UseAuthentication();
		}
	}
}