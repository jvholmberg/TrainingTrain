using Application.Data;
using Application.Helpers;
using Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			var appSettingsSection = Configuration.GetSection("AppSettings");
			services.Configure<AppSettings>(appSettingsSection);
			var appSettings = appSettingsSection.Get<AppSettings>();
			
			// Basic configuration

			services.AddCors();
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
			// Load settings

			var connectionString = Configuration.GetConnectionString("DatabaseConnection");
			services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationContext>(options => options.UseNpgsql(connectionString));

			// Configure JWT authentication
			var key = Encoding.ASCII.GetBytes(appSettings.Secret);
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.RequireHttpsMetadata = appSettings.RequireHttpsMetadata;
				options.SaveToken = appSettings.SaveToken;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = appSettings.ValidateIssuerSigningKey,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = appSettings.ValidateIssuer,
					ValidateAudience = appSettings.ValidateAudience,
					ValidateLifetime = appSettings.ValidateLifeTime,
					ValidIssuer = appSettings.Issuer,
					ValidAudience = appSettings.Issuer,
				};
			});
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IUserService, UserService>();

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseStaticFiles();
			app.UseHttpsRedirection();
			app.UseCors(options => options
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader()
			);
			app.UseAuthentication();

			app.UseMvc();
		}
	}
}
