using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Application.Auth;
using Application.User;
using Application.Promo;

namespace Application
{
	public class Startup
	{
		public IConfiguration _Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			_Configuration = configuration;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors();
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            // Configure Auth
            var authConnectionString = _Configuration.GetConnectionString("AuthDatabaseConnection");
            services.SetupApplicationAuth(authConnectionString);

            // Configure User
            var userConnectionString = _Configuration.GetConnectionString("UserDatabaseConnection");
            services.SetupApplicationUser(userConnectionString);

			// Configure Promo
			var promoConnectionString = _Configuration.GetConnectionString("PromoDatabaseConnection");
			services.SetupApplicationPromo(promoConnectionString);


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

			app.SetupApplicationAuth();

			app.UseMvc();
		}
	}
}
