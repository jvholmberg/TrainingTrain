using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace MsvcAuth
{
	public static class Setup
	{

		public static void SetupMsvcAuth(this IServiceCollection services, string connectionString)
		{
			
			// Establish database connection
			services.AddEntityFrameworkNpgsql().AddDbContext<Context.AuthContext>(options => options.UseNpgsql(connectionString));

			// Add service
			services.AddScoped<Services.IAuthService, Services.AuthService>();

            // Config authentication
			var key = Encoding.ASCII.GetBytes(Helpers.AuthConstants.Secret);
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.Events = new JwtBearerEvents()
				{
					OnTokenValidated = context =>
					{
						var token = context.SecurityToken as JwtSecurityToken;
						var payload = token.Payload;

						// Create new header containing userId
						if (payload.TryGetValue("unique_name", out object userIdObj))
						{
							var userId = userIdObj as string;
							context.Request.Headers.Add("msvcAuthUserId", userId);
						}

						// create new header containing userRole
						if(payload.TryGetValue("role", out object userRoleObj))
						{
							var userRole = userRoleObj as string;
							context.Request.Headers.Add("msvcAuthUserRole", userRole);
						}
						
						return Task.CompletedTask;
					}
				};
				options.RequireHttpsMetadata = false;
				options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true,
					// ValidIssuer = appSettings.Issuer,
					// ValidAudience = appSettings.Issuer,
				};
			});
		}

		public static void SetupMsvcAuth(this IApplicationBuilder app)
		{
			app.UseAuthentication();
		}
	}
}